namespace KeySkills.Core.Models
{
    /// <summary>
    /// Represents many-to-many relationship between <see cref="Vacancy"/> and <see cref="Keyword"/>
    /// </summary>
    public class VacancyKeyword
    {
        /// <summary>
        /// Gets or sets vacancy id
        /// </summary>
        public int VacancyId { get; set; }

        /// <summary>
        /// Gets or sets keyword id
        /// </summary>
        public int KeywordId { get; set; }
        
        /// <summary>
        /// Gets or sets instance of the vacancy
        /// </summary>
        public Vacancy Vacancy { get; set; }

        /// <summary>
        /// Gets or sets instance of the keyword
        /// </summary>
        public Keyword Keyword { get; set; }
    }
}