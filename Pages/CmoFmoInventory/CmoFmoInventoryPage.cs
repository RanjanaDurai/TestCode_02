 using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;
using WiseMigrateTests.Pages.ProjectInventory;

namespace WiseMigrateTests.Pages.CmoFmoInventory
{
    public class CmoFmoInventoryPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;
        private readonly FutureModeOperationPage _fmoPage=null!;
        private readonly ProjectInventoryPage _projectInventoryPage=null!;
        private readonly CmoFmoInventoryPage _CmoFmoInventoryPage=null!;
        

        public CmoFmoInventoryPage(IPage page)
        {
            _page = page;
            _projectInventoryPage= new ProjectInventoryPage(page);
            _fmoPage = new FutureModeOperationPage(page);
            _basicSetup = new BasicSetup(page);
        }
        public async Task NavigateToMigrationPlanTrackingAsync()
        {
            await _projectInventoryPage.NavigateToInventoryPageAsync();
            // await _projectInventoryPage.InitiateMigrationFromInventoryAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        // public async Task InitiateMigrationFromInventoryAsync()
        // {
        //     await _projectInventoryPage.InitiateMigrationFromInventoryAsync();
        //     await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // }


        // public async Task ClickViewPlanDetailsAsync()
        // {
        //    await _page.WaitForSelectorAsync("table tbody tr");
        //     var firstRow = _page.Locator("table tbody tr").First;
        //     var viewPlanButton = firstRow.Locator("td:last-child button").First;

        //     await viewPlanButton.ScrollIntoViewIfNeededAsync();
        //     await viewPlanButton.ClickAsync();

        //     await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // }

        // REGRESSION TEST CASES 

        public ILocator GoToInventoryButton =>
            _page.Locator(".mr-3 > div:nth-child(2) > .mud-button-root");

        public ILocator BackToFMOButton =>
            _page.Locator(".mud-button-root.mud-icon-button.mud-button").First;

        public ILocator InventoryHeading =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "CMO/FMO Inventory" });

        
        public ILocator ServerViewButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Server View" });

        public ILocator ServerInventoryHeading =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Server Inventory" });

        public ILocator ApplicationViewButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Application View" });

        public ILocator ApplicationInventoryHeading =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Application Inventory" });

        public ILocator ExportButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Export" });

        public ILocator SyncButton =>
            _page.GetByRole(AriaRole.Group)
                 .Filter(new() { HasText = "Filters Server View" })
                 .GetByRole(AriaRole.Button).Nth(4);

        public ILocator FilterButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Filters" });

        public ILocator FilterOptionsHeading =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Filter Options" });

        public ILocator FilterByAppNameTextbox =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Filter by Application Name" });

        public ILocator ClearAllButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Clear All" });

        public async Task NavigateToInventoryPageAsync()
        {
            await _fmoPage.NavigateToFMOPageAsync();
            await GoToInventoryButton.ClickAsync();
            await Assertions.Expect(InventoryHeading).ToBeVisibleAsync();
        }


    }

}