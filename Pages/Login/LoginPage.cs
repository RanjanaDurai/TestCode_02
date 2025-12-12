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
            var user = ConfigReader.GetCredential("empty_username");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        
        }

        public async Task ValidateEmptyPassword(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = ConfigReader.GetCredential("empty_password");
            await _basicSetup.LoginAsync(user.Username, user.Password);            
        }

        public async Task ValidateMigrateAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = ConfigReader.GetCredential("migrateadmin");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateSuperAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = ConfigReader.GetCredential("superadmin");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }


        public async Task ValidateInValidCredentials(IPage page)
        {
            await NavigateToLoginPageAsync();
            var user = ConfigReader.GetCredential("invalid");
            await _basicSetup.LoginAsync(user.Username, user.Password);
        }
        
        public async Task MicrosoftSignInAsync(IPage page)
        {
            await Assertions.Expect(page).ToHaveURLAsync(_basicSetup.LoginPageUrl);
            await page.ClickAsync("text.contains('Sign in with Microsoft')");
        }
    }
}

