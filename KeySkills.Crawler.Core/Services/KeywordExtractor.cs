using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Services
{
    public class KeywordExtractor : IKeywordsExtractor
    {
        private readonly IEnumerable<(Keyword keyword, Regex regex)> _keywords;
        
        public KeywordExtractor(IEnumerable<Keyword> keywords) =>
            _keywords = keywords.Select(keyword => (
                keyword, 
                new Regex(keyword.Pattern, 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase)
                ));

        public IObservable<Keyword> Extract(Vacancy vacancy) =>
            Observable.Return(String.Concat(vacancy.Title, " ", vacancy.Description))
                .SelectMany(vacancyText =>
                    _keywords.ToObservable()
                        .Where(k => k.regex.IsMatch(vacancyText))
                        .Select(k => k.keyword));
    }
}