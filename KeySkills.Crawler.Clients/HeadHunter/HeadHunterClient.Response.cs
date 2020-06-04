using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Clients.HeadHunter
{
    public partial class HeadHunterClient
    {
        /// <summary>
        /// Contains HeadHunter API response structures
        /// </summary>
        public class Response
        {
            /// <summary>
            /// Represents response of the root endpoint
            /// </summary>
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
                /// <summary>
                /// WebAPI item URL
                /// </summary>
                [JsonPropertyName("url")]
                public string Url { get; set; }

                /// <summary>
                /// Website item URL
                /// </summary>
                [JsonPropertyName("alternate_url")]
                public string AlternateUrl { get; set; }
            }

            /// <summary>
            /// Represents job post details
            /// </summary>
            public class JobPost
            {
                /// <summary>
                /// Job post id
                /// </summary>
                [JsonPropertyName("id")]
                public string Id { get; set; }

                /// <summary>
                /// Job post title
                /// </summary>
                [JsonPropertyName("name")]
                public string Name { get; set; }

                /// <summary>
                /// Region info
                /// </summary>
                [JsonPropertyName("area")]
                public Area Area { get; set; }

                [JsonPropertyName("description")]
                public string Description { get; set; }

                [JsonPropertyName("published_at")]
                public DateTime PublishedAt { get; set; }

                /// <summary>
                /// Website item URL
                /// </summary>
                [JsonPropertyName("alternate_url")]
                public string AlternateUrl { get; set; }

                /// <summary>
                /// Converts job post details to the <see cref="Vacancy"/>
                /// </summary>
                /// <param name="getAreaInfoFunc">Function to get area information</param>
                /// <returns>New instance of <see cref="Vacancy"/></returns>
                public async Task<Vacancy> GetVacancy(Func<string, Task<AreaInfo>> getAreaInfoFunc) =>
                    new Vacancy {
                        Link = AlternateUrl,
                        Title = Name,
                        Description = Description,
                        PublishedAt = PublishedAt,
                        CountryCode = Area != null ? 
                            await Area.GetCountry(getAreaInfoFunc) : 
                            null
                    };
            }

            public class Area
            {
                [JsonPropertyName("id")]
                public string Id { get; set; }

                /// <summary>
                /// Returns <see cref="Country"/> which is related to the area
                /// </summary>
                /// <param name="getAreaInfoFunc">Function to get area information</param>
                /// <returns><see cref="Country"/> or <see langword="null"/></returns>
                public async Task<Country?> GetCountry(Func<string, Task<AreaInfo>> getAreaInfoFunc) 
                {
                    async Task<Country?> GetCountryByAreaId(string id) =>
                        await getAreaInfoFunc(id) switch {
                            { ParentId: null, Name: var name } => CountryHelper.TryGetCountryByName(name),
                            { ParentId: var parentId } => await GetCountryByAreaId(parentId)
                        };

                    return await GetCountryByAreaId(this.Id);
                }
            }

            /// <summary>
            /// Represents response of the areas endpoint
            /// </summary>
            public class AreaInfo
            {
                [JsonPropertyName("id")]
                public string Id { get; set; }

                [JsonPropertyName("parent_id")]
                public string ParentId { get; set; }

                [JsonPropertyName("name")]
                public string Name { get; set; }            
            }
        }
    }
}