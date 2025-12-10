using Microsoft.Playwright;
using System.Text.RegularExpressions;
using WiseMigrateTests.Pages.Landing;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;

namespace WiseMigrateTests.Pages.Landing
{
    [Collection("Playwright collection")]
    public class LandingPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LandingPage _landingPage = null!;
        
        private BasicSetup _setup=null!;

        public LandingPageTests()
        {
             
        }
        

       public async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _setup = new BasicSetup(Page);
            _landingPage = new LandingPage(Page);
            await _landingPage.NavigateToLandingPageAsync();
            Logger.Info("Navigated to landing page for all tests");
        }
    
    

        [Fact]
        [Trait("TestCaseID", "TC_NAV_01")]
        [Trait("Category", "Smoke")]
        public async Task LandingPage_Should_Load_Successfully()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("AppUrl"));
            await _setup.WaitForPageAsync(2);            
            Logger.Info("TC_NAV_01 – Landing page loaded and URL verified");

            await ScreenshotHelper.TakeScreenshotAsync(
                Page, 
                nameof(LandingPage_Should_Load_Successfully)); 
        }

        /*
        [Fact]
        [Trait("Category", "UI")]
        public async Task LandingPage_Should_Have_Logo()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("AppUrl"));
            await _setup.WaitForPageAsync(5);
            var logo = Page.Locator("img[alt='/.*Wise.*Migrate.*Icon/i']");
            await Assertions.Expect(logo).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, nameof(LandingPage_Should_Have_Logo), "LogoVisible");
            Logger.Info("Company logo is visible on landing page");
        }
        */

        [Fact]
        [Trait("TestCaseID", "TC_NAV_02")]
        [Trait("Category", "Smoke")]
        public async Task LandingPage_Should_Have_ClickableButtons()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("AppUrl"));
            await _setup.WaitForPageAsync(3);

            var generatePlanBtn = Page.Locator("text=/Generate.*Plan/i");
            var startAssessmentBtn = Page.Locator("text=/Migration.*Assessment/i");

            await Assertions.Expect(generatePlanBtn).ToBeEnabledAsync();
            await Assertions.Expect(startAssessmentBtn).ToBeEnabledAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, nameof(LandingPage_Should_Have_ClickableButtons));
            Logger.Info("TC_NAV_02 – Both CTA buttons are visible and enabled");
        }

        [Fact]
        [Trait("TestCaseID", "TC_NAV_03")]
        [Trait("Category", "Smoke")]
        public async Task Click_GenerateMigrationPlan_Button_Should_Navigate_Correctly()
        {
            await _landingPage.ClickGenerateMigrationPlanAsync();
            await _setup.WaitForPageAsync(2);
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/.*Account/Login.*", RegexOptions.IgnoreCase));
            await ScreenshotHelper.TakeScreenshotAsync(
                Page, 
                nameof(Click_GenerateMigrationPlan_Button_Should_Navigate_Correctly), 
                "AfterClick");

            Logger.Info("TC_NAV_03 – Successfully navigated to login via Generate Migration Plan");
        }

        [Fact]
        [Trait("TestCaseID", "TC_NAV_04")]
        [Trait("Category", "Smoke")]
        public async Task Click_StartMigrationAssessment_Button_Should_Navigate_Correctly()
        {
            await _landingPage.ClickMigrationAssessmentAsync(); 
            await _setup.WaitForPageAsync(2);
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/.*Account/Login.*", RegexOptions.IgnoreCase));
            await ScreenshotHelper.TakeScreenshotAsync(
                Page, 
                nameof(Click_StartMigrationAssessment_Button_Should_Navigate_Correctly), 
                "AfterClick");

            Logger.Info("TC_NAV_04 – Successfully navigated to login via Start Migration Assessment");
        }
    }
}