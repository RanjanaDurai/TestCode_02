using Microsoft.Playwright;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.InfrastructureUpload;
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

        private InfrastructureUploadPage _infrastructureUploadPage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _projectList = new ProjectListPage(Page);
            _projectCreation = new ProjectCreationPage(Page);
            _infrastructureUploadPage = new InfrastructureUploadPage(Page, _basicSetup);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.ValidateSuperAdminAccount(Page);
            Logger.Info("Logged in as Super Admin");
        }


        [Fact]
        [AllureOwner("TC_PROJECT_004")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_004_Create_Project_Successfully()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _projectList.ClickCreateNewProjectAsync();

            await _basicSetup.CreateProjectAsync();

            await Assertions.Expect(
                Page.GetByText("Upload Infrastructure Data")
            ).ToBeVisibleAsync();
        }

        [Fact]
        [AllureOwner("TC_PROJECT_005")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_005_Project_Should_Appear_On_Dashboard()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _projectList.ClickCreateNewProjectAsync();

            string projectName = $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
            await _basicSetup.CreateProjectAsync(projectName);

            await _infrastructureUploadPage.ClickCancelAsync();
            await _projectList.WaitForDashboardToBeReadyAsync();

            await _projectList.AssertProjectVisibleAsync(projectName);
        }

        // REGRESSION TEST CASES

        [Fact]
        [AllureOwner("TC_Project_01")]
        [AllureTag("Regression")]
        public async Task TC_project_01_SelectRandomTargetCloudeProvider()
        {
            await _projectList.ClickCreateNewProjectAsync();
            await _projectCreation.RandomTargetCloudeProvider();

            Logger.Info("Seleced random cloude provider Successfully");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_01_SelectRandomTargetCloudeProvider");
        }

        [Fact]
        [AllureOwner("TC_Project_02")]
        [AllureTag("Regression")]
        public async Task TC_project_02_ClickBackTodashboard()
        {
            await _projectList.ClickCreateNewProjectAsync();
            await _projectCreation.ClickBackToDashBoardButton();

            Logger.Info("Clicked Back To Dashboard Button");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_project_02_ClickBackTodashboard");
        }

    }
}
