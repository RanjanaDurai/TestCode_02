
using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;
using WiseMigrateTests.Pages.MigrationPlanTracking;

namespace WiseMigrateTests.Pages.ProjectInventory
{
    public class MigrationControlCentrePage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;

        private readonly MigrationPlanTrackingPage _migrationPlanTrackingPage=null!;

        public MigrationControlCentrePage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _migrationPlanTrackingPage = new MigrationPlanTrackingPage(page);
        }
        
        public async Task NavigateToMigrationControlCentreAsync()
        {
            await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
            await _migrationPlanTrackingPage.InitiateMigrationFromInventoryAsync();
            await _migrationPlanTrackingPage.ClickViewPlanDetailsAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task OpenDetailedPlanningTabAsync()
        {
            await _page.GetByText("Detailed Planning", new() { Exact = true }).ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Planning"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Planning verified");
        }

        public async Task OpenTimelineTabAsync()
        {
            Logger.Info("Opening Timeline");

            await _page.GetByText("Timeline", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Execution Schedule"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Wave Execution Schedule verified");
        }

        public async Task OpenManageTabAsync()
        {
            Logger.Info("Opening Manage");

            await _page.GetByText("Manage", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Plan Management"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Wave Plan Management verified");
        }

        public async Task OpenPoliciesAndRiskTabAsync()
        {
            Logger.Info("Opening Policies & Risk");

            await _page.GetByText("Policies & Risk", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Policies"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Policies verified");
        }
    }
}