using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.ClientModeOperation
{
    [Collection("Playwright collection")]
    [AllureSuite("Client Mode Operation Tests")]
    public class ClientModeOperationPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;

        private ProjectListPage _projectList = null!;

        // private BasicSetup _basicSetup = null!;
        private ClientModeOperationPage _cmoPage = null!;

        public async new Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
            _cmoPage = new ClientModeOperationPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            Logger.Info("Logged in as Super Admin and created a basic project");
        }

        [Fact]
        [AllureOwner("TC_001_CMO_Page_UI_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_001_CMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await Assertions.Expect(Page.GetByText("Current Mode of Operation - Architectural View")).ToBeVisibleAsync();
                Logger.Info("CMO Page UI loaded successfully");
            }, nameof(TC_001_CMO_Page_UI_Should_Load));
        }

        [Fact]
        [AllureOwner("TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("smoke")]
        public async Task TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                //await Page.Locator("Current Mode of Operation - Architectural View").WaitForAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
                Logger.Info("CMO Page has clickable navigation buttons");
            }, nameof(TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons));
        }
    }
}