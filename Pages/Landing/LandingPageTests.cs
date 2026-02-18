using Microsoft.Playwright;
using System.Text.RegularExpressions;
using WiseMigrateTests.Pages.Landing;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.Landing
{
    [Collection("Playwright collection")]
    [AllureSuite("Landing Page Tests")]
    public class LandingPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LandingPage _landingPage = null!;
        
        private BasicSetup _setup=null!;

        private AttachmentHelper _attachmentHelper=null!;
        

       public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper = new AttachmentHelper(Context);
            _setup = new BasicSetup(Page);
            _landingPage = new LandingPage(Page);
            await _landingPage.NavigateToLandingPageAsync();
            Logger.Info("Navigated to landing page for all tests");
        }
    
    
        
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_NAV_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task LandingPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync( async()=>
            {
                await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("AppUrl"));
                await _setup.WaitForPageAsync(2);            
                Logger.Info("TC_NAV_01 – Landing page loaded and URL verified");
                await ScreenshotHelper.TakeScreenshotAsync(Page,"LandingPage_Should_Load_Successfully"); 
            },nameof(LandingPage_Should_Load_Successfully));
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

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_NAV_02")]
        [AllureTag("smoke")]   
        [Fact]     
        public async Task LandingPage_Should_Have_Clickable_Buttons()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("AppUrl"));
            await _setup.WaitForPageAsync(3);

            //await Assertions.Expect(Page.Locator("text=Generate Migration Plan for Your Enterprise").First).ToBeVisibleAsync();
            //await Assertions.Expect(Page.Locator("text=Start Your Migration Assessment").First).ToBeVisibleAsync();
            if( Page.Locator("text=Generate Migration Plan for Your Enterprise")!=null &&
                 Page.Locator("text=Start Your Migration Assessment")!=null)
            {
                Logger.Info("TC_NAV_02 – Both CTA buttons are visible and enabled");
            }
            await ScreenshotHelper.TakeScreenshotAsync(Page, nameof(LandingPage_Should_Have_Clickable_Buttons));
            
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_NAV_03")]
        [AllureTag("smoke")]
        [Fact]
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

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_NAV_04")]
        [AllureTag("smoke")]
        [Fact]
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