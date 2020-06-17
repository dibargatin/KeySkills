using System;
using System.Net.Http;
using System.Text;

namespace KeySkills.Crawler.Clients.HeadHunter
{
    public partial class HeadHunterClient
    {
        /// <summary>
        /// Defines request factory interface for HeadHunter API
        /// </summary>
        public interface IRequestFactory
        {
            /// <summary>
            /// Creates request to the root endpoint
            /// </summary>
            /// <param name="page">Number of the requested page</param>
            /// <returns>HTTP request to the root endpoint</returns>
            HttpRequestMessage CreateRootRequest(int page);

            /// <summary>
            /// Creates request to get job details
            /// </summary>
            /// <param name="url">URL of the job post</param>
            /// <returns>HTTP request to get job details</returns>
            HttpRequestMessage CreateJobDetailsRequest(string url);

            /// <summary>
            /// Creates request to get information about job location
            /// </summary>
            /// <param name="id">Area Id</param>
            /// <returns>HTTP request to the areas endpoint</returns>
            HttpRequestMessage CreateAreaRequest(string id);
        }

        /// <summary>
        /// Implements <see cref="IRequestFactory"/>
        /// </summary>
        public class RequestFactory : IRequestFactory
        {
            /// <summary>
            /// Represents parameters for the root endpoint
            /// </summary>
            public class RootRequestParams
            {
                /// <summary>
                /// Gets or sets root endpoint relative URL
                /// </summary>
                public string Url { get; set; }

                /// <summary>
                /// Gets or sets text to search for OR query
                /// See query syntax on https://hh.ru/article/1175
                /// </summary>
                public string Text { get; set; }

                /// <summary>
                /// Gets or sets industry to search
                /// See possible values on https://api.hh.ru/industries
                /// </summary>
                public string Industry { get; set; }  

                /// <summary>
                /// Known API limit
                /// </summary>
                public static int MaxItemsPerPage => 100;

                private string GetParam(string name, string value, bool isLast = false) =>
                    value != null ? 
                        String.Concat(name, "=", Uri.EscapeDataString(value), (isLast ? String.Empty : "&")) :
                        String.Empty;

                /// <summary>
                /// Prepares query string
                /// </summary>
                /// <param name="page">Number of the requested page</param>
                /// <returns>Query string</returns>
                public string GetQueryString(int page) =>
                    new StringBuilder()
                        .Append(Url == null ? String.Empty : Uri.EscapeUriString(Url)).Append("?")
                        .Append(GetParam("text", Text))
                        .Append(GetParam("industry", Industry))
                        .Append(GetParam("per_page", MaxItemsPerPage.ToString()))
                        .Append(GetParam("page", page.ToString(), isLast: true))
                        .ToString();
            }

            /// <summary>
            /// Absolute WebAPI endpoint URI
            /// </summary>
            public Uri BaseUri { get; set; }

            /// <summary>
            /// Root endpoint parameters
            /// </summary>
            public RootRequestParams RootParams { get; set; }

            /// <summary>
            /// Area endpoint relative URL
            /// </summary>
            public string AreaUrl { get; set; }

            private HttpRequestMessage CreateRequest(Uri uri)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                request.Headers.Add("User-Agent", ".NET");
                request.Headers.Host = uri.Host;

                return request;
            }

            /// <inheritdoc/>
            public HttpRequestMessage CreateRootRequest(int page) =>
                CreateRequest(new Uri(BaseUri, RootParams.GetQueryString(page)));

            /// <inheritdoc/>
            public HttpRequestMessage CreateJobDetailsRequest(string url) =>
                CreateRequest(new Uri(url));

            /// <inheritdoc/>
            public HttpRequestMessage CreateAreaRequest(string id) =>
                CreateRequest( 
                    new Uri(BaseUri, AreaUrl != null ?
                        String.Concat(Uri.EscapeUriString(AreaUrl), "/", Uri.EscapeDataString(id ?? String.Empty)) :
                        Uri.EscapeDataString(id ?? String.Empty)));
        }
    }
}