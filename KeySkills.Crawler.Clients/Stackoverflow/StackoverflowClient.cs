using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Linq;
using KeySkills.Crawler.Core.Models;
using KeySkills.Crawler.Core;

namespace KeySkills.Crawler.Clients.Stackoverflow
{
    public partial class StackoverflowClient : BaseJobBoardClient
    {
        public StackoverflowClient(HttpClient http) : base(http) {}

        public override IObservable<Vacancy> GetVacancies() =>
            ExecuteRequest<Response.JobPostCollection>(
                new HttpRequestMessage(HttpMethod.Get, "/jobs/feed"), 
                Deserializer.Xml.Default
            ).ToObservable()
            .SelectMany(list => list.Posts)
            .Select(job => job.GetVacancy());
    }
}