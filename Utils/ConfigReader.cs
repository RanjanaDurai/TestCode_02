using Microsoft.Extensions.Configuration;

namespace WiseMigrateTests.Utils
{
    public static class ConfigReader
    {
        private static readonly IConfigurationRoot _config;

       static ConfigReader()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var configPath = Path.Combine(projectRoot, "Config", "appsettings.json");

            Console.WriteLine($"[ConfigReader] Looking for config at: {configPath}");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(
                    $"CRITICAL: appsettings.json not found!\nExpected location: {configPath}\n" +
                    $"Current directory: {Directory.GetCurrentDirectory()}");
            }

            try
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(projectRoot)
                    .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var test = _config["AppSettings:AppUrl"];
                Console.WriteLine($"[ConfigReader] AppUrl loaded: {test ?? "NULL!"}");
                if (test == null)
                    throw new InvalidOperationException("AppSettings:AppUrl is missing in JSON!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigReader] FAILED to load config: {ex.Message}");
                throw;
            }
        }

        public static string Get(string key)
        {
            return _config[$"AppSettings:{key}"];
        }
    }
}
