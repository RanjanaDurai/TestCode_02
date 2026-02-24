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
    public class ProjectCreationPageTests : TestBaseFixture
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private ProjectCreationPage _projectCreation = null!;

        private InfrastructureUploadPage _infrastructureUploadPage = null!;
        // private BasicSetup _basicSetup = null!;

        public async Task InitializeAsync()
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


        // [Fact]
        // [AllureOwner("TC_001_Create_Project_Successfully")]
        // [AllureTag("smoke")]
        // public async Task TC_001_Create_Project_Successfully()
        // {
        //     await _attachmentHelper.RunWithTracingAsync(async () =>
        //     {
        //         await _projectList.WaitForDashboardToBeReadyAsync();
        //         await _projectList.ClickCreateNewProjectAsync();
        //         await _basicSetup.CreateProjectAsync();

        //         await Assertions.Expect(
        //             Page.GetByText("Upload Infrastructure Data")
        //         ).ToBeVisibleAsync();
        //     }, nameof(TC_001_Create_Project_Successfully));
        // }

        [Fact]
        [AllureOwner("TC_001_Create_Project_Successfully")]
        [AllureTag("smoke")]
        public async Task TC_001_Create_Project_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await _projectList.ClickCreateNewProjectAsync();

                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.ProjectName;

                await Assertions.Expect(
                    Page.GetByText("Upload Infrastructure Data")
                ).ToBeVisibleAsync();

            }, nameof(TC_001_Create_Project_Successfully));
        }

        [Fact]
        [AllureOwner("TC_002_Project_Should_Appear_On_Dashboard")]
        [AllureTag("smoke")]
        public async Task TC_002_Project_Should_Appear_On_Dashboard()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.WaitForDashboardToBeReadyAsync();
                await _projectList.ClickCreateNewProjectAsync();

                // string projectName = $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
                // await _basicSetup.CreateProjectAsync(projectName);

                string projectName = $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
                await _basicSetup.CreateProjectAsync(projectName);
                _createdProjectName = projectName;

                await _infrastructureUploadPage.ClickCancelAsync();
                await _projectList.WaitForDashboardToBeReadyAsync();

                await _projectList.AssertProjectVisibleAsync(projectName);
            }, nameof(TC_002_Project_Should_Appear_On_Dashboard));
        }

        // REGRESSION TEST CASES

        [Trait("Category", "Regression")]
        [Fact]
        [AllureOwner("TC_003_SelectRandomTargetCloudeProvider")]
        [AllureTag("Regression")]
        public async Task TC_003_SelectRandomTargetCloudeProvider()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCreateNewProjectAsync();
                await _projectCreation.RandomTargetCloudeProvider();

                Logger.Info("Seleced random cloude provider Successfully");
            }, nameof(TC_003_SelectRandomTargetCloudeProvider));
        }

        [Trait("Category", "Regression")]
        [Fact]
        [AllureOwner("TC_004_ClickBackTodashboard")]
        [AllureTag("Regression")]
        public async Task TC_004_ClickBackTodashboard()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectList.ClickCreateNewProjectAsync();
                await _projectCreation.ClickBackToDashBoardButton();

                Logger.Info("Clicked Back To Dashboard Button");
            }, nameof(TC_004_ClickBackTodashboard));
        }

    }
}
