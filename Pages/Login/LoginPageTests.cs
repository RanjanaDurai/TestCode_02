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

        [AllureOwner("TC_005_LoginPage_Should_Load_Successfully")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_005_LoginPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_005_Login page loaded successfully.");
            }, nameof(TC_005_LoginPage_Should_Load_Successfully));
        }

        [AllureOwner("TC_006_LoginPage_Should_Have_Clickable_Buttons")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_006_LoginPage_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.Locator("text=Sign In").First).ToBeVisibleAsync();
                if (await Page.Locator("text=Sign In").First.IsEnabledAsync())
                {
                    Logger.Info("Sign In button is clickable");
                }
            }, nameof(TC_006_LoginPage_Should_Have_Clickable_Buttons));
        }

        [AllureOwner("TC_007_Validate_Empty_UserName_Field")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_007_Validate_Empty_UserName_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("LoginPageUrl"));
                await _loginPage.ValidateEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Login");
                Logger.Info("TC_LOGIN_03: Login with empty username unsuccessful.");
            }, nameof(TC_007_Validate_Empty_UserName_Field));
        }

        [AllureOwner("TC_008_Validate_Empty_Password_Field")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_008_Validate_Empty_Password_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateEmptyPassword(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Login");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Login failed: Invalid credentials. Please try again or contact support.");
                Logger.Info("TC_LOGIN_04: Login with empty password unsuccessful.");
            }, nameof(TC_008_Validate_Empty_Password_Field));
        }

        [AllureOwner("TC_009_Validate_MigrateAdmin_Account")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_009_Validate_MigrateAdmin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateMigrateAdminAccount(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                Logger.Info("TC_LOGIN_05: Login with valid Migrate Admin credentials successful.");
            }, nameof(TC_009_Validate_MigrateAdmin_Account));
        }

        [AllureOwner("TC_010_Validate_SuperAdmin_Account")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_010_Validate_SuperAdmin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateSuperAdminAccount(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                Logger.Info("TC_LOGIN_06: Login with valid Super Admin credentials successful.");
            }, nameof(TC_010_Validate_SuperAdmin_Account));
        }

        
        [AllureOwner("TC_011_Login_With_Valid_Credentials")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_011_Login_With_Valid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.SignInAsync();
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Migrate");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("You have logged in successfully.");
                Logger.Info("TC_LOGIN_07: Login with valid credentials successful.");
            }, nameof(TC_011_Login_With_Valid_Credentials));
        }

        [AllureOwner("TC_012_MicrosoftSignInPage_Must_Load_Successfully")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_012_MicrosoftSignInPage_Must_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.NavigateToMicrosoftSignInAsync(Page);
                await Assertions.Expect(Page).ToHaveTitleAsync("Sign in to your account");
                Logger.Info("TC_SIGNIN_08: Microsoft Sign In page loaded successfully.");
            }, nameof(TC_012_MicrosoftSignInPage_Must_Load_Successfully));
        }

    }
}
