using Microsoft.Extensions.Configuration;
namespace WiseMigrateTests.Utils
{
    public static class ConfigReader
    {
        public class Credential
    {
        public string Id { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    
    }
        private static readonly IConfigurationRoot _config;
        private static List<Credential>? _cachedCredentials;

       static ConfigReader()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var configPath = Path.Combine(projectRoot, "Config", "appsettings.json");


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
                if (test == null)
                    throw new InvalidOperationException("AppSettings:AppUrl is missing in JSON!");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string Get(string key)
        {
            return _config[$"AppSettings:{key}"];
        }

        private static List<Credential> LoadCredentials()
        {
            if (_cachedCredentials != null)
                return _cachedCredentials;

            var section = _config.GetSection("AppSettings:Credentials");
            if (section == null )
                throw new InvalidOperationException("AppSettings:Credentials section is missing or empty in appsettings.json");

            _cachedCredentials = section.Get<List<Credential>>();
            return _cachedCredentials;
        }
        public static Credential GetCredential(string id = "standard_user")
        {
            var creds = LoadCredentials();
            var cred = creds.FirstOrDefault(c => 
                c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            return cred ?? throw new KeyNotFoundException($"No credential found with Id = '{id}'");
        }

        /*
        public static Credential GetCredentialByRole(string role)
        {
            var creds = LoadCredentials();
            var cred = creds.FirstOrDefault(c => 
                c.Role.Contains(role, StringComparison.OrdinalIgnoreCase));

            return cred ?? throw new KeyNotFoundException($"No credential found with role containing '{role}'");
        }
        */

        public static Credential GetRandomCredential()
        {
            var active = LoadCredentials().ToList();
            if (!active.Any())
                throw new InvalidOperationException("No credentials found in appsettings.json");

            return active[new Random().Next(active.Count)];
        }

        public static string GetProjectDetails(string key)
        {
            return _config[$"ProjectDetails:{key}"];
        }
        
       
    }
}
