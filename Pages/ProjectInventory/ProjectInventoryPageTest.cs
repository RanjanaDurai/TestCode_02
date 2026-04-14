using Microsoft.Playwright;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Project;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.ProjectInventory
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Inventory Tests")]
    public class ProjectInventoryPageTests : TestBaseFixture, IAsyncLifetime
    {
        // private BasicSetup _basicSetup = null!;
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private ProjectInventoryPage _projectInventoryPage = null!;
        protected override bool UseClassLevelGrouping => true;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _projectInventoryPage = new ProjectInventoryPage(Page);

            await NavigateToPageAsync("/inventory");
            _createdProjectName = _basicSetup.projectName;

            Logger.Info("Project dashboard ready for Project Actions tests");
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_150_Inventory_Page_Should_Load_Sucessfully")]
        [AllureTag("Smoke")]
        public async Task TC_150_Inventory_Page_Should_Load_Sucessfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(
                    Page.GetByRole(AriaRole.Heading, new() { Name = "CMO/FMO Inventory" })
                ).ToBeVisibleAsync();
                await _basicSetup.WaitForPageAsync(2);
                Logger.Info("TC_Inventory_001 completed successfully");
            }, nameof(TC_150_Inventory_Page_Should_Load_Sucessfully));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_151_Verify_Migration_Control_Center_Flow")]
        [AllureTag("Smoke")]
        public async Task TC_151_Verify_Migration_Control_Center_Flow()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _basicSetup.WaitForPageAsync(2);
                await _projectInventoryPage.InitiateMigrationFromInventoryAsync();
                await Assertions.Expect(
                    Page.GetByRole(AriaRole.Heading, new() { Name = "M*(xwhvigration Plan Tracking" })
                ).ToBeVisibleAsync();

                Logger.Info("TC_Inventory_151 verification of Migration Control Center flow completed successfully");
            }, nameof(TC_151_Verify_Migration_Control_Center_Flow));
        }
    }
}