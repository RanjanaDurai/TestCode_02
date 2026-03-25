using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.InfrastructureUpload
{
    public class InfrastructureUploadPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly ProjectCreationPage _projectCreation;

        public ILocator NextButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

        public ILocator CancelButton =>
            _page.GetByText("Cancel");
        public InfrastructureUploadPage(IPage page, BasicSetup basicSetup)
        {
            _page = page;
            _basicSetup = basicSetup;
            _projectCreation= new ProjectCreationPage(page);
        }

        public async Task NavigateToInfrastructureUploadPageAsync()
        {
            await _projectCreation.NavigateToProjectCreationPageAsync();
            await _basicSetup.CreateProjectAsync();
            await _basicSetup.WaitForPageAsync(2);
                         
        }

        public async Task selectRadioButtonAsync(string option)
        {
            await _page.Locator($"label:has-text('{option}')").ClickAsync();
        }

        public async Task ClickNextAsync()
        {
            await NextButton.ClickAsync();
        }

        public async Task ClickCancelAsync()
        {
            await CancelButton.ClickAsync();
        }

        public async Task ClickBackAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Back" }).ClickAsync();
        }

        public async Task SetInfrastructureDataAsync(String value ="Default Upload Mode")
        {
            string cmdbFile = FileUploadHelper.GetRandomCMDBFile(); 
            string udZipFile = FileUploadHelper.GetRandomUDZipFile();
            string appTemplateFile = FileUploadHelper.GetRandomAppTemplateFile();
            string ssotFile = FileUploadHelper.GetRandomSSOTFile(); 

            switch(value)
            {
                case string s when s.Equals("CMDB Upload", StringComparison.OrdinalIgnoreCase):
                    await _page.SetInputFilesAsync("input[id='cmdbFileInput']", cmdbFile);
                    break;
                case string s when s.Equals("UD Zip Upload", StringComparison.OrdinalIgnoreCase):
                    await _page.SetInputFilesAsync("input[id='udFileInput']", udZipFile);
                    break;
                case string s when s.Equals("App Template Upload", StringComparison.OrdinalIgnoreCase):
                    await _page.SetInputFilesAsync("input[id='appFileInput']", appTemplateFile);
                    break;
                case string s when s.Equals("SSOT Upload", StringComparison.OrdinalIgnoreCase):
                    await _page.SetInputFilesAsync("input[id='ssotFileInput']", ssotFile);
                    break;

                default:
                    await _page.SetInputFilesAsync("input[id='cmdbFileInput']", cmdbFile);
                    await _page.SetInputFilesAsync("input[id='udFileInput']", udZipFile);
                    await _page.SetInputFilesAsync("input[id='appFileInput']", appTemplateFile);
                    break;
            }         
        }

        public async Task ClickVerifyButtonAsync()
        {
            var button =_page.GetByRole(AriaRole.Button, new() { Name = "Verify" });
            await button.ScrollIntoViewIfNeededAsync();
            await button.ClickAsync();
        }

        public async Task InfrastructureUploadOperationAsync()
        {
            await selectRadioButtonAsync("No");
            await ClickNextAsync();
            await SetInfrastructureDataAsync();
        }

        public async Task SSOTUploadOperationAsync()
        {
            //await NavigateToInfrastructureUploadPageAsync();
            await selectRadioButtonAsync("Yes");
            await ClickNextAsync();
            await SetInfrastructureDataAsync("SSOT Upload");
        }

        public async Task ExpandDataPreviewAsync()
        {
            var headerElement= _page.Locator(".mud-expand-panel-header");
            var count= await headerElement.CountAsync();
            for(int i=0;i<count;i++)
            {
                var element= headerElement.Nth(i);
                await element.Locator("svg.mud-expand-panel-icon").ClickAsync();
                await Assertions.Expect(element.Locator(".mud-table-container")).ToBeVisibleAsync();
            }
        }

        public async Task ClosePreviewAsync()
        {
            var element= _page.GetByRole(AriaRole.Dialog);
            await element.GetByLabel("Close dialog").ClickAsync();
        }

        public async Task ClickUploadButtonAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Upload Files" }).ClickAsync();

        }

        public async Task GenerateSSOTOperationAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Generate SSOT" }).ClickAsync();

        }

        public async Task PreviewSSOTAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Preview SSOT" }).ClickAsync();
        }

        public async Task GenerateArchitectureAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Generate Architecture" }).ClickAsync();
        }

        public async Task ViewArchitectureAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "View Architecture" }).ClickAsync();
        }

        public async Task ClickConfirmAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Yes"}).ClickAsync();
        }

        public async Task HandleSSOTReconciliationSelectionAsync()
        {
            var dialog = _page.GetByRole(AriaRole.Dialog);
            var tab= dialog.Locator(".mud-tab");
            var count = await tab.CountAsync();
            var randVal=new Random ().Next(0, 2);
            for(int i=0;i< count;i++)
            {
                await dialog.GetByText("Select All").Nth(randVal).ClickAsync();
            }
            await dialog.GetByRole(AriaRole.Button, new() { Name = "Save Changes"}).ClickAsync();
        }

        // Regression TEST CASES 

        public async Task VerifyClientInfradata()
        {
            await Assertions.Expect(_page.GetByRole(AriaRole.Heading, new() { Name = "Upload Infrastructure Data" })).ToBeVisibleAsync();
        }
        public async Task ClickRandomUser()
        {
            await Task.Delay(5000);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "User" }).ClickAsync();
        }
        public async Task ViewActivityLog()
        {
            await _page.Locator("div:nth-child(3) > .mud-button-root").First.ClickAsync();
        }

        public async Task ClickRandomStatus()
        {
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Status" }).ClickAsync();
        }

        public async Task ClickDateRange()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Open Date Range Picker" }).ClickAsync();
        }
        private ILocator ViewUploadedfilesButton => _page.Locator(".mud-menu > .mud-button-root");
        public async Task<string> GetUploadedFileTypeAsync()
        {
            Logger.Info("Waiting for SSOT or CMDB page to load");

            await Assertions.Expect(_page.GetByRole(AriaRole.Textbox, new() { Name = "Search.." })).ToBeVisibleAsync(new(){Timeout = 10000});

            if (await _page.GetByText("SSOT", new() { Exact = true }).IsVisibleAsync())return "SSOT";

            if (await _page.GetByText("CMDB", new() { Exact = true }).IsVisibleAsync())return "CMDB";

            throw new Exception("Unknown uploaded file type");
        }

        public async Task HandleUploadedFileFlowAsync()
        {
            // await Assertions.Expect(_page.GetByRole(AriaRole.Heading, new() { Name = "Upload Infrastructure Data" })).ToBeVisibleAsync();

            await ViewUploadedfilesButton.ClickAsync();

            await _page.GetByText("Uploaded Files").ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            string type = await GetUploadedFileTypeAsync();

            Logger.Info($"Detected uploaded file type: {type}");

            if (type == "SSOT")
            {
                await ExecuteSSOTFlowAsync();
            }
            else if (type == "CMDB")
            {
                await ExecuteCMDBFlowAsync();
            }
        }

        private async Task ExecuteSSOTFlowAsync()
        {
            await Assertions.Expect(_page.GetByRole(AriaRole.Group).Filter(new() { HasText = "A2AA2SMAppAppInstDBNetworkServer" }).First).ToBeVisibleAsync();
            await Assertions.Expect(_page.GetByText("EXPORT SSOT")).ToBeVisibleAsync();
        }

        private async Task ExecuteCMDBFlowAsync()
        {
            await Assertions.Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Sheet1" })).ToBeVisibleAsync();
        }
   
        public async Task CloseDialog ()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Close dialog" }).ClickAsync();
        }
        public async Task SSOT_Data_Viewer()
        {
            await ViewUploadedfilesButton.ClickAsync();
            await _page.GetByRole(AriaRole.Group).Filter(new() { HasText = "SSOT" }).First.ClickAsync();
        }

        public async Task CloseDataQualityReport()
        {
            await _page.GetByTitle("Close").Locator("path").Nth(1).ClickAsync();
        }
        public async Task Data_quality_report()
        {
            await ViewUploadedfilesButton.ClickAsync();
            await _page.GetByRole(AriaRole.Group).Filter(new() { HasText = "Quality Report" }).ClickAsync();
        }

    }
}