namespace KeySkills.Crawler.App.Services
{
    /// <summary>
    /// Defines generic service scope factory. It is used to avoid a service locator anti-pattern.
    /// </summary>
    /// <typeparam name="TService">Service type</typeparam>
    public interface IServiceScopeFactory<TService> where TService : class
    {
        /// <summary>
        /// Creates a lifetime scope for the service
        /// </summary>
        /// <returns><see cref="IServiceScope{TService}"/></returns>
        IServiceScope<TService> CreateScope();
    }
}