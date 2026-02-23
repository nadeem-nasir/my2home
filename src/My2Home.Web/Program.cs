using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.IO;

namespace My2Home.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        { 
        var currentDirectoryPath = Directory.GetCurrentDirectory();
        var envSettingsPath = Path.Combine(currentDirectoryPath, "envsettings.json");
        var envSettings = JObject.Parse(File.ReadAllText(envSettingsPath));
        var enviromentValue = envSettings["ASPNETCORE_ENVIRONMENT"].ToString();


            var webHostBuilder =  WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            if (!string.IsNullOrWhiteSpace(enviromentValue))
            {
                webHostBuilder.UseEnvironment(enviromentValue);
            }
            return webHostBuilder;

        }
    }
}
