using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core
{
    public partial class HeadHunterJobBoardClient : BaseJobBoardClient
    {
        private readonly IRequestFactory _requestFactory;

        public HeadHunterJobBoardClient(HttpClient http, IRequestFactory requestFactory) : base(http) =>
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
        
        public override IObservable<Vacancy> GetVacancies()
        {
            throw new NotImplementedException();
        }
        
        #region HeadHunter WebAPI Response

        public class Root
        {
            [JsonPropertyName("items")]
            public Item[] Items { get; set; }
            
            [JsonPropertyName("pages")]
            public int PagesCount { get; set; }
            
            [JsonPropertyName("page")]
            public int CurrentPage { get; set; }            
        }

        public class Item
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }

        public class JobPost
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("area")]
            public Area Area { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("published_at")]
            public DateTime PublishedAt { get; set; }
        }

        public class Area
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public class AreaInfo
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("parent_id")]
            public string ParentId { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }            
        }

        #endregion
    }
}