using System;
using System.IO;


using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using ActionCommandGame.Ui.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using ActionCommandGame.Sdk;


namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            
            
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            //Zelfgemaakte database
            //DbInitializer.DbInitializer.Seed(app);

            //var database = ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
            /*MigrateDatabase();
            SeedDatabase();*/

            
            var game = ServiceProvider.GetRequiredService<Game>();
            game.Start();
        }

        public static void ConfigureServices(IServiceCollection services)

        {
            var appSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), appSettings);

            services.AddSingleton(appSettings);

            //Register the EntityFramework database In Memory as a Singleton
            /*services.AddDbContext<ActionButtonGameDbContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\VivesGame;Database=ActionCommandGameDatabase;Trusted_Connection=True;TrustServerCertificate=True");

            }, ServiceLifetime.Singleton);*/
            var apiSettings = new ApiSettings();
            Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);
            services.AddHttpClient("ActionCommandGameApi", options =>
            {
                if (!string.IsNullOrWhiteSpace(apiSettings.BaseAddress))
                {
                    options.BaseAddress = new Uri(apiSettings.BaseAddress);
                }
            });

            services.AddTransient<Game>();

            services.AddTransient<IGameService, GameService>();
            services.AddScoped<ItemSdk>();
            services.AddScoped<PlayerItemSdk>();
            services.AddScoped<NegativeGameEventSdk>();
            services.AddScoped<PositiveGameEventSdk>();
            services.AddScoped<PlayerSdk>();
        }

       /* public static void MigrateDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
                dbContext.Database.Migrate();
            }
        }

        public static void SeedDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
                DbInitializer.DbInitializer.Seed(dbContext);
            }
        }*/
    }
}
