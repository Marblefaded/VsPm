using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vs.Pm.Web.Data.Service;

namespace Vs.Pm.TestDb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TaskService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<StatusService>();
            services.AddScoped<LogApplicationService>();
            services.AddScoped<TaskTypeService>();
            services.AddScoped<UserService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            
        }
    }
}
