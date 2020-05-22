using System;
using System.Net.Http;
using System.Text;

namespace KeySkills.Crawler.Core
{
    public partial class HeadHunterJobBoardClient
    {
        public interface IRequestFactory
        {
            HttpRequestMessage CreateRootRequest(int page);
            HttpRequestMessage CreateJobDetailsRequest(string url);
            HttpRequestMessage CreateAreaRequest(string id);
        }

        public class RequestFactory : IRequestFactory
        {
            public class RootRequestParams
            {
                /// <summary>
                /// Root endpoint relative URL
                /// </summary>
                public string Url { get; set; }

                /// <summary>
                /// Text to search for OR query
                /// See query syntax on https://hh.ru/article/1175
                /// </summary>
                public string Text { get; set; }

                /// <summary>
                /// Industry to search
                /// See possible values on https://api.hh.ru/industries
                /// </summary>
                public string Industry { get; set; }  

                /// <summary>
                /// Known API limitation
                /// </summary>
                public static int MaxItemsPerPage = 100;

                private string GetParam(string name, string value, bool isLast = false) =>
                    value != null ? 
                        String.Concat(name, "=", Uri.EscapeDataString(value), (isLast ? String.Empty : "&")) :
                        String.Empty;

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

            public HttpRequestMessage CreateRootRequest(int page) =>
                new HttpRequestMessage(HttpMethod.Get, 
                    new Uri(BaseUri, RootParams.GetQueryString(page)));

            public HttpRequestMessage CreateJobDetailsRequest(string url) =>
                new HttpRequestMessage(HttpMethod.Get, url);

            public HttpRequestMessage CreateAreaRequest(string id) =>
                new HttpRequestMessage(HttpMethod.Get, 
                    new Uri(BaseUri, AreaUrl != null ?
                        String.Concat(Uri.EscapeUriString(AreaUrl), "/", Uri.EscapeDataString(id ?? String.Empty)) :
                        Uri.EscapeDataString(id ?? String.Empty)));
        }
    }
}