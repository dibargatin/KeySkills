using System;
using System.Collections.Generic;
using System.Linq;
using KeySkills.Core.Models;

namespace KeySkills.Core.Data.SeedData
{
    public class KeywordSeedData
    {
        public class Item
        {
            public Keyword Keyword { get; set; }
            public IEnumerable<string> PositiveTests { get; set; }
            public IEnumerable<string> NegativeTests { get; set; }
        }

        private static string WholeWord(string pattern) =>
            String.Concat(
                @"(^|[\p{C}\p{P}\p{S}\p{Z}]+)(",
                pattern, 
                @")([\p{C}\p{P}\p{S}\p{Z}]+|$)"
            );

        private static string[] Test(string word) => new[] {
            word, word.ToUpper(), word.ToLower(), $" {word}", $"{word} ", $" {word} ", $@"
            {word}", $",{word}", $"{word},", $",{word},", $"\t{word}", $"{word}\t", $"\t{word}\t", 
            $"<{word}", $"{word}>", $"<{word}>"
        };

        private static IEnumerable<string> Collection(params string[][] items) =>
            items.SelectMany(i => i);

        public IEnumerable<Item> Items { get; } = new[] {
            new Item {
                Keyword = new Keyword {
                    KeywordId = 1,
                    Name = ".NET",
                    Pattern = WholeWord(@"\.net")
                },
                PositiveTests = Collection(
                    Test(".Net"), 
                    Test(".NET Framework"), 
                    Test(".Net Core"), 
                    Test(".NeT 4")
                ),
                NegativeTests = new[] {
                    "xnet",".network","asp.net",".netx"
                }
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 2,
                    Name = "ASP.NET",
                    Pattern = WholeWord(@"asp\p{Z}*\.\p{Z}*net")
                },
                PositiveTests = Collection(
                    Test("ASP.Net"), 
                    Test("asp .NET"),
                    Test("Asp. net"),
                    Test("Asp.Net Core"),
                    Test("Asp.Net 5")
                ),
                NegativeTests = new[] {
                    "xasp.net","asp.network","asp. network","xasp.network"
                }
            }
        };
    }
}