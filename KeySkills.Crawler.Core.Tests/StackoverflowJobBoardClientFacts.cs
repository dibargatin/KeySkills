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

namespace KeySkills.Crawler.Core.Tests
{
    public class StackoverflowJobBoardClientFacts
    {
        public class JobPostFacts
        {
            public class GetVacancy_Should
            {                
                private static readonly string _correctPublishedDateString = "Sun, 1 Mar 2020 00:00:00 Z";

                private StackoverflowJobBoardClient.JobPost GetJobPost(string location, string publishedDateString) =>
                    new StackoverflowJobBoardClient.JobPost { 
                        Location = location,
                        PublishedDateString = publishedDateString 
                    };

                public static TheoryData<string> LoremIpsum =>
                    new TheoryData<string> {
                        { null },
                        { String.Empty },
                        { "Lorem ipsum" }
                    };

                [Theory]
                [MemberData(nameof(LoremIpsum))]
                public void ReturnLink(string link) =>
                    new StackoverflowJobBoardClient.JobPost { 
                        Link = link,
                        PublishedDateString = _correctPublishedDateString
                    }.GetVacancy().Link.Should().Be(link);

                [Theory]
                [MemberData(nameof(LoremIpsum))]
                public void ReturnTitle(string title) =>
                    new StackoverflowJobBoardClient.JobPost { 
                        Title = title,
                        PublishedDateString = _correctPublishedDateString
                    }.GetVacancy().Title.Should().Be(title);

                [Theory]
                [MemberData(nameof(LoremIpsum))]
                public void ReturnDescription(string description) =>
                    new StackoverflowJobBoardClient.JobPost { 
                        Description = description,
                        PublishedDateString = _correctPublishedDateString
                    }.GetVacancy().Description.Should().Be(description);

                public static TheoryData<string, DateTime> ConvertCorrectPublishedDateStringData =>
                    new TheoryData<string, DateTime> {
                        { "Sun, 1 Mar 2020 00:00:00 Z", 1.March(2020).At(0, 0, 0).AsUtc() },
                        { "Thu, 19 Mar 2020 09:00:19 Z", 19.March(2020).At(9, 0, 19).AsUtc() },
                        { "Tue, 31 Mar 2020 23:59:59 Z", 31.March(2020).At(23, 59, 59).AsUtc() },
                    };

                [Theory]
                [MemberData(nameof(ConvertCorrectPublishedDateStringData))]
                public void ConvertCorrectPublishedDateString(string publishedDateString, DateTime expected) =>
                    GetJobPost(null, publishedDateString).GetVacancy().PublishedAt
                        .Should().Be(expected, $"because of {publishedDateString}");                

                [Theory]
                [InlineData("")]
                [InlineData("abc")]
                [InlineData("2020-01-01 xyz")]
                public void ThrowOnWrongFormatOfPublishedDateString(string publishedDateString) =>
                    this.Should().Invoking(_ => GetJobPost(null, publishedDateString).GetVacancy())                    
                        .Should().Throw<FormatException>($"because {publishedDateString} isn't a valid date");

                [Theory]
                [InlineData("London, UK", Country.GB)]
                [InlineData("Jersey City, NJ", Country.US)]
                [InlineData("Dresden, Deutschland", Country.DE)]
                [InlineData("München, Germany", Country.DE)]
                [InlineData("Ecublens VD, Schweiz", Country.CH)]
                [InlineData("Mariehamn, Åland Islands", Country.AX)]
                public void ExtractCountryFromLocation(string location, Country expected) =>
                    GetJobPost(location, _correctPublishedDateString).GetVacancy().CountryCode
                        .Should().Be(expected, $"because of {location}");
                
                [Theory]
                [MemberData(nameof(LoremIpsum))]
                public void ReturnNullForCountryWhenLocationIsEmpty(string location) => 
                    GetJobPost(location, _correctPublishedDateString).GetVacancy().CountryCode
                        .Should().BeNull($"because {location} doesn't contain region info");
            }
        }

        public class GetVacancies_Should
        {
            private Uri _baseUri = new Uri("https://stackoverflow.com");
            
            private StackoverflowJobBoardClient GetStackoverflowJobBoardClient(HttpMessageHandler handler) =>
                new StackoverflowJobBoardClient(
                    new HttpClient(handler) {
                        BaseAddress = _baseUri
                    });

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
                
                GetStackoverflowJobBoardClient(httpMessageHandler.Object)
                    .GetVacancies().ToList();
                
                httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // Only single GET request is expected
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri(_baseUri, "/jobs/feed")
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

            public static TheoryData<string, IEnumerable<Vacancy>> CorrectlyDeserializeResponseData =>
                new TheoryData<string, IEnumerable<Vacancy>> {
                    { VacanciesToXml(Single(_vacancies[0])), Single(_vacancies[0]) },
                    { VacanciesToXml(_vacancies), _vacancies }
                };

            [Theory]
            [MemberData(nameof(CorrectlyDeserializeResponseData))]
            public async void CorrectlyDeserializeResponse(string responseContent, IEnumerable<Vacancy> expected) =>            
                (await GetStackoverflowJobBoardClient(
                    GetHttpMessageHandlerMock(
                        expectedResponse: new HttpResponseMessage() { 
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(responseContent)
                        }).Object
                    ).GetVacancies().ToList().ToTask()
                ).Should().BeEquivalentTo(expected);
        }
    }
}