using Microsoft.Playwright;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.Project
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Inventory Tests")]
    public class ProjectInventoryPageTests : TestBaseFixture, IAsyncLifetime
    {
        private BasicSetup _basicSetup = null!;
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private ProjectInventoryPage _projectInventoryPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _projectInventoryPage = new ProjectInventoryPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();

            Logger.Info("Project dashboard ready for Project Actions tests");
        }

        public new Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        [AllureOwner("TC_Inventory_001")]
        [AllureTag("smoke")]
        public async Task TC_Inventory_001_Inventory_Page_Should_Load_Sucessfully()
        {
            await _projectInventoryPage.NavigateToInventoryPageAsync();
            await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "CMO/FMO Inventory" })).ToBeVisibleAsync();
            await _basicSetup.WaitForPageAsync(2);
            Logger.Info("TC_Inventory_001 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_Inventory_002")]
        [AllureTag("smoke")]
        public async Task TC_Inventory_002_Verify_Migration_Control_Center_Flow()
        {
            Logger.Info("TC_Inventory_002 started");

            await _projectInventoryPage.NavigateToInventoryPageAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _projectInventoryPage.InitiateMigrationFromInventoryAsync();
            await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Plan Tracking" })).ToBeVisibleAsync();
            Logger.Info("TC_Inventory_002 verification of Migration Control Center flow completed successfully");

        }
    }
}