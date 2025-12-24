using Microsoft.Playwright;
using System.Text.RegularExpressions;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using Xunit;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.Login
{
    [Collection("Playwright collection")]
    [AllureSuite("Login Page Tests")]
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


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task LoginPage_Should_Load_Successfully()
        {
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_01_LoginPage_Should_Load_Successfully");
            Logger.Info("TC_LOGIN_01: Login page loaded successfully.");
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task LoginPage_Should_Have_Clickable_Buttons()
        {
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
       }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_UserName_Field()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
            await _loginPage.ValidateEmptyUserName(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Login");
            //await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Login failed: Invalid credentials. Please try again or contact support");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_03_Login_With_Empty_Username_Unsuccessful");
            Logger.Info("TC_LOGIN_03: Login with empty username unsuccessful.");
        } 


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_Password_Field()
        {
            await _loginPage.ValidateEmptyPassword(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Login");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Login failed: Invalid credentials. Please try again or contact support.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_04_Login_With_Empty_Password_Unsuccessful");
            Logger.Info("TC_LOGIN_04: Login with empty password unsuccessful.");
        } 

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_05")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_MigrateAdmin_Account()
        {
            //await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_05_Login_With_Valid_MigrateAdmin_Successful");
            Logger.Info("TC_LOGIN_05: Login with valid Migrate Admincredentials successful.");
        } 

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_SuperAdmin_Account()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
            await _loginPage.ValidateSuperAdminAccount(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_06_Login_With_Valid_SuperAdmin_Successful");
            Logger.Info("TC_LOGIN_06: Login with valid Super Admin credentials successful.");
        } 


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Login_With_Valid_Credentials()
        {
            await _loginPage.SignInAsync();
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
            //await Assertions.Expect(Page.Locator(".mud-snackbar-location-top-right .mud-snackbar-content-message")).ToBeVisibleAsync();
            //await Page.Locator(".mud-snackbar-location-top-right .mud-snackbar-content-message").("You have been successfully logged out");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("You have logged in successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_07_Login_With_Valid_credentials_Successful");
            Logger.Info("TC_LOGIN_07: Login with valid credentials successful.");
        }  

    }
}
