using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ProjectInventory;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.MigrationPlanTracking
{
    [Collection("Playwright collection")]
    [AllureSuite("Migration Control Centre Tests")]
    public class MigrationControlCentreTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private BasicSetup _basicSetup = null!;
        private MigrationControlCentrePage _migrationControlCentrePage = null!;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _migrationControlCentrePage = new MigrationControlCentrePage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
        }

        [Fact]
        [AllureOwner("TC_MigrationControlCentre_001")]
        [AllureTag("smoke")]
        public async Task TC_MigrationControlCentre_001_MigrationControlCentre_Page_UI_Should_Load()
        {
            //await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
            await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Control Center" })).ToBeVisibleAsync();
            Logger.Info("Migration Control Centre page loaded");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_MigrationControlCentre_001_MigrationControlCentre_Page_UI");
        }
        
        /*[Fact]
        [AllureOwner("TC_MigrationControlCentre_002")]
        [AllureTag("smoke")]
        public async Task TC_MigrationControlCentre_002_Navigate_To_Detailed_Planning_Tab()
        {
            await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();
            await Assertions.Expect(Page.GetByText("Migration Planning")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_MigrationControlCentre_002_DetailedPlanning_Tab_UI");
            Logger.Info("Detailed Planning tab loaded");
        }
        */

        [Fact]
        [AllureOwner("TC_MigrationControlCentre_002")]
        [AllureTag("smoke")]
        public async Task TC_MigrationControlCentre_002_Navigate_To_Timeline_Tab()
        {
            
            await _migrationControlCentrePage.OpenTimelineTabAsync();
            await Assertions.Expect(Page.GetByText("Wave Execution Schedule")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_MigrationControlCentre_003_Timeline_Tab_UI");
            Logger.Info("Timeline tab loaded");
        }

        [Fact]
        [AllureOwner("TC_MigrationControlCentre_003")]
        [AllureTag("smoke")]
        public async Task TC_MigrationControlCentre_003_Navigate_To_Manage_Tab()
        {
            await _migrationControlCentrePage.OpenManageTabAsync();
            await Assertions.Expect(Page.GetByText("Wave Plan Management")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_MigrationControlCentre_004_Manage_Tab_UI");
            Logger.Info("Manage tab loaded");
        }

        [Fact]
        [AllureOwner("TC_MigrationControlCentre_004")]
        [AllureTag("smoke")]
        public async Task TC_MigrationControlCentre_004_Navigate_To_PoliciesAndRisk_Tab()
        {
            await _migrationControlCentrePage.OpenPoliciesAndRiskTabAsync();
            await Assertions.Expect(Page.GetByText("Migration Policies")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_MigrationControlCentre_005_PoliciesAndRisk_Tab_UI");
            Logger.Info("Policies and Risk tab loaded");
        }
    }
}