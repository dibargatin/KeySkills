using System;
using System.Net.Http;

namespace KeySkills.Crawler.Clients.Stackoverflow
{
    public partial class StackoverflowClient
    {
        public interface IRequestFactory
        {
            HttpRequestMessage CreateRequest();
        }
        
        public class RequestFactory : IRequestFactory
        {
            /// <summary>
            /// Absolute WebAPI endpoint URI
            /// </summary>
            public Uri EndpointUri { get; set; }

            public HttpRequestMessage CreateRequest() =>
                new HttpRequestMessage(HttpMethod.Get, EndpointUri);
        }
    }
}