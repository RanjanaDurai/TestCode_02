using Microsoft.Playwright;
using System.Threading.Tasks;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.SignUp
{
    public class SignUpPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly LoginPage _loginPage;

        public SignUpPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _loginPage= new LoginPage(page);
        }

        public async Task NavigateToSignUpPageAsync()
        {
            await _loginPage.NavigateToLoginPageAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _page.GetByText("Sign up").ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("input[type='email']");               
        }
        public async Task SignUpAsync()
        {
            await NavigateToSignUpPageAsync();
            await _basicSetup.SignUpAsync();
        }

        public async Task ValidateRegisterationEmptyUserName(IPage page)
        {
            await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("empty_username");
            var generatedPassword = Guid.NewGuid().ToString("N");
            await _basicSetup.SignUpAsync(user.Role, user.Username, generatedPassword);        
        }

        public async Task ValidateRegisterationEmptyPassword(IPage page)
        {
            await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("empty_password");
            var generatedUsername=Guid.NewGuid().ToString("N")+"@migrate.com";
            await _basicSetup.SignUpAsync(user.Role, generatedUsername, user.Password);             
        }

        public async Task ValidateRegisterationDuplicateEmail(IPage page)
        {
            await NavigateToSignUpPageAsync();
            var username ="DuplicateUser@migrate.com";
            var user = Utils.ConfigReader.GetCredential("standard_user");
            for (int i = 0; i < 2; i++)
            {await _basicSetup.SignUpAsync(user.Role, username, user.Password);} 
            
        }

        public async Task ValidateRegisterationMismatchingPassword(IPage page)
        {
            await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("invalid");
            await _basicSetup.SignUpAsync(user.Role, user.Username, user.Password); 
        }

        public async Task ValidateRegisterationEmptyOrganization(IPage page)
        {
             await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("invalid");
            await _basicSetup.SignUpAsync(user.Role, user.Username, user.Password); 
        }
        
        public async Task ValidateRegisterationValidCredentials(IPage page)
        {
            await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("standard_user");
            var generatedUsername=Guid.NewGuid().ToString("N")+ "@migrate.com";
            await _basicSetup.SignUpAsync(user.Role, generatedUsername, user.Password); 
        }
        public async Task MicrosoftSignInAsync(IPage page)
        {
            await Assertions.Expect(page).ToHaveURLAsync(_basicSetup.LoginPageUrl);
            await page.ClickAsync("text.contains('Sign in with Microsoft')");
        }
    }
}

