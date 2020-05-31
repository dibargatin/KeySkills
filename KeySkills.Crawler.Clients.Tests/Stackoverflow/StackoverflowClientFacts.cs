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

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class StackoverflowClientFacts
    {
        public class GetVacancies_Should
        {
            private RequestFactory _requestFactory = new RequestFactory {
                EndpointUri = new Uri("https://stackoverflow.com/jobs/feed")
            };

            private StackoverflowClient GetStackoverflowClient(HttpMessageHandler handler, bool isVacancyExisted = false) =>
                new StackoverflowClient(new HttpClient(handler), _requestFactory, _ => isVacancyExisted);

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

            private static Vacancy[] _vacancies = new[] {
                new Vacancy {
                    Link = "abc",
                    Title = "xyz",
                    Description = "qwerty",
                    PublishedAt = 1.March(2020).At(12, 0).AsUtc()
                },
                new Vacancy {
                    Link = "zzz",
                    Title = "yyy",
                    Description = "asdfg",
                    PublishedAt = 2.March(2020).At(12, 0).AsUtc(),
                    CountryCode = Country.FR
                }
            };

            private static IEnumerable<T> Single<T>(T item) => new[] { item };

            private static string VacancyToXml(Vacancy vacancy) => 
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

            private static string VacanciesToXml(IEnumerable<Vacancy> vacancies) =>
                $@"<rss><channel>{
                    String.Join(String.Empty, vacancies.Select(v => VacancyToXml(v)))
                }</channel></rss>";
            
            private static HttpResponseMessage CreateResponse(string responseContent) =>
                new HttpResponseMessage() { 
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                };

            public static TheoryData<HttpResponseMessage, IEnumerable<Vacancy>> ExpectedResponseData =>
                new TheoryData<HttpResponseMessage, IEnumerable<Vacancy>> {
                    { CreateResponse(VacanciesToXml(Single(_vacancies[0]))), Single(_vacancies[0]) },
                    { CreateResponse(VacanciesToXml(_vacancies)), _vacancies }
                };

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void ReturnExpectedResponse(HttpResponseMessage response, IEnumerable<Vacancy> expected) =>            
                (await GetStackoverflowClient(GetHttpMessageHandlerMock(response).Object).GetVacancies().ToList().ToTask())
                    .Should().BeEquivalentTo(expected);

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void FilterOutExistedVacancies(HttpResponseMessage response, IEnumerable<Vacancy> nope) =>            
                (await GetStackoverflowClient(
                        handler: GetHttpMessageHandlerMock(response).Object,
                        isVacancyExisted: true // should filter out all vacancies
                    ).GetVacancies().ToList().ToTask()
                ).Should().BeEmpty();
        }
    }
}