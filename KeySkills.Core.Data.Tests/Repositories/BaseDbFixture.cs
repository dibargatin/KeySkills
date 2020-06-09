using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Tests
{   
    public abstract class BaseDbFixture<TContext> : IDisposable
        where TContext : BaseDbContext
    {
        public BaseDbFixture(DbConnection connection)
        {
            Connection = connection;

            SeedData();

            Connection.Open();
        }

        public DbConnection Connection { get; }
        
        public abstract TContext CreateContext();

        public TContext CreateContext(DbTransaction transaction = null)
        {
            var context = CreateContext();

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }
            
            return context;
        }

        private static readonly object _lock = new object(); 
        private static bool _databaseInitialized; 

        protected void SeedData()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}