using Microsoft.Playwright;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Pages.Login;
using Xunit;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPageTests : TestBaseFixture
    {
        // ---------------------------------------------------
        // TC_PROJECT_001
        // ---------------------------------------------------
        [Fact]
        public async Task TC_PROJECT_001_Project_Dashboard_Should_Load_After_Login()
        {
            var loginPage = new LoginPage(Page);

            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            await Assertions.Expect(
                Page.Locator("text=Project Dashboard")
            ).ToBeVisibleAsync();
        }

        // ---------------------------------------------------
        // TC_PROJECT_002
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_002")]
        [Trait("Category", "Smoke")]
        public async Task TC_PROJECT_002_Create_New_Project_Button_Should_Navigate_To_Create_Page()
        {
            var loginPage = new LoginPage(Page);

            await loginPage.ValidateMigrateAdminAccount(Page);
            await Page.WaitForURLAsync("**/projects");

            var createProjectButton = Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            );

            await Assertions.Expect(createProjectButton).ToBeVisibleAsync();
            await createProjectButton.ClickAsync();

            await Page.WaitForURLAsync("**/project-creation");
        }

        // ---------------------------------------------------
        // TC_PROJECT_003 (UI fields visibility)
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_003")]
        [Trait("Category", "Smoke")]
        public async Task TC_PROJECT_003_Project_Creation_Page_Should_Load_With_All_Required_Fields()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            ).ClickAsync();

            await Page.WaitForURLAsync("**/project-creation");

            // Project details
            await Assertions.Expect(Page.GetByLabel("Project Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Customer Name")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByLabel("Description")).ToBeVisibleAsync();

            // Migration configuration
            await Assertions.Expect(Page.GetByText("Type of Migration")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Source Infrastructure Type")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Target Infrastructure Type")).ToBeVisibleAsync();
            await Assertions.Expect(Page.GetByText("Target Cloud Provider")).ToBeVisibleAsync();
        }

        // ---------------------------------------------------
        // TC_CUST_001
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_CUST_001")]
        [Trait("Category", "Smoke")]
        public async Task TC_CUST_001_Verify_Customer_Name_Field_Is_Visible()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            ).ClickAsync();

            await Page.WaitForURLAsync("**/project-creation");

            var customerNameField = Page.GetByRole(
                AriaRole.Textbox,
                new() { Name = "Customer Name" }
            );

            await Assertions.Expect(customerNameField).ToBeVisibleAsync();
            await Assertions.Expect(customerNameField).ToBeEnabledAsync();
        }

        // ---------------------------------------------------
        // TC_DESC_001
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_DESC_001")]
        [Trait("Category", "Smoke")]
        public async Task TC_DESC_001_Verify_Description_Field_Is_Visible_And_Editable()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            ).ClickAsync();

            await Page.WaitForURLAsync("**/project-creation");

            var descriptionField = Page.GetByRole(
                AriaRole.Textbox,
                new() { Name = "Description" }
            );

            await Assertions.Expect(descriptionField).ToBeVisibleAsync();
            await Assertions.Expect(descriptionField).ToBeEnabledAsync();

            await descriptionField.FillAsync("Automation test description");
            await Assertions.Expect(descriptionField)
                .ToHaveValueAsync("Automation test description");
        }

        // ---------------------------------------------------
        // TC_DESC_002
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_DESC_002")]
        [Trait("Category", "Smoke")]
        public async Task TC_DESC_002_Verify_Saving_Valid_Description_Text()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            var createButton = Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            );

            await createButton.ClickAsync();
            await Page.WaitForURLAsync("**/project-creation");

            string projectName = $"AutoProject_{DateTime.Now.Ticks}";
            string customerName = "Test Customer";
            string description = "Valid project description for automation test.";

            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Project Name" })
                .FillAsync(projectName);

            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Customer Name" })
                .FillAsync(customerName);

            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Description" })
                .FillAsync(description);

            await Assertions.Expect(createButton).ToBeEnabledAsync();

            await createButton.ClickAsync();

            await Page.WaitForURLAsync("**/projects**");

            await Assertions.Expect(Page.GetByText(description)).ToBeVisibleAsync();
        }

        // ---------------------------------------------------
        // TC_PROJECT_004 (Create Project – Working SPA version)
        // ---------------------------------------------------
        [Fact]
        [Trait("TestCaseID", "TC_PROJECT_004")]
        [Trait("Category", "Smoke")]
        public async Task TC_PROJECT_004_Create_Project_With_Valid_Data()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.ValidateMigrateAdminAccount(Page);

            await Page.WaitForURLAsync("**/projects");

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "CREATE NEW PROJECT" }
            ).ClickAsync();

            await Page.WaitForSelectorAsync("text=Project Initialization");

            await Page.Locator("input").Nth(0).FillAsync("Auto_test_1");
            await Page.Locator("input").Nth(1).FillAsync("test_case");
            await Page.Locator("textarea").FillAsync("test_case");

            await Page.WaitForTimeoutAsync(800);

            var createBtn = Page.Locator("button", new() { HasText = "Create Project" });

            await createBtn.ScrollIntoViewIfNeededAsync();
            await createBtn.ClickAsync(new() { Force = true });

            await Page.WaitForFunctionAsync(
                "() => window.location.href.includes('data-gathering')",
                null,
                new() { Timeout = 60000 }
            );
        }
    }
}
