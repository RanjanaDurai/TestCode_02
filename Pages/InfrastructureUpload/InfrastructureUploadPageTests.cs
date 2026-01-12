using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.InfrastructureUpload
{
    [Collection("Playwright collection")]
    [AllureSuite("Infrastructure Upload Page Tests")]
    public class InfrastructureUploadPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private InfrastructureUploadPage _infrastructureUploadPage = null!;

        private BasicSetup _basicSetup = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _infrastructureUploadPage = new InfrastructureUploadPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.ValidateSuperAdminAccount(Page);
            Logger.Info("Logged in as Super Admin");
        }
        
        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_006_Upload_Infrastructure_Page_Should_Load()
        {
            await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();

            Logger.Info("Upload Infrastructure page loaded");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Upload_Infrastructure");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_007")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_007_Proceed_Infrastrcuture_Data_Upload()
        {
            await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await _infrastructureUploadPage.selectRadioButtonAsync("No");
            await _infrastructureUploadPage.ClickNextAsync();
            await Assertions.Expect(Page.GetByText("Infrastructure Data Upload")).ToBeVisibleAsync();
        }

        [Fact]
        [AllureOwner("TC_PROJECT_008")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_008_Proceed_SSOT_Data_Upload()
        {
            await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await _infrastructureUploadPage.selectRadioButtonAsync("Yes");
            await _infrastructureUploadPage.ClickNextAsync();
            await Assertions.Expect(Page.GetByText("SSOT Data Upload")).ToBeVisibleAsync();
        }

        [Fact]
        [AllureOwner("TC_PROJECT_009")]
        [AllureTag("regression")]
        public async Task TC_PROJECT_009_Cancel_Upload_Operation()
        {
            await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await _infrastructureUploadPage.ClickCancelAsync();
            await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
        }

        [Fact]
        [AllureOwner("TC_PROJECT_010")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_010_Upload_Infrastructure_Data()
        {
            await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await _infrastructureUploadPage.selectRadioButtonAsync("No");
            await _infrastructureUploadPage.ClickNextAsync();
            await _infrastructureUploadPage.SetInfrastructureDataAsync("Default Upload Mode");
            await _basicSetup.WaitForPageAsync(2);
            await Assertions.Expect(Page.GetByText("Overall Validation Status: Valid")).ToBeVisibleAsync();
            var elements= Page.Locator(".mud-expand-panel-header");
            await Assertions.Expect(elements).ToHaveCountAsync(8);

        }

        [Fact]
        [AllureOwner("TC_PROJECT_011")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_010_Verify_Infrastructure_Data()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            
        }

    }

}
