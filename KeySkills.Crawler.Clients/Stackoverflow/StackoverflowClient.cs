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
        private readonly IRequestFactory _requestFactory;

        public StackoverflowClient(HttpClient http, IRequestFactory requestFactory) : base(http) =>
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));

        public override IObservable<Vacancy> GetVacancies() =>
            ExecuteRequest<Response.JobPostCollection>(
                _requestFactory.CreateRequest(), 
                Deserializer.Xml.Default
            ).ToObservable()
            .SelectMany(list => list.Posts)
            .Select(job => job.GetVacancy());
    }
}