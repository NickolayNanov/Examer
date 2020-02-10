namespace OnlineExamer.Web
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using OnlineExamer.Data;
    using OnlineExamer.Data.Seeding;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    OnlineExamerDbContext dbContext = scope.ServiceProvider.GetRequiredService<OnlineExamerDbContext>();
                    dbContext.Database.Migrate();
                    Seeder.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }



            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                });
    }
}