namespace KeySkills.Core.Models
{
    /// <summary>
    /// Represents keyword
    /// </summary>
    public class Keyword
    {
        /// <summary>
        /// Gets or sets the keyword display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the regex pattern to match the keyword
        /// </summary>
        public string Pattern { get; set; }
    }
}