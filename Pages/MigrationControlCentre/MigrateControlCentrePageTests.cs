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
            // await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
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
                await Assertions.Expect(Page.GetByText("No results found", new() { Exact = false })).ToBeVisibleAsync();
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
                if(Page.Locator("tr")!=null)
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
                Console.WriteLine($"Wave Name : {waveName}");
                await _migrationControlCentrePage.OpenRescheduleCalendarAsync(waveName);
                await Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Reschedule Wave"}).WaitForAsync(new() { State = WaitForSelectorState.Visible });
                string selectedDate= await _migrationControlCentrePage.RescheduleOperationAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _migrationControlCentrePage.SaveEditsAsync(waveName);
                Logger.Info("Filter by All status applied");
            }, nameof(TC_020_Verify_WaveRescheduleOperation));
        }

        #endregion
    }
}
