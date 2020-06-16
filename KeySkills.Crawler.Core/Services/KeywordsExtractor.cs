using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using KeySkills.Core.Models;

namespace KeySkills.Crawler.Core.Services
{
    /// <summary>
    /// Implements <see cref="IKeywordsExtractor"/>
    /// </summary>
    public class KeywordsExtractor : IKeywordsExtractor
    {
        private readonly IEnumerable<(Keyword keyword, Regex regex)> _keywords;
        
        /// <summary>
        /// Initializes instance of keyword extractor
        /// </summary>
        /// <param name="keywords">Collection of the keywords to be extracted</param>
        public KeywordsExtractor(IEnumerable<Keyword> keywords) =>
            _keywords = keywords.Select(keyword => (
                keyword, 
                new Regex(keyword.Pattern, 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase)
                ));

        /// <inheritdoc/>
        public IObservable<Keyword> Extract(Vacancy vacancy) =>
            Observable.Return(String.Concat(vacancy.Title, " ", vacancy.Description))
                .SelectMany(vacancyText =>
                    _keywords.ToObservable()
                        .Where(k => k.regex.IsMatch(vacancyText))
                        .Select(k => k.keyword));
    }
}