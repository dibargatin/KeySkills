namespace KeySkills.Crawler.Core.Models
{
    /// <summary>
    /// Represents keyword
    /// </summary>
    public class Keyword
    {
        /// <summary>
        /// Keyword display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Regex pattern to match the keyword
        /// </summary>
        public string Pattern { get; set; }
    }
}