using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace KeySkills.Crawler.App.Services
{
    /// <summary>
    /// Implements <see cref="IServiceScope{TService}"/>
    /// </summary>
    /// <typeparam name="TService">Type of the service</typeparam>
    public class ServiceScope<TService> : IServiceScope<TService> where TService : class
    {
        private readonly IServiceScope _scope;

        /// <summary>
        /// Initializes service scope instance
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is <see langword="null"/></exception>
        public ServiceScope(IServiceScope scope) => 
            _scope = scope ?? throw new ArgumentException(nameof(scope));

        /// <inheritdoc/>
        public TService GetRequiredService() => 
            _scope.ServiceProvider.GetRequiredService<TService>();

        /// <inheritdoc/>
        public TService GetService() => 
            _scope.ServiceProvider.GetService<TService>();

        /// <inheritdoc/>
        public IEnumerable<TService> GetServices() => 
            _scope.ServiceProvider.GetServices<TService>();


        #region IDisposable/Dispose methods ( https://stackoverflow.com/a/538238/530545 )
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool invokedNotFromGC)
        {
            if (!_disposed)
            {
                if (invokedNotFromGC)
                {
                    // dispose of manged resources
                    _scope?.Dispose();
                }
                _disposed = true;
            }
        }

        ~ServiceScope() { Dispose(false); }
        #endregion
    }
}