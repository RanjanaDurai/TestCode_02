using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ClientModeOperation;
using System.Text.RegularExpressions;

namespace WiseMigrateTests.Pages.FutureModeOperation
{
    public class FutureModeOperationPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;
        private readonly ClientModeOperationPage _cmoPage;

        public ILocator NextButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

        public ILocator CancelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

        public FutureModeOperationPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _cmoPage= new ClientModeOperationPage(page);
        }

        public async Task NavigateToFMOPageAsync()
        {
            await _cmoPage.NavigateToCMOPageAsync();
            await _cmoPage.ClickNavigationNextAsync();
            await _basicSetup.WaitForPageAsync(2);                         
        }

        public async Task ClickNavigationNextAsync()
        {
            await _page.Locator(".mr-3").GetByRole(AriaRole.Button).Last.ClickAsync();
        }


        //  Regression TEST 

        public ILocator Diagram => _page.Locator("#diagram");

        // ================= Toolbar Buttons =================
        public ILocator ZoomInButton =>
            _page.Locator(".main-content > .d-flex.flex-column.gap-3 > .mud-toolbar > div > .mud-button-root").First;

        public ILocator ZoomOutButton =>
            _page.Locator(".main-content > .d-flex.flex-column.gap-3 > .mud-toolbar > div:nth-child(2) > .mud-button-root");

        public ILocator ResetZoomButton =>
            _page.Locator(".main-content > .d-flex.flex-column.gap-3 > .mud-toolbar > div:nth-child(3) > .mud-button-root");

        public ILocator DownloadSvgButton =>
            _page.Locator("div:nth-child(5) > .mud-button-root");

        public ILocator FullScreenButton =>
            _page.Locator("div:nth-child(6) > .mud-button-root");

        public ILocator ShowLegendButton =>
            _page.Locator("div:nth-child(7) > .mud-button-root");

        public ILocator CloseLegendButton =>
            _page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.mud-icon-button-size-small");

        public ILocator ExportToExcelButton =>
            _page.Locator(".mud-toolbar > div:nth-child(3) > .mud-button-root").First;

        public ILocator SyncDataButton =>
            _page.Locator(".mud-button-root.mud-icon-button.mud-info-text").First;

        // ================= Search =================
        public ILocator SearchButton =>
            _page.Locator(".search-container > .mud-tooltip-root > .mud-button-root");

        public ILocator SearchTextbox =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Search Server..." });

        // ================= Migration Cost =================
        public ILocator ViewMigrationCostButton =>
            _page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.my-2");

        public ILocator MigrationCostHeader =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Server Cost Details" });

        public ILocator CloseMigrationCostButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Close dialog" });
        

        public ILocator OneYearReserved =>
            _page.GetByText("1 Year Reserved");

        public ILocator ThreeYearReserved =>
            _page.GetByText("3 Year Reserved");

        public ILocator ChooseTimePeriod =>
            _page.GetByText("Per Month");


        // ================= Reusable Actions =================

        public async Task PerformZoomActionsAsync()
        {
            await ZoomInButton.ClickAsync();
            await ZoomOutButton.ClickAsync();
            await ResetZoomButton.ClickAsync();
        }

        public async Task ToggleLegendAsync()
        {
            await ShowLegendButton.ClickAsync();
            await CloseLegendButton.ClickAsync();
        }

        public async Task PerformSearchAsync()
        {
            await SearchButton.ClickAsync();
            await SearchTextbox.FillAsync("svr");
            await SearchTextbox.PressAsync("Enter");
        }

        public async Task VerifyMigrationCostAsync()
        {
            await ViewMigrationCostButton.First.ClickAsync();
            await Assertions.Expect(MigrationCostHeader).ToBeVisibleAsync();

            await OneYearReserved.ClickAsync();
            await ThreeYearReserved.ClickAsync();

            await CloseMigrationCostButton.ClickAsync();
        }

        private static readonly Random _random = new();
        public async Task RandomSelector()
        {
            var options = _page
                .Locator("#fullscreen-container div")
                .Filter(new() { HasTextRegex = new Regex("^SRV") });

            await options.First.WaitForAsync();

            int count = await options.CountAsync();

            if (count == 0)
                throw new Exception("No SRV options found.");

            int index = _random.Next(count);

            await options.Nth(index).ClickAsync();
        }
    }
}
