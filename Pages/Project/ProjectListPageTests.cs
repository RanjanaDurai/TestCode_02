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

        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_004_Open_Current_Mode()
        {
            Logger.Info("TC_PROJECT_004 started");

            await _projectList.ClickCurrentModeAsync();

            await Assertions.Expect(
                Page.GetByRole(AriaRole.Heading, new() { Name = "Current Mode of Operation - Architectural View" })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Current_Mode");
            Logger.Info("TC_PROJECT_004 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_005_Open_Future_Mode()
        {
            Logger.Info("TC_PROJECT_005 started");

            await _projectList.ClickFutureModeAsync();

            await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View"))
                .ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Future_Mode");
            Logger.Info("TC_PROJECT_005 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_006_Open_Inventory()
        {
            Logger.Info("TC_PROJECT_006 started");

            await _projectList.ClickInventorySpecificationsAsync();

            await Assertions.Expect(Page. GetByRole(AriaRole.Heading, new() { Name = "CMO/FMO Inventory" })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Inventory");
            Logger.Info("TC_PROJECT_006 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_007")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_007_Open_Track_Plan()
        {
            Logger.Info("TC_PROJECT_007 started");

            await _projectList.ClickTrackPlanAsync();

            await Assertions.Expect(Page.GetByText("Track Plan")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_007_Track_Plan");
            Logger.Info("TC_PROJECT_007 completed successfully");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_008")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_008_Open_Wave_And_Move()
        {
            Logger.Info("TC_PROJECT_008 started");
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

            await _projectList.ClickWaveAndMoveGroupingPlanAsync();

            await Assertions.Expect(Page.GetByText("Migration Control Center")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_008_Wave_Move");
            Logger.Info("TC_PROJECT_008 completed successfully");
            },nameof(TC_PROJECT_008_Open_Wave_And_Move));
        }

        [Fact]
        [AllureOwner("TC_PROJECT_009")]
        [AllureTag("Regression")]
        public async Task TC_PROJECT_009_Verify_Project_Actions_Menu()
        {
            Logger.Info("TC_PROJECT_009 started");

            await _projectList.VerifyProjectActionsMenuAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_009_Project_Actions_Menu");
            Logger.Info("TC_PROJECT_009 completed successfully");
        }
    }
}