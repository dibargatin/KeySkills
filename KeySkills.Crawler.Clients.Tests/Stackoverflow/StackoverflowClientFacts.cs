using System;
using Xunit;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Crawler.Core.Models;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Clients.Stackoverflow;
using static KeySkills.Crawler.Clients.Stackoverflow.StackoverflowClient;
using KeySkills.Crawler.Core.Services;

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class StackoverflowClientFacts
    {
        public class GetVacancies_Should
        {
            private RequestFactory _requestFactory = new RequestFactory {
                EndpointUri = new Uri("https://stackoverflow.com/jobs/feed")
            };
            
            private static Mock<IKeywordsExtractor> GetKeywordsExtractorMock() 
            {
                var mock = new Mock<IKeywordsExtractor>();
                
                mock.Setup(e => e.Extract(It.IsAny<Vacancy>()))
                    .Returns(Observable.Empty<Keyword>())
                    .Verifiable();

                return mock;
            }

            private StackoverflowClient GetStackoverflowClient(
                HttpMessageHandler handler, 
                bool isVacancyExisted = false,
                IKeywordsExtractor keywordsExtractor = null
            ) =>
                new StackoverflowClient(
                    new HttpClient(handler),
                    _requestFactory, 
                    _ => isVacancyExisted,
                    keywordsExtractor ?? GetKeywordsExtractorMock().Object
                );

            private Mock<HttpMessageHandler> GetHttpMessageHandlerMock(HttpResponseMessage expectedResponse)
            {
                var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

                httpMessageHandlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    ).ReturnsAsync(expectedResponse)
                    .Verifiable();

                return httpMessageHandlerMock;
            }
            
            [Fact]
            public void MakeSingleHttpGetRequest()
            {
                var httpMessageHandler = GetHttpMessageHandlerMock(
                    expectedResponse: new HttpResponseMessage() { 
                        StatusCode = HttpStatusCode.OK
                    });
                
                GetStackoverflowClient(httpMessageHandler.Object)
                    .GetVacancies().ToList();
                
                httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // Only single GET request is expected
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == _requestFactory.EndpointUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }

            public class ExpectedResponse
            {
                public IEnumerable<Vacancy> Vacancies { get; set; }

                public ExpectedResponse(IEnumerable<Vacancy> vacancies) =>
                    Vacancies = vacancies ?? throw new ArgumentNullException(nameof(vacancies));

                public ExpectedResponse(Vacancy vacancy) : this(new[] { vacancy }) {}

                private string VacancyToXml(Vacancy vacancy) => 
                    $@"<item>
                        <link>{vacancy.Link}</link>
                        <title>{vacancy.Title}</title>
                        <description>{vacancy.Description}</description>
                        <pubDate>{vacancy.PublishedAt.ToString("O")}</pubDate>
                        {(
                            vacancy.CountryCode.HasValue ?
                                $@"<location xmlns='http://stackoverflow.com/jobs/'>City, {
                                    CountryHelper.GetCountryName(vacancy.CountryCode.Value)
                                }</location>" :
                                String.Empty
                        )}
                    </item>";

                private string VacanciesToXml(IEnumerable<Vacancy> vacancies) =>
                    $@"<rss><channel>{
                        String.Join(String.Empty, vacancies.Select(v => VacancyToXml(v)))
                    }</channel></rss>";
                
                public HttpResponseMessage CreateResponse() =>
                    new HttpResponseMessage() { 
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(VacanciesToXml(Vacancies))
                    };
            }

            public static TheoryData<ExpectedResponse> ExpectedResponseData =>
                new TheoryData<ExpectedResponse> {
                    { 
                        new ExpectedResponse(
                            new Vacancy {
                                Link = "abc",
                                Title = "xyz",
                                Description = "qwerty",
                                PublishedAt = 1.March(2020).At(12, 0).AsUtc(),
                                Keywords = Enumerable.Empty<Keyword>()
                            }) 
                    },
                    { 
                        new ExpectedResponse(new[] {
                            new Vacancy {
                                Link = "yuit",
                                Title = "ghj",
                                Description = "dff",
                                PublishedAt = 3.March(2020).At(12, 0).AsUtc(),
                                Keywords = Enumerable.Empty<Keyword>()
                            },
                            new Vacancy {
                                Link = "zzz",
                                Title = "yyy",
                                Description = "asdfg",
                                PublishedAt = 2.March(2020).At(12, 0).AsUtc(),
                                CountryCode = Country.FR,
                                Keywords = Enumerable.Empty<Keyword>()
                            }
                        })
                    }
                };

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void ReturnExpectedResponse(ExpectedResponse expected) =>            
                (await GetStackoverflowClient(GetHttpMessageHandlerMock(expected.CreateResponse()).Object).GetVacancies().ToList().ToTask())
                    .Should().BeEquivalentTo(expected.Vacancies);

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void FilterOutExistedVacancies(ExpectedResponse expected) =>            
                (await GetStackoverflowClient(
                        handler: GetHttpMessageHandlerMock(expected.CreateResponse()).Object,
                        isVacancyExisted: true // should filter out all vacancies
                    ).GetVacancies().ToList().ToTask()
                ).Should().BeEmpty();

            
            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void InvokeKeywordsExtractor(ExpectedResponse expected)
            {
                var keywordsExtractorMock = GetKeywordsExtractorMock();

                var vacancies = await GetStackoverflowClient(
                        handler: GetHttpMessageHandlerMock(expected.CreateResponse()).Object,
                        isVacancyExisted: false,
                        keywordsExtractor: keywordsExtractorMock.Object
                    ).GetVacancies().ToList().ToTask();

                keywordsExtractorMock.Verify(
                    k => k.Extract(It.IsAny<Vacancy>()), 
                    Times.Exactly(expected.Vacancies.Count())
                );
            }
        }
    }
}