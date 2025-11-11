using System.IO;
using Microsoft.Extensions.Configuration;

namespace FscmBridgeServices.Util
{
    public class GetConfig
    {
        public static IConfiguration AppSetting { get; }

        static GetConfig()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}