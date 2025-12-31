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
    }

}
