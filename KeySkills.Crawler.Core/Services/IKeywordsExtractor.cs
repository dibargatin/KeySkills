using System;
using KeySkills.Core.Models;

namespace KeySkills.Crawler.Core.Services
{
    /// <summary>
    /// Defines keywords extractor interface
    /// </summary>
    public interface IKeywordsExtractor
    {
        /// <summary>
        /// Extracts keywords from a vacancy
        /// </summary>
        /// <param name="vacancy">Vacancy for keywords extraction</param>
        /// <returns>Observable sequence of extracted keywords</returns>
        IObservable<Keyword> Extract(Vacancy vacancy);
    }
}