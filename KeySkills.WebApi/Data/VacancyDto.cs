using System;
using System.Collections.Generic;
using KeySkills.Core.Models;

namespace KeySkills.WebApi.Data
{
    public class VacancyDto
    {
        public int VacancyId { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public Country? CountryCode { get; set; }
        public IEnumerable<VacancyKeywordDto> Keywords { get; set; }
    }
}