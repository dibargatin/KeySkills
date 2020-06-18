using System;
using Microsoft.Extensions.DependencyInjection;

namespace KeySkills.Crawler.App.Services
{
    /// <summary>
    /// Implements <see cref="IServiceScopeFactory{TService}"/>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ServiceScopeFactory<TService> : IServiceScopeFactory<TService> where TService : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes service scope factory instance
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="serviceScopeFactory"/> is <see langword="null"/></exception>
        public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) => 
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        /// <inheritdoc/>
        public IServiceScope<TService> CreateScope() => 
            new ServiceScope<TService>(_serviceScopeFactory.CreateScope());
    }
}