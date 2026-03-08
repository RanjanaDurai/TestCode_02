
using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;
using WiseMigrateTests.Pages.MigrationPlanTracking;

namespace WiseMigrateTests.Pages.ProjectInventory
{
    public class MigrationControlCentrePage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;

        private readonly MigrationPlanTrackingPage _migrationPlanTrackingPage=null!;
        private ILocator HomeBreadcrumbLink=> _page.Locator ("a[href='/projects']:has-text('Home')");

        private ILocator ProjectCreationBreadcrumbLink=> _page.Locator ("a[href='/project-creation*']");       

        private ILocator SSOTBreadcrumbLink=> _page.Locator ("a[href='/data-gathering']:has-text('SSOT Integration')");

        private ILocator CMOBreadcrumbLink=> _page.Locator ("a[href='/clientrarch']:has-text('CMO Architecure')");
        private ILocator FMOBreadcrumbLink=> _page.Locator ("a[href='/fmoarch']:has-text('FMO Architecture')");
        private ILocator InventoryBreadcrumbLink=> _page.Locator ("a[href='/inventory']:has-text('CMO/FMO Inventory')");
        private ILocator MigrationPlanTrackingBreadcrumbLink=> _page.Locator ("a[href='/track-plan']:has-text('Migration Plan Tracking')");
        private ILocator MigrationControlCentreBreadcrumbLink=> _page.GetByRole(AriaRole.Link, new() { Name = "Migration Control Centre"}); 
        private ILocator SearchInput => _page.Locator("input[placeholder='Search...']");
        private ILocator FilterStatusDropdown => _page.Locator("label:has-text('Filter Status')");
        private ILocator FilterOption(string status) => _page.Locator($".mud-list-item-text:has-text('{status}')");
        private ILocator EditButton=> _page.Locator("div.mud-card-header-content role='group' button").Nth(0);

        public int WaveCount => _page.Locator("mud-table-row").CountAsync().Result; 
        public int GroupCount => _page.Locator("tr.mud-table-row").CountAsync().Result;
        private ILocator StatusDropdown(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') mud-select");

        private ILocator RescheduleCalendarIcon(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') button").Nth(1);
        private ILocator DatePicker => _page.GetByRole(AriaRole.Button, new() { Name = "Open Date Picker" });
        private ILocator RescheduleButton=> _page.Locator("button:has-text('Reschedule')");

        private ILocator PlannedStartDate(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}) td[data-label='Planned Start Date']");

        private ILocator ScheduleMigrationButton => _page.Locator("button:has-text('Schedule Migration'), button[aria-label*='Schedule']");

        private ILocator ProjectStartDateInput => 
            _page.Locator("input[name='projectStartDate'], input[placeholder*='Start Date'], [role='dialog'] input[type='text']");

        private ILocator ProjectStartDateCalendarIcon => 
            _page.Locator("[role='dialog'] button[aria-label*='calendar'], [role='dialog'] svg[data-icon*='calendar']");

        private ILocator AddManualHolidayAccordion => 
            _page.Locator("div[role='button']:has-text('Add Manual Holiday'), button:has-text('Add Manual Holiday')");

        private ILocator ManualHolidayDateInput => 
            _page.Locator("#manual-holiday-date input, input[name='holidayDate']");

        private ILocator ManualHolidayNameInput => 
            _page.Locator("input[name='holidayName'], input[placeholder*='e.g., Bank Holiday']");

        private ILocator ManualHolidayRegionDropdown => 
            _page.Locator("select[name='region'], div[role='combobox']:has-text('EMEA')");

        private ILocator ManualHolidayRegionOption(string region) => 
            _page.Locator($"div[role='option']:has-text('{region}')");

        private ILocator AddHolidayButton => 
            _page.Locator("button:has-text('Add'), button[type='submit']:has-text('Add')");

        private ILocator HolidayTableRow => 
            _page.Locator("table tbody tr");

        private ILocator ChooseFileButton => 
            _page.Locator("button:has-text('Choose File'), input[type='file']");

        private ILocator FormatHelpIcon => 
            _page.Locator("button[aria-label*='help'], i[title*='Format'], svg[data-icon*='info']");

        private ILocator FormatHelpModal => 
            _page.Locator("div[role='dialog']:has-text('CSV FORMAT'), div[role='dialog']:has-text('JSON FORMAT')");

        private ILocator FormatHelpCloseButton => 
            _page.Locator("button:has-text('CLOSE'), [aria-label='Close']");

        private ILocator CancelSchedulerButton => _page.Locator("button:has-text('Cancel')");

        private ILocator GenerateScheduleButton => 
            _page.Locator("button:has-text('GENERATE TIMELINE SCHEDULE'), button:has-text('GENERATE')");

        private ILocator ConfigureParametersSection => _page.Locator("section:has-text('Parameters'), div:has-text('Configure Parameters')");

        private ILocator ParameterInput(string paramName) => 
            _page.Locator($"input[name*='{paramName}'], input[aria-label*='{paramName}'], label:has-text('{paramName}') + input");

        private ILocator ExportButton => 
            _page.Locator("button:has-text('Export'), button[aria-label*='Export'], svg[data-icon*='download']");

        private ILocator StatisticsPanel => 
            _page.Locator("div.statistics, section:has-text('Statistics'), div:has-text('servers'), div:has-text('waves')");

        private ILocator TimelineTab => _page.GetByText("Timeline", new() { Exact = true });

        private ILocator WaveExpandIcon(string waveName) => 
            _page.Locator($"div:has-text('{waveName}') button[aria-label*='expand'], svg[data-icon*='expand']");

        private ILocator ManageTab => _page.GetByText("Manage", new() { Exact = true });

        private ILocator GroupRow(string groupName) => 
            _page.Locator($"tr:has-text('{groupName}'), div:has-text('{groupName}')");

        private ILocator MoveToWaveDropdown(string groupName) => 
            _page.Locator($"tr:has-text('{groupName}') select, tr:has-text('{groupName}') [role='combobox']");

        private ILocator RollbackButton(string groupName) => 
            _page.Locator($"tr:has-text('{groupName}') button:has-text('Rollback'), tr:has-text('{groupName}') [aria-label*='rollback']");

        private ILocator BacklogCheckbox(string groupName) => 
            _page.Locator($"tr:has-text('{groupName}') input[type='checkbox'], tr:has-text('{groupName}') [role='checkbox']");

        public MigrationControlCentrePage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _migrationPlanTrackingPage = new MigrationPlanTrackingPage(page);
        }
        
        public async Task NavigateToMigrationControlCentreAsync()
        {
            await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
            await _migrationPlanTrackingPage.ClickViewPlanDetailsAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task OpenDetailedPlanningTabAsync()
        {
            await _page.GetByText("Detailed Planning", new() { Exact = true }).ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Planning"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Planning verified");
        }

        public async Task OpenTimelineTabAsync()
        {
            Logger.Info("Opening Timeline");

            await _page.GetByText("Timeline", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Execution Schedule"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Wave Execution Schedule verified");
        }

        public async Task OpenManageTabAsync()
        {
            Logger.Info("Opening Manage");

            await _page.GetByText("Manage", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Wave Plan Management"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Wave Plan Management verified");
        }

        public async Task OpenPoliciesAndRiskTabAsync()
        {
            Logger.Info("Opening Policies & Risk");

            await _page.GetByText("Policies & Risk", new() { Exact = true })
                .ClickAsync();

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new()
                {
                    Name = "Migration Policies"
                })
            ).ToBeVisibleAsync();

            Logger.Info("Migration Policies verified");
        }

        public async Task ClickBreadcrumbAsync(string sectionName)
        {
            ILocator breadcrumbLink = sectionName switch
            {
                "Home" => HomeBreadcrumbLink,
                "Project Creation" => ProjectCreationBreadcrumbLink,
                "SSOT Integration" => SSOTBreadcrumbLink,
                "CMO Architecture" => CMOBreadcrumbLink,
                "FMO Architecture" => FMOBreadcrumbLink,
                "CMO/FMO Inventory" => InventoryBreadcrumbLink,
                "Migration Plan Tracking" => MigrationPlanTrackingBreadcrumbLink,
                "Migration Control Centre" => MigrationControlCentreBreadcrumbLink,
                _ => throw new ArgumentException($"Invalid section name: {sectionName}")
            };

            await breadcrumbLink.ClickAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task PerformSearchAsync(string searchTerm)
        {
            await SearchInput.ClearAsync();
            await SearchInput.FillAsync(searchTerm);
            await SearchInput.PressAsync("Enter");
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task SelectFilterStatusAsync(string status)
        {
            await FilterStatusDropdown.ClickAsync();
            await FilterOption(status).ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task ClickEditDetailsAsync()
        {
            await EditButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task ChangeStatusAsync(string waveOrGroupName)
        {
            await StatusDropdown(waveOrGroupName).ClickAsync();
            string statusOption = new[] { "Pending", "In Progress", "Completed", "Failed", "Planned" }[Random.Shared.Next(5)];
            await FilterOption(statusOption).ClickAsync(); 
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task OpenRescheduleCalendarAsync(string waveOrGroupName)
        {
            await RescheduleCalendarIcon(waveOrGroupName).ClickAsync();
            await Task.Delay(100);
        }

        public async Task<string> RescheduleOperationAsync()
        {
            await DatePicker.ClickAsync();
            string selectedDate = await _basicSetup.RandomDateSelectorAsync();
            await RescheduleButton.ClickAsync();
            return selectedDate;
        }

        public async Task<string> RetreivePlannedStartDateAsync(string waveOrGroupName)
        {
            var textContents = await PlannedStartDate(waveOrGroupName).AllTextContentsAsync();
            return textContents.FirstOrDefault() ?? string.Empty;             
        }

        public async Task OpenScheduleMigrationModalAsync()
        {
            await ScheduleMigrationButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task SetProjectStartDateAsync(string date) // format: YYYY-MM-DD
        {
            await ProjectStartDateInput.FillAsync(date);
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task AddManualHolidayAsync(string date, string name, string region)
        {
            await AddManualHolidayAccordion.ClickAsync();
            await ManualHolidayDateInput.FillAsync(date);
            await ManualHolidayNameInput.FillAsync(name);
            await ManualHolidayRegionDropdown.ClickAsync();
            await ManualHolidayRegionOption(region).ClickAsync();
            await AddHolidayButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task UploadHolidayFileAsync(string filePath)
        {
            await ChooseFileButton.SetInputFilesAsync(filePath);
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task OpenFormatHelpAsync()
        {
            await FormatHelpIcon.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task CloseFormatHelpAsync()
        {
            await FormatHelpCloseButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task CancelSchedulerAsync()
        {
            await CancelSchedulerButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task GenerateTimelineScheduleAsync()
        {
            await GenerateScheduleButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task<bool> IsHolidayInTableAsync(string date, string name)
        {
            var count = await HolidayTableRow.Filter(new() { HasText = date }).Filter(new() { HasText = name }).CountAsync();
            return count > 0;
        }

        public async Task SetParameterValueAsync(string paramName, string value)
        {
            await ParameterInput(paramName).ClearAsync();
            await ParameterInput(paramName).FillAsync(value);
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task ExportPlanAsync()
        {
            var downloadTask = _page.WaitForDownloadAsync();
            await ExportButton.ClickAsync();
            var download = await downloadTask;
            // You can add file path assertion in tests if needed
        }

        public async Task ExpandWaveInTimelineAsync(string waveName)
        {
            await WaveExpandIcon(waveName).ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task MoveGroupToWaveAsync(string groupName, string targetWave)
        {
            await MoveToWaveDropdown(groupName).ClickAsync();
            await _page.Locator($"div[role='option']:has-text('{targetWave}')").ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task RollbackGroupAsync(string groupName, string targetWave = "")
        {
            await RollbackButton(groupName).ClickAsync();
            if (!string.IsNullOrEmpty(targetWave))
            {
                await _page.Locator($"div[role='option']:has-text('{targetWave}')").ClickAsync();
            }
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task BacklogGroupAsync(string groupName)
        {
            await BacklogCheckbox(groupName).CheckAsync();
            await _basicSetup.WaitForPageAsync(1);
        }
    }
}