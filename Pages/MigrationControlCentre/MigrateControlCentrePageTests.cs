using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;
using Allure.Xunit.Attributes.Steps;
using AspectInjector.Broker;
using System.ComponentModel.DataAnnotations;


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

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_003_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
            await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();
            await Assertions.Expect(Page.GetByText("Migration Planning")).ToBeVisibleAsync();
            Logger.Info("Detailed Planning tab loaded");
            }, nameof(TC_003_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab));
        }
        

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
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Home");
                await Assertions.Expect(Page.GetByText("Project DashBoard")).ToBeVisibleAsync(new() { Timeout = 10000 });
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Project Creation");
                await Assertions.Expect(Page.GetByText("Project Initialization")).ToBeVisibleAsync(new() { Timeout = 10000 });
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
                await _migrationControlCentrePage.ClickBreadcrumbAsync("FMO Architecture");
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync(new() { Timeout = 10000 });
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
                await _migrationControlCentrePage.ClickBreadcrumbAsync("CMO/FMO Inventory");
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "CMO/FMO Inventory" })).ToBeVisibleAsync();
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
                await _migrationControlCentrePage.ClickBreadcrumbAsync("Migration Plan Tracking");
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Plan Tracking" })).ToBeVisibleAsync();
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
                await _migrationControlCentrePage.ExpandButton.ClickAsync();
                
                var Text=await Page.Locator("li.mud-breadcrumb-item.mud-disabled").TextContentAsync();
                Assert.Equal("Migration Control Center", Text);
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
                await Page.GetByRole(AriaRole.Cell, new(){Name="Wave"}).ClickAsync();
                var elements=await Page.QuerySelectorAllAsync("td[data-label='Group Name']:has-text('Wave ')");
                foreach(var element in elements)
                {
                    await element.ScrollIntoViewIfNeededAsync();
                    await element.ClickAsync();
                    await _basicSetup.WaitForPageAsync(2);
                    if(await Page.GetByText(searchInputText).IsVisibleAsync())
                    break;
                }
                await Assertions.Expect(Page.GetByText(searchInputText)).ToBeVisibleAsync();
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
                await _basicSetup.WaitForPageAsync(1);
                await Assertions.Expect(Page.GetByText("No records found matching the search criteria")).ToBeVisibleAsync();
                Logger.Info("No results message displayed correctly");
            }, nameof(TC_010_Verify_TextSearch_NoResultsFound));
        }

        #endregion

        #region  Filter Status

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Verify_FilterStatus_Default")]
        [AllureTag("Regression")]
        public async Task TC_011_Verify_FilterStatus_Default()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("All");
                await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(_migrationControlCentrePage.WaveCount); 
                Logger.Info("Filter by All status applied");
            }, nameof(TC_011_Verify_FilterStatus_Default));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Verify_FilterStatus_Planned")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_FilterStatus_Planned()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Planned");
                if(Page.Locator("tr")!=null)
                {
                  await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(await Page.GetByText("Planned", new() { Exact = true }).CountAsync()-1);  
                }
                await Assertions.Expect(Page.GetByText("No records found matching the search criteria.")).ToBeVisibleAsync();
                Logger.Info("Filter by Planned status applied");
            }, nameof(TC_012_Verify_FilterStatus_Planned));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Verify_FilterStatus_Pending")]
        [AllureTag("Regression")]
        public async Task TC_013_Verify_FilterStatus_Pending()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Pending");
                await _basicSetup.WaitForPageAsync(1);
                if(await Page.GetByText("Wave Name").IsVisibleAsync())    
                {
                    var elements = await Page.QuerySelectorAllAsync("text='Pending'");
                    foreach (var Ele in elements)
                    {
                        await Ele.ScrollIntoViewIfNeededAsync();
                    }
                    int count = elements.Count();
                    await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(count);
                }
                else
                {
                    await Assertions.Expect(Page.GetByText("No records found matching the search criteria.")).ToBeVisibleAsync();
                }
                Logger.Info("Filter by Pending status applied");
            }, nameof(TC_013_Verify_FilterStatus_Pending));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Verify_FilterStatus_InProgress")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_FilterStatus_InProgress()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("In Progress");
                if(await Page.GetByText("Wave Name").IsVisibleAsync())
                {
                    var elements = await Page.QuerySelectorAllAsync("text=In Progress");
                    int count = elements.Count();
                    await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(count);  
                }
                else{
                await Assertions.Expect(Page.GetByText("No records found matching the search criteria.")).ToBeVisibleAsync();
                }
                Logger.Info("Filter by In Progress status applied");
            }, nameof(TC_014_Verify_FilterStatus_InProgress));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Verify_FilterStatus_Completed")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_FilterStatus_Completed()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Completed");
                if(await Page.GetByText("Wave Name").IsVisibleAsync())
                {
                    var elements = await Page.QuerySelectorAllAsync("text=Completed");
                    int count = elements.Count();
                    await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(count);
                }
                else{
                await Assertions.Expect(Page.GetByText("No records found matching the search criteria.")).ToBeVisibleAsync();
                }
                Logger.Info("Filter by Completed status applied");
            }, nameof(TC_015_Verify_FilterStatus_Completed));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Verify_FilterStatus_Failed")]
        [AllureTag("Regression")]
        public async Task TC_016_Verify_FilterStatus_Failed()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.SelectFilterStatusAsync("Failed");
                if(await Page.GetByText("Wave Name").IsVisibleAsync())
                {
                    var elements = await Page.QuerySelectorAllAsync("text=Failed");
                    int count = elements.Count();
                    await Assertions.Expect(Page.Locator("tr")).ToHaveCountAsync(count);
                }
                else
                {
                    await Assertions.Expect(Page.GetByText("No records found matching the search criteria.")).ToBeVisibleAsync();
                }
                
                Logger.Info("Filter by Failed status applied");
            }, nameof(TC_016_Verify_FilterStatus_Failed));
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
                await Assertions.Expect(Page.GetByText("Wave status updated successfully.")).ToBeVisibleAsync();
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
                string waveName = "Wave "+ new Random().Next(1, _migrationControlCentrePage.WaveCount +1);
                string groupName = "Group "+ new Random().Next(1, _migrationControlCentrePage.GroupCount +1);
                await Page.GetByText(waveName).ClickAsync();
                Console.WriteLine($"Group Name :{groupName}");
                await _migrationControlCentrePage.ChangeStatusAsync(groupName);
                await _migrationControlCentrePage.SaveEditsAsync(groupName);
                await Assertions.Expect(Page.GetByText("Group status updated successfully.")).ToBeVisibleAsync();
                Logger.Info("Filter by All status applied");
            }, nameof(TC_019_Verify_EditOperation_GroupStatus));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Verify_WaveRescheduleOperation")]
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
        public async Task TC_021_WaveRescheduleOperation_PastDate()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.ClickEditDetailsAsync();

                string waveName = "Wave 1";
                await _migrationControlCentrePage.OpenRescheduleCalendarAsync(waveName);
                string pastDate = DateTime.UtcNow.AddDays(-1).ToString("dd");
                await _migrationControlCentrePage.DatePicker.ClickAsync();
                var pastDayButton = Page.GetByRole(AriaRole.Button, new(){Name=pastDate});
                bool isDisabled = await pastDayButton.GetAttributeAsync("disabled") != null ||
                                  await pastDayButton.EvaluateAsync<bool>("el => el.classList.contains('disabled') || el.ariaDisabled === 'true'");

                if (!isDisabled)
                {
                    await pastDayButton.ClickAsync(new() { Timeout = 5000 });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Reschedule" }).ClickAsync();
                    await _basicSetup.WaitForPageAsync(2);
                    await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Planned Start Date cannot be in the past");
                }
                else
                {
                    Assert.True(isDisabled);
                    Logger.Info("Past date selection allowed for Planned Start Date");
                }

                Logger.Info("Past date selection correctly blocked or produces validation error");
            }, nameof(TC_021_WaveRescheduleOperation_PastDate));
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
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Timeline Scheduler"})).ToBeVisibleAsync();
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
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Migration schedule generated successfully.");
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
                await _migrationControlCentrePage.AddManualHolidayAsync("Christmas Day", "EMEA", Page.GetByRole(AriaRole.Button, new() {Name="Open Date Picker"}).Last);
                var isAdded = await _migrationControlCentrePage.IsHolidayInTableAsync("Christmas Day");
                Assert.True(isAdded);
                Logger.Info("Manual holiday added and visible in table");
            }, nameof(TC_024_Verify_AddManualHoliday));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_Add_Manual_Holiday_Missing_Required_Field_Negative")]
        [AllureTag("Regression")]
        public async Task TC_025_AddManualHoliday_MissingRequiredField_Negative()
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
            }, nameof(TC_025_AddManualHoliday_MissingRequiredField_Negative));
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
                await Assertions.Expect(Page.Locator("tr.mud-table-row").Nth(1).GetByText("Wave Name Sequence Planned")).Not.ToBeVisibleAsync();
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
                await Assertions.Expect(Page.Locator("role=alert").Last).ToHaveTextAsync("Oops! We only support CSV and JSON files right now. Please upload one of those.");
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
                await _basicSetup.WaitForPageAsync(1);
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
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Servers per Move","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Expected Servers per Move","4");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Servers per Move Group","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Move Groups per Wave*","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Move Groups per Wave*","2");
                await _migrationControlCentrePage.GenerateMigrationScheduleAsync();
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("The input value cannot be lesser than the default value.");
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
        [AllureOwner("TC_035_Verify_ExpandWave")]
        [AllureTag("Regression")]
        public async Task TC_035_Verify_ExpandGroup()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.TimelineTab.ClickAsync();      
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() {Name ="Wave Execution Schedule"})).ToBeVisibleAsync();     
                var waveValue = new Random().Next(1,await Page.Locator("div.mud-timeline-item-content")
                      .Filter(new() { HasText = "Wave " })
                      .CountAsync()+1);
                Console.WriteLine($"Wave Value : {waveValue}");
                var groupValue=new Random().Next(1,3);                
                string waveName= "Wave "+waveValue+" Group "+groupValue;
                await _migrationControlCentrePage.ExpandWaveInTimelineAsync(waveName);
                await Assertions.Expect(Page.GetByRole(AriaRole.Columnheader,new() {Name="Server Name"}).First).ToBeVisibleAsync();
                Logger.Info("Group expanded and servers visible in timeline");
            }, nameof(TC_035_Verify_ExpandGroup));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_036_MoveGroup_AcrossWave")]
        [AllureTag("Regression")]
        public async Task TC_036_MoveGroup_AcrossWave()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenManageTabAsync();
                var waveValue = new Random().Next(1,await Page.Locator("h5").CountAsync()-2);
                var groupValue=new Random().Next(1,await Page.GetByText(string.Concat("Wave ",waveValue," Group ")).CountAsync());  
                var sourceGroupName=string.Concat("Wave ",waveValue," Group ",groupValue);
                var targetWaveName="Wave "+ (waveValue+1);
                await _basicSetup.WaitForPageAsync(5);
                await _migrationControlCentrePage.MoveGroupToWaveAsync(sourceGroupName, targetWaveName);
                var waveHeading = Page.Locator("h5.mud-typography-h5").Filter(new() { HasText = targetWaveName });
                var waveContainer = waveHeading.Locator("xpath=ancestor::div[contains(@class,'mud-paper') " +  "and contains(@class,'pa-6')][1]");
                await _basicSetup.WaitForPageAsync(2);
                await Assertions.Expect(waveContainer.Locator(".mud-card-content").GetByText(sourceGroupName)).ToBeVisibleAsync();
            }, nameof(TC_036_MoveGroup_AcrossWave));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_037")]
        [AllureTag("Regression")]
        public async Task TC_037_Verify_BackloggedGroup()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenManageTabAsync();
                var waveValue = new Random().Next(1,await Page.Locator("h5").CountAsync()-2);
                var groupValue=new Random().Next(1,await Page.GetByText(string.Concat("Wave ",waveValue," Group ")).CountAsync());  
                var sourceGroupName=string.Concat("Wave ",waveValue," Group ",groupValue);
                await _migrationControlCentrePage.BacklogGroupAsync(sourceGroupName);
                var backlogSection = Page.Locator("div.mud-paper").Filter(new() {Has = Page.Locator("h5.mud-typography-h5").Filter(new() { HasText = "Move Group Backlog" })});
                var groupTitle = backlogSection.Locator("p.mud-typography-subtitle2").Filter(new() { HasText = sourceGroupName });
                await Assertions.Expect(groupTitle).ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByText("Groups moved to backlog successfully.")).ToBeVisibleAsync();            
                Logger.Info("Group backlogged successfully");
            }, nameof(TC_037_Verify_BackloggedGroup));
        }

         [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_038_Rollback_BackloggedGroupToTargetWave")]
        [AllureTag("Regression")]
        public async Task TC_038_Rollback_BackloggedGroup()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _migrationControlCentrePage.OpenManageTabAsync();
                var waveValue = new Random().Next(1,await Page.Locator("h5").CountAsync()-2);
                var groupValue=new Random().Next(1,await Page.GetByText(string.Concat("Wave ",waveValue," Group ")).CountAsync());  
                var sourceGroupName=string.Concat("Wave ",waveValue," Group ",groupValue);
                var sourceWaveName = string.Concat("Wave ",waveValue);
        

                await _migrationControlCentrePage.BacklogGroupAsync(sourceGroupName);
                await _basicSetup.WaitForPageAsync(1);
                await _migrationControlCentrePage.RollbackGroupAsync();

                var waveHeading = Page.Locator("h5.mud-typography-h5").Filter(new() { HasText = sourceWaveName });
                var waveContainer = waveHeading.Locator("xpath=ancestor::div[contains(@class,'mud-paper') " +  "and contains(@class,'pa-6')][1]");
                await _basicSetup.WaitForPageAsync(2);
                await Assertions.Expect(waveContainer.Locator(".mud-card-content").GetByText(sourceGroupName)).ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByText($"Group {sourceGroupName} restored successfully.")).ToBeVisibleAsync();
                Logger.Info("Backlogged group rolled back successfully");
            }, nameof( TC_038_Rollback_BackloggedGroup));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_039_Rollback_BackloggedGroupToTargetWave")]
        [AllureTag("Regression")]
        public async Task TC_039_Rollback_BackloggedGroupToTargetWave()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _migrationControlCentrePage.OpenManageTabAsync();
                var waveValue = new Random().Next(1,await Page.Locator("h5").CountAsync()-2);
                var groupValue=new Random().Next(1,await Page.GetByText(string.Concat("Wave ",waveValue," Group ")).CountAsync());  
                var sourceGroupName=string.Concat("Wave ",waveValue," Group ",groupValue);
                var targetWaveName="Wave "+ (waveValue+1);

                await _migrationControlCentrePage.BacklogGroupAsync(sourceGroupName);
                await _basicSetup.WaitForPageAsync(1);
                await _migrationControlCentrePage.RollbackGroupToTargetWaveAsync(targetWaveName);

                var waveHeading = Page.Locator("h5.mud-typography-h5").Filter(new() { HasText = targetWaveName });
                await _basicSetup.WaitForPageAsync(2);
                var waveContainer = waveHeading.Locator("xpath=ancestor::div[contains(@class,'mud-paper') " +  "and contains(@class,'pa-6')][1]");
                await _basicSetup.WaitForPageAsync(2);
                await Assertions.Expect(waveContainer.Locator(".mud-card-content").GetByText(sourceGroupName)).ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByText($"Group {sourceGroupName} restored successfully.")).ToBeVisibleAsync();
                Logger.Info("Backlogged group rolled back successfully");
            }, nameof( TC_039_Rollback_BackloggedGroupToTargetWave));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_040_Configure_Parameters_ZeroEdgeValue")]
        [AllureTag("Regression")]
        public async Task TC_040_Configure_Parameters_ZeroEdgeValue()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenParameterConfiguration();
                await Page.WaitForSelectorAsync("text=Configure and Generate Migration Plan");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Servers per Move","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Expected Servers per Move","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Servers per Move Group","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Move Groups per Wave*","0");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Move Groups per Wave*","0");
                await _migrationControlCentrePage.GenerateMigrationScheduleAsync();
                await Assertions.Expect(Page.Locator("role=alert")).ToContainTextAsync("The input value cannot be lesser than the deafult value.");
                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_040_Configure_Parameters_ZeroEdgeValue));
               
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_041_Configure_Parameters_NonIntegerInput")]
        [AllureTag("regression")]
        public async Task TC_041_Configure_Parameters_NonIntegerInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenParameterConfiguration();
                await Page.WaitForSelectorAsync("text=Configure and Generate Migration Plan");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Servers per Move","1.5");
                await _migrationControlCentrePage.SetParameterValueAsync("Expected Servers per Move","5.5");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Servers per Move Group","2.5");
                await _migrationControlCentrePage.SetParameterValueAsync("Minimum Move Groups per Wave*","2.5");
                await _migrationControlCentrePage.SetParameterValueAsync("Maximum Move Groups per Wave*","3.5");
                await _migrationControlCentrePage.GenerateMigrationScheduleAsync();
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("The input value cannot be lesser than the default value.");
                Logger.Info("Valid parameter values saved successfully");
            }, nameof(TC_041_Configure_Parameters_NonIntegerInput));
        }

        #endregion
        
    }
}
