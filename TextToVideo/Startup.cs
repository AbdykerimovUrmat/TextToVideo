using DAL.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models.Options;
using System.IO;
using Uploader.Infrastructure;
using Uploader.Services;


namespace Uploader
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Uploader", Version = "v1" });
            });

            services.Configure<MediaOptions>(x => Configuration.GetSection("MediaProperties").Bind(x));

            var connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
            services.AddOptions<MediaOptions>("");
            services.AddTransient<ImageService>();
            services.AddTransient<AudioService>();
            services.AddTransient<VideoService>();
            services.AddTransient<RequestService>();
            services.AddHostedService<RequestHandler>();
            MapsterProfile.Register();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uploader v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
