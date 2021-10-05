using System;
using DHwD.Repository;
using DHwD.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogs, LogsRepository>();
        }

        public void Configure(IServiceProvider provider)
        { }
    }
}
