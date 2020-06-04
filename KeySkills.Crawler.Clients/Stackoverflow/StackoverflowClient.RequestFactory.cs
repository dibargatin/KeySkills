using System;
using System.Net.Http;

namespace KeySkills.Crawler.Clients.Stackoverflow
{
    public partial class StackoverflowClient
    {
        /// <summary>
        /// Defines request factory interface for Stackoverflow API
        /// </summary>
        public interface IRequestFactory
        {
            /// <summary>
            /// Creates request to the root endpoint
            /// </summary>
            /// <returns>HTTP request to the root endpoint</returns>
            HttpRequestMessage CreateRequest();
        }
        
        /// <summary>
        /// Implements <see cref="IRequestFactory"/>
        /// </summary>
        public class RequestFactory : IRequestFactory
        {
            /// <summary>
            /// Absolute WebAPI endpoint URI
            /// </summary>
            public Uri EndpointUri { get; set; }

            /// <inheritdoc/>
            public HttpRequestMessage CreateRequest() =>
                new HttpRequestMessage(HttpMethod.Get, EndpointUri);
        }
    }
}