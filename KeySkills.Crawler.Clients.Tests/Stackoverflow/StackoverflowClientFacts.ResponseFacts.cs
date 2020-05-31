using System;
using Xunit;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Crawler.Core.Models;
using System.Reactive.Linq;
using static KeySkills.Crawler.Clients.Stackoverflow.StackoverflowClient;

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class StackoverflowClientFacts
    {
        public class ResponseFacts
        {
            public class JobPostFacts
            {
                public class GetVacancy_Should
                {                
                    private static readonly string _correctPublishedDateString = "Sun, 1 Mar 2020 00:00:00 Z";

                    private Response.JobPost GetJobPost(string location, string publishedDateString) =>
                        new Response.JobPost { 
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
                        new Response.JobPost { 
                            Link = link,
                            PublishedDateString = _correctPublishedDateString
                        }.GetVacancy().Link.Should().Be(link);

                    [Theory]
                    [MemberData(nameof(LoremIpsum))]
                    public void ReturnTitle(string title) =>
                        new Response.JobPost { 
                            Title = title,
                            PublishedDateString = _correctPublishedDateString
                        }.GetVacancy().Title.Should().Be(title);

                    [Theory]
                    [MemberData(nameof(LoremIpsum))]
                    public void ReturnDescription(string description) =>
                        new Response.JobPost { 
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
        }
    }
}