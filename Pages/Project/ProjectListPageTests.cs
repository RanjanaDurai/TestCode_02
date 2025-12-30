using Microsoft.Playwright;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.PreRequisites;
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
        private BasicSetup _basicSetup = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            Logger.Info("Logged in as Migrate Admin");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        // TC_PROJECT_001
        
        [Fact]
        [AllureOwner("TC_PROJECT_001")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_001_Project_Dashboard_Should_Load()
        {
            await _projectList.WaitForDashboardAsync();

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
            await _projectList.WaitForDashboardAsync();
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
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();

            Logger.Info("Project creation UI is visible");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_003_UI");
        }

        // TC_PROJECT_004

        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_004_Create_Project_Successfully()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            string projectName = _basicSetup.GenerateProjectName();

            await _basicSetup.CreateProjectAsync(
                projectName,
                "test_customer",
                "automation test"
            );

            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();

            Logger.Info($"Project created successfully: {projectName}");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Project_Created");
        }

        // TC_PROJECT_005

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_005_Project_Should_Appear_On_Dashboard()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            string projectName = _basicSetup.GenerateProjectName();

            await _basicSetup.CreateProjectAsync(
                projectName,
                "test_customer",
                "verify dashboard"
            );

            await Page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
            await _projectList.WaitForDashboardAsync();

            await _projectList.AssertProjectVisibleAsync(projectName);

            Logger.Info($"Project visible on dashboard: {projectName}");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Project_Visible");
        }

        // TC_PROJECT_006

        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_006_Upload_Infrastructure_Page_Should_Load()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();

            string projectName = _basicSetup.GenerateProjectName();

            await _basicSetup.CreateProjectAsync(
                projectName,
                "test_customer",
                "upload infra test"
            );

            await Page.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();

            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();

            Logger.Info("Upload Infrastructure page loaded");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Upload_Infrastructure");
        }
    }
}
