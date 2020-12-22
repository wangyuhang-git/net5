using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Learn.Common
{
    public static class ConfigHelper
    {
        private static IConfiguration configuration;

        static ConfigHelper()
        {
            string fileName = "appsettings.json";
            string directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            string filePath = $"{directory}{fileName}";
            if (!File.Exists(filePath))
            {
                int length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}{fileName}";
            }
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(filePath, false, true);

            configuration = builder.Build();
        }

        public static string GetSectionValue(string key)
        {
            return configuration.GetSection(key).Value;
        }
    }
}
