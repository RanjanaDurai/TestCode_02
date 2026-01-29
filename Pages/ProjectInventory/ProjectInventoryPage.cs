using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;

namespace WiseMigrateTests.Pages.ProjectInventory
{
    public class ProjectInventoryPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;

        private readonly FutureModeOperationPage _fmoPage=null!;

        public ProjectInventoryPage(IPage page)
        {
            _page = page;
            _fmoPage = new FutureModeOperationPage(page);
            _basicSetup = new BasicSetup(page);
        }

        public async Task NavigateToInventoryPageAsync()
        {
            await _fmoPage.NavigateToFMOPageAsync();
            await _fmoPage.ClickNavigationNextAsync();
            await _basicSetup.WaitForPageAsync(2);
            
        }

        private async Task<ILocator> GetMigrationStatusCellWithIconsAsync(int iconCount = 7)
        {
            Logger.Info($"Searching for project row with {iconCount} migration icons");

            var migrationCells = _page.Locator("td[data-label='Migration Status']");
            int cellCount = await migrationCells.CountAsync();

            for (int i = 0; i < cellCount; i++)
            {
                var cell = migrationCells.Nth(i);
                int currentIconCount = await cell.Locator(".mud-icon-root").CountAsync();

                if (currentIconCount == iconCount)
                {
                    Logger.Info($"Matching project row found at index {i + 1}");
                    return cell;
                }
            }

            throw new Exception($"No project row found with {iconCount} icons");
        }
        
         public async Task ClickInventorySpecificationsAsync()
        {
            Logger.Info("Opening Inventory Specifications");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(4);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task WaitForInventoryToFullyLoadAsync(int timeoutMs = 90000)
        {
            Logger.Info("Waiting for Application Inventory to fully load");

            await _page.WaitForSelectorAsync(
                "table tbody tr",
                new PageWaitForSelectorOptions { Timeout = timeoutMs }
            );

            await _page.WaitForFunctionAsync(
                @"() => {
                    // MudBlazor progress bar / loading indicator
                    const progressBars = document.querySelectorAll(
                        '.mud-progress-linear, .mud-progress-circular'
                    );

                    return progressBars.length === 0;
                }",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = timeoutMs
                }
            );

            Logger.Info("Application Inventory fully loaded");
        }

        public async Task InitiateMigrationFromInventoryAsync()
        {
            Logger.Info("Initiating migration from Inventory");

            await WaitForInventoryToFullyLoadAsync();

            var migrateButton = _page.GetByRole(
                AriaRole.Button,
                new() { Name = "MIGRATE ALL IN-SCOPE APPS" }
            );

            await Assertions.Expect(migrateButton).ToBeEnabledAsync();
            await migrateButton.ClickAsync();

            await Assertions.Expect(_page.GetByText("Confirm Migration"))
                .ToBeVisibleAsync();

            await _page.GetByRole(AriaRole.Button, new() { Name = "Proceed" })
                .Last.ClickAsync();

            await Assertions.Expect(_page.GetByText("Initial Migration Parameters"))
                .ToBeVisibleAsync();

            await _page.GetByRole(AriaRole.Button, new() { Name = "Proceed" })
                .Last.ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            Logger.Info("Migration initiated successfully");
        }        
        
    }
}