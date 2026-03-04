using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ClientModeOperation;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.FutureModeOperation
{
    [Collection("Playwright collection")]
    [AllureSuite("Future Mode Operation Tests")]
    public class FutureModeOpertionPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private FutureModeOperationPage _fmoPage = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper=new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
            _fmoPage = new FutureModeOperationPage(Page);


            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            _createdProjectName = _basicSetup.projectName;

            Logger.Info("Logged in as Super Admin and created a basic project");

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_FMO_Page_UI_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_001_FMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                Logger.Info("FMO Page UI loaded successfully");
            }, nameof(TC_001_FMO_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("smoke")]
        public async Task TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
                Logger.Info("FMO Page has clickable navigation buttons");
            }, nameof(TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons));
        }

        // REGRESSION TEST 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_FMO_Zoom_In_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_003_FMO_Zoom_In_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.Diagram).ToBeVisibleAsync();
                await Assertions.Expect(_fmoPage.ZoomInButton).ToBeEnabledAsync();

                await _fmoPage.ZoomInButton.ClickAsync();

                Logger.Info("Zoom In button clicked successfully");
            }, nameof(TC_003_FMO_Zoom_In_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_FMO_Zoom_Out_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_004_FMO_Zoom_Out_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.ZoomOutButton).ToBeEnabledAsync();

                await _fmoPage.ZoomOutButton.ClickAsync();

                Logger.Info("Zoom Out button clicked successfully");
            }, nameof(TC_004_FMO_Zoom_Out_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_FMO_Reset_Zoom_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_005_FMO_Reset_Zoom_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.ResetZoomButton).ToBeEnabledAsync();

                await _fmoPage.ResetZoomButton.ClickAsync();

                Logger.Info("Reset Zoom button clicked successfully");
            }, nameof(TC_005_FMO_Reset_Zoom_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_FMO_Show_Legend_Should_Open")]
        [AllureTag("regression")]
        public async Task TC_006_FMO_Show_Legend_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ShowLegendButton.ClickAsync();

                await Assertions.Expect(_fmoPage.CloseLegendButton).ToBeVisibleAsync();

                Logger.Info("Legend opened successfully");
            }, nameof(TC_006_FMO_Show_Legend_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_FMO_Close_Legend_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_007_FMO_Close_Legend_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ShowLegendButton.ClickAsync();
                await _fmoPage.CloseLegendButton.ClickAsync();

                Logger.Info("Legend closed successfully");
            }, nameof(TC_007_FMO_Close_Legend_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_FMO_Search_Box_Should_Open")]
        [AllureTag("regression")]
        public async Task TC_008_FMO_Search_Box_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.SearchButton.ClickAsync();

                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                Logger.Info("Search textbox displayed");
            }, nameof(TC_008_FMO_Search_Box_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_FMO_Search_Should_Accept_Input")]
        [AllureTag("regression")]
        public async Task TC_009_FMO_Search_Should_Accept_Input()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.SearchButton.ClickAsync();
                await _fmoPage.SearchTextbox.FillAsync("svr");

                await Assertions.Expect(_fmoPage.SearchTextbox).ToHaveValueAsync("svr");

                Logger.Info("Search input accepted successfully");
            }, nameof(TC_009_FMO_Search_Should_Accept_Input));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_FMO_View_Migration_Cost_Should_Open_Modal")]
        [AllureTag("regression")]
        public async Task TC_010_FMO_View_Migration_Cost_Should_Open_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                Logger.Info("Migration cost modal opened");
            }, nameof(TC_010_FMO_View_Migration_Cost_Should_Open_Modal));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_FMO_One_Year_Reserved_Should_Work")]
        [AllureTag("sregression")]
        public async Task TC_011_FMO_One_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.OneYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.OneYearReserved).ToBeVisibleAsync();

                Logger.Info("1 Year Reserved selected");
            }, nameof(TC_011_FMO_One_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_FMO_Three_Year_Reserved_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_012_FMO_Three_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.ThreeYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.ThreeYearReserved).ToBeVisibleAsync();

                Logger.Info("3 Year Reserved selected");
            }, nameof(TC_012_FMO_Three_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_FMO_Close_Migration_Cost_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_013_FMO_Close_Migration_Cost_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.CloseMigrationCostButton.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).Not.ToBeVisibleAsync();

                Logger.Info("Migration cost modal closed successfully");
            }, nameof(TC_013_FMO_Close_Migration_Cost_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_FMO_Export_To_Excel_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_014_FMO_Export_To_Excel_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.ExportToExcelButton).ToBeEnabledAsync();
                await _fmoPage.ExportToExcelButton.ClickAsync();

                Logger.Info("Export to Excel button clicked");
            }, nameof(TC_014_FMO_Export_To_Excel_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_FMO_Sync_Data_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_015_FMO_Sync_Data_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.SyncDataButton).ToBeEnabledAsync();
                await _fmoPage.SyncDataButton.ClickAsync();

                Logger.Info("Sync data button clicked");
            }, nameof(TC_015_FMO_Sync_Data_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_FMO_Select_Time_Period_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_016_FMO_Select_Time_Period_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                await _fmoPage.ChooseTimePeriod.ClickAsync();

                // selecting random dropdown option
                await _basicSetup.RandomSelector();

                Logger.Info("Time Period filter selected successfully");
            }, nameof(TC_016_FMO_Select_Time_Period_Should_Work));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_FMO_Fullscreen_Should_Activate")]
        [AllureTag("regression")]
        public async Task TC_017_FMO_Fullscreen_Should_Activate()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await Assertions.Expect(_fmoPage.FullScreenButton).ToBeEnabledAsync();
                await _fmoPage.FullScreenButton.ClickAsync();

                Logger.Info("Full screen activated");
            }, nameof(TC_017_FMO_Fullscreen_Should_Activate));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_FMO_Fullscreen_Should_Exit_On_ESC")]
        [AllureTag("regression")]
        public async Task TC_018_FMO_Fullscreen_Should_Exit_On_ESC()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.FullScreenButton.ClickAsync();

                // Press ESC to exit fullscreen
                await Page.Keyboard.PressAsync("Escape");

                Logger.Info("Exited full screen using ESC key");
            }, nameof(TC_018_FMO_Fullscreen_Should_Exit_On_ESC));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_FMO_Search_And_Select_Random_Server_Should_Work")]
        [AllureTag("regression")]
        public async Task TC_019_FMO_Search_And_Select_Random_Server_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();

                await _fmoPage.SearchButton.ClickAsync();

                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                await _fmoPage.SearchTextbox.FillAsync("SRV");
                
                // await Page.Locator("#fullscreen-container div").Filter(new() { HasText = "Foun" }).Nth(3).ClickAsync();
                var container = Page.Locator("#fullscreen-container div")
                .Filter(new() { HasText = "Found" })
                .Nth(3);

            await Assertions.Expect(container).ToBeVisibleAsync();
            await _fmoPage.RandomSelector();

                Logger.Info("Search filter applied and random server selected");
            }, nameof(TC_019_FMO_Search_And_Select_Random_Server_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_FMO_Search_Invalid_Value_Should_Show_No_Results")]
        [AllureTag("regression")]
        public async Task TC_020_FMO_Search_Invalid_Value_Should_Show_No_Results()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.NavigateToFMOPageAsync();
                await _fmoPage.SearchButton.ClickAsync();
                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                string invalidSearch = "uisiy43265125748ugfbhjfdhjvbj";
                await _fmoPage.SearchTextbox.FillAsync(invalidSearch);

                var options = Page.Locator(".mud-list-item:not(.mud-disabled)");
                await Page.WaitForTimeoutAsync(1000);
                int count = await options.CountAsync();

                Assert.True(count == 0, 
                    $"Expected no search results, but found {count} results.");

                Logger.Info("Invalid search correctly returned no results");
            }, nameof(TC_020_FMO_Search_Invalid_Value_Should_Show_No_Results));
        }
    }
}