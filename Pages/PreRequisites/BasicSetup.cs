using Microsoft.Playwright;
using System.Linq;
using System.Text.Json;
using WiseMigrateTests.Utils;
using WiseMigrateTests.TestHooks;


namespace WiseMigrateTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;
        public string AppUrl => WiseMigrateTests.Utils.ConfigReader.Get("AppUrl");
        public string LoginPageUrl => WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl");
        public string Username => WiseMigrateTests.Utils.ConfigReader.Get("Username");
        public string Password => WiseMigrateTests.Utils.ConfigReader.Get("Password");

        public readonly IPlaywright playwright;

        public WiseMigrateTests.Utils.ConfigReader.Credential defaultCredential=WiseMigrateTests.Utils.ConfigReader.GetCredential("standard_user");

        public BasicSetup(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAppAsync() => await _page.GotoAsync(AppUrl);
       
        public async Task SelectRandomFromSelectAsync(ILocator selectLocator)
        {           
            var playwright = await Playwright.CreateAsync();
            var requestContext = await playwright.APIRequest.NewContextAsync();


            var response = await requestContext.GetAsync("https://wisemigrate.services-internal.wisework.in/api/v0.2.19/organizations");
            var json = await response.JsonAsync();                 
            var organizations = new List<string>();
            if(json.HasValue)
            {
                if (json.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in json.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            string org = item.GetString()!;
                            organizations.Add(org);
                            //Console.WriteLine($"Organization: {org}");
                        }
                    }
                }
            }              

            if (organizations.Count == 0)
            throw new Exception("Organization list is empty");

             await selectLocator.Page.WaitForFunctionAsync(@"
                () => {
                    const select = document.querySelector('#organization');
                    return select && select.options.length > 1;
                }
            ");

            var uiOptions = await selectLocator.Locator("option").AllTextContentsAsync();
            var randomOption = organizations[new Random().Next(organizations.Count)];            
            await selectLocator.SelectOptionAsync(new SelectOptionValue { Value = randomOption });
            Logger.Info($"Randomly selected from <select>: {randomOption}");
        
        }
        
/*
        public async Task SelectRandomFromSelectAsync(ILocator selectLocator)
        {
            // 1. Wait for select to be attached
            await selectLocator.WaitForAsync(new()
            {
                State = WaitForSelectorState.Attached,
                Timeout = 60000
            });

            // 2. Wait for UI-side API to populate options
            await selectLocator.Page.WaitForFunctionAsync(@"
                () => {
                    const select = document.querySelector('#organization');
                    return select && select.options.length > 1;
                }
            ");

            // 3. Read UI options
            var options = await selectLocator.Locator("option").AllTextContentsAsync();

            var validOptions = options
                .Where(o => !string.IsNullOrWhiteSpace(o) &&
                            !o.Contains("Select", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!validOptions.Any())
                throw new Exception("No organizations available in dropdown");

            // 4. Select random
            var randomOption = validOptions[new Random().Next(validOptions.Count)];

            await selectLocator.SelectOptionAsync(new SelectOptionValue
            {
                Value = randomOption
            });

            Logger.Info($"Selected organization: {randomOption}");
        }
        */

        public async Task LoginAsync() => await LoginAsync(defaultCredential.Username, defaultCredential.Password);

        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync("input[type='email']", username);
            await _page.FillAsync("input[type='Password']", password);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SignUpAsync(string name, string username, string password)
        {
            await _page.FillAsync("input[type='text']",name);
            await _page.FillAsync("input[type='email']", username);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await _page.WaitForTimeoutAsync(1000);
            var organizationField=_page.Locator("role=combobox");
            await SelectRandomFromSelectAsync(organizationField);
            await _page.Locator("input[type='password']").First.FillAsync(password);
            await _page.Locator("input[type='password']").Last.FillAsync(password);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SignUpAsync()
        {
            var generatedUsername=Guid.NewGuid().ToString("N")+"@migrate.com";
            var generatedPassword = Guid.NewGuid().ToString("N");
            var cred=Utils.ConfigReader.GetRandomCredential();
            await SignUpAsync(cred.Role, generatedUsername, generatedPassword);
        }

        
        public async Task WaitForPageAsync(int seconds) => await Task.Delay(seconds * 1000);
    }

}
