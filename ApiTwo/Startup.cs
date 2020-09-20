using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiTwo
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            var identiyServerUrl = _config.GetSection("Servers").GetSection("IdentityServer").Value;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config => {
                    config.Authority = identiyServerUrl;

                    config.Audience = "ApiTwo";

                    config.RequireHttpsMetadata = false;
                });

            services.AddHttpClient();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
