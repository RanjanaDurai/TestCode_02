using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectCreationPage
    {
        private readonly IPage _page;

        public ProjectCreationPage(IPage page)
        {
            _page = page;
        }

        private ILocator ProjectName => _page.Locator("input").Nth(0);
        private ILocator CustomerName => _page.Locator("input").Nth(1);
        private ILocator Description => _page.Locator("textarea");

        private ILocator CreateButton =>
            _page.Locator("button", new() { HasText = "Create Project" });

        public async Task WaitForPageAsync()
        {
            await _page.WaitForSelectorAsync("text=Project Initialization");
        }

        public string GenerateProjectName()
        {
            return $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        public async Task FillProjectDetails(string projectName, string customer, string description)
        {
            await ProjectName.FillAsync(projectName);
            await CustomerName.FillAsync(customer);
            await Description.FillAsync(description);

            // MudBlazor stabilization
            await _page.WaitForTimeoutAsync(800);
        }

        public async Task ClickCreateAsync()
        {
            await CreateButton.ScrollIntoViewIfNeededAsync();
            await CreateButton.ClickAsync(new() { Force = true });
        }

        public async Task WaitForCreationSuccessAsync()
        {
            await _page.WaitForFunctionAsync(
                "() => window.location.href.includes('data-gathering')",
                null,
                new() { Timeout = 60000 }
            );
        }
        public async Task ClickCancelAndReturnToDashboardAsync()
{
    // Cancel button on data-gathering page
    var cancelButton = _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

    await cancelButton.WaitForAsync();
    await cancelButton.ClickAsync();

    // Wait until dashboard is loaded
    await _page.WaitForURLAsync("**/projects");
}

public async Task ClickNextOnDataGatheringAsync()
{
    var nextButton = _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

    await nextButton.WaitForAsync();
    await nextButton.ClickAsync();
}


public async Task WaitForUploadInfrastructurePageAsync()
{
    // Wait until Upload Infrastructure page header appears
    await _page.WaitForSelectorAsync("text=Upload Infrastructure Data");

    // Extra safety: ensure we are on data-gathering route
    await _page.WaitForURLAsync("**/data-gathering**");
}


    }
}
