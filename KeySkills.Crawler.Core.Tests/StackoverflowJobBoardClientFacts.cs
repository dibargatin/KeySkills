using System;
using Xunit;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Tests
{
    public class StackoverflowJobBoardClientFacts
    {
        public class JobPostFacts
        {
            public class GetVacancy_Should
            {                
                private static readonly string _correctPublishedDateString = "Sun, 1 Mar 2020 00:00:00 Z";

                private StackoverflowJobBoardClient.JobPost GetJobPost(string title, string publishedDateString) =>
                    new StackoverflowJobBoardClient.JobPost { 
                        Title = title,
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
                    GetJobPost(title, _correctPublishedDateString).GetVacancy()
                        .Title.Should().Be(title);

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
                [InlineData("PostgreSQL DBA at Starling Bank (London, UK)", Country.GB)]
                [InlineData("Machine Learning Engineer at JP Morgan Chase (Jersey City, NJ)", Country.US)]
                [InlineData("Softwareentwickler Java (m/w/d) (Dresden, Deutschland)", Country.DE)]
                [InlineData("Senior Software Engineer- Media Platform at Joyn (München, Germany)", Country.DE)]
                [InlineData("Software Engineer Full Stack Java (Ecublens VD, Schweiz)(allows remote)", Country.CH)]
                [InlineData("Senior Software Developer/Software Developer (Mariehamn, Åland Islands)", Country.AX)]
                public void ExtractCountryFromTitleWithRegionInfo(string title, Country expected) =>
                    GetJobPost(title, _correctPublishedDateString).GetVacancy().CountryCode
                        .Should().Be(expected, $"because of {title}");
                
                [Theory]
                [MemberData(nameof(LoremIpsum))]
                public void ReturnNullForCountryWhenTitleWithoutRegionInfo(string title) => 
                    GetJobPost(title, _correctPublishedDateString).GetVacancy().CountryCode
                        .Should().BeNull($"because {title} doesn't contain region info");
            }
        }
    }
}