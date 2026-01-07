using Microsoft.Playwright;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPage
    {
        private readonly IPage _page;

        public ProjectListPage(IPage page)
        {
            _page = page;
        }

        public ILocator CreateNewProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CREATE NEW PROJECT" });

        public async Task WaitForDashboardToBeReadyAsync()
        {
            // Logger.Info("Waiting for Project Dashboard to be fully loaded");

            // 1. Ensure correct URL
            await _page.WaitForURLAsync("**/projects");

            // 2. Ensure dashboard heading is visible
            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new() { Name = "Project Dashboard" })
            ).ToBeVisibleAsync();

            // 3. Ensure table header is rendered (key signal)
            await Assertions.Expect(
                _page.GetByRole(AriaRole.Columnheader, new() { Name = "Project Name" })
            ).ToBeVisibleAsync();

            // Logger.Info("Project Dashboard is fully ready");
        }

        public async Task ClickCreateNewProjectAsync()
        {
            await CreateNewProjectButton.WaitForAsync();
            await CreateNewProjectButton.ClickAsync();
        }

        public async Task AssertProjectVisibleAsync(string projectName)
        {
            await _page.Locator("tr", new() { HasText = projectName }).WaitForAsync();
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

        public async Task WaitForProjectDashboardIconsAsync(int iconCount = 7)
        {
            Logger.Info("Waiting for project dashboard icons to load");

            await _page.WaitForFunctionAsync(
                @"(expectedIcons) => {
                    const cells = document.querySelectorAll('td[data-label=""Migration Status""]');
                    return Array.from(cells).some(cell =>
                        cell.querySelectorAll('.mud-icon-root').length === expectedIcons
                    );
                }",
                iconCount,
                new PageWaitForFunctionOptions
                {
                    Timeout = 30000
                }
            );

            Logger.Info("Project dashboard icons loaded successfully");
        }

        public async Task ClickCurrentModeAsync()
        {
            Logger.Info("Opening Current Mode of Operation");

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