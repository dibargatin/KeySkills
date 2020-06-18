using System;
using System.Net.Http;
using System.Threading.Tasks;
using KeySkills.Core.Data;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Data.Sqlite;
using KeySkills.Core.Repositories;
using KeySkills.Crawler.App.Services;
using KeySkills.Crawler.Clients.HeadHunter;
using KeySkills.Crawler.Clients.Stackoverflow;
using KeySkills.Crawler.Core.Clients;
using KeySkills.Crawler.Core.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Polly;
using Polly.Extensions.Http;

namespace KeySkills.Crawler.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();

            try
            {
                logger.Debug("Initializing program");

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>())
                    {
                        context.Database.Migrate();
                    }
                }
                
                logger.Debug("Running the program");

                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unhandled exception occurred");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IServiceScopeFactory<IVacancyRepository>, ServiceScopeFactory<IVacancyRepository>>();

                    services.AddTransient<IKeywordRepository, KeywordRepository>();
                    services.AddTransient<IVacancyRepository, VacancyRepository>();
                    services.AddTransient<BaseDbContext, SqliteDbContext>(provider =>
                        new SqliteDbContext(
                            new DbContextOptionsBuilder<SqliteDbContext>()
                                .UseSqlite(new SqliteConnection($"Filename=KeySkills.db"))
                                .Options
                            )
                    );
                    services.AddScoped<Func<string, Task<bool>>>(provider =>
                        vacancyUrl => provider.GetRequiredService<IVacancyRepository>()
                            .AnyAsync(v => v.Link == vacancyUrl)
                    );

                    services.AddSingleton<IServiceScopeFactory<IJobBoardClient>, ServiceScopeFactory<IJobBoardClient>>();
                    services.AddScoped<IKeywordsExtractor, KeywordsExtractor>(provider => 
                        new KeywordsExtractor(
                            provider.GetRequiredService<IKeywordRepository>()
                                .GetAllAsync().GetAwaiter().GetResult()
                        )
                    );
                    
                    AddStackoverflowClient(services);
                    AddHeadHunterClient(services);

                    services.AddHostedService<Worker>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();

        private static void AddStackoverflowClient(IServiceCollection services)
        {
            services.AddHttpClient<IJobBoardClient, StackoverflowClient>()
                .AddPolicyHandler(GetCircuitBreakerPolicy())                
                .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<StackoverflowClient.IRequestFactory>(provider =>
                new StackoverflowClient.RequestFactory {
                    EndpointUri = new Uri("https://stackoverflow.com/jobs/feed")
                }
            );
        }

        private static void AddHeadHunterClient(IServiceCollection services)
        {
            services.AddHttpClient<IJobBoardClient, HeadHunterClient>()
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetRetryForTooManyRequestsPolicy())
                .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<HeadHunterClient.IRequestFactory>(provider =>
                new HeadHunterClient.RequestFactory {
                    BaseUri = new Uri("https://api.hh.ru"),
                    AreaUrl = "/areas",
                    RootParams = new HeadHunterClient.RequestFactory.RootRequestParams {
                        Url = "/vacancies",
                        Industry = "7",
                        Text = "software OR engineer OR developer OR разработчик OR программист"
                    }
                }
            );
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(6, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        private static IAsyncPolicy<HttpResponseMessage> GetRetryForTooManyRequestsPolicy() =>
            Policy.HandleResult<HttpResponseMessage>(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(100, retryAttempt => TimeSpan.FromSeconds(2));

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
