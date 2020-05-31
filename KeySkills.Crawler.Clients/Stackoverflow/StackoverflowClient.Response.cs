using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Globalization;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Clients.Stackoverflow
{
    public partial class StackoverflowClient
    {
        public class Response
        {
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
        }
    }
}