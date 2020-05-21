using System;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Xml.Serialization;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Globalization;
using System.Linq;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core
{
    public class StackoverflowJobBoardClient : BaseJobBoardClient
    {
        public StackoverflowJobBoardClient(HttpClient http) : base(http) {}

        public override IObservable<Vacancy> GetVacancies() =>
            ExecuteRequest<JobPostCollection>(
                new HttpRequestMessage(HttpMethod.Get, "/jobs/feed"), 
                Deserializer.Xml.Default
            ).ToObservable()
            .SelectMany(list => list.Posts)
            .Select(job => job.GetVacancy());

        #region Stackoverflow Response Structure

        [XmlRoot("rss")]
        public class JobPostCollection
        {
            [XmlArray("channel")]
            [XmlArrayItem("item", typeof(JobPost))]
            public JobPost[] Posts { get; set; }
        }
        
        public class JobPost
        {
            [XmlElement("link")]
            public string Link { get; set; }

            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("pubDate")]
            public string PublishedDateString { get; set; }

            [XmlElement("location", Namespace="http://stackoverflow.com/jobs/")]
            public string Location { get; set; }

            public Vacancy GetVacancy() => 
                new Vacancy {
                    Link = Link,
                    Title = Title,
                    Description = Description,
                    PublishedAt = Convert
                        .ToDateTime(PublishedDateString, CultureInfo.GetCultureInfo("en-EN"))
                        .ToUniversalTime(),
                    CountryCode = GetCountryCode()
                };

            private Country? GetCountryCode() =>
                GetRegionName() switch {
                    null => null,
                    "UK" => Country.GB,
                    var name when CountryHelper.IsUsaState(name) => Country.US,
                    var name => CountryHelper.TryGetCountryByName(name).GetValueOrDefault(),
                };
            
            private string GetRegionName() =>                
                Regex.Match(Location ?? String.Empty, @"[\w\s]+,\s(?<region>[\w\s]+)") switch {
                    var match when match.Success => match.Groups["region"].Value,
                    _ => null
                };                    
        }

        #endregion
    }
}