using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Pages.ClientModeOperation;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Allure.Xunit.Attributes;


namespace WiseMigrateTests.Pages.FutureModeOperation
{
    [Collection("Playwright collection")]
    [AllureSuite("Future Mode Operation Tests")]
    public class FutureModeOperationPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;

        private ProjectListPage _projectList = null!;

        private BasicSetup _basicSetup = null!;
        private ClientModeOperationPage _cmoPage = null!;

        private FutureModeOperationPage _fmoPage = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
            _fmoPage = new FutureModeOperationPage(Page);


            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();

            Logger.Info("Logged in as Super Admin and created a basic project");

        }

        [Fact]
        [AllureOwner("TC_FMO_001")]
        [AllureTag("smoke")]
        public async Task TC_FMO_001_FMO_Page_UI_Should_Load()
        {
            await _fmoPage.NavigateToFMOPageAsync();
            await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_FMO_001_FMO_Page_UI");
            Logger.Info("FMO Page UI loaded successfully");
        }

        [Fact]
        [AllureOwner("TC_FMO_002")]
        [AllureTag("smoke")]
        public async Task TC_FMO_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _fmoPage.NavigateToFMOPageAsync();
            await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
            await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
            await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_FMO_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons");
            Logger.Info("FMO Page has clickable navigation buttons");
        } 
    
    }
}