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
            _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

        public InfrastructureUploadPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
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

        public async Task SetInfrastructureDataAsync(String value ="Default Upload Mode")
        {
            string cmdbFile = FileUploadHelper.GetRandomCMDBFile(); 
            string udZipFile = FileUploadHelper.GetRandomUDZipFile();
            string appTemplateFile = FileUploadHelper.GetRandomAppTemplateFile(); 

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
                default:
                    await _page.SetInputFilesAsync("input[id='cmdbFileInput']", cmdbFile);
                    await _page.SetInputFilesAsync("input[id='udFileInput']", udZipFile);
                    await _page.SetInputFilesAsync("input[id='appFileInput']", appTemplateFile);
                    break;
            }         
            

        }

        public async Task ClickVerifyButtonAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Verify" }).ClickAsync();
        }

        public async Task InfrastructureUploadOperationAsync()
        {
            await NavigateToInfrastructureUploadPageAsync();
            await selectRadioButtonAsync("No");
            await ClickNextAsync();
            await SetInfrastructureDataAsync();
        }

       
    }
}
