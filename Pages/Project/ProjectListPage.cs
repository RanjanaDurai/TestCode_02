using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.TestHooks;
using System.Threading.Tasks;
using WiseMigrateTests.Pages.PreRequisites;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPage
    {
        private readonly IPage _page;

        public ProjectListPage(IPage page)
        {
            _page = page;
        }

        public ILocator CreateNewProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CREATE NEW PROJECT" });

        public async Task WaitForDashboardToBeReadyAsync()
        {

            await _page.WaitForURLAsync("**/projects");

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new() { Name = "Project Dashboard" })
            ).ToBeVisibleAsync();

        }

        public async Task ClickCreateNewProjectAsync()
        {
            await CreateNewProjectButton.WaitForAsync();
            await CreateNewProjectButton.ClickAsync();
        }

        public async Task AssertProjectVisibleAsync(string projectName)
        {
            await _page.Locator("tr", new() { HasText = projectName }).WaitForAsync();
        }

        private async Task<ILocator> GetMigrationStatusCellWithIconsAsync(int iconCount = 7)
        {
            Logger.Info($"Searching for project row with {iconCount} migration icons");

            var migrationCells = _page.Locator("td[data-label='Migration Status']");
            int cellCount = await migrationCells.CountAsync();

            for (int i = 0; i < cellCount; i++)
            {
                var cell = migrationCells.Nth(i);
                int currentIconCount = await cell.Locator(".mud-icon-root").CountAsync();

                if (currentIconCount == iconCount)
                {
                    Logger.Info($"Matching project row found at index {i + 1}");
                    return cell;
                }
            }

            throw new Exception($"No project row found with {iconCount} icons");
        }

        public async Task WaitForProjectDashboardIconsAsync(int iconCount = 7)
        {
            Logger.Info("Waiting for project dashboard icons to load");

            await _page.WaitForFunctionAsync(
                @"(expectedIcons) => {
                    const cells = document.querySelectorAll('td[data-label=""Migration Status""]');
                    return Array.from(cells).some(cell =>
                        cell.querySelectorAll('.mud-icon-root').length === expectedIcons
                    );
                }",
                iconCount,
                new PageWaitForFunctionOptions
                {
                    Timeout = 30000
                }
            );

            Logger.Info("Project dashboard icons loaded successfully");
        }
        public async Task ClickProjectCreationStatus()
        {
            await _page.Locator(".mud-icon-root.mud-svg-icon.mud-success-text > path:nth-child(2)").First.ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Project Initialization" }).ClickAsync();
        }
        public async Task ClickGatheringClientInfradata()
        {
            // await _page.Locator(".mud-icon-root.mud-icon-default.mud-svg-icon.mud-icon-size-small").First.ClickAsync();
            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(1);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // await _page.GetByRole(AriaRole.Heading, new() { Name = "Infrastructure Data Upload" }).ClickAsync();
        }
        public async Task ClickCurrentModeAsync()
        {
            Logger.Info("Opening Current Mode of Operation");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(2);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new() { Name = "Current Mode of Operation - Architectural View" })
            ).ToBeVisibleAsync();
        }

        public async Task ClickFutureModeAsync()
        {
            Logger.Info("Opening Future Mode of Operation");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(3);

            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(_page.GetByText("Future Mode of Operation - Architectural View"))
                .ToBeVisibleAsync();
        }

        public async Task ClickInventorySpecificationsAsync()
        {
            Logger.Info("Opening Inventory Specifications");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(4);
            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(_page.GetByText("CMO/FMO Inventory")).ToBeVisibleAsync();

        }

        public async Task ClickTrackPlanAsync()
        {
            Logger.Info("Opening Track Plan");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(5);
            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(_page.GetByText("Track Plan")).ToBeVisibleAsync();

        }

        public async Task ClickWaveAndMoveGroupingPlanAsync()
        {
            Logger.Info("Opening Wave & Move Grouping Plan");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            var icon = cell.Locator(".mud-icon-root").Nth(6);
            await icon.ScrollIntoViewIfNeededAsync();
            await icon.ClickAsync(new()
            {
                Force = true,
                Position = new Position { X = 12, Y = 12 }
            });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(_page.GetByText("Migration Control Center")).ToBeVisibleAsync();

        }

        public async Task VerifyProjectActionsMenuAsync()
        {
            Logger.Info("Verifying Project Actions menu");

            var actionsButton = _page
                .Locator("td:last-child")
                .GetByRole(AriaRole.Button)
                .First;

            await actionsButton.ScrollIntoViewIfNeededAsync();
            await actionsButton.ClickAsync();
            var menu = _page.Locator("div.mud-popover-open");
            await menu.WaitForAsync();
            await Assertions.Expect(menu.GetByText("View Activity Logs")).ToBeVisibleAsync();
            await Assertions.Expect(menu.GetByText("Manage Collaborators")).ToBeVisibleAsync();
            await Assertions.Expect(menu.GetByText("Delete Project")).ToBeVisibleAsync();
            await _page.ClickAsync("body", new() { Position = new() { X = 10, Y = 10 } });
        }
        private ILocator ActionButton => _page.Locator("td:nth-child(8)").First;
        public async Task ClickActionButton()
        {
            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            await ActionButton.ClickAsync();
        }

        //Regression testing 
        private ILocator ViewActivityLogs=>_page.GetByRole(AriaRole.Textbox, new() { Name="View Activity Logs"});
        private ILocator TotalProjectsCard =>_page.Locator("text=Total Projects");
        private ILocator CompletedCard =>_page.Locator("text=Completed");
        private ILocator InProgressCard => _page.Locator("text=In Progress");
        private ILocator FirstProjectRow =>_page.Locator("tbody tr").First;
        private ILocator WaveMoveIcon =>_page.Locator("td[data-label='Migration Status'] .mud-icon-root").Nth(6);
        private ILocator VisibleTooltip =>_page.Locator(".mud-tooltip:visible, .mud-popover:visible");
        private ILocator Filter => _page.GetByRole(AriaRole.Textbox , new() {Name = " Filter by Type"}); 
        private ILocator  ActivityLog=> _page.GetByRole(AriaRole.Paragraph).Filter(new() { HasTextRegex = new ("^View Activity Logs$") });
        private ILocator ManageCollaborators => _page.GetByText("Manage Collaborators");
        private ILocator DeleteProject => _page.GetByText("Delete Project");
        private ILocator LogoutButton  => _page.GetByRole(AriaRole.Button, new() { Name = "Logout" });
        private ILocator ProfileButton => _page.GetByRole(AriaRole.Navigation).GetByRole(AriaRole.Button).Nth(1);

        public async Task Logout()
        {
            await ProfileButton.ClickAsync();
            var LogoutButton = _page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync;
            var ConfirmLogout = _page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
            var VerifyLogout = _page.GetByText("You have been successfully");
        }
        public async Task CancelLogout()
        {
            await ProfileButton.ClickAsync();
            await LogoutButton.ClickAsync();
            var CancelLogout = _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
        }
        public async Task CloseLogout()
        {
            await ProfileButton.ClickAsync();
            var ClickCloseDialog=_page.GetByRole(AriaRole.Button, new() { Name = "Close dialog" }).ClickAsync();
        }
        public async Task ManageUsersAndRoles()
        {
            await ProfileButton.ClickAsync();
            var ManageRole = _page.GetByRole(AriaRole.Button, new() { Name = "Manage Role" }).ClickAsync();
            var VerifyManageROle = _page.GetByRole(AriaRole.Button, new() { Name = "Close dialog" });
        }
        public async Task CreateUser()
        {
            await ProfileButton.ClickAsync();
            var ManageRole = _page.GetByRole(AriaRole.Button, new() { Name = "Manage Role" }).ClickAsync();
            var CreateUser = _page.GetByText("Create User").First.ClickAsync();
            var ClickCreateuser = _page.GetByRole(AriaRole.Button, new() { Name = "Create User" }).ClickAsync(); 
        }
        public async Task UpdateProfile()
        {
            await ProfileButton.ClickAsync();
            var ClickEdite = _page.Locator("#popovercontent-1242dfee-a7bf-43be-9518-8bbf1bcbbd6f").GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).ClickAsync();
            var clickUpdatePRofile = _page.GetByRole(AriaRole.Button, new() { Name = "Update Profile" }).ClickAsync();
   
        }
        public async Task ApplyFiletr()
        {
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Filter by Type" }).ClickAsync();
            await _page.GetByText("Rehost").First.ClickAsync();    
        }

        private static readonly Random _random = new();
        public async Task RandomSelector()
        {
            var options = _page.Locator(".mud-list-item:not(.mud-disabled)");
            await options.First.WaitForAsync();
            int count = await options.CountAsync();
            await options.Nth(_random.Next(count)).ClickAsync();
        }
        public async Task SelectRandomPageSizeAsync()
        {
            await _page.Locator(".mud-input.mud-input-text").ClickAsync();
            await RandomSelector();
        }
        public async Task OrganizationStatus()
        {
            var orgButton = _page.GetByRole(AriaRole.Navigation).GetByRole(AriaRole.Button).First;
            await Assertions.Expect(orgButton).ToBeVisibleAsync();
            await orgButton.ClickAsync();
            var selectOrg = _page.GetByRole(AriaRole.Heading, new() { Name = "Select an Organization" });
            await Assertions.Expect(selectOrg).ToBeVisibleAsync();
            var closeDialog = _page.GetByRole(AriaRole.Button, new() { Name = "Close dialog" });
            await closeDialog.ClickAsync();
        }

        public async Task ViewDataUploadedFile()
        {
            var ViewDataUploadButton = _page.Locator("td:nth-child(5)").First;
            await Assertions.Expect(ViewDataUploadButton).ToBeVisibleAsync();
            await ViewDataUploadButton.ClickAsync(); 
            var verifySSOT = _page.GetByText("SSOT", new() { Exact = true });
        }
        private async Task ExpectTooltipToContainAsync(string expectedText)
        {
            Logger.Info($"Waiting for tooltip containing: {expectedText}");

           var tooltipText = VisibleTooltip.GetByText(expectedText, new()
            {
                Exact = false
            });

            await tooltipText.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 7000
            });

            await Assertions.Expect(tooltipText).ToBeVisibleAsync();
        }
        public async Task ClickTotalProjectsFilter()
        {
            Logger.Info("Clicking Total Projects card");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            await CompletedCard.ClickAsync();

            await FirstProjectRow
                .Locator(".mud-icon-root")
                .Nth(6)
                .HoverAsync();

            await ExpectTooltipToContainAsync("Completed");
        }
        public async Task ClickCompletedProjectFilter()
        {
            Logger.Info("Clicking Completed card");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            await CompletedCard.WaitForAsync();
            await CompletedCard.ClickAsync();

            await WaveMoveIcon.First.HoverAsync();
            await ExpectTooltipToContainAsync("Completed");

        }
        public async Task ClickInProgressProjectFilter()
        {
            Logger.Info("Clicking In Progress card");

            var cell = await GetMigrationStatusCellWithIconsAsync(7);
            await InProgressCard.WaitForAsync();
            await InProgressCard.ClickAsync();
            await WaveMoveIcon.First.HoverAsync();
            await ExpectTooltipToContainAsync("Not started");
        }
         public async Task CilckActivityLogButton()
        {
            await ActivityLog.ClickAsync();
            await _page.GetByRole(AriaRole.Columnheader, new() { Name = "Details" }).ClickAsync();
        }
        public async Task ClickManageCollaborators()
        {
            await ManageCollaborators.ClickAsync();
            await Assertions.Expect(_page.GetByRole(AriaRole.Heading, new() {Name="Project Activity"})).ToBeVisibleAsync();
        }
        public async Task DeleteProjectButton()
        {
            await DeleteProject.ClickAsync();
        }
        public async Task ClickFilterByType()
        {
            await Filter.ClickAsync();
        }
        public async Task ClickSyncData()
        {
            await _page.GetByRole(AriaRole.Group).Filter(new() { HasText = "Project Dashboard Monitor and manage all migration projects Create New Project" }).GetByRole(AriaRole.Button).Nth(2).ClickAsync();
        }

        public async Task ClickViewActivityLogs()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "View Activity Logs" }).ClickAsync();
        }
        public async Task SelectRandomUser()
        {
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "User", Exact = true }).ClickAsync();
            await RandomSelector();
        }

        public async Task SelectRandomProject()
        {
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Project", Exact = true }).ClickAsync();
            await RandomSelector();
        }

        public async Task SelectRandomStatus()
        {
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Status" }).ClickAsync();
            await RandomSelector();   
        }

        public async Task SelectDateRange()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Open Date Range Picker" }).ClickAsync();
        }
        public async Task ClearAllFilter()
        {
            await _page.GetByRole(AriaRole.Group).Filter(new() { HasText = "Advanced Filters Filter and" }).GetByRole(AriaRole.Button).Nth(2).ClickAsync();
        }
        public async Task ClickSync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Sync" }).ClickAsync();
        }

        // private ILocator PreviousMonthButton =>_page.GetByRole(AriaRole.Button, new(){NameRegex = new Regex("^Previous month")});
        // private ILocator NextMonthButton =>_page.GetByRole(AriaRole.Button, new(){NameRegex = new Regex("^Next month")});


        // public async Task SelectLast7DaysRangeAsync()
        // {
        //     var today = DateTime.Today;
        //     var startDate = today.AddDays(-7);

        //     Logger.Info($"Selecting range: {startDate:dd MMM yyyy} - {today:dd MMM yyyy}");

        //     // Select start date
        //     await SelectDateAsync(startDate);

        //     // Select end date (today)
        //     await SelectDateAsync(today);
        // }

        // private async Task SelectDateAsync(DateTime date)
        // {
        //     string day = date.Day.ToString();

        //     // If month not visible, navigate
        //     while (!await IsMonthVisibleAsync(date))
        //     {
        //         if (date < DateTime.Today)
        //         {
        //             await PreviousMonthButton.ClickAsync();
        //         }
        //         else
        //         {
        //             await NextMonthButton.ClickAsync();
        //         }

        //         await _page.WaitForTimeoutAsync(300);
        //     }
        //     await _page.GetByRole(AriaRole.Button, new() { Name = day }).First.ClickAsync();
        // }

        // private async Task<bool> IsMonthVisibleAsync(DateTime date)
        // {
        //     string monthYear = date.ToString("MMMM yyyy");

        //     return await _page.GetByText(monthYear).IsVisibleAsync();
        // }

        
    }
}


