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
        [AllureOwner("TC_001_LandingPage_Should_Load_Successfully")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_001_LandingPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("AppUrl"));
                await _setup.WaitForPageAsync(2);
                Logger.Info("TC_NAV_01 – Landing page loaded and URL verified");
            }, nameof(TC_001_LandingPage_Should_Load_Successfully));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_002_LandingPage_Should_Have_Clickable_Buttons")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_002_LandingPage_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(WiseMigrateTests.Utils.ConfigReader.Get("AppUrl"));
                await _setup.WaitForPageAsync(3);

                if (Page.Locator("text=Generate Migration Plan for Your Enterprise") != null &&
                    Page.Locator("text=Start Your Migration Assessment") != null)
                {
                    Logger.Info("TC_NAV_02 – Both CTA buttons are visible and enabled");
                }
            }, nameof(TC_002_LandingPage_Should_Have_Clickable_Buttons));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_003_Click_GenerateMigrationPlan_Button_Should_Navigate_Correctly")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_003_Click_GenerateMigrationPlan_Button_Should_Navigate_Correctly()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _landingPage.ClickGenerateMigrationPlanAsync();
                await _setup.WaitForPageAsync(2);
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(new Regex(@"/.*Account/Login.*", RegexOptions.IgnoreCase));

                Logger.Info("TC_NAV_03 – Successfully navigated to login via Generate Migration Plan");
            }, nameof(TC_003_Click_GenerateMigrationPlan_Button_Should_Navigate_Correctly));
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_004_Click_StartMigrationAssessment_Button_Should_Navigate_Correctly")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_004_Click_StartMigrationAssessment_Button_Should_Navigate_Correctly()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _landingPage.ClickMigrationAssessmentAsync();
                await _setup.WaitForPageAsync(2);
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(new Regex(@"/.*Account/Login.*", RegexOptions.IgnoreCase));

                Logger.Info("TC_NAV_04 – Successfully navigated to login via Start Migration Assessment");
            }, nameof(TC_004_Click_StartMigrationAssessment_Button_Should_Navigate_Correctly));
        }

        /*
        [Fact]
        [Trait("Category", "UI")]
        public async Task LandingPage_Should_Have_Logo()
        {   
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page).ToHaveURLAsync(ConfigReader.Get("AppUrl"));
                await _setup.WaitForPageAsync(5);
                var logo = Page.Locator("img[alt='/.*Wise.*Migrate.*Icon/i']");
                await Assertions.Expect(logo).ToBeVisibleAsync();
                Logger.Info("Company logo is visible on landing page");
            }, nameof(LandingPage_Should_Have_Logo));

        }
        */
    }
}