using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bll.Bases;
using bll.Services;
using core.Logger;
using dal.Bases;
using dal.Models;
using dal.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace user_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection _services)
        {
            _services.AddControllers();
            
            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "user_api", Version = "v1" });
            });

            _services.AddDbContext<DalDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Base"),
                    x => x.UseNetTopologySuite());
            });

            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            _services.AddScoped<ILogService, LogService>();

            _services.AddScoped<IUserService, UserService>();

            _services.AddCors(o => o.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder _app, 
            IWebHostEnvironment _env, 
            ILoggerFactory _loggerFactory)
        {
            if (_env.IsDevelopment())
            {
                _app.UseDeveloperExceptionPage();
                _app.UseSwagger();
                _app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "user_api v1"));
            }

            _loggerFactory.AddProvider(
                new LoggerProvider(Configuration["Logging:Directory"]));

            _app.UseCors("CorsPolicy");

            _app.UseHttpsRedirection();

            _app.UseRouting();

            _app.UseAuthorization();

            _app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
