using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeySkills.Core.Repositories;
using KeySkills.Crawler.App.Services;
using KeySkills.Crawler.Core.Clients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeySkills.Crawler.App
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory<IJobBoardClient> _clientsScopeFactory;
        private readonly IServiceScopeFactory<IVacancyRepository> _repositoryScopeFactory;

        public Worker(
            ILogger<Worker> logger, 
            IServiceScopeFactory<IJobBoardClient> clientsScopeFactory,
            IServiceScopeFactory<IVacancyRepository> repositoryScopeFactory)
        {
            _logger = logger;
            _clientsScopeFactory = clientsScopeFactory;
            _repositoryScopeFactory = repositoryScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var clientScope = _clientsScopeFactory.CreateScope();
                var clients = clientScope.GetServices();

                using var repositoryScope = _repositoryScopeFactory.CreateScope();
                var repository = repositoryScope.GetRequiredService();

                clients.ToObservable()
                    .SelectMany(client => client.GetVacancies())
                    .Buffer(TimeSpan.FromSeconds(30), 50)
                    .Subscribe(
                        onNext: async vacancies => {
                            _logger.LogInformation("Saving {count} new vacancies", vacancies.Count());                            
                            try
                            {
                                await repository.AddRangeAsync(vacancies);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error while saving vacancies");
                            }
                        },
                        onError: exception => _logger.LogError(exception, "Error while getting vacancies"),
                        onCompleted: () => _logger.LogInformation("Session completed")
                    );
                
                await Task.Delay(60 * 60 * 1000, stoppingToken);
            }
        }
    }
}
