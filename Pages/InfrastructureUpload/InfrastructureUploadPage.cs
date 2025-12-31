using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.SignUp
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

        public async Task ValidateRegisterationEmptyUserName(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("empty_username");
            await _basicSetup.SignUpAsync(user.Role, user.Username, user.Password, user.Password);      
        }

        public async Task ValidateRegisterationEmptyPassword(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("empty_password");
            var generatedUsername=Guid.NewGuid().ToString("N").Substring(0, 10)+"@migrate.com";
            await _basicSetup.SignUpAsync(user.Role, generatedUsername, user.Password, user.Password);             
        }

        public async Task ValidateRegisterationDuplicateEmail(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var username ="DuplicateUser@migrate.com";
            var user = Utils.ConfigReader.GetCredential("standard_user");
            for (int i = 0; i < 2; i++)
            {
                await _basicSetup.SignUpAsync(user.Role, username, user.Password, user.Password);
            } 
            
        }

        public async Task ValidateRegisterationMismatchingPassword(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("invalid");
            await _basicSetup.SignUpAsync(user.Role, user.Username, user.Password, Guid.NewGuid().ToString().Substring(0, 10)); 
        }

        public async Task ValidateRegisterationEmptyOrganization(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("invalid");
            await _basicSetup.SignUpAsync(user.Role, user.Username, user.Password, user.Password, isOrganizationEmpty:true); 
        }
        
        public async Task ValidateRegisterationValidCredentials(IPage page)
        {
            //await NavigateToSignUpPageAsync();
            var user = Utils.ConfigReader.GetCredential("standard_user");
            var generatedUsername=Guid.NewGuid().ToString("N").Substring(0, 10)+ "@migrate.com";
            await _basicSetup.SignUpAsync(user.Role, generatedUsername, user.Password, user.Password); 
        }

    }
}
