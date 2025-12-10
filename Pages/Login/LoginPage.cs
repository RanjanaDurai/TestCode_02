/*using Microsoft.Playwright;
using System.Threading.Tasks;
using WiseMigrateTests.Pages.Landing;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;

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
            await _page.ClickAsync("text.contains('Generate Migration Plan for Your Enterprise')");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                        
        }


        public async Task SignInAsync()
        {
            await _basicSetup.LoginAsync();
        }

        public async Task ValidateEmptyUserName(IPage page)
        {
            await page.GotoAsync(_basicSetup.LoginPageUrl);
            await _basicSetup.LoginAsync();
        
        }

        public async Task ValidateEmptyPassword(IPage page)
        {
            
        }
        
        public async Task MicrosoftSignInAsync(IPage page)
        {
            await Assertions.Expect(page).ToHaveURLAsync(_basicSetup.LoginPageUrl);
            await page.ClickAsync("text.contains('Sign in with Microsoft')");
        }
    }
}
*/
