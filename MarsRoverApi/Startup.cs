using MarsRoverApi.Infrastructure;
using MarsRoverApi.Interfaces;
using MarsRoverApi.Models;
using MarsRoverApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NasaApiLib;
using NasaApiLib.Interfaces;
using NasaApiLib.Models;
using System;

namespace MarsRoverApi
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
            services.AddDbContext<MarsRoverDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SqlLiteConnection"));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarsRoverApi", Version = "v1" });
            });


            INasaApiSettings nasaApiSettings = new NasaApiSettings();
            Configuration.GetSection("NasaApi").Bind(nasaApiSettings);
            services.AddSingleton(nasaApiSettings);

            IImagePaths imagePaths = new ImagePaths();
            Configuration.GetSection("ImagePaths").Bind(imagePaths);
            services.AddSingleton(imagePaths);

            services.AddHttpClient<INasaApiClient, NasaApiClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(nasaApiSettings.NasaApiUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(45);
            });

            // Initializing service
            services.AddTransient<InitializationService>();

            services.AddSingleton<IImageProviderSettings, ImageProviderSettings>(serviceProvider =>
            {
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                ImageProviderSettings imageProvider = new ImageProviderSettings()
                {
                    FileProvider = webHostEnvironment.ContentRootFileProvider
                };

                return imageProvider;
            });

            // Services
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IMarsPhotoRetrievalService, MarsPhotoRetrievalService>();
            services.AddScoped<IDateService, DateService>();
            services.AddScoped<IMarsRoverDbRepository, MarsRoverDbRepository>();

            // Automapper
            services.AddAutoMapper(
                options => options.AddProfile<MappingProfile>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env, 
            InitializationService initializationService,
            MarsRoverDbContext marsRoverDbContext)
        {
            marsRoverDbContext.Database.Migrate();

            // Note: If we wanted to secure the API from only one origin do something similar to below code
            // app.UseCors(options => options
            //          .WithOrigins("http://localhost:4200")

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarsRoverApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // ***********************************
            // Initialize the photo data
            // ***********************************
            initializationService.Initialize();
        }
    }
}
