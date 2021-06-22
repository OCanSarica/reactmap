using System;
using entity_creator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.ComponentModel;

namespace entity_creator
{
    class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration _configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", true, true).
                Build();

            var _services = new ServiceCollection().
                AddDbContext<EntityDbContext>(x =>
                    x.UseNpgsql(_configuration.GetConnectionString("Base")));

            _services.AddSingleton(_configuration);

            _services.AddSingleton<EntityBuilder>();

            var _serviceProvider = _services.BuildServiceProvider();

            var _builder = _serviceProvider.GetService<EntityBuilder>();

            _builder.Build();

            Console.ReadKey();
        }
    }
}
