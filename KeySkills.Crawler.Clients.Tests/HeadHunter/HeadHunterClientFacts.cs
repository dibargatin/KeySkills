using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Crawler.Clients.HeadHunter;
using KeySkills.Core.Models;
using KeySkills.Crawler.Core.Services;
using Moq;
using Moq.Protected;
using Xunit;
using static KeySkills.Crawler.Clients.HeadHunter.HeadHunterClient;
using static KeySkills.Crawler.Clients.Tests.HeadHunterClientFacts.GetVacancies_Should.ExpectedResponse;

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class HeadHunterClientFacts
    {
        public class GetVacancies_Should
        {
            public class ExpectedResponse
            {
                public class Page 
                {
                    public class PageItem
                    {
                        public Response.JobPost JobPost { get; set; }

                        public string GetJobPostJson() =>
                            $@"{{""name"":""{
                                JobPost.Name
                            }"",""area"":{{""id"":""{
                                JobPost.Area.Id
                            }""}},""description"":""{
                                JobPost.Description
                            }"",""published_at"":""{
                                JobPost.PublishedAt.ToString("O")
                            }"",""alternate_url"":""{
                                JobPost.AlternateUrl
                            }""}}";
                    }

                    public int PageNumber {get; set; }
                    public Response.Root Root { get; set; }
                    public IEnumerable<PageItem> PageItems { get; set; }

                    public string GetRootJson() =>
                        $@"{{""items"":{
                            GetRootItemsJson()
                        },""pages"":{
                            Root.PagesCount
                        },""page"":{
                            Root.CurrentPage
                        }}}";

                    private string GetRootItemsJson() => 
                        Root.Items == null ? "[]" : 
                            String.Concat("[",
                                String.Join(',', Root.Items.Select(i => 
                                    $@"{{""url"":""{
                                        i.Url
                                    }"",""alternate_url"":""{
                                        i.AlternateUrl
                                    }""}}")
                                ), "]");
                }

                public IEnumerable<Page> Pages { get; set; }

                public IEnumerable<Vacancy> Vacancies { get; set; }

                private HttpResponseMessage CreateResponse(string content) =>
                    new HttpResponseMessage {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(content)
                    };

                private static string GetNullableJson<T>(T value) =>
                    value == null ? "null" : typeof(T) switch {
                        var t when t == typeof(string) => $@"""{value}""",
                        _ => value.ToString()
                    };

                private static string GetAreaJson(Response.AreaInfo area) => 
                    $@"{{""id"":""{
                        area.Id
                    }"",""parent_id"":{
                        GetNullableJson(area.ParentId)
                    },""name"":""{
                        area.Name
                    }""}}";

                private IEnumerable<Tuple<HttpRequestMessage, Func<HttpResponseMessage>>> GetHttpMessages() => 
                    // Messages for root endpoint
                    Pages.Select(page => new Tuple<HttpRequestMessage, Func<HttpResponseMessage>>(
                        _requestFactory.CreateRootRequest(page.PageNumber),
                        () => CreateResponse(page.GetRootJson())
                    ))
                    // Messages for job details endpoint
                    .Union(Pages.SelectMany(page => 
                        page.PageNumber * RequestFactory.RootRequestParams.ItemsPerPage < RequestFactory.RootRequestParams.MaxItemsCount ?
                            page.PageItems.Select(item => new Tuple<HttpRequestMessage, Func<HttpResponseMessage>>(
                                _requestFactory.CreateJobDetailsRequest(GetJobPostWebApiUrl(item.JobPost.Id)),
                                () => CreateResponse(item.GetJobPostJson())
                            )) : Enumerable.Empty<Tuple<HttpRequestMessage, Func<HttpResponseMessage>>>()
                    ));

                /// <summary>
                /// Creates http messages for areas info endpoint
                /// </summary>
                private IEnumerable<Tuple<HttpRequestMessage, Func<HttpResponseMessage>>> GetAreaHttpMessages() =>
                    _areaInfo.Select(area => new Tuple<HttpRequestMessage, Func<HttpResponseMessage>>(
                        _requestFactory.CreateAreaRequest(area.Id),
                        () => CreateResponse(GetAreaJson(area))
                    ));

                public Mock<HttpMessageHandler> GetHttpMessageHandlerMock()
                {
                    var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

                    foreach (var message in GetHttpMessages().Union(GetAreaHttpMessages()))
                    {
                        HttpRequestMessage expectedRequest = message.Item1;
                        Func<HttpResponseMessage> expectedResponse = message.Item2;

                        if (expectedRequest != null)
                        {
                            httpMessageHandlerMock.Protected()
                                .Setup<Task<HttpResponseMessage>>(
                                    "SendAsync",
                                    ItExpr.Is<HttpRequestMessage>(request => 
                                        request.Method == expectedRequest.Method
                                        && request.RequestUri == expectedRequest.RequestUri
                                    ),
                                    ItExpr.IsAny<CancellationToken>()
                                ).ReturnsAsync(expectedResponse)
                                .Verifiable();
                        }
                    }

                    return httpMessageHandlerMock;
                }

                public void VerifyHttpMessageHandlerMock(Mock<HttpMessageHandler> mock) =>
                    GetHttpMessages().ToList().ForEach(request => {
                        if (request.Item1 != null)
                            mock.Protected().Verify(
                                "SendAsync",
                                Times.Exactly(1), // Only single GET request is expected
                                ItExpr.Is<HttpRequestMessage>(req =>
                                    req.Method == request.Item1.Method
                                    && req.RequestUri == request.Item1.RequestUri
                                ),
                                ItExpr.IsAny<CancellationToken>()
                            );
                    });

                public static ExpectedResponse Create() =>
                    new ExpectedResponse {
                        Pages = Enumerable.Empty<ExpectedResponse.Page>(),
                        Vacancies = Enumerable.Empty<Vacancy>()
                    };

                public ExpectedResponse AddPage()
                {
                    var pagesCount = Pages.Count() + 1;

                    foreach(var page in Pages)
                        page.Root.PagesCount = pagesCount;

                    Pages = Pages.Append(new Page {
                        PageNumber = pagesCount,
                        Root = new Response.Root {
                            Items = Enumerable.Empty<Response.Item>().ToArray(),
                            CurrentPage = pagesCount,
                            PagesCount = pagesCount
                        },
                        PageItems = Enumerable.Empty<Page.PageItem>()
                    });

                    return this;
                }

                public ExpectedResponse AddItem(string id, Response.AreaInfo area, string description, string name)
                {
                    var page = Pages.Last();
                    var publishedAt = 1.March(2020).At(0, 0, 0).AsUtc();

                    page.Root.Items = page.Root.Items.Append(
                        new Response.Item {
                        AlternateUrl = GetJobPostWebsiteUrl(id),
                        Url = GetJobPostWebApiUrl(id)
                    }).ToArray();

                    page.PageItems = page.PageItems.Append(
                        new ExpectedResponse.Page.PageItem {
                            JobPost = new Response.JobPost {
                                Id = id,
                                AlternateUrl = GetJobPostWebsiteUrl(id),
                                Area = new Response.Area {
                                    Id = area.Id
                                },
                                Description = description,
                                Name = name,
                                PublishedAt = publishedAt
                            }
                        });

                    Vacancies = Vacancies.Append(
                        new Vacancy {
                            Link = GetJobPostWebsiteUrl(id),
                            CountryCode = Country.RU,
                            Description = description,
                            Title = name,
                            PublishedAt = publishedAt,
                            Keywords = Enumerable.Empty<VacancyKeyword>()
                        });

                    return this;
                }
            }

            private static RequestFactory _requestFactory =
                new RequestFactory {
                    BaseUri = _baseUri,
                    AreaUrl = "/areas",
                    RootParams = new RequestFactory.RootRequestParams {
                        Url = "/vacancies",
                        Industry = "7",
                        Text = "developer"
                    }
                };
            
            private static Mock<IKeywordsExtractor> GetKeywordsExtractorMock() 
            {
                var mock = new Mock<IKeywordsExtractor>();
                
                mock.Setup(e => e.Extract(It.IsAny<Vacancy>()))
                    .Returns(Observable.Empty<Keyword>())
                    .Verifiable();

                return mock;
            }

            private HeadHunterClient GetHeadHunterClient(
                HttpMessageHandler handler, 
                bool isVacancyExisted = false,
                IKeywordsExtractor keywordsExtractor = null
            ) =>
                new HeadHunterClient(
                    new HttpClient(handler),
                    _requestFactory,
                    _ => Task.FromResult(isVacancyExisted),
                    keywordsExtractor ?? GetKeywordsExtractorMock().Object
                );

            private static string GetJobPostWebApiUrl(string id) =>
                new Uri(_requestFactory.BaseUri, String.Concat(_requestFactory.RootParams.Url, "/", id)).ToString();

            private static string GetJobPostWebsiteUrl(string id) =>
                $"https://hh.ru/vacancy/{id}";

            public static TheoryData<ExpectedResponse> ExpectedResponseData => 
                new TheoryData<ExpectedResponse> {                    
                    // Single empty page
                    ExpectedResponse.Create().AddPage(),
                    // Single page with single item
                    ExpectedResponse.Create().AddPage()
                        .AddItem(
                            id: "1",
                            area: _areaInfo[0],
                            description: "zxc",
                            name: "asd"
                        ),                    
                    // Single page with two items
                    ExpectedResponse.Create().AddPage()
                        .AddItem(
                            id: "2",
                            area: _areaInfo[0],
                            description: "qwerty",
                            name: "yui"
                        ).AddItem(
                            id: "3",
                            area: _areaInfo[1],
                            description: "trewq",
                            name: "dsa"
                        ),
                    // Two pages with a single item on each
                    ExpectedResponse.Create()
                        .AddPage().AddItem(
                            id: "4",
                            area: _areaInfo[1],
                            description: "ghjkl",
                            name: "sdfr"
                        ).AddPage().AddItem(
                            id: "5",
                            area: _areaInfo[2],
                            description: "oite",
                            name: "lkjfd"
                        ),
                    // Last two pages with max items count
                    LastTwoPagesWithMaxItemsCount()
                };

            private static ExpectedResponse LastTwoPagesWithMaxItemsCount()
            {
                var result = ExpectedResponse.Create()
                    .AddPage().AddItem(
                        id: (RequestFactory.RootRequestParams.MaxItemsCount - 1).ToString(),
                        area: _areaInfo[1],
                        description: "ghjkl",
                        name: "sdfr"
                    ).AddPage().AddItem(
                        id: RequestFactory.RootRequestParams.MaxItemsCount.ToString(),
                        area: _areaInfo[2],
                        description: "oite",
                        name: "lkjfd"
                    );
                
                var totalPages = 
                    RequestFactory.RootRequestParams.MaxItemsCount 
                    / RequestFactory.RootRequestParams.ItemsPerPage;

                var first = result.Pages.First();
                
                first.Root.PagesCount = totalPages;
                first.Root.CurrentPage = totalPages - 1;

                var last = result.Pages.Last();

                last.PageNumber = totalPages;
                last.Root.PagesCount = totalPages;
                last.Root.CurrentPage = totalPages;

                result.Vacancies = 
                    result.Vacancies.Where(vacancy => 
                        !last.PageItems.Any(pi => pi.JobPost.AlternateUrl == vacancy.Link));

                return result;
            }

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void ReturnExpectedResult(ExpectedResponse expected) =>
                (await GetHeadHunterClient(expected.GetHttpMessageHandlerMock().Object)
                    .GetVacancies().ToList().ToTask()
                ).Should().BeEquivalentTo(expected.Vacancies);

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void FilterOutExistedVacancies(ExpectedResponse expected) =>
                (await GetHeadHunterClient(
                        handler: expected.GetHttpMessageHandlerMock().Object,
                        isVacancyExisted: true // should filter out all vacancies
                    ).GetVacancies().ToList().ToTask()
                ).Should().BeEmpty();

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void MakeHttpGetRequests(ExpectedResponse expected)
            {
                var handlerMock = expected.GetHttpMessageHandlerMock();

                var vacancies = await GetHeadHunterClient(handlerMock.Object).GetVacancies().ToList().ToTask();

                expected.VerifyHttpMessageHandlerMock(handlerMock);
            }

            [Theory]
            [MemberData(nameof(ExpectedResponseData))]
            public async void InvokeKeywordsExtractor(ExpectedResponse expected)
            {
                var keywordsExtractorMock = GetKeywordsExtractorMock();

                var vacancies = await GetHeadHunterClient(
                        handler: expected.GetHttpMessageHandlerMock().Object,
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