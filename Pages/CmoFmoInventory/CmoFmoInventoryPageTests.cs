using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.CmoFmoInventory
{
    [Collection("Playwright collection")]
    [AllureSuite("CMO/FMO Inventory page")]
    public class CmoFmoInventoryPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private CmoFmoInventoryPage _cmoFmoInventoryPage = null!;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _cmoFmoInventoryPage = new CmoFmoInventoryPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            _createdProjectName = _basicSetup.projectName;
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Navigate_To_Inventory_Page")]
        [AllureTag("Regression")]
        public async Task TC_001_Navigate_To_Inventory_Page()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                await _cmoFmoInventoryPage.NavigateToMigrationPlanTrackingAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.InventoryHeading).ToBeVisibleAsync();

                Logger.Info("CMO/FMO Inventory page loaded successfully");

            }, nameof(TC_001_Navigate_To_Inventory_Page));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Back_To_FMO_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_002_Back_To_FMO_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.BackToFMOButton.ClickAsync();
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();

                Logger.Info("Navigated back to FMO page successfully");

            }, nameof(TC_002_Back_To_FMO_Should_Work));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_Server_View_Should_Load")]
        [AllureTag("Regression")]
        public async Task TC_003_Server_View_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.ServerViewButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.ServerInventoryHeading).ToBeVisibleAsync();

                Logger.Info("Server Inventory loaded successfully");

            }, nameof(TC_003_Server_View_Should_Load));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Application_View_Should_Load")]
        [AllureTag("Regression")]
        public async Task TC_004_Application_View_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.ApplicationViewButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.ApplicationInventoryHeading).ToBeVisibleAsync();

                Logger.Info("Application Inventory loaded successfully");

            }, nameof(TC_004_Application_View_Should_Load));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Export_Button_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_005_Export_Button_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.ExportButton).ToBeEnabledAsync();
                await _cmoFmoInventoryPage.ExportButton.ClickAsync();

                Logger.Info("Export button clicked successfully");

            }, nameof(TC_005_Export_Button_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Sync_Button_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_006_Sync_Button_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.SyncButton).ToBeEnabledAsync();
                await _cmoFmoInventoryPage.SyncButton.ClickAsync();

                Logger.Info("Sync button clicked successfully");

            }, nameof(TC_006_Sync_Button_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Filter_Panel_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_007_Filter_Panel_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.FilterOptionsHeading).ToBeVisibleAsync();

                Logger.Info("Filter panel opened successfully");

            }, nameof(TC_007_Filter_Panel_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Filter_Application_Name_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_008_Filter_Application_Name_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await _cmoFmoInventoryPage.FilterByAppNameTextbox.ClickAsync();
                await _basicSetup.RandomSelector();

                Logger.Info("Application name filter applied using random selection");

            }, nameof(TC_008_Filter_Application_Name_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Clear_All_Filters_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_009_Clear_All_Filters_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoFmoInventoryPage.NavigateToInventoryPageAsync();
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await _cmoFmoInventoryPage.FilterByAppNameTextbox.ClickAsync();
                await _basicSetup.RandomSelector();
                await _cmoFmoInventoryPage.ClearAllButton.ClickAsync();

                Logger.Info("Clear All filters clicked successfully");

            }, nameof(TC_009_Clear_All_Filters_Should_Work));
        }

    }
}
