using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Core.Data
{
    public static class AppSettingsJson
    {
        public static string ApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            return appRoot;
        }

        public static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = ApplicationExeDirectory();

            var builder = new ConfigurationBuilder().SetBasePath(applicationExeDirectory)
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }


    }
}
