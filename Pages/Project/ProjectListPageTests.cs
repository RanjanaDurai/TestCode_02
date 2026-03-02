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
        [AllureOwner("TC_001_Project_Dashboard_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_001_Project_Dashboard_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
                Logger.Info("Dashboard loaded");
            }, nameof(TC_001_Project_Dashboard_Should_Load));
        }

        [Fact]
        [AllureOwner("TC_002_Navigate_To_Create_Project_Page")]
        [AllureTag("smoke")]
        public async Task TC_002_Navigate_To_Create_Project_Page()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _projectList.ClickCreateNewProjectAsync();
                await Page.WaitForURLAsync("**/project-creation");
                Logger.Info("Navigated to Create Project page");
            }, nameof(TC_002_Navigate_To_Create_Project_Page));
        }

        [Fact]
        [AllureOwner("TC_003_Project_Creation_UI_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_003_Project_Creation_UI_Should_Load()
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
            }, nameof(TC_003_Project_Creation_UI_Should_Load));
        }
        
        //  REGRESSION TEST CASES 

        [Fact]
        [AllureOwner("TC_004_ViewUploadedData")]
        [AllureTag("Regression")]
        public async Task TC_004_ViewUploadedData()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ViewDataUploadedFile();
                Logger.Info("Click View upload data button");
            }, nameof(TC_004_ViewUploadedData));
        }

        [Fact]
        [AllureOwner("TC_005_ClickProjectCreationStatusButton")]
        [AllureTag("Regression")]
        public async Task TC_005_ClickProjectCreationStatusButton()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickProjectCreationStatus();
                Logger.Info("Click Project Creation stutus button");
            }, nameof(TC_005_ClickProjectCreationStatusButton));
        }

        [Fact]
        [AllureOwner("TC_006_ClickGatheringClientInfradata")]
        [AllureTag("Regression")]
        public async Task TC_006_ClickGatheringClientInfradata()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickGatheringClientInfradata();
                Logger.Info("Click Project Creation stutus button");
            }, nameof(TC_006_ClickGatheringClientInfradata));
        }

        [Fact]
        [AllureOwner("TC_007_Open_Current_Mode")]
        [AllureTag("Regression")]
        public async Task TC_007_Open_Current_Mode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCurrentModeAsync();
                Logger.Info("Current mode verified");
            }, nameof(TC_007_Open_Current_Mode));
        }

        [Fact]
        [AllureOwner("TC_008_Open_Future_Mode")]
        [AllureTag("Regression")]
        public async Task TC_008_Open_Future_Mode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickFutureModeAsync();
                Logger.Info("Future Mode verified");
            }, nameof(TC_008_Open_Future_Mode));
        }

        [Fact]
        [AllureOwner("TC_009_Open_Inventory")]
        [AllureTag("Regression")]
        public async Task TC_009_Open_Inventory()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickInventorySpecificationsAsync();
                Logger.Info("Inventory Specifications Verified ");
            }, nameof(TC_009_Open_Inventory));
        }

        [Fact]
        [AllureOwner("TC_010_Open_Track_Plan")]
        [AllureTag("Regression")]
        public async Task TC_010_Open_Track_Plan()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickTrackPlanAsync();
                Logger.Info("Track Paln Verified");
            }, nameof(TC_010_Open_Track_Plan));
        }

        [Fact]
        [AllureOwner("TC_011_Open_Wave_And_Move")]
        [AllureTag("Regression")]
        public async Task TC_011_Open_Wave_And_Move()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                Logger.Info("TC_PROJECT_008 started");
                await _projectList.ClickWaveAndMoveGroupingPlanAsync();
                Logger.Info("Wave_And_Move Page opend successfully");
                Logger.Info("TC_PROJECT_008 completed successfully");
            }, nameof(TC_011_Open_Wave_And_Move));
        }

        [Fact]
        [AllureOwner("TC_012_Verify_Project_Actions_Menu")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_Project_Actions_Menu()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.VerifyProjectActionsMenuAsync();
                Logger.Info("Verified_Project_Actions_Menu successfully");
            }, nameof(TC_012_Verify_Project_Actions_Menu));
        }

        [Fact]
        [AllureOwner("TC_013_Verify_Total_Projects_Filter")]
        [AllureTag("Regression")]
        public async Task TC_013_Verify_Total_Projects_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickTotalProjectsFilter();
                Logger.Info("Verified_Total_Projects_Filter");
            }, nameof(TC_013_Verify_Total_Projects_Filter));
        }

        [Fact]
        [AllureOwner("TC_014_Verify_Completed_Filter")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_Completed_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCompletedProjectFilter();
                Logger.Info("Verified_Completed_project_Filter");
            }, nameof(TC_014_Verify_Completed_Filter));
        }

        [Fact]
        [AllureOwner("TC_015_Verify_InProgress_Filter")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_InProgress_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickInProgressProjectFilter();
                Logger.Info("Verified_Completed_project_Filter");
            }, nameof(TC_015_Verify_InProgress_Filter));
        }

        [Fact]
        [AllureOwner("TC_016_Verify_filter_DropDown")]
        [AllureTag("Regression")]
        public async Task TC_016_Verify_filter_DropDown()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickFilterByType();
                Logger.Info("Verified_filter_Dropdown_Button");
            }, nameof(TC_016_Verify_filter_DropDown));
        }

        [Fact]
        [AllureOwner("TC_017_ViewActivityLog")]
        [AllureTag("Regression")]
        public async Task TC_017_ViewActivityLog()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.CilckActivityLogButton();
                Logger.Info("Verified_Avtivity_log");
            }, nameof(TC_017_ViewActivityLog));
        }

        [Fact]
        [AllureOwner("TC_018_ManageColaborator")]
        [AllureTag("Regression")]
        public async Task TC_018_ManageColaborator()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.ClickManageCollaborators();
                Logger.Info("Verified Manage Collaborator");
            }, nameof(TC_018_ManageColaborator));
        }

                [Fact]
        [AllureOwner("TC_019_Select_Organization")]
        [AllureTag("Regression")]
        public async Task TC_019_Select_Organization()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.OrganizationStatus();
                Logger.Info("Verified Select Organization");
            }, nameof(TC_019_Select_Organization));
        }

        [Fact]
        [AllureOwner("TC_020_Delete_Project")]
        [AllureTag("Regression")]
        public async Task TC_020_Delete_Project()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickActionButton();
                await _projectList.DeleteProjectButton();
                Logger.Info("Project Deleted");
            }, nameof(TC_020_Delete_Project));
        }

        [Fact]
        [AllureOwner("TC_21_Logout")]
        [AllureTag("Regression")]
        public async Task TC_021_Logout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.Logout();
                Logger.Info("Logout Successful");
            }, nameof(TC_021_Logout));
        }

        [Fact]
        [AllureOwner("TC_022_CancelLogout")]
        [AllureTag("Regression")]
        public async Task TC_022_CancelLogout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CancelLogout();
                Logger.Info("Cancel Lagout");
            }, nameof(TC_022_CancelLogout));
        }

        [Fact]
        [AllureOwner("TC_023_CloseLogout")]
        [AllureTag("Regression")]
        public async Task TC_023_CloseLogout()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CloseLogout();
                Logger.Info("Close  Lagout");
            }, nameof(TC_023_CloseLogout));
        }

        [Fact]
        [AllureOwner("TC_024_CreateUser")]
        [AllureTag("Regression")]
        public async Task TC_024_CreateUser()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.CreateUser();
                Logger.Info("Created user");
            }, nameof(TC_024_CreateUser));
        }

        [Fact]
        [AllureOwner("TC_025_ApplyFilter")]
        [AllureTag("Regression")]
        public async Task TC_025_ApplyFilter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ApplyFiletr();
                Logger.Info("Filter Aplied");
            }, nameof(TC_025_ApplyFilter));
        }

        [Fact]
        [AllureOwner("TC_026_SelectRandomPageSizeAsync")]
        [AllureTag("Regression")]
        public async Task TC_026_SelectRandomPageSizeAsync()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.SelectRandomPageSizeAsync();
                Logger.Info("Slected Randon Page Size ");
            }, nameof(TC_026_SelectRandomPageSizeAsync));
        }

        [Fact]
        [AllureOwner("TC_027_ClickSyncDataButton")]
        [AllureTag("Regression")]
        public async Task TC_027_ClickSyncDataButton()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickSyncData();
                Logger.Info("Clicked Sync Button Successfully");
            }, nameof(TC_027_ClickSyncDataButton));
        }

        [Fact]
        [AllureOwner("TC_028_Filter_user_in_log")]
        [AllureTag("Regression")]
        public async Task TC_028_Filter_user_in_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomUser();
                Logger.Info("Filtered user in activity log");
            }, nameof(TC_028_Filter_user_in_log));
        }

        [Fact]
        [AllureOwner("TC_029_Filter_Project_In_log")]
        [AllureTag("Regression")]
        public async Task TC_029_Filter_Project_In_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomProject();
                Logger.Info("Filterd Project in activity log");
            }, nameof(TC_029_Filter_Project_In_log));
        }

        [Fact]
        [AllureOwner("TC_030_Filter_Projects_Status_In_Log")]
        [AllureTag("Regression")]
        public async Task TC_030_Filter_Projects_Status_In_Log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectRandomStatus();
                Logger.Info("Filterd Status in Avtivity log");
            }, nameof(TC_030_Filter_Projects_Status_In_Log));
        }

        [Fact]
        [AllureOwner("TC_031_Select_Date_Range")]
        [AllureTag("Regression")]
        public async Task TC_031_Select_Date_Range()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.SelectDateRange();
                await _basicSetup.SelectLast7DaysRangeAsync();
                Logger.Info("Selected Date range Successfully");
            }, nameof(TC_031_Select_Date_Range));
        }

        [Fact]
        [AllureOwner("TC_032_Clear_Filter_In_activity_log")]
        [AllureTag("Regression")]
        public async Task TC_032_Clear_Filter_In_activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.ClearAllFilter();
                Logger.Info("Clicked Clear filetr Button Successfully");
            }, nameof(TC_032_Clear_Filter_In_activity_log));
        }

        [Fact]
        [AllureOwner("TC_033_ClickSyncDataButtonInLog")]
        [AllureTag("Regression")]
        public async Task TC_033_ClickSyncDataButtonInLog()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickViewActivityLogs();
                await _projectList.ClickSync();
                Logger.Info("Clicked Sync Button in activity log Successfully");
            }, nameof(TC_033_ClickSyncDataButtonInLog));
        }  
    }
}