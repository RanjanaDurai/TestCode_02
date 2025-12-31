using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
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
    }
}
