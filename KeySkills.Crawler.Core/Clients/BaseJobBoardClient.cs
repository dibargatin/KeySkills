using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KeySkills.Crawler.Core.Models;
using KeySkills.Crawler.Core.Services;

namespace KeySkills.Crawler.Core.Clients
{
    /// <summary>
    /// Base implementation of a vacancies provider interface
    /// <see cref="IJobBoardClient"/>
    /// </summary>
    public abstract class BaseJobBoardClient : IJobBoardClient
    {
        /// <summary>
        /// HttpClient to make API requests
        /// </summary>
        protected readonly HttpClient _http;

        /// <summary>
        /// Predicate for filtering already downloaded vacancies by URL
        /// </summary>
        protected readonly Func<string, bool> _isVacancyExisted;

        /// <summary>
        /// IKeywordsExtractor to extract keywords from vacancies
        /// </summary>
        protected readonly IKeywordsExtractor _keywordsExtractor;

        /// <summary>
        /// Initializes protected fields
        /// </summary>
        /// <param name="http">Instance of HttpClient to make API requests</param>
        /// <param name="isVacancyExisted">Predicate for filtering already downloaded vacancies by URL</param>
        /// <param name="keywordsExtractor">Instance of IKeywordsExtractor to extract keywords from vacancies</param>
        /// <exception cref="ArgumentNullException"><paramref name="http"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="isVacancyExisted"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="keywordsExtractor"/> is <see langword="null" /></exception>
        public BaseJobBoardClient(HttpClient http, Func<string, bool> isVacancyExisted, IKeywordsExtractor keywordsExtractor)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _isVacancyExisted = isVacancyExisted ?? throw new ArgumentNullException(nameof(isVacancyExisted));
            _keywordsExtractor = keywordsExtractor ?? throw new ArgumentNullException(nameof(keywordsExtractor));
        }
        
        /// <inheritdoc/>
        public abstract IObservable<Vacancy> GetVacancies();

        /// <summary>
        /// Executes HTTP request to job board API
        /// </summary>
        /// <param name="request">Request to execute</param>
        /// <param name="deserializer">Deserializer to extract expected structure from response</param>
        /// <typeparam name="T">Type of expected response structure</typeparam>
        /// <returns>Deserialized response</returns>
        protected async Task<T> ExecuteRequest<T>(HttpRequestMessage request, Deserializer deserializer)
        {
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            return await deserializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
        }
        
        /// <summary>
        /// Implements default deserializers
        /// </summary>
        protected abstract class Deserializer
        {
            /// <summary>
            /// Deserializes supplied stream to an object of the expected type <typeparamref name="T"/>
            /// </summary>
            /// <param name="stream">Stream to deserialize</param>
            /// <typeparam name="T">Type of required object</typeparam>
            /// <returns>Deserialized object of type <typeparamref name="T"/></returns>
            public abstract Task<T> Deserialize<T>(Stream stream);

            /// <summary>
            /// Implements default XML deserializer
            /// </summary>
            public sealed class Xml : Deserializer
            {
                private static readonly Lazy<Xml> _instance = new Lazy<Xml>(() => new Xml());

                /// <summary>
                /// Singleton default instance of XML deserializer
                /// </summary>
                public static Xml Default => _instance.Value;

                /// <inheritdoc/>
                public override Task<T> Deserialize<T>(Stream stream) =>
                    Task.Run(() => (T)(new XmlSerializer(typeof(T)).Deserialize(stream)));
            }

            /// <summary>
            /// Implements default JSON deserializer
            /// </summary>
            public sealed class Json : Deserializer
            {
                private static readonly Lazy<Json> _instance = new Lazy<Json>(() => new Json());

                /// <summary>
                /// Singleton default instance of JSON deserializer
                /// </summary>
                public static Json Default => _instance.Value;

                /// <inheritdoc/>
                public override Task<T> Deserialize<T>(Stream stream) =>
                    JsonSerializer.DeserializeAsync<T>(stream).AsTask();
            }
        }
    }
}