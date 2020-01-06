namespace OnlineExamer.Web
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using System.IO;
    using System.Reflection;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
               webBuilder.UseIISIntegration();
               webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;

                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                       .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true);

                   if (env.IsDevelopment())
                   {
                       var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                       if (appAssembly != null)
                       {
                           config.AddUserSecrets(appAssembly, optional: true);
                       }
                   }

                   config.AddEnvironmentVariables();

                   if (args != null)
                   {
                       config.AddCommandLine(args);
                   }
               });

               webBuilder.UseStartup<Startup>();
               webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
           });
    }
}
