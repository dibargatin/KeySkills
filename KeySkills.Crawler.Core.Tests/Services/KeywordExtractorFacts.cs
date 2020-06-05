using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using FluentAssertions;
using KeySkills.Core.Models;
using KeySkills.Crawler.Core.Services;
using Xunit;

namespace KeySkills.Crawler.Core.Tests
{
    public class KeywordExtractorFacts
    {
        public class Extract_Should
        {
            private static Keyword[] _keywords = new[] {
                new Keyword {
                    Name = "C#",
                    Pattern = @"c#|c\s*sharp"
                },
                new Keyword {
                    Name = ".NET",
                    Pattern = @".net|dot\s*net"
                }
            };

            private static KeywordExtractor _extractor = new KeywordExtractor(_keywords);

            private static T[] Single<T>(T value) => new[] { value };

            public static TheoryData<Vacancy, IEnumerable<Keyword>> ExpectedData =>
                new TheoryData<Vacancy, IEnumerable<Keyword>> {
                    {
                        new Vacancy(),
                        Enumerable.Empty<Keyword>()
                    },
                    {
                        new Vacancy {
                            Title = _keywords[0].Name
                        },
                        Single(_keywords[0])
                    },
                    {
                        new Vacancy {
                            Description = _keywords[0].Name
                        },
                        Single(_keywords[0])
                    },
                    {
                        new Vacancy {
                            Title = _keywords[0].Name,
                            Description = _keywords[1].Name
                        },
                        _keywords
                    },
                };

            [Theory]
            [MemberData(nameof(ExpectedData))]
            public async void ReturnExpectedResult(Vacancy vacancy, IEnumerable<Keyword> expected) => 
                (await _extractor.Extract(vacancy).ToArray().ToTask())
                    .Should().BeEquivalentTo(expected);
        }
    }
}