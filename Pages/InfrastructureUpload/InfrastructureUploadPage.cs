using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.InfrastructureUpload
{
    public class InfrastructureUploadPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly ProjectCreationPage _projectCreation;
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
       
    }
}
