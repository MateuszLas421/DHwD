using DHwD.Repository;
using DHwD.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests
{
    public class Startup
    {
        public Startup()
        {
            var config = new ConfigurationBuilder();
            Configuration = config.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogs, LogsRepository>();
        }

        public void Configure(IServiceProvider provider)
        { }
    }
}
