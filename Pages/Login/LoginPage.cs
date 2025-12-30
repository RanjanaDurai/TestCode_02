using Microsoft.Playwright;
using System.Threading.Tasks;
using WiseMigrateTests.Pages.Landing;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.Login
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly LandingPage _landingPage;

        public LoginPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _landingPage= new LandingPage(page);
        }

        public async Task NavigateToLoginPageAsync()
        {

            await _landingPage.NavigateToLandingPageAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _page.GetByText("Generate Migration Plan for Your Enterprise").ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("input[type='email']");
                        
        }


        public async Task SignInAsync()
        {
            await _basicSetup.LoginAsync();
        }

        public async Task ValidateEmptyUserName(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("empty_username");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        
        }

        public async Task ValidateEmptyPassword(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("empty_password");
            await _basicSetup.LoginAsync(user.Username, user.Password);            
        }

        public async Task ValidateMigrateAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("migrateadmin");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateSuperAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("superadmin");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }


        public async Task ValidateInValidCredentials(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("invalid");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }
        
        public async Task NavigateToMicrosoftSignInAsync(IPage page)
        {
            await NavigateToLoginPageAsync();
            await _page.GetByText("Sign in with Microsoft").ClickAsync();
        }

        /*
        // Automation Feasibility to be determined
        public async Task MicrosoftSignInAsync(IPage page)
        {
            await NavigateToMicrosoftSignInAsync(page);
            var user = WiseMigrateTests.Utils.ConfigReader.GetCredential("microsoft_user");
            await _page.FillAsync("input[type='email']",user.Username);
            await _page.ClickAsync("input[type='submit']");
            await _page.FillAsync("input[type='password']", user.Password);
            await _page.ClickAsync("input[type='submit']");
            await _page.GetByText("Next").ClickAsync();
            if (_page.TitleAsync().Result=="My Sign-Ins | Register | Microsoft.com")
            {
                await _page.GetByText("Next").ClickAsync();
                await _page.GetByText("Next").ClickAsync();
                await _page.GetByText("Next").ClickAsync();
            }
        }

        */

    }
}

