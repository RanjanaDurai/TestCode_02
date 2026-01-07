using Microsoft.Playwright;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectInventoryPage
    {
        private readonly IPage _page;

        public ProjectInventoryPage(IPage page)
        {
            _page = page;
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

        public async Task NavigateToMigrationPlanTrackingFromInventoryAsync()
        {
            Logger.Info("Navigating to Migration Plan Tracking from Inventory");

            await ClickInventorySpecificationsAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "CMO/FMO Inventory",
                    Exact = false
                })
            ).ToBeVisibleAsync();

            await InitiateMigrationFromInventoryAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Plan Tracking"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Plan Tracking page loaded");
        }

        public async Task ClickViewPlanDetailsAsync()
        {
            Logger.Info("Clicking View Plan Details icon");

            await _page.WaitForSelectorAsync("table tbody tr");

            var firstRow = _page.Locator("table tbody tr").First;

            var viewPlanButton = firstRow
                .Locator("td:last-child button")
                .First;

            await viewPlanButton.ScrollIntoViewIfNeededAsync();
            await viewPlanButton.ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Control Center"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Control Center page loaded");
        }

        public async Task VerifyMigrationControlCenterLoadedAsync()
        {
            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Control Center"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Control Center verified");
        }

        public async Task OpenDetailedPlanningTabAsync()
        {
            Logger.Info("Opening Detailed Planning");

            await _page.GetByText("Detailed Planning", new() { Exact = true })
                .ClickAsync();

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