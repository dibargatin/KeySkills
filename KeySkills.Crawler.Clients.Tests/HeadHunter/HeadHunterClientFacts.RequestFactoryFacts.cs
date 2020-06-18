using System;
using System.Net.Http;
using FluentAssertions;
using Xunit;
using static KeySkills.Crawler.Clients.HeadHunter.HeadHunterClient;
using static KeySkills.Crawler.Clients.HeadHunter.HeadHunterClient.RequestFactory;

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class HeadHunterClientFacts
    {
        protected static Uri _baseUri = new Uri("https://api.hh.ru");

        public class RequestFactoryFacts : HeadHunterClientFacts
        {
            public class CreateRootRequest_Should : RequestFactoryFacts
            {                
                public static TheoryData<RequestFactory, int, Uri> ReturnRequestToExpectedUriData =>
                    new TheoryData<RequestFactory, int, Uri> {
                        {
                            new RequestFactory {
                                BaseUri = _baseUri,
                                RootParams = new RootRequestParams()
                            },
                            1,
                            new Uri(_baseUri, $"?per_page={RootRequestParams.ItemsPerPage}&page=1")
                        }
                    };

                [Theory]
                [MemberData(nameof(ReturnRequestToExpectedUriData))]
                public void ReturnRequestToExpectedUri(RequestFactory factory, int page, Uri expected) =>
                    factory.CreateRootRequest(page).RequestUri
                        .Should().Be(expected);

                [Fact]
                public void ReturnGetRequest() =>
                    new RequestFactory {
                        BaseUri = _baseUri,
                        RootParams = new RootRequestParams()
                    }.CreateRootRequest(1)
                    .Method.Should().Be(HttpMethod.Get);  

                [Fact]
                public void ReturnNullWhenThereIsMaxItemsCountOnRequestedPage()
                {
                    new RequestFactory {
                        BaseUri = _baseUri,
                        RootParams = new RootRequestParams()
                    }.CreateRootRequest(RootRequestParams.MaxItemsCount / RootRequestParams.ItemsPerPage)
                    .Should().BeNull();
                }
            }

            public class CreateJobDetailsRequest_Should : RequestFactoryFacts
            {
                [Fact]
                public void ReturnRequestToExpectedUri() =>
                    new RequestFactory().CreateJobDetailsRequest(_baseUri.ToString())
                        .RequestUri.Should().Be(_baseUri);

                [Fact]
                public void ReturnGetRequest() =>
                    new RequestFactory().CreateJobDetailsRequest(_baseUri.ToString())
                        .Method.Should().Be(HttpMethod.Get); 
            }

            public class CreateAreaRequest_Should : RequestFactory
            {
                public static TheoryData<RequestFactory, string, Uri> ReturnRequestToExpectedUriData =>
                    new TheoryData<RequestFactory, string, Uri> {
                        { new RequestFactory { BaseUri = _baseUri }, null, _baseUri },
                        { new RequestFactory { BaseUri = _baseUri }, String.Empty, _baseUri },
                        { new RequestFactory { BaseUri = _baseUri }, "xyz", new Uri(_baseUri, "/xyz") },
                        { new RequestFactory {
                                BaseUri = _baseUri,
                                AreaUrl = "/areas"
                            },
                            "abc",
                            new Uri(_baseUri, "/areas/abc")
                        }
                    };

                [Theory]
                [MemberData(nameof(ReturnRequestToExpectedUriData))]
                public void ReturnRequestToExpectedUri(RequestFactory factory, string id, Uri expected) =>
                    factory.CreateAreaRequest(id).RequestUri
                        .Should().Be(expected);

                [Fact]
                public void ReturnGetRequest() =>
                    new RequestFactory {
                        BaseUri = _baseUri
                    }.CreateAreaRequest("abc")
                    .Method.Should().Be(HttpMethod.Get);
            }

            public class RootRequestParamsFacts
            {
                public class GetQueryString_Should
                {
                    public static TheoryData<RootRequestParams, int, string> ReturnExpectedQueryData => 
                        new TheoryData<RootRequestParams, int, string> {
                            { 
                                new RootRequestParams(), 
                                1, 
                                $@"?per_page={
                                    RootRequestParams.ItemsPerPage
                                }&page=1"
                            },
                            { 
                                new RootRequestParams {
                                    Url = String.Empty
                                }, 
                                10, 
                                $@"?per_page={
                                    RootRequestParams.ItemsPerPage
                                }&page=10"
                            },
                            { 
                                new RootRequestParams {
                                    Url = "/vacancies"
                                }, 
                                20, 
                                $@"/vacancies?per_page={
                                    RootRequestParams.ItemsPerPage
                                }&page=20"
                            },
                            { 
                                new RootRequestParams {
                                    Url = "/jobs",
                                    Text = "developer"
                                }, 
                                30, 
                                $@"/jobs?text=developer&per_page={
                                    RootRequestParams.ItemsPerPage
                                }&page=30"
                            },
                            { 
                                new RootRequestParams {
                                    Url = "/job list",
                                    Text = "software engineer",
                                    Industry = "7 5"
                                },
                                40, 
                                $@"/job%20list?text=software%20engineer&industry=7%205&per_page={
                                    RootRequestParams.ItemsPerPage
                                }&page=40"
                            },
                        };

                    [Theory]
                    [MemberData(nameof(ReturnExpectedQueryData))]
                    public void ReturnExpectedQuery(RootRequestParams requestParams, int page, string expected) =>
                        requestParams.GetQueryString(page)
                            .Should().Be(expected);
                }
            }
        }
    }
}