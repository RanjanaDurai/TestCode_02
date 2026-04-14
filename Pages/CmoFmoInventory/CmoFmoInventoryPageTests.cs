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
        protected override bool UseClassLevelGrouping => true;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _cmoFmoInventoryPage = new CmoFmoInventoryPage(Page);

            await NavigateToPageAsync("/inventory");
            _createdProjectName = _basicSetup.projectName;
        }

    
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_152_Navigate_To_Inventory_Page")]
        [AllureTag("Regression")]
        public async Task TC_152_Navigate_To_Inventory_Page()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                await Assertions.Expect(_cmoFmoInventoryPage.InventoryHeading).ToBeVisibleAsync();

                Logger.Info("CMO/FMO Inventory page loaded successfully");

            }, nameof(TC_152_Navigate_To_Inventory_Page));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_153_Back_To_FMO_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_153_Back_To_FMO_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoFmoInventoryPage.BackToFMOButton.ClickAsync();
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();

                Logger.Info("Navigated back to FMO page successfully");

            }, nameof(TC_153_Back_To_FMO_Should_Work));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_154_Server_View_Should_Load")]
        [AllureTag("Regression")]
        public async Task TC_154_Server_View_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                
                await _cmoFmoInventoryPage.ServerViewButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.ServerInventoryHeading).ToBeVisibleAsync();

                Logger.Info("Server Inventory loaded successfully");

            }, nameof(TC_154_Server_View_Should_Load));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_155_Application_View_Should_Load")]
        [AllureTag("Regression")]
        public async Task TC_155_Application_View_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoFmoInventoryPage.ApplicationViewButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.ApplicationInventoryHeading).ToBeVisibleAsync();

                Logger.Info("Application Inventory loaded successfully");

            }, nameof(TC_155_Application_View_Should_Load));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_156_Export_Button_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_156_Export_Button_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(_cmoFmoInventoryPage.ExportButton).ToBeEnabledAsync();
                await _cmoFmoInventoryPage.ExportButton.ClickAsync();

                Logger.Info("Export button clicked successfully");

            }, nameof(TC_156_Export_Button_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_157_Sync_Button_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_157_Sync_Button_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(_cmoFmoInventoryPage.SyncButton).ToBeEnabledAsync();
                await _cmoFmoInventoryPage.SyncButton.ClickAsync();

                Logger.Info("Sync button clicked successfully");

            }, nameof(TC_157_Sync_Button_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_158_Filter_Panel_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_158_Filter_Panel_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await Assertions.Expect(_cmoFmoInventoryPage.FilterOptionsHeading).ToBeVisibleAsync();

                Logger.Info("Filter panel opened successfully");

            }, nameof(TC_158_Filter_Panel_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_159_Filter_Application_Name_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_159_Filter_Application_Name_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await _cmoFmoInventoryPage.FilterByAppNameTextbox.ClickAsync();
                await _basicSetup.RandomSelector();

                Logger.Info("Application name filter applied using random selection");

            }, nameof(TC_159_Filter_Application_Name_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_160_Clear_All_Filters_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_160_Clear_All_Filters_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoFmoInventoryPage.FilterButton.ClickAsync();
                await _cmoFmoInventoryPage.FilterByAppNameTextbox.ClickAsync();
                await _basicSetup.RandomSelector();
                await _cmoFmoInventoryPage.ClearAllButton.ClickAsync();

                Logger.Info("Clear All filters clicked successfully");

            }, nameof(TC_160_Clear_All_Filters_Should_Work));
        }

    }
}
