using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiOne
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
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = identiyServerUrl;

                    config.Audience = "ApiOne";

                    config.RequireHttpsMetadata = false;
                });

            services.AddCors(confg =>
                confg.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("AllowAll");

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
