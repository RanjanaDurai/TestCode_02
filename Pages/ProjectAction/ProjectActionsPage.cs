using Microsoft.Playwright;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectActionsPage
    {
        private readonly IPage _page;

        public ProjectActionsPage(IPage page)
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

        public async Task ClickClientModeAsync()
        {
            Logger.Info("Opening Client Mode of Operation");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(2);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickFutureModeAsync()
        {
            Logger.Info("Opening Future Mode of Operation");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(3);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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

        public async Task ClickTrackPlanAsync()
        {
            Logger.Info("Opening Track Plan");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(5);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickWaveAndMoveGroupingPlanAsync()
        {
            Logger.Info("Opening Wave & Move Grouping Plan");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(6);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task VerifyProjectActionsMenuAsync()
        {
            Logger.Info("Verifying Project Actions menu");

            var actionsButton = _page
                .Locator("td:last-child")
                .GetByRole(AriaRole.Button)
                .First;

            await actionsButton.ScrollIntoViewIfNeededAsync();
            await actionsButton.ClickAsync();

            var menu = _page.Locator("div.mud-popover-open");
            await menu.WaitForAsync();

            await Assertions.Expect(menu.GetByText("View Activity Logs")).ToBeVisibleAsync();
            await Assertions.Expect(menu.GetByText("Manage Collaborators")).ToBeVisibleAsync();
            await Assertions.Expect(menu.GetByText("Delete Project")).ToBeVisibleAsync();

            await _page.ClickAsync("body", new() { Position = new() { X = 10, Y = 10 } });
        }
    }
}
