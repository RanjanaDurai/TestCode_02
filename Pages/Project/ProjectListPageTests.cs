// using Microsoft.Playwright;
using Microsoft.Playwright;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Pages.Login;
using Xunit;



namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPageTests : TestBaseFixture
    {

        // TC_PROJECT_001

        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_001")]
        public async Task TC_PROJECT_001_Project_Dashboard_Should_Load_After_Login()
        {
            var login = new LoginPage(Page);
            await login.ValidateMigrateAdminAccount(Page);

            var projectList = new ProjectListPage(Page);
            await projectList.WaitForDashboardAsync();

            await Assertions.Expect(Page.Locator("text=Project Dashboard")).ToBeVisibleAsync();
        }

        // TC_PROJECT_002

        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_002")]
        public async Task TC_PROJECT_002_Navigate_To_Create_Project_Page()
        {
            var login = new LoginPage(Page);
            await login.ValidateMigrateAdminAccount(Page);

            var projectList = new ProjectListPage(Page);
            await projectList.WaitForDashboardAsync();
            await projectList.ClickCreateNewProjectAsync();

            await Page.WaitForURLAsync("**/project-creation");
        }

        // TC_PROJECT_003

        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_003")]
        public async Task TC_PROJECT_003_Project_Creation_Page_Should_Load()
        {
            var login = new LoginPage(Page);
            await login.ValidateMigrateAdminAccount(Page);

            var projectList = new ProjectListPage(Page);
            await projectList.WaitForDashboardAsync();
            await projectList.ClickCreateNewProjectAsync();

            var projectPage = new ProjectCreationPage(Page);
            await projectPage.WaitForPageAsync();

            await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();
            
            // Migration configuration

            await Assertions.Expect(Page.GetByText("Type of Migration")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Source Infrastructure Type")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Target Infrastructure Type")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Target Cloud Provider")).ToBeVisibleAsync();
        }

        // TC_PROJECT_004 

        
        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_004")]
        public async Task TC_PROJECT_004_Create_Project_With_Valid_Data()
        {
            var login = new LoginPage(Page);
            await login.ValidateMigrateAdminAccount(Page);

            var projectList = new ProjectListPage(Page);
            await projectList.WaitForDashboardAsync();
            await projectList.ClickCreateNewProjectAsync();

            var projectPage = new ProjectCreationPage(Page);
            await projectPage.WaitForPageAsync();

            string projectName = projectPage.GenerateProjectName();

            await projectPage.FillProjectDetails(
                projectName,
                "test_customer",
                "automation project creation test"
            );

            await projectPage.ClickCreateAsync();
            await projectPage.WaitForCreationSuccessAsync();
        }
        [Fact]
[Trait("TestCaseID", "TC_PROJECT_005")]
[Trait("Category", "Smoke")]
public async Task TC_PROJECT_005_Verify_Created_Project_Is_Visible_On_Dashboard()
{
    var login = new LoginPage(Page);
    await login.ValidateMigrateAdminAccount(Page);

    var projectList = new ProjectListPage(Page);
    await projectList.WaitForDashboardAsync();
    await projectList.ClickCreateNewProjectAsync();

    var projectCreation = new ProjectCreationPage(Page);
    await projectCreation.WaitForPageAsync();

    // Generate unique project name
    string projectName = projectCreation.GenerateProjectName();

    await projectCreation.FillProjectDetails(
        projectName,
        "test_customer",
        "verify project appears on dashboard"
    );

    await projectCreation.ClickCreateAsync();

    // Wait for redirect to data-gathering
    await Page.WaitForURLAsync("**/data-gathering**");

    // Click Cancel → go back to dashboard
    await projectCreation.ClickCancelAndReturnToDashboardAsync();

    // Final assertion
    await Assertions.Expect(
        Page.Locator("tr", new() { HasText = projectName })
    ).ToBeVisibleAsync();
}

[Fact]
[Trait("TestCaseID", "TC_PROJECT_006")]
[Trait("Category", "Smoke")]
public async Task TC_PROJECT_006_Verify_Upload_Infrastructure_Page_Loads_After_Clicking_Next()
{
    var login = new LoginPage(Page);
    await login.ValidateMigrateAdminAccount(Page);

    var projectList = new ProjectListPage(Page);
    await projectList.WaitForDashboardAsync();
    await projectList.ClickCreateNewProjectAsync();

    var projectCreation = new ProjectCreationPage(Page);
    await projectCreation.WaitForPageAsync();

    // Create project
    string projectName = projectCreation.GenerateProjectName();

    await projectCreation.FillProjectDetails(
        projectName,
        "test_customer",
        "verify upload infrastructure page"
    );

    await projectCreation.ClickCreateAsync();

    // Wait until redirected to data gathering page
    await Page.WaitForURLAsync("**/data-gathering**");

    // Click NEXT
    await projectCreation.ClickNextOnDataGatheringAsync();

    // ✅ FINAL ASSERTION — this ends the test successfully
    await Assertions.Expect(
        Page.GetByText("Upload Infrastructure Data")
    ).ToBeVisibleAsync();
}
















    }
}
