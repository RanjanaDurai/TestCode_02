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
            _createdProjectName = _basicSetup.projectName;
            await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_MigrationControlCentre_Page_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_001_MigrationControlCentre_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                //await _migrationControlCentrePage.NavigateToMigrationControlCentreAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Control Center" }))
                    .ToBeVisibleAsync();
                Logger.Info("Migration Control Centre page loaded");
            }, nameof(TC_001_MigrationControlCentre_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_Navigate_To_Timeline_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_002_Navigate_To_Timeline_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenTimelineTabAsync();
                await Assertions.Expect(Page.GetByText("Wave Execution Schedule"))
                    .ToBeVisibleAsync();
                Logger.Info("Timeline tab loaded");
            }, nameof(TC_002_Navigate_To_Timeline_Tab));
        }

        /*[Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab)]
        [AllureTag("Smoke")]
        public async Task TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
            await _migrationControlCentrePage.OpenDetailedPlanningTabAsync();
            await Assertions.Expect(Page.GetByText("Migration Planning")).ToBeVisibleAsync();
            Logger.Info("Detailed Planning tab loaded");
            }, nameof(TC_002_MigrationControlCentre_Navigate_To_Detailed_Planning_Tab));
        }
        */

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_Navigate_To_Manage_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_003_Navigate_To_Manage_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenManageTabAsync();
                await Assertions.Expect(Page.GetByText("Wave Plan Management"))
                    .ToBeVisibleAsync();
                Logger.Info("Manage tab loaded");
            }, nameof(TC_003_Navigate_To_Manage_Tab));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_004_Navigate_To_PoliciesAndRisk_Tab")]
        [AllureTag("Smoke")]
        public async Task TC_004_Navigate_To_PoliciesAndRisk_Tab()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationControlCentrePage.OpenPoliciesAndRiskTabAsync();
                await Assertions.Expect(Page.GetByText("Migration Policies"))
                    .ToBeVisibleAsync();
                Logger.Info("Policies and Risk tab loaded");
            }, nameof(TC_004_Navigate_To_PoliciesAndRisk_Tab));
        }
    }
}