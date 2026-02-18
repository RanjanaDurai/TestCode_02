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
        private BasicSetup _basicSetup = null!;

        private AttachmentHelper _attachmentHelper=null!;

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
            Logger.Info("Logged in as Migrate Admin and project dashboard ready");
        }

        // TC_PROJECT_001
        [Fact]
        [AllureOwner("TC_PROJECT_001")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_001_Project_Dashboard_Should_Load()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();

            await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
            Logger.Info("Dashboard loaded");

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_001_Dashboard");
        }

        // TC_PROJECT_002
        [Fact]
        [AllureOwner("TC_PROJECT_002")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_002_Navigate_To_Create_Project_Page()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _projectList.ClickCreateNewProjectAsync();

            await Page.WaitForURLAsync("**/project-creation");

            Logger.Info("Navigated to Create Project page");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_002_Create_Project");
        }

        // TC_PROJECT_003
        [Fact]
        [AllureOwner("TC_PROJECT_003")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_003_Project_Creation_UI_Should_Load()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _projectList.ClickCreateNewProjectAsync();

            await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();

            Logger.Info("Project creation UI is visible");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_003_UI");
        }

        //  REGRESSION TEST CASES 

        [Fact]
        [AllureOwner("TC_Project_01")]
        [AllureTag("Regression")]
        public async Task TC_project_01_ViewUploadedData()
        {
            await _projectList.ViewDataUploadedFile();

            Logger.Info("Click View upload data button");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_01_ViewUploadedData");
        }

        [Fact]
        [AllureOwner("TC_Project_02")]
        [AllureTag("Regression")]
        public async Task TC_project_02_ClickProjectCreationStatusButton()
        {
            await _projectList.ClickProjectCreationStatus();

            Logger.Info("Click Project Creation stutus button");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_02_ClickProjectCreationStatusButton");
        }
        
        [Fact]
        [AllureOwner("TC_Project_03")]
        [AllureTag("Regression")]
        public async Task TC_project_03_ClickGatheringClientInfradata()
        {
            await _projectList.ClickGatheringClientInfradata();

            Logger.Info("Click Project Creation stutus button");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_02_ClickProjectCreationStatusButton");
        }
        
        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_004_Open_Current_Mode()
        {
            await _projectList.ClickCurrentModeAsync();

            Logger.Info("Current mode verified");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Current_Mode");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_005_Open_Future_Mode()
        {
            await _projectList.ClickFutureModeAsync();

            Logger.Info("Future Mode verified");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Future_Mode");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_006_Open_Inventory()
        {

            await _projectList.ClickInventorySpecificationsAsync();

            await Assertions.Expect(Page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Inventory");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_007")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_007_Open_Track_Plan()
        {
            await _projectList.ClickTrackPlanAsync();

            Logger.Info("Track Paln Verified");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_007_Track_Plan");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_008")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_008_Open_Wave_And_Move()
        {
            Logger.Info("TC_PROJECT_008 started");

            await _projectList.ClickWaveAndMoveGroupingPlanAsync();
            
            Logger.Info("Wave_And_Move Page opend successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_008_Wave_Move");
            Logger.Info("TC_PROJECT_008 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_009")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_009_Verify_Project_Actions_Menu()
        {
            await _projectList.VerifyProjectActionsMenuAsync();

            Logger.Info("Verified_Project_Actions_Menu successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_009_Project_Actions_Menu");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_010")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_010_Verify_Total_Projects_Filter()
        {
            await _projectList.ClickTotalProjectsFilter();

            Logger.Info("Verified_Total_Projects_Filter");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_010_Verify_Total_Projects_Filter");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_011")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_011_Verify_Completed_Filter()
        {
            await _projectList.ClickCompletedProjectFilter();
            
            Logger.Info("Verified_Completed_project_Filter");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_011_Verify_Completed_Filter");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_012")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_012_Verify_InProgress_Filter()
        {

            await _projectList.ClickInProgressProjectFilter();

            Logger.Info("Verified_Completed_project_Filter");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_012_Verify_InProgress_Filter");
        }

        [Fact]
        [AllureOwner("TC_Project_013")]
        [AllureTag("Regression")]
        
        public async Task TC_Project_013_Verify_filter_DropDown()
        {
            await _projectList.ClickFilterByType();

            Logger.Info("Verified_filter_Dropdown_Button");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_Project_013_Verify_filter");
        }

        [Fact]
        [AllureOwner("TC_Project_014")]
        [AllureTag("Regression")]
        
        public async Task TC_Project_014_ViewActivityLog()
        {
            await _projectList.ClickActionButton();
            await _projectList.CilckActivityLogButton();

            Logger.Info("Verified_Avtivity_log");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_Project_014_ViewActivityLog");        
        }

        [Fact]
        [AllureOwner("TC_Project_015")]
        [AllureTag("Regression")]

        public async Task TC_Project_015_ManageColaborator()
        {
            await _projectList.ClickActionButton();
            await _projectList.ClickManageCollaborators();

            Logger.Info("Verified Manage Collaborator");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_Project_015_ManageColaborator");
        }

        [Fact]
        [AllureOwner("TC_Project_016")]
        [AllureTag("Regression")]

        public async Task TC_project_016_Select_Organization()
        {
            await _projectList.OrganizationStatus();

            Logger.Info("Verified Select Organization");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_016_Select_Organization");
        }

        [Fact]
        [AllureOwner("TC_Project_017")]
        [AllureTag("Regression")]

        public async Task TC_project_017_Delete_Project()
        {
            await _projectList.ClickActionButton();
            await _projectList.DeleteProjectButton();

            Logger.Info("Project Deleted");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_017_Delete_Project");
        }

        [Fact]
        [AllureOwner("TC_Project_018")]
        [AllureTag("Regression")]
        public async Task TC_project_018_Logout()
        {
            await _projectList.Logout();

            Logger.Info("Logout Successful");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_018_Logout");
        }

        [Fact]
        [AllureOwner("TC_Project_019")]
        [AllureTag("Regression")]
        public async Task TC_project_018_CancelLogout()
        {
            await _projectList.CancelLogout();

            Logger.Info("Cancel Lagout");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_018_CancelLogout");
        }

        [Fact]
        [AllureOwner("TC_Project_020")]
        [AllureTag("Regression")]
        public async Task TC_project_020_CloseLogout()
        {
            await _projectList.CloseLogout();

            Logger.Info("Close  Lagout");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_020_CloseLogout");
        }

        [Fact]
        [AllureOwner("TC_Project_021")]
        [AllureTag("Regression")]
        public async Task TC_project_021_CreateUser()
        {
            await _projectList.CreateUser();

            Logger.Info("Created user");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_021_CreateUser");        
        }

        [Fact]
        [AllureOwner("TC_Project_022")]
        [AllureTag("Regression")]
        public async Task TC_project_022_ApplyFilter()
        {
            await _projectList.ApplyFiletr();

            Logger.Info("Filter Aplied");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_022_ApplyFilter");
        }

        [Fact]
        [AllureOwner("TC_Project_023")]
        [AllureTag("Regression")]
        public async Task TC_project_023_SelectRandomPageSizeAsync()
        {
            await _projectList.SelectRandomPageSizeAsync();

            Logger.Info("Slected Randon Page Size ");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_023_SelectRandomPageSizeAsync");
            
        }

        [Fact]
        [AllureOwner("TC_Project_024")]
        [AllureTag("Regression")]
        public async Task TC_project_026_ClickSyncDataButton()
        {
            await _projectList.ClickSyncData();
 
            Logger.Info("Clicked Sync Button Successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_022_ApplyFilter");
        }

        [Fact]
        [AllureOwner("TC_Project_025")]
        [AllureTag("Regression")]
        public async Task TC_project_026_Filter_user_in_log()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.SelectRandomUser();

            Logger.Info("Filtered user in activity log");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_026_ActivityLog_Filter_user");
        }

        [Fact]
        [AllureOwner("TC_Project_026")]
        [AllureTag("Regression")]
        public async Task TC_project_026_Filter_Project_In_log()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.SelectRandomProject();

            Logger.Info("Filterd Project in activity log");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_026_ActivityLog_Filter_Project");
        }

        [Fact]
        [AllureOwner("TC_Project_027")]
        [AllureTag("Regression")]
        public async Task TC_project_027_Filter_Projects_Status_In_Log()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.SelectRandomStatus();

            Logger.Info("Filterd Status in Avtivity log");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_027_Filter_Projects_Status_In_Log");
        }

        [Fact]
        [AllureOwner("TC_Project_028")]
        [AllureTag("Regression")]
        public async Task TC_project_028_Select_Date_Range()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.SelectDateRange();
            await _basicSetup.SelectLast7DaysRangeAsync();

            Logger.Info("Selected Date range Successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_028_Select_Date_Range");
        }

        [Fact]
        [AllureOwner("TC_Project_029")]
        [AllureTag("Regression")]
        public async Task TC_project_029_Clear_Filter_In_activity_log()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.ClearAllFilter();

            Logger.Info("Clicked Clear filetr Button Successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_029_Clear_Filter_In_activity_log");
        }

        [Fact]
        [AllureOwner("TC_Project_030")]
        [AllureTag("Regression")]
        public async Task TC_project_030_ClickSyncDataButtonInLog()
        {
            await _projectList.ClickViewActivityLogs();
            await _projectList.ClickSync();

            Logger.Info("Clicked Sync Button in activity log Successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_030_ClickSyncDataButtonInLog");
        }  
    }
}