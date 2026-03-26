using Microsoft.Playwright;
using System.Linq;
using System.Text.Json;
using WiseMigrateTests.Utils;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Pages.ProjectCreation;
using System.Text.RegularExpressions;
using WiseMigrateTests.Pages.InfrastructureUpload;


namespace WiseMigrateTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;
        private readonly ProjectCreationPage _projectCreation;

        private readonly InfrastructureUploadPage _infrastructureUploadPage;
        public string AppUrl => WiseMigrateTests.Utils.ConfigReader.Get("AppUrl");
        public string LoginPageUrl => WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl");
        public string Username => WiseMigrateTests.Utils.ConfigReader.Get("Username");
        public string Password => WiseMigrateTests.Utils.ConfigReader.Get("Password");

        // public static string projectName=string.Empty;  
        public string projectName=string.Empty; 
        public readonly IPlaywright playwright;
        public WiseMigrateTests.Utils.ConfigReader.Credential defaultCredential=WiseMigrateTests.Utils.ConfigReader.GetCredential("standard_user");

        public BasicSetup(IPage page)
        {
            _page = page;
            _projectCreation = new ProjectCreationPage(page);
            _infrastructureUploadPage = new InfrastructureUploadPage(page, this);
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
        public async Task LoginAsync() => await LoginAsync(defaultCredential.Username, defaultCredential.Password);

        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync("input[type='email']", username);
            await _page.FillAsync("input[type='Password']", password);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SignUpAsync(string name, string username, string password, string confirmPassword, bool isOrganizationEmpty=false)
        {
            await _page.FillAsync("input[type='text']",name);
            await _page.FillAsync("input[type='email']", username);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await _page.WaitForTimeoutAsync(1000);
            if(!isOrganizationEmpty)
            {
                var organizationField=_page.Locator("role=combobox");
                await SelectRandomFromSelectAsync(organizationField);
            }
            await _page.Locator("input[type='password']").First.FillAsync(password);
            await _page.Locator("input[type='password']").Last.FillAsync(confirmPassword);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SignUpAsync()
        {
            var generatedUsername=Guid.NewGuid().ToString("N")+"@migrate.com";
            var generatedPassword = Guid.NewGuid().ToString("N");
            var cred=Utils.ConfigReader.GetRandomCredential();
            await SignUpAsync(cred.Role, generatedUsername, generatedPassword, generatedPassword);
        }

        
        public async Task WaitForPageAsync(int seconds) => await Task.Delay(seconds * 1000);
    
        public async Task CreateProjectAsync(string projectName, string customerName="WiseMigrate", string description="Test Project")
        {
            await _projectCreation.EnterProjectNameAsync(projectName);
            await _projectCreation.EnterCustomerNameAsync(customerName);
            await _projectCreation.EnterDescriptionAsync(description);
            await _projectCreation.RandomTargetCloudeProvider();
            await WaitForPageAsync(1);
            await _projectCreation.ClickCreateProjectAsync();

            await _page.WaitForURLAsync("**/data-gathering**", new()
            {
                Timeout = 60000
            });
        }                                                       

        public async Task CreateProjectAsync( )
        {
            projectName= $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
            var customerName = Utils.ConfigReader.GetProjectDetails("CustomerName");
            var description = Utils.ConfigReader.GetProjectDetails("ProjectDescription");
            await CreateProjectAsync( projectName, customerName, description);
        }

        public async Task InfrastructureUploadPageSetupAsync()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await WaitForPageAsync(3);
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await WaitForPageAsync(5);
            await _infrastructureUploadPage.ViewArchitectureAsync();

        }
        public async Task DeleteProjectAsync(string projectNameGlobal)
        {
            if (!string.IsNullOrEmpty(projectNameGlobal))
            {
                // Wait for table rows to load
                await _page.WaitForSelectorAsync("tr", new() { Timeout = 30000 });

                var row = _page.Locator("tr").Filter(new() { Has = _page.GetByText(projectNameGlobal, new() { Exact = true }) });  // Add Exact for reliability
                await row.WaitForAsync(new() { Timeout = 10000 });

                var column = row.Locator("td[data-label='Actions']");
                await column.ClickAsync();

                var deleteButton = _page.Locator(".mud-list-item").GetByText("Delete Project").First;
                await deleteButton.ClickAsync();

                var element = _page.GetByRole(AriaRole.Dialog);
                await element.WaitForAsync(new() { Timeout = 10000 });
                await element.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();

                await Assertions.Expect(_page.GetByText("Project deleted successfully")).ToBeVisibleAsync();

                // Wait for deletion to complete (e.g., snackbar or reload)
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            }
            else
            {
                Logger.Info("Project name is null or empty. Skipping project deletion.");
            }
        }
 
        // public async Task DeleteProjectAsync(string projectNameGlobal)
        // {
        //     if(!string.IsNullOrEmpty(projectNameGlobal))             
        //     {
        //         var row = _page.Locator("tr").Filter(new() { Has = _page.GetByText(projectNameGlobal) });
        //         var column =row.Locator("td[data-label='Actions']");
        //         await column.ClickAsync();
        //         var deleteButton = _page.Locator(".mud-list-item").GetByText("Delete Project").First;
        //         await deleteButton.ClickAsync();
        //         var element= _page.GetByRole(AriaRole.Dialog);
        //         await element.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();        
        //     }
        //     else
        //     {
        //         Logger.Info("Project name is null or empty. Skipping project deletion.");
        //     }
        // }

        private static readonly Random _random = new();
        public async Task RandomSelector()
        {
            var options = _page.Locator(".mud-list-item:not(.mud-disabled)");
            await options.First.WaitForAsync();
            int count = await options.CountAsync();
            await options.Nth(_random.Next(count)).ClickAsync();
        }

        private ILocator PreviousMonthButton =>_page.GetByRole(AriaRole.Button, new(){NameRegex = new Regex("^Previous month")});
        private ILocator NextMonthButton =>_page.GetByRole(AriaRole.Button, new(){NameRegex = new Regex("^Next month")});
        public async Task SelectDaysRangeAsync(int value =-7)
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(value);

            Logger.Info($"Selecting range: {startDate:dd MMM yyyy} - {today:dd MMM yyyy}");

            await SelectDateAsync(startDate);

            await SelectDateAsync(today);
        }

        private async Task SelectDateAsync(DateTime date)
        {
            string day = date.Day.ToString();

            while (!await IsMonthVisibleAsync(date))
            {
                if (date < DateTime.Today)
                {
                    await PreviousMonthButton.ClickAsync();
                }
                else
                {
                    await NextMonthButton.ClickAsync();
                }

                await _page.WaitForTimeoutAsync(300);
            }
            await _page.GetByRole(AriaRole.Button, new() { Name = day }).First.ClickAsync();
        }

        private async Task<bool> IsMonthVisibleAsync(DateTime date)
        {
            string monthYear = date.ToString("MMMM yyyy");

            return await _page.GetByText(monthYear).IsVisibleAsync();
        }

        public async Task<string> RandomDateSelectorAsync()
        {
            var today = DateTime.Today;
            var lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            int totalDaysInRange = (lastDayOfMonth - today).Days + 1;
            int randomOffset = Random.Shared.Next(0, totalDaysInRange);
            var targetDate = today.AddDays(randomOffset);
            string targetDay = targetDate.Day.ToString();
            var dayButton = _page.GetByRole(AriaRole.Button, new() { Name = targetDay }).First;
            await dayButton.ClickAsync();

            return targetDay;
        }
    }
}
