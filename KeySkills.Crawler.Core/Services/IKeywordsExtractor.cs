using System;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Services
{
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