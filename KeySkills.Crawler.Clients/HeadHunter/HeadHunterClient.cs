using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using KeySkills.Crawler.Core;
using KeySkills.Crawler.Clients.Helpers;
using KeySkills.Crawler.Core.Models;
using static KeySkills.Crawler.Clients.HeadHunter.HeadHunterClient.Response;
using KeySkills.Crawler.Core.Services;

namespace KeySkills.Crawler.Clients.HeadHunter
{
    /// <summary>
    /// Implements HeadHunter job board client
    /// </summary>
    public partial class HeadHunterClient : BaseJobBoardClient
    {
        /// <summary>
        /// Factory to create HTTP requests to HeadHunter API
        /// </summary>
        private readonly IRequestFactory _requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadHunterClient"/> class
        /// </summary>
        /// <param name="http">Instance of HttpClient to make API requests</param>
        /// <param name="requestFactory">Instance of IRequestFactory to get required requests</param>
        /// <param name="isVacancyExisted">Predicate for filtering already downloaded vacancies by URL</param>
        /// <param name="keywordsExtractor">Instance of IKeywordsExtractor to extract keywords from vacancies</param>
        /// <exception cref="ArgumentNullException"><paramref name="http"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="requestFactory"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="isVacancyExisted"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentNullException"><paramref name="keywordsExtractor"/> is <see langword="null" /></exception>
        public HeadHunterClient(
            HttpClient http, 
            IRequestFactory requestFactory, 
            Func<string, bool> isVacancyExisted,
            IKeywordsExtractor keywordsExtractor
        ) : base(http, isVacancyExisted, keywordsExtractor) =>
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
        
        /// <inheritdoc/>
        public override IObservable<Vacancy> GetVacancies() =>
            GetItems()
                .Where(item => !_isVacancyExisted(item.AlternateUrl))
                .SelectMany(item => Observable.FromAsync(async () => await GetJobDetails(item.Url)))
                .SelectMany(job => Observable.FromAsync(async () => await job.GetVacancy(GetAreaInfo)))
                .Select(vacancy => {
                    vacancy.Keywords = Observable.ToEnumerable(
                        _keywordsExtractor.Extract(vacancy)
                    );
                    return vacancy;
                });

        private Task<Root> GetNextRoot(int page) =>
            ExecuteRequest<Root>(
                _requestFactory.CreateRootRequest(page),
                Deserializer.Json.Default
            );
        
        private IObservable<Item> GetItems() => 
            ObservableHelper.GenerateFromAsync(
                () => GetNextRoot(1),
                prev => prev.CurrentPage < prev.PagesCount,
                prev => GetNextRoot(prev.CurrentPage + 1),
                result => result
            ).SelectMany(root => root.Items);

        private Task<JobPost> GetJobDetails(string url) =>
            ExecuteRequest<JobPost>(
                _requestFactory.CreateJobDetailsRequest(url),
                Deserializer.Json.Default
            );

        private Task<AreaInfo> GetAreaInfo(string id) =>
            ExecuteRequest<AreaInfo>(
                _requestFactory.CreateAreaRequest(id), 
                Deserializer.Json.Default
            );
    }
}