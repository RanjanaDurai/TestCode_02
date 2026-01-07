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
    public class ProjectInventoryPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LandingPage _landingPage = null!;
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private ProjectInventoryPage _ProjectInventory = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _landingPage = new LandingPage(Page);
            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _ProjectInventory = new ProjectInventoryPage(Page);

            await _landingPage.NavigateToLandingPageAsync();
            await _landingPage.ClickGenerateMigrationPlanAsync();
            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _projectList.WaitForProjectDashboardIconsAsync();

            Logger.Info("Project dashboard ready for Project Actions tests");
        }

        public new Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        [AllureOwner("TC_Inventory_001")]
        [AllureTag("smoke")]
        public async Task TC_Inventory_001_Initiate_Migration_From_Inventory()
        {
            Logger.Info("TC_Inventory_001 started");

            await _ProjectInventory.ClickInventorySpecificationsAsync();

            await Assertions.Expect(Page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync();

            await Task.Delay(15000);

            await _ProjectInventory.InitiateMigrationFromInventoryAsync();

            await Assertions.Expect(
                Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Plan Tracking" })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_008_Migration_Plan_Created");

            Logger.Info("TC_Inventory_001 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_Inventory_002")]
        [AllureTag("smoke")]
        public async Task TC_Inventory_002_Verify_Migration_Control_Center_Flow()
        {
            Logger.Info("TC_Inventory_002 started");

            await _projectList.ClickInventorySpecificationsAsync();

            await Assertions.Expect(Page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync();

            await Task.Delay(15000);

            await _ProjectInventory.InitiateMigrationFromInventoryAsync();

            await _ProjectInventory.ClickViewPlanDetailsAsync();

            await _ProjectInventory.VerifyMigrationControlCenterLoadedAsync();

            await _ProjectInventory.OpenDetailedPlanningTabAsync();
            await _ProjectInventory.OpenTimelineTabAsync();
            await _ProjectInventory.OpenManageTabAsync();
            await _ProjectInventory.OpenPoliciesAndRiskTabAsync();

            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_Inventory_002_Migration_Control_Center_Flow"
            );

            Logger.Info("TC_Inventory_002 completed successfully");
        }
    }
}