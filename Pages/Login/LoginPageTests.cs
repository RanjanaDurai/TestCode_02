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
        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper= new AttachmentHelper(Context);
            _setup = new BasicSetup(Page);
            _loginPage = new LoginPage(Page);
            await _loginPage.NavigateToLoginPageAsync();
            await _setup.WaitForPageAsync(5);
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_001_LoginPage_Should_Load_Successfully")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_001_LoginPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_001_Login page loaded successfully.");
            }, nameof(TC_001_LoginPage_Should_Load_Successfully));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_002_LoginPage_Should_Have_Clickable_Buttons")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_002_LoginPage_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.Locator("text=Sign In").First).ToBeVisibleAsync();
                if (await Page.Locator("text=Sign In").First.IsEnabledAsync())
                {
                    Logger.Info("Sign In button is clickable");
                }
            }, nameof(TC_002_LoginPage_Should_Have_Clickable_Buttons));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_003_Validate_Empty_UserName_Field")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_003_Validate_Empty_UserName_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
                await _loginPage.ValidateEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Login");
                Logger.Info("TC_LOGIN_03: Login with empty username unsuccessful.");
            }, nameof(TC_003_Validate_Empty_UserName_Field));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_004_Validate_Empty_Password_Field")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_004_Validate_Empty_Password_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateEmptyPassword(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Login");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Login failed: Invalid credentials. Please try again or contact support.");
                Logger.Info("TC_LOGIN_04: Login with empty password unsuccessful.");
            }, nameof(TC_004_Validate_Empty_Password_Field));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_005_Validate_MigrateAdmin_Account")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_005_Validate_MigrateAdmin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateMigrateAdminAccount(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                Logger.Info("TC_LOGIN_05: Login with valid Migrate Admin credentials successful.");
            }, nameof(TC_005_Validate_MigrateAdmin_Account));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_006_Validate_SuperAdmin_Account")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_006_Validate_SuperAdmin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
                await _loginPage.ValidateSuperAdminAccount(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                Logger.Info("TC_LOGIN_06: Login with valid Super Admin credentials successful.");
            }, nameof(TC_006_Validate_SuperAdmin_Account));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_007_Login_With_Valid_Credentials")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_007_Login_With_Valid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.SignInAsync();
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("You have logged in successfully.");
                Logger.Info("TC_LOGIN_07: Login with valid credentials successful.");
            }, nameof(TC_007_Login_With_Valid_Credentials));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_008_MicrosoftSignInPage_Must_Load_Successfully")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_008_MicrosoftSignInPage_Must_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.NavigateToMicrosoftSignInAsync(Page);
                await Assertions.Expect(Page).ToHaveTitleAsync("Sign in to your account");
                Logger.Info("TC_SIGNIN_08: Microsoft Sign In page loaded successfully.");
            }, nameof(TC_008_MicrosoftSignInPage_Must_Load_Successfully));
        }

        /*
        //Automation feasibility to be assessed.
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNIN_09")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Microsoft_SignIn()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.MicrosoftSignInAsync(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");            
                Logger.Info("TC_SIGNIN_09: Microsoft Sign In successful.");
            }, nameof(MicrosoftSignInPage_Must_Load_Successfully));
            
        } 

         */
    }
}
