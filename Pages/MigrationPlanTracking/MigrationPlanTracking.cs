 using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;
using WiseMigrateTests.Pages.ProjectInventory;

namespace WiseMigrateTests.Pages.MigrationPlanTracking
{
    public class MigrationPlanTrackingPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;

        private readonly FutureModeOperationPage _fmoPage=null!;

        private readonly ProjectInventoryPage _projectInventoryPage=null!;
    
        public MigrationPlanTrackingPage(IPage page)
        {
            _page = page;
            _projectInventoryPage= new ProjectInventoryPage(page);
            _fmoPage = new FutureModeOperationPage(page);
            _basicSetup = new BasicSetup(page);
        }
        public async Task NavigateToMigrationPlanTrackingAsync()
        {
            await _projectInventoryPage.NavigateToInventoryPageAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        }

        public async Task InitiateMigrationFromInventoryAsync()
        {
            await _projectInventoryPage.InitiateMigrationFromInventoryAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        }

        public async Task ClickViewPlanDetailsAsync()
        {
           await _page.WaitForSelectorAsync("table tbody tr");
            var firstRow = _page.Locator("table tbody tr").First;
            var viewPlanButton = firstRow.Locator("td:last-child button").First;

            await viewPlanButton.ScrollIntoViewIfNeededAsync();
            await viewPlanButton.ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}