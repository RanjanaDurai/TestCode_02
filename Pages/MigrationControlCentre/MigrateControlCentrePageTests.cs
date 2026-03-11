using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;
using Allure.Xunit.Attributes.Steps;


namespace WiseMigrateTests.Pages.MigrationControlCentre
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
                string status =await _migrationControlCentrePage.ChangeStatusAsync(waveName);
                await _migrationControlCentrePage.SaveEditsAsync(waveName);
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
                await _migrationControlCentrePage.SaveEditsAsync(groupName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_019_Verify_EditOperation_GroupStatus));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Verify_WaveRescheduleOperation_")]
        [AllureTag("Regression")]
        public async Task TC_020_Verify_WaveRescheduleOperation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();
                await _basicSetup.WaitForPageAsync(1);
                string waveName = "Wave "+ new Random().Next(1, _migrationControlCentrePage.WaveCount +1);
                await _migrationControlCentrePage.OpenRescheduleCalendarAsync(waveName);
                await Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Reschedule Wave"}).WaitForAsync(new() { State = WaitForSelectorState.Visible });
                string selectedDate= await _migrationControlCentrePage.RescheduleOperationAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _migrationControlCentrePage.SaveEditsAsync(waveName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_020_Verify_WaveRescheduleOperation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_WaveRescheduleOperation__PastDate")]
        [AllureTag("Regression")]
        public async Task TC_021_WaveRescheduleOperation__PastDate()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();

                string waveName = "Wave 1";
                await _migrationControlCentrePage.OpenRescheduleCalendarAsync(waveName);
                string pastDate = DateTime.UtcNow.AddDays(-1).ToString("dd");
                var pastDayButton = Page.Locator($"button[aria-label*='{pastDate}']");
                bool isDisabled = await pastDayButton.GetAttributeAsync("disabled") != null ||
                                  await pastDayButton.EvaluateAsync<bool>("el => el.classList.contains('disabled') || el.ariaDisabled === 'true'");

                if (!isDisabled)
                {
                    await pastDayButton.ClickAsync(new() { Timeout = 5000 });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                    await Assertions.Expect(Page.GetByText("Planned Start Date cannot be in the past", new() { Exact = false }))
                        .ToBeVisibleAsync(new() { Timeout = 8000 });
                }
                else
                {
                    Assert.True(isDisabled);
                    Logger.Info("Past date selection allowed for Planned Start Date");
                }

                Logger.Info("Past date selection correctly blocked or produces validation error");
            }, nameof(TC_021_WaveRescheduleOperation__PastDate));
        }


        #endregion
        
        #region Schedule Migration
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_022_Open_Schedule_Migration")]
        [AllureTag("Rregression")]
        public async Task TC_022_Open_Schedule_Migration()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Reschedule Wave"})).ToBeVisibleAsync();
                Logger.Info("Schedule Migration modal opened successfully");
            }, nameof(TC_022_Open_Schedule_Migration));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_023_ScheduleMigration_ProjectStartDate")]
        [AllureTag("Regression")]
        public async Task TC_023_ScheduleMigration_ProjectStartDate()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.SetRandomProjectStartDateAsync();
                await _migrationControlCentrePage.GenerateTimelineScheduleAsync();
                await _basicSetup.WaitForPageAsync(1);
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Migration schedule generated scheduled successfully.");
                Logger.Info("Future start date selected");
            }, nameof(TC_023_ScheduleMigration_ProjectStartDate));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_024_Verify_AddManualHoliday")]
        [AllureTag("Regression")]
        public async Task TC_024_Verify_AddManualHoliday()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.AddManualHolidayAsync("2026-12-25", "Christmas Day", "EMEA");
                var isAdded = await _migrationControlCentrePage.IsHolidayInTableAsync("2026-12-25", "Christmas Day");
                Assert.True(isAdded);
                Logger.Info("Manual holiday added and visible in table");
            }, nameof(TC_024_Verify_AddManualHoliday));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_Add_Manual_Holiday_Missing_Required_Field_Negative")]
        [AllureTag("Regression")]
        public async Task TC_025_Add_Manual_Holiday_Missing_Required_Field_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();

                await _migrationControlCentrePage.AddManualHolidayButton.ClickAsync();
                await _migrationControlCentrePage.ManualHolidayRegionDropdown.ClickAsync();
                await _migrationControlCentrePage.ManualHolidayRegionOption("EMEA").ClickAsync();
                await _migrationControlCentrePage.AddHolidayButton.ClickAsync();
                var hasError = await Page.GetByText("required", new() { Exact = false }).IsVisibleAsync() ||
                               await Page.GetByText("Date*", new() { Exact = false }).IsVisibleAsync() ||
                               await Page.Locator("[aria-invalid='true']").IsVisibleAsync();

                Assert.True(hasError, "Validation error should appear when required fields are missing");

                Logger.Info("Attempt to add manual holiday without date/name correctly blocked");
            }, nameof(TC_025_Add_Manual_Holiday_Missing_Required_Field_Negative));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_026_Upload_Valid_HolidayFile")]
        [AllureTag("Regression")]
        public async Task TC_026_Upload_Valid_HolidayFile()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.HoliDayFileUploadOperationAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Row)).ToBeVisibleAsync();
                Logger.Info("Valid CSV holidays uploaded and displayed");
            }, nameof(TC_026_Upload_Valid_HolidayFile));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_027_Upload_Invalid_File_Format_Negative")]
        [AllureTag("Regression")]
        public async Task TC_027_Upload_Invalid_File_Format_Negative()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                string invalidFilePath = FileUploadHelper.GetInvalidFile();

                var fileChooserTask = Page.WaitForFileChooserAsync();
                await Page.GetByRole(AriaRole.Button, new(){Name="Choose File"}).ClickAsync();
                await Page.SetInputFilesAsync("input[id='holiday-file']", invalidFilePath);   
                await _basicSetup.WaitForPageAsync(1);
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Input document is not in the expected format.");
                Assert.True(await Page.GetByRole(AriaRole.Row).CountAsync() == 0);
                Logger.Info("Invalid file upload correctly rejected");
            }, nameof(TC_027_Upload_Invalid_File_Format_Negative));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_028_Verify_FormatHelpAccess")]
        [AllureTag("Regression")]
        public async Task TC_028_Verify_FormatHelpAccess()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.OpenFormatHelpAsync();
                await Assertions.Expect(_migrationControlCentrePage.FormatHelpDialog).ToBeVisibleAsync();
                await _migrationControlCentrePage.CloseFormatHelpAsync();
                await Assertions.Expect(_migrationControlCentrePage.FormatHelpDialog).Not.ToBeVisibleAsync();
                Logger.Info("Verified Format help access successfully");
            }, nameof(TC_028_Verify_FormatHelpAccess));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_029_Cancel_ScheduleMigration")]
        [AllureTag("Regression")]
        public async Task TC_029_Cancel_ScheduleMigration()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenScheduleMigrationModalAsync();
                await _migrationControlCentrePage.CancelSchedulerAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Timeline Scheduler"})).Not.ToBeVisibleAsync();
                Logger.Info("Migration scheduling cancelled successfully");
            }, nameof(TC_029_Cancel_ScheduleMigration));
        }

        #endregion

        #region Parameter Configuration
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_030__Verify_ParameterConfgurationSection")]
        [AllureTag("Regression")]
        public async Task TC_030__Verify_ParameterConfgurationSection()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenParameterConfiguration();
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new(){Name="Configure and Generate"})).ToBeVisibleAsync();
                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_030__Verify_ParameterConfgurationSection));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_031_ConfigureParameters_ValidInput")]
        [AllureTag("Regression")]
        public async Task TC_031_ConfigureParameters_ValidInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenParameterConfiguration();
                await Page.WaitForSelectorAsync("text=Configure and Generate Migration Plan");
                await _migrationControlCentrePage.SetParameterValueAsync();
                await _migrationControlCentrePage.GenerateMigrationScheduleAsync();
                await Assertions.Expect(Page.GetByText("Migration plan generated successfully")).ToBeVisibleAsync();
                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_031_ConfigureParameters_ValidInput));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_032_ConfigureParameters_InValidInput")]
        [AllureTag("Regression")]
        public async Task TC_032_ConfigureParameters_InValidInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenParameterConfiguration();
                await Page.WaitForSelectorAsync("text=Configure and Generate Migration Plan");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Servers per Move",0);
                await _migrationControlCentrePage.SetParameterValueAsync("Expected Servers per Move",4);
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Servers per Move Group",0);
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Move Groups per Wave*",0);
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Move Groups per Wave*",2);
                await _migrationControlCentrePage.GenerateMigrationScheduleAsync();
                await Assertions.Expect(Page.Locator("[class='me-auto']")).ToContainTextAsync("The input value cannot be lesser than the deafult value.");
                await Assertions.Expect(Page.GetByText("Migration plan generated successfully")).ToBeVisibleAsync();
                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_032_ConfigureParameters_InValidInput));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_033_Export_Scheduled_Plan")]
        [AllureTag("regression")]
        public async Task TC_033_Export_Scheduled_Plan()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var downloadTask = Page.WaitForDownloadAsync();
                await _migrationControlCentrePage.ExportPlanAsync();
                var download = await downloadTask;
                Assert.NotNull(download);
                Assert.EndsWith(".xlsx", download.SuggestedFilename);
                Logger.Info("Migration plan exported as Excel successfully");
            }, nameof(TC_033_Export_Scheduled_Plan));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_034_Verify_Statistics_Display")]
        [AllureTag("Regression")]
        public async Task TC_034_Verify_Statistics_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenStatisticsPanelAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog)
                    .GetByRole(AriaRole.Heading, new() {Name ="Migration Plan Summary"})).ToBeVisibleAsync();
                Logger.Info("Statistics panel displays server/wave/group counts");
            }, nameof(TC_034_Verify_Statistics_Display));
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
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() {Name ="Wave Execution Schedule"})).ToBeVisibleAsync();     
                var waveValue = new Random().Next(1,_migrationControlCentrePage.WaveCount+1);
                var groupValue=new Random().Next(1,3);                
                string waveName= "Wave "+waveValue+" Group "+groupValue;
                await _migrationControlCentrePage.ExpandWaveInTimelineAsync(waveName);
                await Assertions.Expect(Page.GetByRole(AriaRole.Table).GetByRole(AriaRole.Columnheader)).ToContainTextAsync("ServerName");
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
                var waveValue = new Random().Next(1,_migrationControlCentrePage.WaveCount+1);
                var groupValue=new Random().Next(1,3);   
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