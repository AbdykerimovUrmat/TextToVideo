using DAL.EF;
using DAL.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Options;
using Models.Validators;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AddIn>());
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "OlympReg API", Version = "v1" });
                x.DescribeAllParametersInCamelCase();

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

                x.CustomSchemaIds(t => t.FullName);
            });

            services.AddIdentityCore<User>(x => 
                {
                    x.Password.RequiredLength = 6;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireUppercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.Password.RequireDigit = false;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "issuer",
                        ValidAudience = "audience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey")),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services.Configure<MediaOptions>(x => Configuration.GetSection("MediaProperties").Bind(x));
            services.AddOptions<MediaOptions>();

            var connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
            services.AddTransient<ImageService>();
            services.AddTransient<AudioService>();
            services.AddTransient<VideoService>();
            services.AddTransient<RequestService>();
            services.AddTransient<UserService>();
            services.AddHostedService<RequestHandler>();

            services.AddCors(x => x.AddDefaultPolicy(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            MapsterProfile.Register();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "OlympReg V1");
                x.RoutePrefix = string.Empty;
                x.DefaultModelExpandDepth(3);
                x.DefaultModelRendering(ModelRendering.Example);
                x.DefaultModelsExpandDepth(-1);
                x.DisplayOperationId();
                x.DisplayRequestDuration();
                x.DocExpansion(DocExpansion.None);
                x.EnableDeepLinking();
                x.EnableFilter();
                x.ShowExtensions();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
