using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.MigrationPlanTracking
{
    [Collection("Playwright collection")]
    [AllureSuite("Migration Control Centre Tests")]
    public class MigrationControlCentreTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private MigrationControlCentrePage _migrationControlCentrePage = null!;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _migrationControlCentrePage = new MigrationControlCentrePage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            _createdProjectName = _basicSetup.projectName;
            await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_MigrationControlCentre_Page_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_001_MigrationControlCentre_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                //await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Control Center" }))
                    .ToBeVisibleAsync();
                Logger.Info("Migration Control Centre page loaded");
            }, nameof(TC_001_MigrationControlCentre_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_Navigate_To_Timeline_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_002_Navigate_To_Timeline_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenTimelineTabAsync();
                await Assertions.Expect(Page.GetByText("Wave Execution Schedule"))
                    .ToBeVisibleAsync();
                Logger.Info("Timeline tab loaded");
            }, nameof(TC_002_Navigate_To_Timeline_Tab));
        }

        /*[Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab)]
        [AllureTag("Smoke")]
        public async Task TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
            await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();
            await Assertions.Expect(Page.GetByText("Migration Planning")).ToBeVisibleAsync();
            Logger.Info("Detailed Planning tab loaded");
            }, nameof(TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab));
        }
        */

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_Navigate_To_Manage_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_003_Navigate_To_Manage_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenManageTabAsync();
                await Assertions.Expect(Page.GetByText("Wave Plan Management"))
                    .ToBeVisibleAsync();
                Logger.Info("Manage tab loaded");
            }, nameof(TC_003_Navigate_To_Manage_Tab));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_004_Navigate_To_PoliciesAndRisk_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_004_Navigate_To_PoliciesAndRisk_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenPoliciesAndRiskTabAsync();
                await Assertions.Expect(Page.GetByText("Migration Policies"))
                    .ToBeVisibleAsync();
                Logger.Info("Policies and Risk tab loaded");
            }, nameof(TC_004_Navigate_To_PoliciesAndRisk_Tab));
        }

        #region Breadcrumb Navigation

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Verify_Home_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_001_Verify_Home_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("CMO Architecture");
                await Assertions.Expect(Page.GetByText("CMO Architecture")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to CMO Architecture successful");
            }, nameof(TC_001_Verify_Home_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Verify_ProjectCreation_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_002_Verify_ProjectCreation_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Project Creation");
                await Assertions.Expect(Page.GetByText("Project Creation")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to Project Creation successful");
            }, nameof(TC_002_Verify_ProjectCreation_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_Verify_SSOT_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_003_Verify_SSOT_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("SSOT Integration");
                await Assertions.Expect(Page.GetByText("SSOT Integration")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to SSOT Integration successful");
            }, nameof(TC_003_Verify_SSOT_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Verify_CMO_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_004_Verify_CMO_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("CMO Architecture");
                await Assertions.Expect(Page.GetByText("CMO Architecture")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to CMO Architecture successful");
            }, nameof(TC_004_Verify_CMO_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Verify_FMO_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_005_Verify_FMO_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("FMO Architecture");
                await Assertions.Expect(Page.GetByText("FMO Architecture")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to FMO Architecture successful");
            }, nameof(TC_005_Verify_FMO_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_Inventory_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_Inventory_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("CMO/FMO Inventory");
                await Assertions.Expect(Page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to Inventory Management successful");
            }, nameof(TC_006_Verify_Inventory_Breadcrumb_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Verify_MigrationPlanTracking_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_007_Verify_MigrationPlanTracking_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Migration Plan Tracking");
                await Assertions.Expect(Page.GetByText("Migration Plan Tracking")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to Migration Plan Tracking successful");
            }, nameof(TC_007_Verify_MigrationPlanTracking_Breadcrumb_Navigation));
        }
        
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_MigrationControlCenter_Breadcrumb_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_MigrationControlCenter_Breadcrumb_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Migration Control Center");
                await Assertions.Expect(Page.GetByText("Migration Control Center")).ToBeVisibleAsync(new() { Timeout = 10000 });
                Logger.Info("Breadcrumb navigation to Migration Control Center successful");
            }, nameof(TC_008_Verify_MigrationControlCenter_Breadcrumb_Navigation));
        }

        #endregion

        #region Text Search
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Verify_Valid_TextSearch")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_Valid_TextSearch()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var searchInputText="SRVTEST0"+new Random().Next(10,53);
                await _migrationControlCentrePage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator($"tr:has-text('{searchInputText}')")).ToBeVisibleAsync();
                Logger.Info("Search returned matching items");
            }, nameof(TC_009_Verify_Valid_TextSearch));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Verify_TextSearch_NoResultsFound")]
        [AllureTag("Regression")]
        public async Task TC_010_Verify_TextSearch_NoResultsFound()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var searchInputText="SRVTEST000"+new Random().Next(0,10);
                await _migrationControlCentrePage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.GetByText("No results found", new() { Exact = false })).ToBeVisibleAsync();
                Logger.Info("No results message displayed correctly");
            }, nameof(TC_010_Verify_TextSearch_NoResultsFound));
        }

        #endregion

        #region  Filter Status

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Verify_Filter_Status_Default")]
        [AllureTag("Regression")]
        public async Task TC_011_Verify_Filter_Status_Default()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("All");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); 
                Logger.Info("Filter by All status applied");
            }, nameof(TC_011_Verify_Filter_Status_Default));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Verify_Filter_Status_Planned")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_Filter_Status_Planned()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Planned");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); 
                Logger.Info("Filter by Planned status applied");
            }, nameof(TC_012_Verify_Filter_Status_Planned));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Verify_Filter_Status_Pending")]
        [AllureTag("Regression")]
        public async Task TC_013_Verify_Filter_Status_Pending()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Pending");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); // or assert only Pending rows visible
                Logger.Info("Filter by Pending status applied");
            }, nameof(TC_013_Verify_Filter_Status_Pending));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Verify_Filter_Status_InProgress")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_Filter_Status_InProgress()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("In Progress");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); // or assert only Pending rows visible
                Logger.Info("Filter by In Progress status applied");
            }, nameof(TC_014_Verify_Filter_Status_InProgress));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Verify_Filter_Status_Completed")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_Filter_Status_Completed()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Completed");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); // or assert only Pending rows visible
                Logger.Info("Filter by Completed status applied");
            }, nameof(TC_015_Verify_Filter_Status_Completed));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Verify_Filter_Status_Failed")]
        [AllureTag("Regression")]
        public async Task TC_016_Verify_Filter_Status_Failed()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Failed");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(0); // or assert only Pending rows visible
                Logger.Info("Filter by Failed status applied");
            }, nameof(TC_016_Verify_Filter_Status_Failed));
        }
        #endregion
        
        #region Edit Details
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Verify_EditButtonOperation")]
        [AllureTag("Regression")]
        public async Task TC_017_Verify_EditButtonOperation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();
                await Assertions.Expect(Page.GetByText("Actions")).ToBeVisibleAsync();
                Logger.Info("Filter by All status applied");
            }, nameof(TC_017_Verify_EditButtonOperation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Verify_EditOperation_Status")]
        [AllureTag("Regression")]
        public async Task TC_018_Verify_EditOperation_WaveStatus()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();
                await _basicSetup.WaitForPageAsync(1);
                string waveName = "Wave "+ new Random().Next(1, _migrationControlCentrePage.WaveCount +1);
                await _migrationControlCentrePage.ChangeStatusAsync(waveName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_018_Verify_EditOperation_WaveStatus));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Verify_EditOperation_GroupStatus")]
        [AllureTag("Regression")]
        public async Task TC_019_Verify_EditOperation_GroupStatus()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();
                await _basicSetup.WaitForPageAsync(1);
                string groupName = "Group "+ new Random().Next(1, _migrationControlCentrePage.GroupCount +1);
                await _migrationControlCentrePage.ChangeStatusAsync(groupName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_019_Verify_EditOperation_GroupStatus));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Verify_WaveRescheduleOperation_")]
        [AllureTag("Regression")]
        public async Task TC_019_Verify_WaveRescheduleOperation_()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();
                await _basicSetup.WaitForPageAsync(1);
                string waveName = "Wave "+ new Random().Next(1, _migrationControlCentrePage.WaveCount +1);
                await _migrationControlCentrePage.OpenRescheduleCalendarAsync(waveName);
                await Page.WaitForSelectorAsync("role=dialog:has-text('Reschedule Wave')");
                string selectedDate= await _migrationControlCentrePage.RescheduleOperationAsync();
                await _basicSetup.WaitForPageAsync(2);
                Console.WriteLine($"selectedDate :{selectedDate}, retreivedDate: {await _migrationControlCentrePage.RetreivePlannedStartDateAsync(waveName)}");
                await _migrationControlCentrePage.ChangeStatusAsync(waveName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_019_Verify_WaveRescheduleOperation_));
        }


        #endregion
        /*
        #region Schedule Migration
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_009")]
        [AllureTag("regression")]
        public async Task TC_MCC_009_Open_Schedule_Migration_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await Assertions.Expect(Page.GetByText("Timeline Scheduler")).ToBeVisibleAsync();
                Logger.Info("Schedule Migration modal opened successfully");
            }, nameof(TC_MCC_009_Open_Schedule_Migration_Modal));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_010")]
        [AllureTag("regression")]
        public async Task TC_MCC_010_Select_Future_Project_Start_Date()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.SetProjectStartDateAsync("2026-04-15");
                await Assertions.Expect(_migrationControlCentrePage.ProjectStartDateInput).ToHaveValueAsync("2026-04-15");
                Logger.Info("Future start date selected");
            }, nameof(TC_MCC_010_Select_Future_Project_Start_Date));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_011")]
        [AllureTag("regression")]
        public async Task TC_MCC_011_Add_Manual_Holiday()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.AddManualHolidayAsync("2026-12-25", "Christmas Day", "EMEA");
                var isAdded = await _migrationControlCentrePage.IsHolidayInTableAsync("2026-12-25", "Christmas Day");
                Assert.True(isAdded);
                Logger.Info("Manual holiday added and visible in table");
            }, nameof(TC_MCC_011_Add_Manual_Holiday));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_013")]
        [AllureTag("regression")]
        public async Task TC_MCC_013_Upload_Valid_Holiday_CSV()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.UploadHolidayFileAsync("test-data/holidays-valid.csv");
                await Assertions.Expect(_migrationControlCentrePage.HolidayTableRow).ToHaveCountAsync( 0);
                Logger.Info("Valid CSV holidays uploaded and displayed");
            }, nameof(TC_MCC_013_Upload_Valid_Holiday_CSV));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_015")]
        [AllureTag("regression")]
        public async Task TC_MCC_015_Verify_Format_Help_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.OpenFormatHelpAsync();
                await Assertions.Expect(_migrationControlCentrePage.FormatHelpModal).ToBeVisibleAsync();
                await _migrationControlCentrePage.CloseFormatHelpAsync();
                await Assertions.Expect(_migrationControlCentrePage.FormatHelpModal).Not.ToBeVisibleAsync();
                Logger.Info("Format help modal opened and closed successfully");
            }, nameof(TC_MCC_015_Verify_Format_Help_Modal));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_016")]
        [AllureTag("regression")]
        public async Task TC_MCC_016_Cancel_Scheduler_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.CancelSchedulerAsync();
                await Assertions.Expect(Page.GetByText("Timeline Scheduler")).Not.ToBeVisibleAsync();
                Logger.Info("Scheduler modal cancelled successfully");
            }, nameof(TC_MCC_016_Cancel_Scheduler_Modal));
        }

        #endregion

                [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_005")]
        [AllureTag("regression")]
        public async Task TC_MCC_005_Verify_Filter_Status_All_Statuses_Visible()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Ensure we are on the correct tab/view with multiple statuses visible
                await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();

                // Default should show "All"
                await Assertions.Expect(_migrationControlCentrePage.FilterStatusDropdown)
                    .ToContainTextAsync("All");

                // Verify that items from different statuses are present (assuming test data has variety)
                var rows = await Page.Locator("tr").CountAsync();
                Assert.True(rows > 1, "Multiple migration items should be visible in 'All' filter");

                // Optional: check for presence of known statuses in the table
                await Assertions.Expect(Page.Locator("tr:has-text('Pending')")).ToBeVisibleAsync(new() { Timeout = 8000 });
                Logger.Info("Filter Status 'All' shows items from multiple statuses");
            }, nameof(TC_MCC_005_Verify_Filter_Status_All_Statuses_Visible));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_006")]
        [AllureTag("regression")]
        public async Task TC_MCC_006_Edit_Cutover_Status_Of_Move_Group()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();

                // Assuming first visible group is editable (Pending/Planned)
                string groupName = "Wave 1 Group 1"; // adjust based on your test data
                await _migrationControlCentrePage.ChangeStatusAsync(groupName, "In Progress");

                // Verify the status changed in UI
                await Assertions.Expect(
                    Page.Locator($"tr:has-text('{groupName}') [role='combobox'], tr:has-text('{groupName}') select")
                ).ToContainTextAsync("In Progress");

                Logger.Info("Cutover status of move group successfully changed to In Progress");
            }, nameof(TC_MCC_006_Edit_Cutover_Status_Of_Move_Group));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_007")]
        [AllureTag("regression")]
        public async Task TC_MCC_007_Reschedule_Wave_Future_Date()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();

                string waveName = "Wave 1"; // adjust to match your test data
                await _migrationControlCentrePage.CalendarIconInRow(waveName).ClickAsync();

                // Select a future date (example: 15 days from now)
                string futureDate = DateTime.UtcNow.AddDays(15).ToString("yyyy-MM-dd");
                await Page.Locator($"button[aria-label*='{futureDate}']").ClickAsync(new() { Timeout = 8000 });

                // Assume there's a Save/Confirm button in the date picker or row
                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                await _basicSetup.WaitForPageAsync(1);

                // Verify date updated (partial match is safer)
                await Assertions.Expect(
                    Page.Locator($"tr:has-text('{waveName}') td:has-text('202')")
                ).ToBeVisibleAsync();

                Logger.Info("Wave successfully rescheduled to a future date");
            }, nameof(TC_MCC_007_Reschedule_Wave_Future_Date));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_008")]
        [AllureTag("regression")]
        public async Task TC_MCC_008_Reschedule_Wave_Past_Date_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();

                string waveName = "Wave 1";
                await _migrationControlCentrePage.CalendarIconInRow(waveName).ClickAsync();

                // Try to select a past date (e.g., yesterday)
                string pastDate = DateTime.UtcNow.AddDays(-1).ToString("dd");
                var pastDayButton = Page.Locator($"button[aria-label*='{pastDate}']");

                // Check if past dates are disabled
                bool isDisabled = await pastDayButton.GetAttributeAsync("disabled") != null ||
                                  await pastDayButton.EvaluateAsync<bool>("el => el.classList.contains('disabled') || el.ariaDisabled === 'true'");

                if (!isDisabled)
                {
                    // If not disabled, attempt to click and expect error
                    await pastDayButton.ClickAsync(new() { Timeout = 5000 });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                    // Expect validation error
                    await Assertions.Expect(Page.GetByText("cannot be in the past", new() { Exact = false }))
                        .ToBeVisibleAsync(new() { Timeout = 8000 });
                }
                else
                {
                    Assert.True(isDisabled, "Past dates should be disabled in the calendar");
                }

                Logger.Info("Past date selection correctly blocked or produces validation error");
            }, nameof(TC_MCC_008_Reschedule_Wave_Past_Date_Negative));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_012")]
        [AllureTag("regression")]
        public async Task TC_MCC_012_Add_Manual_Holiday_Missing_Required_Field_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();

                await _migrationControlCentrePage.AddManualHolidayAccordion.ClickAsync();

                // Fill only region — leave date & name empty
                await _migrationControlCentrePage.ManualHolidayRegionDropdown.ClickAsync();
                await _migrationControlCentrePage.ManualHolidayRegionOption("EMEA").ClickAsync();

                // Try to add
                await _migrationControlCentrePage.AddHolidayButton.ClickAsync();

                // Expect validation error (one of these patterns)
                var hasError = await Page.GetByText("required", new() { Exact = false }).IsVisibleAsync() ||
                               await Page.GetByText("Date*", new() { Exact = false }).IsVisibleAsync() ||
                               await Page.Locator("[aria-invalid='true']").IsVisibleAsync();

                Assert.True(hasError, "Validation error should appear when required fields are missing");

                Logger.Info("Attempt to add manual holiday without date/name correctly blocked");
            }, nameof(TC_MCC_012_Add_Manual_Holiday_Missing_Required_Field_Negative));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_014")]
        [AllureTag("regression")]
        public async Task TC_MCC_014_Upload_Invalid_File_Format_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();

                // Try to upload invalid file (e.g. .txt or .pdf)
                // Note: You need to have an invalid file in test-data folder
                string invalidFilePath = "test-data/invalid-holidays.txt";

                var fileChooserTask = Page.WaitForFileChooserAsync();
                await _migrationControlCentrePage.ChooseFileButton.ClickAsync();
                var fileChooser = await fileChooserTask;

                await fileChooser.SetFilesAsync(invalidFilePath);

                // Wait for potential error
                await _basicSetup.WaitForPageAsync(1);

                // Expect error message (adjust text based on real app)
                bool hasError = await Page.GetByText("only .csv or .json", new() { Exact = false }).IsVisibleAsync() ||
                                await Page.GetByText("invalid file", new() { Exact = false }).IsVisibleAsync() ||
                                await Page.GetByText("not supported", new() { Exact = false }).IsVisibleAsync();

                Assert.True(hasError || await _migrationControlCentrePage.HolidayTableRow.CountAsync() == 0,
                    "Invalid file format should be rejected with error or no table update");

                Logger.Info("Invalid file upload correctly rejected");
            }, nameof(TC_MCC_014_Upload_Invalid_File_Format_Negative));
        }
                [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_018")]
        [AllureTag("regression")]
        public async Task TC_MCC_018_Configure_Parameters_Valid_Input()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Assuming parameters are visible after scheduling
                await _migrationControlCentrePage.SetParameterValueAsync("MaxParallel", "10");
                await _migrationControlCentrePage.SetParameterValueAsync("Timeout", "30");

                // Assume there's a Save button
                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                await Assertions.Expect(_migrationControlCentrePage.ParameterInput("MaxParallel"))
                    .ToHaveValueAsync("10");

                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_MCC_018_Configure_Parameters_Valid_Input));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_020")]
        [AllureTag("regression")]
        public async Task TC_MCC_020_Configure_Parameters_Negative_Value()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SetParameterValueAsync("MaxParallel", "-5");

                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                await Assertions.Expect(Page.GetByText("positive", new() { Exact = false }))
                    .ToBeVisibleAsync(new() { Timeout = 8000 });

                Logger.Info("Negative value correctly rejected");
            }, nameof(TC_MCC_020_Configure_Parameters_Negative_Value));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_023")]
        [AllureTag("regression")]
        public async Task TC_MCC_023_Export_Scheduled_Plan()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var downloadTask = Page.WaitForDownloadAsync();
                await _migrationControlCentrePage.ExportPlanAsync();
                var download = await downloadTask;

                Assert.NotNull(download);
                Assert.EndsWith(".xlsx", download.SuggestedFilename);

                Logger.Info("Migration plan exported as Excel successfully");
            }, nameof(TC_MCC_023_Export_Scheduled_Plan));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_024")]
        [AllureTag("regression")]
        public async Task TC_MCC_024_Verify_Statistics_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(_migrationControlCentrePage.StatisticsPanel)
                    .ToBeVisibleAsync();

                await Assertions.Expect(Page.GetByText("servers", new() { Exact = false }))
                    .ToBeVisibleAsync(new() { Timeout = 8000 });

                Logger.Info("Statistics panel displays server/wave/group counts");
            }, nameof(TC_MCC_024_Verify_Statistics_Display));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_026")]
        [AllureTag("regression")]
        public async Task TC_MCC_026_Expand_Wave_In_Timeline()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.TimelineTab.ClickAsync();
                await _migrationControlCentrePage.ExpandWaveInTimelineAsync("Wave 1");

                await Assertions.Expect(Page.GetByText("Group 1", new() { Exact = false }))
                    .ToBeVisibleAsync();

                Logger.Info("Wave expanded and groups visible in timeline");
            }, nameof(TC_MCC_026_Expand_Wave_In_Timeline));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_028")]
        [AllureTag("regression")]
        public async Task TC_MCC_028_Move_Group_To_Different_Wave()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.MoveGroupToWaveAsync("Wave 1 Group 1", "Wave 2");

                await Assertions.Expect(
                    Page.Locator("tr:has-text('Wave 2'):has-text('Group 1')")
                ).ToBeVisibleAsync();

                Logger.Info("Group successfully moved to another wave");
            }, nameof(TC_MCC_028_Move_Group_To_Different_Wave));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_029")]
        [AllureTag("regression")]
        public async Task TC_MCC_029_Rollback_Backlogged_Group()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Assume group is already backlogged
                await _migrationControlCentrePage.RollbackGroupAsync("Wave 1 Group 1", "Wave 1");

                await Assertions.Expect(
                    Page.Locator("tr:has-text('Wave 1'):has-text('Group 1')")
                ).ToBeVisibleAsync();

                Logger.Info("Backlogged group rolled back successfully");
            }, nameof(TC_MCC_029_Rollback_Backlogged_Group));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_030")]
        [AllureTag("regression")]
        public async Task TC_MCC_030_Backlog_Group_Via_Checkbox()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.BacklogGroupAsync("Wave 1 Group 1");

                await Assertions.Expect(
                    Page.Locator("tr:has-text('Group 1') [aria-label*='backlog'], tr:has-text('Group 1') [class*='backlog']")
                ).ToBeVisibleAsync(new() { Timeout = 10000 });

                Logger.Info("Group successfully marked as backlogged");
            }, nameof(TC_MCC_030_Backlog_Group_Via_Checkbox));
        }

                [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_019")]
        [AllureTag("regression")]
        public async Task TC_MCC_019_Configure_Parameters_Zero_Value_Edge()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Navigate to parameters section (adjust tab if needed)
                await Page.GetByText("Configure Parameters", new() { Exact = false }).ClickAsync(new() { Timeout = 10000 });

                // Example parameter - adjust name based on real UI
                string paramName = "ParallelMigrations"; // or "BatchSize", etc.
                await _migrationControlCentrePage.SetParameterValueAsync(paramName, "0");

                // Attempt to save (assume there's a Save button)
                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync(new() { Timeout = 8000 });

                // Check result - depending on business rule
                bool isAccepted = await _migrationControlCentrePage.ParameterInput(paramName)
                    .EvaluateAsync<bool>("el => el.value === '0'");

                if (isAccepted)
                {
                    Logger.Info("Zero value accepted for parameter (business rule allows it)");
                }
                else
                {
                    await Assertions.Expect(Page.GetByText("must be greater than zero", new() { Exact = false }))
                        .ToBeVisibleAsync(new() { Timeout = 8000 });
                    Logger.Info("Zero value correctly rejected with validation error");
                }
            }, nameof(TC_MCC_019_Configure_Parameters_Zero_Value_Edge));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_021")]
        [AllureTag("regression")]
        public async Task TC_MCC_021_Configure_Parameters_Non_Integer_Input_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Page.GetByText("Configure Parameters", new() { Exact = false }).ClickAsync(new() { Timeout = 10000 });

                string paramName = "TimeoutMinutes"; // adjust to real parameter name
                await _migrationControlCentrePage.SetParameterValueAsync(paramName, "12.5"); // decimal

                // Try save
                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                // Expect rejection or field correction
                var inputValue = await _migrationControlCentrePage.ParameterInput(paramName).InputValueAsync();
                bool hasError = await Page.GetByText("integer", new() { Exact = false }).IsVisibleAsync() ||
                                await Page.GetByText("whole number", new() { Exact = false }).IsVisibleAsync() ||
                                inputValue == "12" || inputValue == ""; // common behaviors: truncate or clear

                Assert.True(hasError || inputValue != "12.5",
                    "Non-integer input should be rejected or corrected");

                Logger.Info("Non-integer value correctly handled/rejected");
            }, nameof(TC_MCC_021_Configure_Parameters_Non_Integer_Input_Negative));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_022")]
        [AllureTag("regression")]
        public async Task TC_MCC_022_Configure_Parameters_Maximum_Value_Edge()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Page.GetByText("Configure Parameters", new() { Exact = false }).ClickAsync(new() { Timeout = 10000 });

                string paramName = "MaxRetries"; // adjust to real parameter
                string maxTestValue = "999999";

                await _migrationControlCentrePage.SetParameterValueAsync(paramName, maxTestValue);

                await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

                // Two possible outcomes: accepted or rejected with max limit error
                var currentValue = await _migrationControlCentrePage.ParameterInput(paramName).InputValueAsync();

                if (currentValue == maxTestValue)
                {
                    Logger.Info("Large integer value accepted (within allowed range)");
                }
                else
                {
                    await Assertions.Expect(Page.GetByText("maximum", new() { Exact = false })).ToBeVisibleAsync(new() { Timeout = 8000 });

                    Logger.Info("Value exceeding maximum correctly rejected");
                }
            }, nameof(TC_MCC_022_Configure_Parameters_Maximum_Value_Edge));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_025")]
        [AllureTag("regression")]
        public async Task TC_MCC_025_Navigate_To_Timeline_Tab_Verify_Linear_View()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Navigate to Timeline tab
                await _migrationControlCentrePage.TimelineTab.ClickAsync();

                // Verify heading or key content
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Execution Schedule",
                    Exact = false
                })).ToBeVisibleAsync(new() { Timeout = 10000 });

                // Check for timeline-specific visual element (Gantt-like or bar chart)
                await Assertions.Expect(Page.Locator(".timeline-container, .gantt-chart, [role='img'], svg"))
                    .ToBeVisibleAsync(new() { Timeout = 15000 });

                Logger.Info("Timeline tab opened and linear schedule visualization is visible");
            }, nameof(TC_MCC_025_Navigate_To_Timeline_Tab_Verify_Linear_View));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_MCC_027")]
        [AllureTag("regression")]
        public async Task TC_MCC_027_Navigate_To_Wave_Plan_Management_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Navigate to Manage tab
                await _migrationControlCentrePage.ManageTab.ClickAsync();

                // Verify tab content loaded
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Plan Management",
                    Exact = false
                })).ToBeVisibleAsync(new() { Timeout = 10000 });

                // Check for presence of wave/group management controls
                await Assertions.Expect(Page.Locator("tr, div.wave-item, button:has-text('Move'), [aria-label*='move']"))
                    .ToBeVisibleAsync(new() { Timeout = 10000 });

                Logger.Info("Wave Plan Management tab loaded successfully with management options");
            }, nameof(TC_MCC_027_Navigate_To_Wave_Plan_Management_Tab));
        }
        */
    }
}