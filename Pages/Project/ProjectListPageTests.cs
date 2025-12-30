using Microsoft.Playwright;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.Project
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Page Tests")]
    public class ProjectListPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private ProjectCreationPage _projectCreation = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _projectCreation = new ProjectCreationPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            Logger.Info("Login successful using Migrate Admin account.");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        // --------------------------------------------------
        // TC_PROJECT_001
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_001")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_001_Project_Dashboard_Should_Load()
        {
            await _projectList.WaitForDashboardAsync();

            await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();

            Logger.Info("Project dashboard loaded successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_001_Project_Dashboard");
        }

        // --------------------------------------------------
        // TC_PROJECT_002
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_002")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_002_Navigate_To_Create_Project_Page()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await Page.WaitForURLAsync("**/project-creation");

            Logger.Info("Navigated to Create Project page.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_002_Create_Project_Page");
        }

        // --------------------------------------------------
        // TC_PROJECT_003
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_003")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_003_Project_Creation_UI_Should_Load()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await _projectCreation.WaitForPageAsync();

            await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();

            Logger.Info("Project creation UI fields are visible.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_003_Project_Creation_UI");
        }

        // --------------------------------------------------
        // TC_PROJECT_004
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_004_Create_Project_Successfully()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await _projectCreation.WaitForPageAsync();

            string projectName = _projectCreation.GenerateProjectName();

            await _projectCreation.FillProjectDetails(
                projectName,
                "test_customer",
                "automation test"
            );

            Logger.Info($"Filling project details → {projectName}");

            await _projectCreation.ClickCreateProjectAsync();

            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data"))
                .ToBeVisibleAsync();

            Logger.Info($"Project created successfully: {projectName}");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Project_Created");
        }

        // --------------------------------------------------
        // TC_PROJECT_005
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_005_Project_Should_Appear_On_Dashboard()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await _projectCreation.WaitForPageAsync();

            string projectName = _projectCreation.GenerateProjectName();

            await _projectCreation.FillProjectDetails(
                projectName,
                "test_customer",
                "verify project on dashboard"
            );

            Logger.Info("Creating project for dashboard validation");

            await _projectCreation.ClickCreateProjectAsync();
            await _projectCreation.ClickCancelAsync();

            await _projectList.AssertProjectVisible(projectName);

            Logger.Info($"Project visible on dashboard: {projectName}");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Project_Visible");
        }

        // --------------------------------------------------
        // TC_PROJECT_006
        // --------------------------------------------------
        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_006_Upload_Infrastructure_Page_Should_Load()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await _projectCreation.WaitForPageAsync();

            string projectName = _projectCreation.GenerateProjectName();

            await _projectCreation.FillProjectDetails(
                projectName,
                "test_customer",
                "verify upload infra page"
            );

            Logger.Info("Creating project and navigating to upload infrastructure page");

            await _projectCreation.ClickCreateProjectAsync();
            await _projectCreation.ClickNextAsync();
            await _projectCreation.WaitForUploadInfrastructurePageAsync();

            Logger.Info("Upload Infrastructure page loaded successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Upload_Infrastructure_Page");
        }
    }
}
