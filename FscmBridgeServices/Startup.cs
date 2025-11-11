using FscmBridgeServices.Middlewares;
using FscmBridgeServices.Repository.DataContext;
using FscmBridgeServices.Services.Impl;
using FscmBridgeServices.Services.Interface;
using FscmBridgeServices.Services.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace FscmBridgeServices
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
            services.AddDbContext<DataContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("UrlDatabase")));

            services.AddScoped<IFscm_Service, Fscm_Service>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<FscmProfile>();
            });

            services.AddControllers();
            services.AddTransient<ExceptionHandlingMiddleware>(); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FscmBridgeServices", Version = "v1" });
            });
     
        }

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
               
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        
        
            //app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
