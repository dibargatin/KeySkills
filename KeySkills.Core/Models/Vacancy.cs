using System;
using System.Collections.Generic;

namespace KeySkills.Core.Models
{
    /// <summary>
    /// Represents a vacancy
    /// </summary>
    public class Vacancy
    {
        /// <summary>
        /// Gets or sets vacancy id
        /// </summary>
        public int VacancyId { get; set; }

        /// <summary>
        /// Gets or sets the vacancy webpage URL
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the vacancy title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the vacancy description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the vacancy publication date and time
        /// </summary>
        public DateTime PublishedAt { get; set; }

        /// <summary>
        /// Gets or sets the vacancy location
        /// </summary>
        /// <value></value>
        public Country? CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the vacancy keywords
        /// </summary>
        public IEnumerable<VacancyKeyword> Keywords { get; set; }
    }
}