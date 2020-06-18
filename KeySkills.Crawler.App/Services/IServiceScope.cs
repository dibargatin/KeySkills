using System;
using System.Collections.Generic;

namespace KeySkills.Crawler.App.Services
{
    /// <summary>
    /// Defines a lifetime scope for the service
    /// </summary>
    /// <typeparam name="TService">Service type</typeparam>
    public interface IServiceScope<TService> : IDisposable where TService : class
    {
        /// <summary>
        /// Gets the service object of the specified type or throws an <see cref="Exception"/> 
        /// when the requested service type has not been registered.
        /// </summary>
        /// <exception cref="Exception">When the requested service type has not been registered</exception>
        /// <returns>Instance of the requested service</returns>
        TService GetRequiredService();

        /// <summary>
        /// Gets the service object of the specified type or <see langword="null"/>
        /// when the requested service type has not been registered.
        /// </summary>
        /// <returns>Instance of the requested service or <see langword="null"/></returns>
        TService GetService();

        /// <summary>
        /// Gets enumeration of the registered services for the type <typeparam name="TService"/>
        /// </summary>
        /// <returns>Enumeration of the registered services</returns>
        IEnumerable<TService> GetServices();
    }
}