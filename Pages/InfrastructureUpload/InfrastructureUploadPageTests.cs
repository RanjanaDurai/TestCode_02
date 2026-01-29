using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;
using WiseMigrateTests.Pages.Project;

namespace WiseMigrateTests.Pages.InfrastructureUpload
{
    [Collection("Playwright collection")]
    [AllureSuite("Infrastructure Upload Page Tests")]
    public class InfrastructureUploadPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private InfrastructureUploadPage _infrastructureUploadPage = null!;

        private BasicSetup _basicSetup = null!;

        private ProjectListPage _projectListPage = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage= new ProjectListPage(Page);
            _infrastructureUploadPage = new InfrastructureUploadPage(Page, _basicSetup);

            await _loginPage.ValidateSuperAdminAccount(Page);
            await _projectListPage.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            Logger.Info("Logged in as Super Admin");
        }
        
        [Fact]
        [AllureOwner("TC_PROJECT_006")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_006_Upload_Infrastructure_Page_Should_Load()
        {
            //await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();

            Logger.Info("Upload Infrastructure page loaded");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_006_Upload_Infrastructure");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_007")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_007_Proceed_Infrastrcuture_Data_Upload()
        {
            //await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
            await _infrastructureUploadPage.selectRadioButtonAsync("No");
            await _infrastructureUploadPage.ClickNextAsync();
            await Assertions.Expect(Page.GetByText("Infrastructure Data Upload")).ToBeVisibleAsync();
            
            Logger.Info("Proceeded to Infrastructure Upload ");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_007_Upload_Infrastructure");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_008")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_008_Proceed_SSOT_Data_Upload()
        {
            await _infrastructureUploadPage.selectRadioButtonAsync("Yes");
            await _infrastructureUploadPage.ClickNextAsync();
            await Assertions.Expect(Page.GetByText("SSOT Data Upload")).ToBeVisibleAsync();
            Logger.Info("Proceeded to SSOT Upload ");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_008_Proceed_SSOT_Data_Upload");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_009")]
        [AllureTag("regression")]
        public async Task TC_PROJECT_009_Cancel_Upload_Operation()
        {
            await _infrastructureUploadPage.ClickCancelAsync();
            await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
            Logger.Info("Cancelled Infrastructure Upload operation.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_009_Cancel_Upload_Operation");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_010")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_010_Upload_Infrastructure_Data()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await Assertions.Expect(Page.GetByText("Overall Validation Status: Valid")).ToBeVisibleAsync();
            var elements= Page.Locator(".mud-expand-panel-header");
            await Assertions.Expect(elements).ToHaveCountAsync(8);
            Logger.Info("Infrastructure Data uploaded successfully and all files are valid.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_010_Upload_Infrastructure_Data");

        }

        [Fact]
        [AllureOwner("TC_PROJECT_011")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_011_Verify_Infrastructure_Data()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(3);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await Assertions.Expect(Page.GetByText("Data Preview")).ToBeVisibleAsync();   
            Logger.Info("Infrastructure Data verified successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_011_Verify_Infrastructure_Data");         
        }

        /*
        [Fact]
        [AllureOwner("TC_PROJECT_012")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_012_Verify_Interaction_With_Infrastructure_Data_Preview()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            var dialog= Page.GetByRole(AriaRole.Dialog);
            await Assertions.Expect(dialog.GetByText("Data Preview")).ToBeVisibleAsync();
            await _infrastructureUploadPage.ExpandDataPreviewAsync();
            await Assertions.Expect(dialog.Locator(".mud-table-container")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_012_Verify_Interaction_With_Infrastructure_Data_Preview");   
            await _infrastructureUploadPage.ClosePreviewAsync();
            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();
            Logger.Info("Interaction with Preview verified successfully.");
            
        }
        */

        [Fact]
        [AllureOwner("TC_PROJECT_013")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_013_Validate_Upload_Infrastructure_Files()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(10);
            await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Your data has been uploaded and merged successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_013_Validate_Upload_Infrastructure_Files");   
            Logger.Info("Upload Infrastructure Data verified successfully.");
            

        }

        [Fact]
        [AllureOwner("TC_PROJECT_014")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_014_Validate_SSOT_Generation_Infrastructure_Files()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await _basicSetup.WaitForPageAsync(3);
            var successAlerts = Page.GetByRole(AriaRole.Alert);
            await Assertions.Expect(successAlerts.Nth(1)).ToHaveTextAsync("SSOT generated successfully!");   
            await Assertions.Expect(successAlerts.Nth(2)).ToContainTextAsync("Data Quality Report generated successfully.");    
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_014_Validate_SSOT_Generation_Infrastructure_Files");   
            Logger.Info("SSOT Generation verified successfully.");
            
        }

        [Fact]
        [AllureOwner("TC_PROJECT_015")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_015_Validate_SSOT_Preview_Infrastructure_Files()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.PreviewSSOTAsync();
            await _basicSetup.WaitForPageAsync(5);
            var dialog= Page.GetByRole(AriaRole.Dialog);
            await Assertions.Expect(dialog.GetByText("SSOT Data Viewer")).ToBeVisibleAsync();
            await Assertions.Expect(dialog.Locator(".mud-table-container")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_015_Validate_SSOT_Preview_Infrastructure_Files");   
            await _infrastructureUploadPage.ClosePreviewAsync();    
            Logger.Info("SSOT Preview verified successfully.");    
        }

        [Fact]
        [AllureOwner("TC_PROJECT_016")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_016_Validate_Generate_Architecture_Operation()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.PreviewSSOTAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await _basicSetup.WaitForPageAsync(5);
            await Assertions.Expect(Page.GetByRole(AriaRole.Alert)).ToHaveTextAsync("Architecture generated successfully!.");  
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_016_Validate_Generate_Architecture_Operation");
            Logger.Info("Architecture generation operation verified successfully.");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_017")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_017_Validate_View_Architecture_Operation()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Preview SSOT"})).ToBeVisibleAsync();
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ViewArchitectureAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch"));
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_017_Validate_View_Architecture_Operation");
            Logger.Info("View Architecture operation verified successfully.");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_018")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_018_Upload_SSOT_Data()
        {
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await Assertions.Expect(Page.GetByText("Overall Validation Status: Valid")).ToBeVisibleAsync();
            var elements= Page.Locator(".mud-expand-panel-header");
            await Assertions.Expect(elements).ToHaveCountAsync(7);
            Logger.Info("SSOT file uploaded successfully and all files are valid.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_018_Upload_SSOT_Data");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_019")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_019_Verify_SSOT_Data()
        {
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(3);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            var dialog= Page.GetByRole(AriaRole.Dialog);
            await Assertions.Expect(dialog.GetByText("Data Preview")).ToBeVisibleAsync();   
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_019_Verify_SSOT_Data");
            Logger.Info("SSOT Data verified successfully.");
      
        }

        /*[Fact]
        [AllureOwner("TC_PROJECT_020")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_020_Verify_Interaction_With_SSOT_Data_Preview()
        {
            
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            var dialog= Page.GetByRole(AriaRole.Dialog);
            await Assertions.Expect(dialog.GetByText("Data Preview")).ToBeVisibleAsync();
            await _infrastructureUploadPage.ExpandDataPreviewAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();
        }  
        */ 

        [Fact]
        [AllureOwner("TC_PROJECT_021")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_021_Validate_Upload_SSOT_File()
        {
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(10);
            await Assertions.Expect(Page.Locator("role=alert").Last).ToHaveTextAsync("Your data has been uploaded and merged successfully.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_021_Validate_Upload_SSOT_File");   
            Logger.Info("Upload SSOT Data verified successfully.");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_022")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_022_Validate_SSOT_Architecture_Generation()
        {
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await _basicSetup.WaitForPageAsync(3);
            await Assertions.Expect(Page.GetByRole(AriaRole.Alert).Last).ToHaveTextAsync("Architecture generated successfully!.");
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_022_Validate_SSOT_Architecture_Generation");
            Logger.Info("Architecture generation using SSOT verified successfully.");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_023")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_23_Validate_View_SSOT_Architecture_Operation()
        {
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await _basicSetup.WaitForPageAsync(5);
            await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "View Architecture"})).ToBeVisibleAsync();
            await _infrastructureUploadPage.ViewArchitectureAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch"));
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_023_Validate_View_SSOT_Architecture_Operation");
            Logger.Info("View Architecture operation using SSOT verified successfully.");
        }

        [Fact]
        [AllureOwner("TC_PROJECT_024")]
        [AllureTag("smoke")]
        public async Task TC_PROJECT_024_Validate_SSOT_Reconciliation_Operation()
        {
            await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.GenerateSSOTOperationAsync();
            await _basicSetup.WaitForPageAsync(2);
            await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Preview SSOT" })).ToBeVisibleAsync();
            await _infrastructureUploadPage.ClickBackAsync();
            await _infrastructureUploadPage.SSOTUploadOperationAsync();
            await _basicSetup.WaitForPageAsync(5);
            await _infrastructureUploadPage.ClickVerifyButtonAsync();
            //await Page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Button, new(){ Name="Yes"}).ClickAsync();
            await _infrastructureUploadPage.ClosePreviewAsync();
            await _basicSetup.WaitForPageAsync(1);
            await _infrastructureUploadPage.ClickUploadButtonAsync();
            await _basicSetup.WaitForPageAsync(2);
            var dialog= Page.GetByRole(AriaRole.Dialog);
            await Assertions.Expect(dialog.GetByText("Confirm SSOT Generation")).ToBeVisibleAsync();
            await _infrastructureUploadPage.ClickConfirmAsync();
            await Assertions.Expect(Page.GetByText("SSOT Data Reconciliation")).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_022_Validate_SSOT_Reconciliation_01");   
            await _infrastructureUploadPage.HandleSSOTReconciliationSelectionAsync();
            await _basicSetup.WaitForPageAsync(5);
            await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).Last.GetByText("Confirm SSOT Update")).ToBeVisibleAsync();
            await _infrastructureUploadPage.ClickConfirmAsync();
            await _basicSetup.WaitForPageAsync(5);
            var successAlerts = Page.GetByRole(AriaRole.Alert);
            await Assertions.Expect(successAlerts.Nth(1)).ToHaveTextAsync("SSOT generated successfully!");   
            await Assertions.Expect(successAlerts.Nth(2)).ToContainTextAsync("Data Quality Report generated successfully.");  
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_PROJECT_022_Validate_SSOT_Reconciliation_02");   
            await _infrastructureUploadPage.GenerateArchitectureAsync();
            await _basicSetup.WaitForPageAsync(2);
            await _infrastructureUploadPage.ViewArchitectureAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch")); 

        } 
   
   }

}
