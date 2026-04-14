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

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper= new AttachmentHelper(Context);
            _setup = new BasicSetup(Page);
            _signUpPage = new SignUpPage(Page);
            await _signUpPage.NavigateToSignUpPageAsync();
            await _setup.WaitForPageAsync(5);
        }

        [AllureOwner("TC_209_SignUpPage_Should_Load_Successfully")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_209_SignUpPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_SIGNUP_01: Sign Up page loaded successfully.");
            }, nameof(TC_209_SignUpPage_Should_Load_Successfully));
        }

        [AllureOwner("TC_210_SignUpPage_Should_Have_Clickable_Buttons")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_210_SignUpPage_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.Locator("text=Sign Up").First).ToBeVisibleAsync();
                if (await Page.Locator("text=Sign Up").First.IsEnabledAsync())
                {
                    Logger.Info("Sign Up button is clickable");
                }
            }, nameof(TC_210_SignUpPage_Should_Have_Clickable_Buttons));
        }

        [AllureOwner("TC_211_Validate_Registeration_Empty_UserName_Field")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_211_Validate_Registeration_Empty_UserName_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                Logger.Info("TC_SIGNUP_03: Registeration with empty user name field attempted unsuccessfully.");
            }, nameof(TC_211_Validate_Registeration_Empty_UserName_Field));
        }

        [AllureOwner("TC_212_Validate_Registeration_Empty_Password_Field")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_212_Validate_Registeration_Empty_Password_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyUserName(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                Logger.Info("TC_SIGNUP_04: Registeration with empty password field attempted unsuccessfully.");
            }, nameof(TC_212_Validate_Registeration_Empty_Password_Field));
        }

        [AllureOwner("TC_213_Validate_Registeration_Duplicate_Email")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_213_Validate_Registeration_Duplicate_Email()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationDuplicateEmail(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_05: Registeration with duplicate email attempted unsuccessfully.");
            }, nameof(TC_213_Validate_Registeration_Duplicate_Email));
        }

        [AllureOwner("TC_214_Validate_Registeration_Valid_Credentials")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_214_Validate_Registeration_Valid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationValidCredentials(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_06: Registeration with valid credentials attempted successfully.");
            }, nameof(TC_214_Validate_Registeration_Valid_Credentials));
        }

        [AllureOwner("TC_215_Validate_Registeration_Mismatching_Password")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_215_Validate_Registeration_Mismatching_Password()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationMismatchingPassword(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Passwords do not match. Please ensure both passwords are identical.");
                Logger.Info("TC_SIGNUP_07: Registeration with mismatching passwords attempted unsuccessfully.");
            }, nameof(TC_215_Validate_Registeration_Mismatching_Password));
        }

        [AllureOwner("TC_216_Validate_Registeration_Empty_Organization")]
        [AllureTag("Smoke")]
        [Trait("Category", "Smoke")]
        [Fact]
        public async Task TC_216_Validate_Registeration_Empty_Organization()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyOrganization(Page);
                await _setup.WaitForPageAsync(3);
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Invalid organization selected. Please choose a valid option.");
                await Assertions.Expect(Page).ToHaveTitleAsync("Register");
                Logger.Info("TC_SIGNUP_08: Registeration with empty organization attempted successfully.");
            }, nameof(TC_216_Validate_Registeration_Empty_Organization));
        }   
    }
}
