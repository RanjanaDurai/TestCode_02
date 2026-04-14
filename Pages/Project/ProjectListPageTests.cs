using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;     
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.Project
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Page Tests")]
    public class ProjectListPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _projectList.WaitForProjectDashboardIconsAsync();
            await _projectList.ClickCompletedProjectButton();
            Logger.Info("Logged in as Migrate Admin and project dashboard ready");
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_013_Project_Dashboard_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_013_Project_Dashboard_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
                Logger.Info("Dashboard loaded");
            }, nameof(TC_013_Project_Dashboard_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_014_Navigate_To_Create_Project_Page")]
        [AllureTag("Smoke")]
        public async Task TC_014_Navigate_To_Create_Project_Page()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _projectList.ClickCreateNewProjectAsync();
                await Page.WaitForURLAsync("**/project-creation");
                Logger.Info("Navigated to Create Project page");
            }, nameof(TC_014_Navigate_To_Create_Project_Page));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_015_Project_Creation_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_015_Project_Creation_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _projectList.ClickCreateNewProjectAsync();

                await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();

                Logger.Info("Project creation UI is visible");
            }, nameof(TC_015_Project_Creation_UI_Should_Load));
        }
        
        //  Regression TEST CASES 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_ViewUploadedData")]
        [AllureTag("Regression")]
        public async Task TC_016_ViewUploadedData()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ViewDataUploadedFile();
                Logger.Info("Click View upload data button");
            }, nameof(TC_016_ViewUploadedData));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_ClickProjectCreationStatusButton")]
        [AllureTag("Regression")]
        public async Task TC_017_ClickProjectCreationStatusButton()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickProjectCreationStatus();
                Logger.Info("Click Project Creation stutus button");
            }, nameof(TC_017_ClickProjectCreationStatusButton));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_ClickGatheringClientInfradata")]
        [AllureTag("Regression")]
        public async Task TC_018_ClickGatheringClientInfradata()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickGatheringClientInfradata();
                Logger.Info("Click Project Creation stutus button");
            }, nameof(TC_018_ClickGatheringClientInfradata));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Open_Current_Mode")]
        [AllureTag("Regression")]
        public async Task TC_019_Open_Current_Mode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCurrentModeAsync();
                Logger.Info("Current mode verified");
            }, nameof(TC_019_Open_Current_Mode));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Open_Future_Mode")]
        [AllureTag("Regression")]
        public async Task TC_020_Open_Future_Mode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickFutureModeAsync();
                Logger.Info("Future Mode verified");
            }, nameof(TC_020_Open_Future_Mode));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_Open_Inventory")]
        [AllureTag("Regression")]
        public async Task TC_021_Open_Inventory()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickInventorySpecificationsAsync();
                Logger.Info("Inventory Specifications Verified ");
            }, nameof(TC_021_Open_Inventory));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_022_Open_Track_Plan")]
        [AllureTag("Regression")]
        public async Task TC_022_Open_Track_Plan()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickTrackPlanAsync();
                Logger.Info("Track Paln Verified");
            }, nameof(TC_022_Open_Track_Plan));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_023_Open_Wave_And_Move")]
        [AllureTag("Regression")]
        public async Task TC_023_Open_Wave_And_Move()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_PROJECT_008 started");
                await _projectList.ClickWaveAndMoveGroupingPlanAsync();
                Logger.Info("Wave_And_Move Page opend successfully");
                Logger.Info("TC_PROJECT_008 completed successfully");
            }, nameof(TC_023_Open_Wave_And_Move));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_024_Verify_Project_Actions_Menu")]
        [AllureTag("Regression")]
        public async Task TC_024_Verify_Project_Actions_Menu()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.VerifyProjectActionsMenuAsync();
                Logger.Info("Verified_Project_Actions_Menu successfully");
            }, nameof(TC_024_Verify_Project_Actions_Menu));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_Verify_Total_Projects_Filter")]
        [AllureTag("Regression")]
        public async Task TC_025_Verify_Total_Projects_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickTotalProjectsFilter();
                Logger.Info("Verified_Total_Projects_Filter");
            }, nameof(TC_025_Verify_Total_Projects_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_026_Verify_Completed_Filter")]
        [AllureTag("Regression")]
        public async Task TC_026_Verify_Completed_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCompletedProjectFilter();
                Logger.Info("Verified_Completed_project_Filter");
            }, nameof(TC_026_Verify_Completed_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_027_Verify_InProgress_Filter")]
        [AllureTag("Regression")]
        public async Task TC_027_Verify_InProgress_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickInProgressProjectFilter();
                Logger.Info("Verified_InProgress_project_Filter");
            }, nameof(TC_027_Verify_InProgress_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_028_Verify_filter_DropDown")]
        [AllureTag("Regression")]
        public async Task TC_028_Verify_filter_DropDown()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickFilterByType();
                Logger.Info("Verified_filter_Dropdown_Button");
            }, nameof(TC_028_Verify_filter_DropDown));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_029_ViewActivityLog")]
        [AllureTag("Regression")]
        public async Task TC_029_ViewActivityLog()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.CilckActivityLogButton();
                Logger.Info("Verified_Avtivity_log");
            }, nameof(TC_029_ViewActivityLog));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_030_ManageColaborator")]
        [AllureTag("Regression")]
        public async Task TC_030_ManageColaborator()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.ClickManageCollaborators();
                Logger.Info("Verified Manage Collaborator");
            }, nameof(TC_030_ManageColaborator));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_031_Select_Organization")]
        [AllureTag("Regression")]
        public async Task TC_031_Select_Organization()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.OrganizationStatus();
                Logger.Info("Verified Select Organization");
            }, nameof(TC_031_Select_Organization));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_032_Delete_Project")]
        [AllureTag("Regression")]
        public async Task TC_032_Delete_Project()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.DeleteProjectButton();
                Logger.Info("Project Deleted");
            }, nameof(TC_032_Delete_Project));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_033_Logout")]
        [AllureTag("Regression")]
        public async Task TC_033_Logout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.Logout();
                Logger.Info("Logout Successful");
            }, nameof(TC_033_Logout));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_034_CancelLogout")]
        [AllureTag("Regression")]
        public async Task TC_034_CancelLogout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CancelLogout();
                Logger.Info("Cancel Lagout");
            }, nameof(TC_034_CancelLogout));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_035_CloseLogout")]
        [AllureTag("Regression")]
        public async Task TC_035_CloseLogout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CloseLogout();
                Logger.Info("Close  Lagout");
            }, nameof(TC_035_CloseLogout));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_036_CreateUser")]
        [AllureTag("Regression")]
        public async Task TC_036_CreateUser()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CreateUser();
                Logger.Info("Created user");
            }, nameof(TC_036_CreateUser));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_037_ApplyFilter")]
        [AllureTag("Regression")]
        public async Task TC_037_ApplyFilter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ApplyFiletr();
                Logger.Info("Filter Aplied");
            }, nameof(TC_037_ApplyFilter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_038_SelectRandomPageSizeAsync")]
        [AllureTag("Regression")]
        public async Task TC_038_SelectRandomPageSizeAsync()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.SelectRandomPageSizeAsync();
                Logger.Info("Slected Randon Page Size ");
            }, nameof(TC_038_SelectRandomPageSizeAsync));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_039_ClickSyncDataButton")]
        [AllureTag("Regression")]
        public async Task TC_039_ClickSyncDataButton()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickSyncData();
                Logger.Info("Clicked Sync Button Successfully");
            }, nameof(TC_039_ClickSyncDataButton));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_040_Filter_user_in_log")]
        [AllureTag("Regression")]
        public async Task TC_040_Filter_user_in_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomUser();
                Logger.Info("Filtered user in activity log");
            }, nameof(TC_040_Filter_user_in_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_041_Filter_Project_In_log")]
        [AllureTag("Regression")]
        public async Task TC_041_Filter_Project_In_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomProject();
                Logger.Info("Filterd Project in activity log");
            }, nameof(TC_041_Filter_Project_In_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_042_Filter_Projects_Status_In_Log")]
        [AllureTag("Regression")]
        public async Task TC_042_Filter_Projects_Status_In_Log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomStatus();
                Logger.Info("Filterd Status in Avtivity log");
            }, nameof(TC_042_Filter_Projects_Status_In_Log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_043_Select_Date_Range")]
        [AllureTag("Regression")]
        public async Task TC_043_Select_Date_Range()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectDateRange();
                await _basicSetup.SelectDaysRangeAsync();
                Logger.Info("Selected Date range Successfully");
            }, nameof(TC_043_Select_Date_Range));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_044_Clear_Filter_In_activity_log")]
        [AllureTag("Regression")]
        public async Task TC_044_Clear_Filter_In_activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.ClearAllFilter();
                Logger.Info("Clicked Clear filetr Button Successfully");
            }, nameof(TC_044_Clear_Filter_In_activity_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_045_ClickSyncDataButtonInLog")]
        [AllureTag("Regression")]
        public async Task TC_045_ClickSyncDataButtonInLog()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.ClickSync();
                Logger.Info("Clicked Sync Button in activity log Successfully");
            }, nameof(TC_045_ClickSyncDataButtonInLog));
        }  
    }
}