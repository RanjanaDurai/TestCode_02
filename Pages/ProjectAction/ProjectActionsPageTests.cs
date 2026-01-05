using Microsoft.Playwright;
using WiseMigrateTests.Pages.Landing;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.Project
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Actions Tests")]
    public class ProjectActionsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LandingPage _landingPage = null!;
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private ProjectActionsPage _projectActions = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _landingPage = new LandingPage(Page);
            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _projectActions = new ProjectActionsPage(Page);

            await _landingPage.NavigateToLandingPageAsync();
            await _landingPage.ClickGenerateMigrationPlanAsync();
            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.WaitForDashboardAsync();

            Logger.Info("Project dashboard ready for Project Actions tests");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        [AllureOwner("TC_PROJECT_001")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_001_Open_Client_Mode()
        {
            Logger.Info("TC_PROJECT_001 started");

            await _projectActions.ClickClientModeAsync();

            await Assertions.Expect(
                Page.GetByRole(AriaRole.Heading, new() { Name = "Current Mode of Operation - Architectural View" })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_001_Client_Mode");
            Logger.Info("TC_PROJECT_001 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_002")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_002_Open_Future_Mode()
        {
            Logger.Info("TC_PROJECT_002 started");

            await _projectActions.ClickFutureModeAsync();

            await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View"))
                .ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_002_Future_Mode");
            Logger.Info("TC_PROJECT_002 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_003")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_003_Open_Inventory()
        {
            Logger.Info("TC_PROJECT_003 started");

            await _projectActions.ClickInventorySpecificationsAsync();

            await Assertions.Expect(Page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_003_Inventory");
            Logger.Info("TC_PROJECT_003 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_004_Open_Track_Plan()
        {
            Logger.Info("TC_PROJECT_004 started");

            await _projectActions.ClickTrackPlanAsync();

            await Assertions.Expect(Page.GetByText("Track Plan")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Track_Plan");
            Logger.Info("TC_PROJECT_004 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_005_Open_Wave_And_Move()
        {
            Logger.Info("TC_PROJECT_005 started");

            await _projectActions.ClickWaveAndMoveGroupingPlanAsync();

            await Assertions.Expect(Page.GetByText("Migration Control Center")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Wave_Move");
            Logger.Info("TC_PROJECT_005 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_007")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_007_Verify_Project_Actions_Menu()
        {
            Logger.Info("TC_PROJECT_007 started");

            await _projectActions.VerifyProjectActionsMenuAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_007_Project_Actions_Menu");
            Logger.Info("TC_PROJECT_007 completed successfully");
        }
    }
}
