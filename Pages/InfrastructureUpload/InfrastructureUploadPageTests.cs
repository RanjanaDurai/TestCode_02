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
        private ProjectListPage _projectListPage = null!;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper= new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectListPage= new ProjectListPage(Page);
            _infrastructureUploadPage = new InfrastructureUploadPage(Page, _basicSetup);

            await _loginPage.ValidateSuperAdminAccount(Page);
            await Page.GetByRole(AriaRole.Button, new() { Name = "Close snackbar" }).ClickAsync();
            
            Logger.Info("Logged in as Super Admin");
        }
        
        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_050_Upload_Infrastructure_Page_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_050_Upload_Infrastructure_Page_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await Assertions.Expect(Page.GetByText("Upload Infrastructure Data")).ToBeVisibleAsync();
                Logger.Info("Upload Infrastructure page loaded");
            }, nameof(TC_050_Upload_Infrastructure_Page_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_051_Proceed_Infrastrcuture_Data_Upload")]
        [AllureTag("Smoke")]
        public async Task TC_051_Proceed_Infrastrcuture_Data_Upload()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                //await _infrastructureUploadPage.NavigateToInfrastructureUploadPageAsync();
                await _infrastructureUploadPage.selectRadioButtonAsync("No");
                await _infrastructureUploadPage.ClickNextAsync();
                await Assertions.Expect(Page.GetByText("Infrastructure Data Upload")).ToBeVisibleAsync();
                Logger.Info("Proceeded to Infrastructure Upload ");
            }, nameof(TC_051_Proceed_Infrastrcuture_Data_Upload));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_052_Proceed_SSOT_Data_Upload")]
        [AllureTag("Smoke")]
        public async Task TC_052_Proceed_SSOT_Data_Upload()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.selectRadioButtonAsync("Yes");
                await _infrastructureUploadPage.ClickNextAsync();
                await Assertions.Expect(Page.GetByText("SSOT Data Upload")).ToBeVisibleAsync();
                Logger.Info("Proceeded to SSOT Upload ");
            }, nameof(TC_052_Proceed_SSOT_Data_Upload));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_053_Cancel_Upload_Operation")]
        [AllureTag("Smoke")]
        public async Task TC_053_Cancel_Upload_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.ClickCancelAsync();
                await Assertions.Expect(Page.GetByText("Project Dashboard")).ToBeVisibleAsync();
                Logger.Info("Cancelled Infrastructure Upload operation.");
            }, nameof(TC_053_Cancel_Upload_Operation));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_054_Upload_Infrastructure_Data")]
        [AllureTag("Smoke")]
        public async Task TC_054_Upload_Infrastructure_Data()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await Assertions.Expect(Page.GetByText("Overall Validation Status: Valid")).ToBeVisibleAsync();
                var elements = Page.Locator(".mud-expand-panel-header");
                await Assertions.Expect(elements).ToHaveCountAsync(8);
                Logger.Info("Infrastructure Data uploaded successfully and all files are valid.");
            }, nameof(TC_054_Upload_Infrastructure_Data));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_055_Verify_Infrastructure_Data")]
        [AllureTag("Smoke")]
        public async Task TC_055_Verify_Infrastructure_Data()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(3);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await Assertions.Expect(Page.GetByText("Data Preview")).ToBeVisibleAsync();
                Logger.Info("Infrastructure Data verified successfully.");
            }, nameof(TC_055_Verify_Infrastructure_Data));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_056_Validate_Upload_Infrastructure_Files")]
        [AllureTag("Smoke")]
        public async Task TC_056_Validate_Upload_Infrastructure_Files()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _basicSetup.WaitForPageAsync(10);
                await Assertions.Expect(Page.Locator("role=alert"))
                    .ToHaveTextAsync("Your data has been uploaded and merged successfully.");
                Logger.Info("Upload Infrastructure Data verified successfully.");
            }, nameof(TC_056_Validate_Upload_Infrastructure_Files));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_057_Validate_SSOT_Generation_Infrastructure_Files")]
        [AllureTag("Smoke")]
        public async Task TC_057_Validate_SSOT_Generation_Infrastructure_Files()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
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
                await Assertions.Expect(successAlerts.Nth(1))
                    .ToHaveTextAsync("SSOT generated successfully!");
                await Assertions.Expect(successAlerts.Nth(2))
                    .ToContainTextAsync("Data Quality Report generated successfully.");

                Logger.Info("SSOT Generation verified successfully.");
            }, nameof(TC_057_Validate_SSOT_Generation_Infrastructure_Files));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_058_Validate_SSOT_Preview_Infrastructure_Files")]
        [AllureTag("Smoke")]
        public async Task TC_058_Validate_SSOT_Preview_Infrastructure_Files()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
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

                var dialog = Page.GetByRole(AriaRole.Dialog);
                await Assertions.Expect(dialog.GetByText("SSOT Data Viewer")).ToBeVisibleAsync();
                await Assertions.Expect(dialog.Locator(".mud-table-container")).ToBeVisibleAsync();

                await _infrastructureUploadPage.ClosePreviewAsync();
                Logger.Info("SSOT Preview verified successfully.");
            }, nameof(TC_058_Validate_SSOT_Preview_Infrastructure_Files));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_059_Validate_Generate_Architecture_Operation")]
        [AllureTag("Smoke")]
        public async Task TC_059_Validate_Generate_Architecture_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
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

                await Assertions.Expect(Page.GetByRole(AriaRole.Alert))
                    .ToHaveTextAsync("Architecture generated successfully!.");
                Logger.Info("Architecture generation operation verified successfully.");
            }, nameof(TC_059_Validate_Generate_Architecture_Operation));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_060_Validate_View_Architecture_Operation")]
        [AllureTag("Smoke")]
        public async Task TC_060_Validate_View_Architecture_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _infrastructureUploadPage.GenerateSSOTOperationAsync();

                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Preview SSOT" }))
                    .ToBeVisibleAsync();

                await _infrastructureUploadPage.GenerateArchitectureAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ViewArchitectureAsync();

                await Assertions.Expect(Page)
                    .ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch"));

                Logger.Info("View Architecture operation verified successfully.");
            }, nameof(TC_060_Validate_View_Architecture_Operation));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_061_Upload_SSOT_Data")]
        [AllureTag("Smoke")]
        public async Task TC_061_Upload_SSOT_Data()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await Assertions.Expect(Page.GetByText("Overall Validation Status: Valid")).ToBeVisibleAsync();
                var elements = Page.Locator(".mud-expand-panel-header");
                await Assertions.Expect(elements).ToHaveCountAsync(7);
                Logger.Info("SSOT file uploaded successfully and all files are valid.");
            }, nameof(TC_061_Upload_SSOT_Data));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_062_Verify_SSOT_Data")]
        [AllureTag("Smoke")]
        public async Task TC_062_Verify_SSOT_Data()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(3);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _basicSetup.WaitForPageAsync(2);
                var dialog = Page.GetByRole(AriaRole.Dialog);
                await Assertions.Expect(dialog.GetByText("Data Preview")).ToBeVisibleAsync();
                Logger.Info("SSOT Data verified successfully.");
            }, nameof(TC_062_Verify_SSOT_Data));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_063_Validate_Upload_SSOT_File")]
        [AllureTag("Smoke")]
        public async Task TC_063_Validate_Upload_SSOT_File()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
               await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _basicSetup.WaitForPageAsync(10);
                await Assertions.Expect(Page.Locator("role=alert").Last)
                    .ToHaveTextAsync("Your data has been uploaded and merged successfully.");
                Logger.Info("Upload SSOT Data verified successfully.");
            }, nameof(TC_063_Validate_Upload_SSOT_File));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_064_Validate_SSOT_Architecture_Generation")]
        [AllureTag("Smoke")]
        public async Task TC_064_Validate_SSOT_Architecture_Generation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _infrastructureUploadPage.GenerateArchitectureAsync();
                await _basicSetup.WaitForPageAsync(3);
                await Assertions.Expect(Page.GetByRole(AriaRole.Alert).Last)
                    .ToHaveTextAsync("Architecture generated successfully!.");
                Logger.Info("Architecture generation using SSOT verified successfully.");
            }, nameof(TC_064_Validate_SSOT_Architecture_Generation));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_065_Validate_View_SSOT_Architecture_Operation")]
        [AllureTag("Smoke")]
        public async Task TC_065_Validate_View_SSOT_Architecture_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;;
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _infrastructureUploadPage.GenerateArchitectureAsync();
                await _basicSetup.WaitForPageAsync(5);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "View Architecture" }))
                    .ToBeVisibleAsync();
                await _infrastructureUploadPage.ViewArchitectureAsync();
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch"));
                Logger.Info("View Architecture operation using SSOT verified successfully.");
            }, nameof(TC_065_Validate_View_SSOT_Architecture_Operation));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_066_Validate_SSOT_Reconciliation_Operation")]
        [AllureTag("Smoke")]
        public async Task TC_066_Validate_SSOT_Reconciliation_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.ClickCreateNewProjectAsync();
                await _basicSetup.CreateProjectAsync();
                _createdProjectName = _basicSetup.projectName;
                await _infrastructureUploadPage.InfrastructureUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _infrastructureUploadPage.GenerateSSOTOperationAsync();
                await _basicSetup.WaitForPageAsync(2);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Preview SSOT" }))
                    .ToBeVisibleAsync();
                await _infrastructureUploadPage.ClickBackAsync();
                await _infrastructureUploadPage.SSOTUploadOperationAsync();
                await _basicSetup.WaitForPageAsync(5);
                await _infrastructureUploadPage.ClickVerifyButtonAsync();
                await _infrastructureUploadPage.ClosePreviewAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _infrastructureUploadPage.ClickUploadButtonAsync();
                await _basicSetup.WaitForPageAsync(2);

                var dialog = Page.GetByRole(AriaRole.Dialog);
                await Assertions.Expect(dialog.GetByText("Confirm SSOT Generation")).ToBeVisibleAsync();
                await _infrastructureUploadPage.ClickConfirmAsync();
                await Assertions.Expect(Page.GetByText("SSOT Data Reconciliation")).ToBeVisibleAsync();

                await _infrastructureUploadPage.HandleSSOTReconciliationSelectionAsync();
                await _basicSetup.WaitForPageAsync(5);
                await Assertions.Expect(Page.GetByRole(AriaRole.Dialog).Last.GetByText("Confirm SSOT Update"))
                    .ToBeVisibleAsync();
                await _infrastructureUploadPage.ClickConfirmAsync();
                await _basicSetup.WaitForPageAsync(5);

                var successAlerts = Page.GetByRole(AriaRole.Alert);
                await Assertions.Expect(successAlerts.Nth(1))
                    .ToHaveTextAsync("SSOT generated successfully!");
                await Assertions.Expect(successAlerts.Nth(2))
                    .ToContainTextAsync("Data Quality Report generated successfully.");

                await _infrastructureUploadPage.GenerateArchitectureAsync();
                await _basicSetup.WaitForPageAsync(2);
                await _infrastructureUploadPage.ViewArchitectureAsync();
                await Assertions.Expect(Page)
                    .ToHaveURLAsync(new System.Text.RegularExpressions.Regex("clientrarch"));
            }, nameof(TC_066_Validate_SSOT_Reconciliation_Operation));
        }

        // Regression TEST CASES 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_067_Select_Ramdon_User_In_Activity_log")]
        [AllureTag("Regression")]
        public async Task TC_067_Select_Ramdon_User_In_Activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await _infrastructureUploadPage.VerifyClientInfradata();
                await _infrastructureUploadPage.ViewActivityLog();
                await Assertions.Expect(Page.Locator(".mud-paper.mud-elevation-0").First).ToBeVisibleAsync();
                await _infrastructureUploadPage.ClickRandomUser();
                await _basicSetup.RandomSelector();
                Logger.Info("Selected random user succsessfully.");
            }, nameof(TC_067_Select_Ramdon_User_In_Activity_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_068_Select_Ramdon_Status_In_Activity_log")]
        [AllureTag("Regression")]
        public async Task TC_068_Select_Ramdon_Status_In_Activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await _infrastructureUploadPage.VerifyClientInfradata();
                await _infrastructureUploadPage.ViewActivityLog();
                await _infrastructureUploadPage.ClickRandomStatus();
                await _basicSetup.RandomSelector();
                Logger.Info("Selected random status (Pass/fail) successfully in recent activity");
            }, nameof(TC_068_Select_Ramdon_Status_In_Activity_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_069_Select_Date_Rang_In_Activity_log")]
        [AllureTag("Regression")]
        public async Task TC_069_Select_Date_Rang_In_Activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await _infrastructureUploadPage.VerifyClientInfradata();
                await _infrastructureUploadPage.ViewActivityLog();
                await _infrastructureUploadPage.ClickDateRange();
                await _basicSetup.SelectDaysRangeAsync();
                Logger.Info("Slected random date range (from past 7 days) succsessfully in recent activity");
            }, nameof(TC_069_Select_Date_Rang_In_Activity_log));
        }
        
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_070_Clear_All_Filter")]
        [AllureTag("Regression")]
        public async Task TC_070_Clear_All_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await _infrastructureUploadPage.VerifyClientInfradata();
                await _infrastructureUploadPage.ViewActivityLog();
                await _infrastructureUploadPage.ClickRandomUser();
                await _basicSetup.RandomSelector();
                await _infrastructureUploadPage.ClickRandomStatus();
                await _basicSetup.RandomSelector();
                await _infrastructureUploadPage.ClickDateRange();
                await _basicSetup.SelectDaysRangeAsync();
                await Page.GetByRole(AriaRole.Button, new() { Name = "Clear Filters" }).ClickAsync();
                Logger.Info("Successfully cleared all the filter applied in recent activity log");
            }, nameof(TC_070_Clear_All_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_071_Close_Recent_Activity_log")]
        [AllureTag("Regression")]
        public async Task TC_071_Close_Recent_Activity_log()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await _infrastructureUploadPage.VerifyClientInfradata();
                await _infrastructureUploadPage.ViewActivityLog();
                await Page.Locator(".d-flex.justify-space-between > .mud-button-root").ClickAsync();
                Logger.Info("Successfully cleared all the filter applied in recent activity log");
            }, nameof(TC_071_Close_Recent_Activity_log));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_072_Verify_file_type")]
        [AllureTag("Regression")]
        public async Task TC_072_Verify_file_type()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Upload Infrastructure Data" })).ToBeVisibleAsync();
                await _infrastructureUploadPage.HandleUploadedFileFlowAsync();
                Logger.Info("Verifed the file type (SSOT/CMDB)");
            }, nameof(TC_072_Verify_file_type));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_073_View_SSOT_file")]
        [AllureTag("Regression")]
        public async Task TC_073_View_SSOT_file()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Upload Infrastructure Data" })).ToBeVisibleAsync();
                await _infrastructureUploadPage.SSOT_Data_Viewer();
                await _infrastructureUploadPage.CloseDialog();
                Logger.Info("SSOT data verified");
            }, nameof(TC_073_View_SSOT_file));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_074_View_Data_Quality_Report")]
        [AllureTag("Regression")]
        public async Task TC_074_View_Data_Quality_Report()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _projectListPage.WaitForDashboardToBeReadyAsync();
                await _projectListPage.WaitForProjectDashboardIconsAsync();
                await _projectListPage.ClickCompletedProjectButton();
                await _projectListPage.ClickGatheringClientInfradata();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Upload Infrastructure Data" })).ToBeVisibleAsync();
                await _infrastructureUploadPage.Data_quality_report();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Data Quality Report" })).ToBeVisibleAsync();
                await _infrastructureUploadPage.CloseDataQualityReport();
                Logger.Info("Verified Data Qaulity report");
            }, nameof(TC_074_View_Data_Quality_Report));
        }
    }
}
