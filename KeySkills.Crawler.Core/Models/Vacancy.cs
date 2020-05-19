using System;

namespace KeySkills.Crawler.Core.Models
{
    public class Vacancy
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public Country? CountryCode { get; set; }        
    }
}