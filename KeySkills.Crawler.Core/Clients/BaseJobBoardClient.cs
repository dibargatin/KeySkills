using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core
{
    public abstract class BaseJobBoardClient : IJobBoardClient
    {
        protected readonly HttpClient _http;

        protected readonly Func<string, bool> _isVacancyExisted;

        public BaseJobBoardClient(HttpClient http, Func<string, bool> isVacancyExisted)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _isVacancyExisted = isVacancyExisted ?? throw new ArgumentNullException(nameof(isVacancyExisted));
        }
        
        public abstract IObservable<Vacancy> GetVacancies();

        protected async Task<T> ExecuteRequest<T>(HttpRequestMessage request, Deserializer deserializer)
        {
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            return await deserializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
        }
        
        protected abstract class Deserializer
        {
            public abstract Task<T> Deserialize<T>(Stream stream);

            public sealed class Xml : Deserializer
            {
                private static readonly Lazy<Xml> _instance = new Lazy<Xml>(() => new Xml());

                public static Xml Default => _instance.Value;

                public override Task<T> Deserialize<T>(Stream stream) =>
                    Task.Run(() => (T)(new XmlSerializer(typeof(T)).Deserialize(stream)));
            }

            public sealed class Json : Deserializer
            {
                private static readonly Lazy<Json> _instance = new Lazy<Json>(() => new Json());

                public static Json Default => _instance.Value;

                public override Task<T> Deserialize<T>(Stream stream) =>
                    JsonSerializer.DeserializeAsync<T>(stream).AsTask();
            }
        }
    }
}