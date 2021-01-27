using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using APILibrary.Repositories;
using APILibrary.Repositories.Users;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;

namespace Documentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(e =>
            {
                e.SwaggerDoc("foo", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Social Media API",
                    Description = "This is a API for creating posts and liking posts.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ludvig Malm",
                        Email = "malmludvig@gmail.com",
                        Url = new Uri("https://github.com/malmludvig")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                var libraryFile = "APILibrary.xml";
                var libraryPath = Path.Combine(AppContext.BaseDirectory, libraryFile);
                e.IncludeXmlComments(xmlPath);
                e.IncludeXmlComments(libraryPath);
            });

            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<IUserRepository, DictionaryUserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(e =>
                {
                    e.SwaggerEndpoint("/swagger/foo/swagger.json,", "Social Media API V1");
                }
            );
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}