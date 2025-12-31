using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.ProjectCreation
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Creation Page Tests")]
    public class ProjectCreationPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private ProjectCreationPage _projectCreation = null!;
        private BasicSetup _basicSetup = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _projectCreation = new ProjectCreationPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.ValidateSuperAdminAccount(Page);
            Logger.Info("Logged in as Super Admin");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        // TC_PROJECT_004

        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_004_Create_Project_Successfully()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();

            Logger.Info($"Project created successfully:");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_004_Project_Created");
        }

        // TC_PROJECT_005

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("regression")]
        public async Task TC_PROJECT_005_Project_Should_Appear_On_Dashboard()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();
            string projectName = $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
            await _basicSetup.CreateProjectAsync(projectName);
            await _projectCreation.ClickCancelAsync();
            await _projectList.WaitForDashboardAsync();          
            await _projectList.AssertProjectVisibleAsync(projectName);

            Logger.Info($"Project visible on dashboard: {projectName}");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_005_Project_Visible");
        }

      }  

}