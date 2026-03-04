using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.MigrationPlanTracking
{
    [Collection("Playwright collection")]
    [AllureSuite("Migration Plan Tracking Tests")]
    public class MigrationPlanTrackingTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectListPage = null!;
        private MigrationPlanTrackingPage _migrationPlanTrackingPage = null!;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage = new ProjectListPage(Page);
            _migrationPlanTrackingPage= new MigrationPlanTrackingPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            _createdProjectName = _basicSetup.projectName;
        }

        [Fact]
        [Trait("Category", "smoke")]
        [AllureOwner("TC_001_MigrationPlanTracking_Page_UI_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_001_MigrationPlanTracking_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
                await _migrationPlanTrackingPage.InitiateMigrationFromInventoryAsync();
                await Assertions.Expect(
                    Page.GetByRole(AriaRole.Heading, new() { Name = "Migration Plan Tracking" })
                ).ToBeVisibleAsync();

                Logger.Info("Migration Plan Tracking page loaded");
            }, nameof(TC_001_MigrationPlanTracking_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "smoke")]
        [AllureOwner("TC_002_MigrationPlanTracking_Should_Have_Clickable_Buttons")]
        [AllureTag("smoke")]
        public async Task TC_002_MigrationPlanTracking_Should_Have_Clickable_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
                await _migrationPlanTrackingPage.InitiateMigrationFromInventoryAsync();
                await Page.WaitForSelectorAsync("table tbody tr");

                var firstRow = Page.Locator("table tbody tr").First;
                var viewPlanButton = firstRow.Locator("td:last-child button").First;

                await Assertions.Expect(viewPlanButton).ToBeEnabledAsync();
                Logger.Info("Migration Plan Tracking page has clickable buttons");
            }, nameof(TC_002_MigrationPlanTracking_Should_Have_Clickable_Buttons));
        }
    }
}