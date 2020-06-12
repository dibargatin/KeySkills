using System;
using Xunit;
using KeySkills.Core.Data.Repositories;

namespace KeySkills.Core.Data.Tests
{
    /// <summary>
    /// Contains integration tests of <see cref="VacancyRepository"/> class
    /// </summary>  
    [Collection(nameof(BaseDbFixture<BaseDbContext>))]
    public abstract partial class BaseVacancyRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        protected readonly TFixture _fixture;

        /// <summary>
        /// Initializes fixture
        /// </summary>
        /// <param name="fixture">Instance of <see cref="BaseDbFixture"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="fixture"/> is <see langword="null" /></exception>
        public BaseVacancyRepositoryFacts(TFixture fixture) =>
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
    }
}