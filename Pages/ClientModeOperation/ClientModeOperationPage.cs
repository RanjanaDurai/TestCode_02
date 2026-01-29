using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.Pages.InfrastructureUpload;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.ClientModeOperation
{
    public class ClientModeOperationPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly InfrastructureUploadPage _infrastructureUpload;

        public ILocator NextButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

        public ILocator CancelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

        public ClientModeOperationPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _infrastructureUpload= new InfrastructureUploadPage(page, _basicSetup);
        }

        public async Task NavigateToCMOPageAsync()
        {
            await _basicSetup.InfrastructureUploadPageSetupAsync();            
            await _basicSetup.WaitForPageAsync(2);                         
        }


        public async Task ClickNavigationNextAsync()
        {
            await _page.Locator(".mr-3").GetByRole(AriaRole.Button).Last.ClickAsync();
        }
    }
}
