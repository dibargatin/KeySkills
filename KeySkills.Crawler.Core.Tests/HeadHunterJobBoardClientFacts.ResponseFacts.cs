using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Crawler.Core.Models;
using Xunit;
using static KeySkills.Crawler.Core.HeadHunterJobBoardClient.Response;

namespace KeySkills.Crawler.Core.Tests
{
    public partial class HeadHunterJobBoardClientFacts
    {
        protected static AreaInfo[] _areaInfo = new[] {
            new AreaInfo {
                Id = "0",
                Name = "Россия"
            },
            new AreaInfo {
                Id = "1",
                ParentId = "0",
                Name = "Самарская область"
            },
            new AreaInfo {
                Id = "2",
                ParentId = "1",
                Name = "Самара"
            },
            new AreaInfo {
                Id = "3",
                Name = "Unknown"
            }
        };

        public class ResponseFacts : HeadHunterJobBoardClientFacts
        {
            protected static Func<string, Task<AreaInfo>> _getAreaInfoFunc = 
                (areaId) => Task.Run(() => _areaInfo[Convert.ToInt32(areaId)]);

            public static TheoryData<Area, Country?> AreaTheoryData =>
                new TheoryData<Area, Country?> {
                    { new Area { Id = _areaInfo[0].Id }, Country.RU },
                    { new Area { Id = _areaInfo[1].Id }, Country.RU },
                    { new Area { Id = _areaInfo[2].Id }, Country.RU },
                    { new Area { Id = _areaInfo[3].Id }, null },
                };
                
            public class AreaFacts : ResponseFacts
            {
                public class GetCountry_Should : AreaFacts
                {
                    [Theory]
                    [MemberData(nameof(AreaTheoryData))]
                    public async void ReturnExpectedResult(Area area, Country? expected) => 
                        (await area.GetCountry(_getAreaInfoFunc))
                            .Should().Be(expected);
                }
            }

            public class JobPostFacts : ResponseFacts
            {
                public class GetVacancy_Should : JobPostFacts
                {
                    public static TheoryData<string> LoremIpsum =>
                        new TheoryData<string> {
                            { null },
                            { String.Empty },
                            { "Lorem ipsum" }
                        };

                    [Theory]
                    [MemberData(nameof(LoremIpsum))]
                    public async void ReturnLink(string link) =>
                        (await (new JobPost { AlternateUrl = link }).GetVacancy(_getAreaInfoFunc))
                            .Link.Should().Be(link);

                    [Theory]
                    [MemberData(nameof(LoremIpsum))]
                    public async void ReturnTitle(string title) =>
                        (await (new JobPost { Name = title }).GetVacancy(_getAreaInfoFunc))
                            .Title.Should().Be(title);

                    [Theory]
                    [MemberData(nameof(LoremIpsum))]
                    public async void ReturnDescription(string description) =>
                        (await (new JobPost { Description = description }).GetVacancy(_getAreaInfoFunc))
                            .Description.Should().Be(description);

                    [Fact]
                    public async void ReturnPublishedAt() 
                    {
                        var expected = 1.March(2020).At(0, 0, 0).AsUtc();
                        var job = new JobPost { PublishedAt = expected };
                        var vacancy = await job.GetVacancy(_getAreaInfoFunc);
                        
                        vacancy.PublishedAt.Should().Be(expected);
                    }

                    [Theory]
                    [MemberData(nameof(AreaTheoryData))]
                    public async void ReturnCountryCode(Area area, Country? expected) =>
                        (await (new JobPost { Area = area }).GetVacancy(_getAreaInfoFunc))
                            .CountryCode.Should().Be(expected);
                }
            }
        }
    }
}