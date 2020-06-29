using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeySkills.Core.Data;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Data.Sqlite;
using KeySkills.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeySkills.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVacancyRepository, VacancyRepository>();
            services.AddTransient<BaseDbContext, SqliteDbContext>(provider =>
                new SqliteDbContext(
                    new DbContextOptionsBuilder<SqliteDbContext>()
                        .UseSqlite(new SqliteConnection(
                            Configuration.GetConnectionString(nameof(SqliteDbContext)))
                        ).Options
                    )
            );
            
            services.AddControllers();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
