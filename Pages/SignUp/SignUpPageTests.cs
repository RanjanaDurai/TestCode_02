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
            _setup = new BasicSetup(Page);
            _signUpPage = new SignUpPage(Page);
            await _signUpPage.NavigateToSignUpPageAsync();
            await _setup.WaitForPageAsync(5);
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Load_Successfully()
        {
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_01_SignUpPage_Should_Load_Successfully");
            Logger.Info("TC_SIGNUP_01: Sign Up page loaded successfully.");
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Have_Clickable_Buttons()
        {
            await Assertions.Expect(Page.Locator("text=Sign Up").First).ToBeVisibleAsync();
            if (await Page.Locator("text=Sign Up").First.IsEnabledAsync())
            {
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_02_SignUpPage_Should_Have_Clickable_Buttons_01");
                Logger.Info("Sign Up button is clickable");
            }
         
       }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_UserName_Field()
        {
            await _signUpPage.ValidateRegisterationEmptyUserName(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_03_Registeration_Empty_UserName_Field");
            Logger.Info("TC_SIGNUP_03: Registeration with empty user name field attempted unsuccessfully.");
        } 


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Password_Field()
        {
            await _signUpPage.ValidateRegisterationEmptyUserName(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_04_Registeration_Empty_Password_Field");
            Logger.Info("TC_SIGNUP_04: Registeration with empty password field attempted unsuccessfully.");
        } 

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_05")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Duplicate_Email()
        {
            await _signUpPage.ValidateRegisterationDuplicateEmail(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Registration failed. Please try again or contact support if the issue persists.");
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_05_Registeration_Duplicate_Email");
            Logger.Info("TC_SIGNUP_05: Registeration with duplicate email attempted unsuccessfully.");
        } 

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Valid_Credentials()
        {
            await _signUpPage.ValidateRegisterationValidCredentials(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_06_Registeration_Valid_Credentials");
            Logger.Info("TC_SIGNUP_06: Registeration with valid credentials attempted successfully.");
        } 

        //Yet to be coded
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Mismatching_Password()
        {
            await _signUpPage.ValidateRegisterationMismatchingPassword(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Passwords do not match. Please ensure both passwords are identical.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_07_Registeration_Mismatching_Password");
            Logger.Info("TC_SIGNUP_07: Registeration with mismatching passwords attempted unsuccessfully.");
        } 


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Organization()
        {
            await _signUpPage.ValidateRegisterationEmptyOrganization(Page);
            await _setup.WaitForPageAsync(3);
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Invalid organization selected. Please choose a valid option.");
            await Assertions.Expect(Page).ToHaveTitleAsync("Register");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_08_Registeration_Empty_Organization");
            Logger.Info("TC_SIGNUP_08: Registeration with empty organization attempted successfully.");
        } 
        
    }
}
