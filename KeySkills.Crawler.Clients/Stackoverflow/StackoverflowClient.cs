using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Linq;
using KeySkills.Crawler.Core.Models;
using KeySkills.Crawler.Core;
using KeySkills.Crawler.Core.Services;

namespace KeySkills.Crawler.Clients.Stackoverflow
{
    /// <summary>
    /// Implements Stackoverflow job board client
    /// </summary>
    public partial class StackoverflowClient : BaseJobBoardClient
    {
        /// <summary>
        /// Factory to create HTTP requests to Stackoverflow API
        /// </summary>
        private readonly IRequestFactory _requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackoverflowClient"/> class
        /// </summary>
        /// <param name="http">Instance of HttpClient to make API requests</param>
        /// <param name="requestFactory">Instance of IRequestFactory to get required requests</param>
        /// <param name="isVacancyExisted">Predicate for filtering already downloaded vacancies by URL</param>
        /// <param name="keywordsExtractor">Instance of IKeywordsExtractor to extract keywords from vacancies</param>
        /// <exception cref="ArgumentNullException"><paramref name="http"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="requestFactory"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="isVacancyExisted"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="keywordsExtractor"/> is <see langword="null" /></exception>
        public StackoverflowClient(
            HttpClient http, 
            IRequestFactory requestFactory,
            Func<string, bool> isVacancyExisted,
            IKeywordsExtractor keywordsExtractor
        ) : base(http, isVacancyExisted, keywordsExtractor) =>
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));

        /// <inheritdoc/>
        public override IObservable<Vacancy> GetVacancies() =>
            ExecuteRequest<Response.JobPostCollection>(
                _requestFactory.CreateRequest(), 
                Deserializer.Xml.Default
            ).ToObservable()
            .SelectMany(list => list.Posts)
            .Select(job => job.GetVacancy())
            .Where(vacancy => !_isVacancyExisted(vacancy.Link))
            .Select(vacancy => {
                vacancy.Keywords = Observable.ToEnumerable(
                    _keywordsExtractor.Extract(vacancy)
                );
                return vacancy;
            });
    }
}