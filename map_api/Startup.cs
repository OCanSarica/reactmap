using dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using core.Logger;
using bll.Bases;
using bll.Services;
using dal.Bases;
using dal.Repositories;

namespace map_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection _services)
        {
            _services.AddControllers();

            _services.AddDbContext<DalDbContext>(options =>
            {
                options.UseNpgsql(
                    Configuration.GetConnectionString("Base"),
                    x => x.UseNetTopologySuite());
            });

            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            _services.AddScoped<ILogService, LogService>();

            _services.AddScoped<IUserService, UserService>();

            _services.AddScoped<ILayerService, LayerService>();

            _services.AddCors(o => o.AddPolicy("CorsPolicy",
                builder => builder.
                    AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader()));
        }

        public void Configure(
            IApplicationBuilder _app,
            IWebHostEnvironment _env,
            ILoggerFactory _loggerFactory)
        {
            if (_env.IsDevelopment())
            {
                _app.UseDeveloperExceptionPage();
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
