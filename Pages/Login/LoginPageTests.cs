/*using Microsoft.Playwright;
using System.Text.RegularExpressions;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using Xunit;


namespace WiseMigrateTests.Pages.Login
{
    [Collection("Playwright collection")]
    public class LoginPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        
        private BasicSetup _setup=null!;

        public LoginPageTests()
        {
            
        }

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _setup = new BasicSetup(Page);
            _loginPage = new LoginPage(Page);
            await _loginPage.NavigateToLoginPageAsync();
            await _setup.WaitForPageAsync(5);
        }


        [Fact]
        [Trait("TestCaseID", "TC_LOGIN_01")]
        [Trait("Category", "Smoke")]
        public async Task LoginPage_Should_Load_Successfully()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("LoginPageUrl"));
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_01_LoginPage_Should_Load_Successfully");
            Logger.Info("TC_LOGIN_01: Login page loaded successfully.");
        }


        [Fact]
        [Trait("TestCaseID", "TC_LOGIN_02")]
        [Trait("Category", "Smoke")]
        public async Task LoginPage_Should_Have_Clickable_Buttons()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("LoginPageUrl"));
            await Assertions.Expect(Page.Locator("text=Sign In").First).ToBeVisibleAsync();
            if (await Page.Locator("text=Sign In").First.IsEnabledAsync())
            {
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_02_LoginPage_Should_Have_Clickable_Buttons_01");
                Logger.Info("Sign In button is clickable");
            }

            /*
            await Expect(_loginPage.Locator("text.contains('Sign in with Microsoft')")).ToBeVisibleAsync();
            if (await Expect(_loginPage.Locator("text.contains('Sign in with Microsoft')")).IsEnabledAsync())
            {
                await ScreenshotHelper.TakeScreenshotAsync(_loginPage, "TC_LOGIN_02_LoginPage_Should_Have_Clickable_Buttons_02");
                await Logger.Info("Microsoft Sign In button is clickable");
            }
            */
    /*    }


        [Fact]
        [Trait("TestCaseID", "TC_LOGIN_03")]
        [Trait("Category", "Smoke")]
        public async Task Validate_Empty_UserName_Field()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("LoginPageUrl"));
            await _loginPage.SignInAsync();
            await _setup.WaitForPageAsync(5);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_03_Login_With_Valid_credntials_Successful");
            Logger.Info("TC_LOGIN_03: Login with valid credentials successful.");
        } 


        [Fact]
        [Trait("TestCaseID", "TC_LOGIN_04")]
        [Trait("Category", "Smoke")]
        public async Task Validate_Empty_Password_Field()
        {
            await _loginPage.SignInAsync();
            await _setup.WaitForPageAsync(5);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("You have been successfully logged in");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_03_Login_With_Valid_credntials_Successful");
            Logger.Info("TC_LOGIN_03: Login with valid credentials successful.");
        } 



        [Fact]
        [Trait("TestCaseID", "TC_LOGIN_05")]
        [Trait("Category", "Smoke")]
        public async Task Login_With_Valid_Credentials()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("LoginPageUrl"));
            await _loginPage.SignInAsync();
            await _setup.WaitForPageAsync(5);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("You have been successfully logged in");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_03_Login_With_Valid_credntials_Successful");
            Logger.Info("TC_LOGIN_03: Login with valid credentials successful.");
        } 


    }
}*/
