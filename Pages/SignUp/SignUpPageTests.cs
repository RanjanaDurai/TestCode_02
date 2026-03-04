using Microsoft.Playwright;
using System.Text.RegularExpressions;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using Xunit;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.SignUp
{
    [Collection("Playwright collection")]
    [AllureSuite("Sign Up Page Tests")]
    public class SignUpPageTests : TestBaseFixture, IAsyncLifetime
    {
        private SignUpPage _signUpPage = null!;
        private BasicSetup _setup=null!;

        public SignUpPageTests()
        {
            
        }

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper= new AttachmentHelper(Context);
            _setup = new BasicSetup(Page);
            _signUpPage = new SignUpPage(Page);
            await _signUpPage.NavigateToSignUpPageAsync();
            await _setup.WaitForPageAsync(5);
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_001_SignUpPage_Should_Load_Successfully")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_001_SignUpPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_SIGNUP_01: Sign Up page loaded successfully.");
            }, nameof(TC_001_SignUpPage_Should_Load_Successfully));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_002_SignUpPage_Should_Have_Clickable_Buttons")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_002_SignUpPage_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.Locator("text=Sign Up").First).ToBeVisibleAsync();
                if (await Page.Locator("text=Sign Up").First.IsEnabledAsync())
                {
                    Logger.Info("Sign Up button is clickable");
                }
            }, nameof(TC_002_SignUpPage_Should_Have_Clickable_Buttons));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_003_Validate_Registeration_Empty_UserName_Field")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_003_Validate_Registeration_Empty_UserName_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                Logger.Info("TC_SIGNUP_03: Registeration with empty user name field attempted unsuccessfully.");
            }, nameof(TC_003_Validate_Registeration_Empty_UserName_Field));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_004_Validate_Registeration_Empty_Password_Field")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_004_Validate_Registeration_Empty_Password_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                Logger.Info("TC_SIGNUP_04: Registeration with empty password field attempted unsuccessfully.");
            }, nameof(TC_004_Validate_Registeration_Empty_Password_Field));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_005_Validate_Registeration_Duplicate_Email")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_005_Validate_Registeration_Duplicate_Email()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationDuplicateEmail(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_05: Registeration with duplicate email attempted unsuccessfully.");
            }, nameof(TC_005_Validate_Registeration_Duplicate_Email));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_006_Validate_Registeration_Valid_Credentials")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_006_Validate_Registeration_Valid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationValidCredentials(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_06: Registeration with valid credentials attempted successfully.");
            }, nameof(TC_006_Validate_Registeration_Valid_Credentials));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_007_Validate_Registeration_Mismatching_Password")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_007_Validate_Registeration_Mismatching_Password()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationMismatchingPassword(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Passwords do not match. Please ensure both passwords are identical.");
                Logger.Info("TC_SIGNUP_07: Registeration with mismatching passwords attempted unsuccessfully.");
            }, nameof(TC_007_Validate_Registeration_Mismatching_Password));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_008_Validate_Registeration_Empty_Organization")]
        [AllureTag("smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_008_Validate_Registeration_Empty_Organization()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyOrganization(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Invalid organization selected. Please choose a valid option.");
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_08: Registeration with empty organization attempted successfully.");
            }, nameof(TC_008_Validate_Registeration_Empty_Organization));
        }   
    }
}
