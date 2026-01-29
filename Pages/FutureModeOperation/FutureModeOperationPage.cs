using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ClientModeOperation;

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
    }
}
