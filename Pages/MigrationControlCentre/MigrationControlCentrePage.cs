
using Microsoft.Playwright;
using WiseMigrateTests.Utils;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.FutureModeOperation;
using WiseMigrateTests.Pages.MigrationPlanTracking;
using System.Text.RegularExpressions;

namespace WiseMigrateTests.Pages.MigrationControlCentre
{
    public class MigrationControlCentrePage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup=null!;

        private readonly MigrationPlanTrackingPage _migrationPlanTrackingPage=null!;
        private ILocator HomeBreadcrumbLink=> _page.Locator ("a[href*='/projects']:has-text('Home')");
        public ILocator ExpandButton => _page.GetByRole(AriaRole.Listitem).Filter(new() { HasTextRegex = new Regex("^$") }).Locator("svg");
        private ILocator ProjectCreationBreadcrumbLink=> _page.Locator ("a[href*='/project-creation']");       
        private ILocator SSOTBreadcrumbLink=> _page.Locator ("a[href*='/data-gathering']:has-text('SSOT Integration')");

        private ILocator CMOBreadcrumbLink=> _page.Locator ("a[href*='/clientrarch']:has-text('CMO Architecture')");
        private ILocator FMOBreadcrumbLink=> _page.Locator("a[href*='/fmoarch']:has-text('FMO Architecture')");
        
        private ILocator InventoryBreadcrumbLink=> _page.Locator ("a[href*='/inventory']:has-text('CMO/FMO Inventory')");
        private ILocator MigrationPlanTrackingBreadcrumbLink=> _page.Locator ("a[href*='/track-plan']:has-text('Migration Plan Tracking')");
        private ILocator MigrationControlCentreBreadcrumbLink=> _page.GetByRole(AriaRole.Link, new() { Name = "Migration Control Centre"}); 
        private ILocator SearchInput => _page.Locator("input[placeholder='Search...']");
        public ILocator FilterStatusDropdown => _page.GetByRole(AriaRole.Textbox, new(){Name="Filter Status"});
        private ILocator FilterOption(string status) => _page.GetByText(status, new() { Exact = true }).First;
        private ILocator EditButton=> _page.Locator(".d-flex.flex-row > div:nth-child(1) > .mud-button-root");
        private ILocator ConfigureParametres => _page.Locator(".d-flex > div:nth-child(3) > .mud-button-root");
        public int WaveCount => _page.Locator("tr").CountAsync().Result; 
        public int GroupCount => _page.Locator("tr.mud-table-row").CountAsync().Result;
        private ILocator StatusDropdown(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') svg").Nth(1);
        private ILocator SaveIcon(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') button").Nth(0);
        private ILocator RescheduleCalendarIcon(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') button").Nth(1);
        public ILocator DatePicker => _page.GetByRole(AriaRole.Button, new() { Name = "Open Date Picker" }).First;
        private ILocator RescheduleButton=> _page.Locator("button:has-text('Reschedule')");

        private ILocator PlannedStartDate(string waveOrGroupName) => _page.Locator($"tr:has-text('{waveOrGroupName}') td[data-label='Planned Start Date']");

        private ILocator ScheduleMigrationButton => _page.Locator(".d-flex.flex-row > div:nth-child(2) > .mud-button-root");
        public ILocator AddManualHolidayButton => _page.GetByText("Add Manual Holiday");
        private ILocator ManualHolidayNameInput => 
            _page.Locator("input[name='holidayName'], input[placeholder*='e.g., Bank Holiday']");

        public ILocator ManualHolidayRegionDropdown => _page.GetByText("EMEA");
        public ILocator ManualHolidayRegionOption(string region) => _page.GetByText(region).Last;
        public ILocator AddHolidayButton => _page.Locator("button:has-text('Add')");
        private ILocator FormatHelpIcon => _page.GetByRole(AriaRole.Button, new() {Name="Format Help"});

        public ILocator FormatHelpDialog => _page.GetByRole(AriaRole.Dialog).GetByRole(AriaRole.Heading, new() { Name = "Holiday File Format"});

        private ILocator FormatHelpCloseButton => _page.GetByRole(AriaRole.Button, new() {Name="Close dialog"}).Last;

        private ILocator CancelSchedulerButton => _page.GetByRole(AriaRole.Button, new() {Name="Cancel"});

        private ILocator GenerateScheduleButton => 
            _page.Locator("button:has-text('GENERATE TIMELINE SCHEDULE')");
        private ILocator ParameterInput(string paramName) => _page.GetByRole(AriaRole.Textbox, new() {Name=paramName});
        private ILocator GenerateButton => _page.GetByRole(AriaRole.Button, new() {Name="Generate Plan"});

        private ILocator ExportButton => _page.Locator(".d-flex > div:nth-child(4) > .mud-button-root");

        private ILocator StatisticsPanel => _page.Locator("div:nth-child(5) > .mud-button-root");

        public ILocator TimelineTab => _page.GetByText("Timeline", new() { Exact = true });

        private ILocator WaveExpandIcon(string waveName) => _page.GetByText(waveName);
        private ILocator ManageTab => _page.GetByText("Manage", new() { Exact = true });
        private ILocator MoveToWaveDropdown(string groupName) => _page.GetByText(groupName);

        private ILocator RollbackButton => _page.Locator("button:has-text('Rollback')");

        private ILocator BacklogCheckbox(string groupName) => _page.Locator("p.mud-typography-subtitle1").Filter(new() { HasText = groupName }).Locator("xpath=ancestor::div[@role='group'][1]//input[@type='checkbox']");        
        private ILocator MoveToBackLogButton => _page.GetByRole(AriaRole.Button, new(){Name="Move Groups to Backlog"});

        public MigrationControlCentrePage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _migrationPlanTrackingPage = new MigrationPlanTrackingPage(page);
        }
        
        public async Task NavigateToMigrationControlCentreAsync()
        {
            await _migrationPlanTrackingPage.NavigateToMigrationPlanTrackingAsync();
            await _migrationPlanTrackingPage.InitiateMigrationFromInventoryAsync();
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
            await ManageTab.ClickAsync();
            await Task.Delay(100);
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

        public async Task<string> ChangeStatusAsync(string waveOrGroupName)
        {
            await StatusDropdown(waveOrGroupName).ClickAsync();
            string statusOption = new[] { "Pending", "In Progress", "Completed", "Failed", "Planned" }[Random.Shared.Next(5)];
            await FilterOption(statusOption).ClickAsync(); 
            await _basicSetup.WaitForPageAsync(1);
            return statusOption;
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

        public async Task SaveEditsAsync(string waveOrGroupName)
        {
            await SaveIcon(waveOrGroupName).ClickAsync();
            await Task.Delay(100);

        }
        public async Task OpenScheduleMigrationModalAsync()
        {
            await ScheduleMigrationButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task<string> SetRandomProjectStartDateAsync(ILocator DatePicker = null) 
        {
            DatePicker ??= this.DatePicker;
            await DatePicker.ClickAsync();
            string selectedDate = await _basicSetup.RandomDateSelectorAsync();
            await _basicSetup.WaitForPageAsync(1);
            return selectedDate;
        }

        public async Task AddManualHolidayAsync(string holidayName, string region, ILocator datePicker=null)
        {
            await AddManualHolidayButton.ClickAsync();
            await SetRandomProjectStartDateAsync(datePicker);
            await ManualHolidayNameInput.FillAsync(holidayName);
            await ManualHolidayRegionDropdown.ClickAsync();
            await ManualHolidayRegionOption(region).ClickAsync();
            await AddHolidayButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }


        public async Task HoliDayFileUploadOperationAsync()
        {
            string holidayListFile = FileUploadHelper.GetRandomHolidayListFile();
            await _page.SetInputFilesAsync("input[id='holiday-file']", holidayListFile);
            await _basicSetup.WaitForPageAsync(1);
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

        public async Task<bool> IsHolidayInTableAsync(string name)
        {
            var count = await _page.Locator("tr").Filter(new() { HasText = name }).CountAsync();
            return count > 0;
        }

         public async Task OpenParameterConfiguration()
        {
            await ConfigureParametres.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task<string[]> SetParameterNames()
        {
            var Elements = await _page.QuerySelectorAllAsync("[role='textbox']");
            var paramName = new List<string>();
            foreach(var element in Elements)
            {
                var ariaLabel = await element.GetAttributeAsync("aria-label");
                if (!string.IsNullOrEmpty(ariaLabel))
                {
                    paramName.Add(ariaLabel);
                }
            }
            return paramName.ToArray();
        }
        public async Task SetParameterValueAsync()
        {
            string[] paramName=await SetParameterNames();
            var count= await _page.GetByRole(AriaRole.Textbox).CountAsync();
            foreach (var param in paramName)
            {
                var value = new Random().Next(1,10);
                if(string.Equals(param, "Cutover Window Duration")|| string.Equals(param, "Minimum Gap Between Move"))
                {
                    continue;
                }
                await ParameterInput(param).ClearAsync();
                await Task.Delay(100);
                await ParameterInput(param).FillAsync(value.ToString());
            }            
            await _basicSetup.WaitForPageAsync(1);
        }
        public async Task SetParameterValueAsync(string paramName, string value)
        {
            await ParameterInput(paramName).ClearAsync();
            await ParameterInput(paramName).FillAsync(value.ToString());
            await Task.Delay(100);
        }
        
        public async Task GenerateMigrationScheduleAsync()
        {
            await GenerateButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(2);
        }
        public async Task ExportPlanAsync()
        {
            var downloadTask = _page.WaitForDownloadAsync();
            await ExportButton.ClickAsync();
            var download = await downloadTask;
        }
        public async Task OpenStatisticsPanelAsync()
        {
            await StatisticsPanel.ClickAsync();
            await Task.Delay(100);
        }
        public async Task ExpandWaveInTimelineAsync(string waveName)
        {
            await WaveExpandIcon(waveName).ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task MoveGroupToWaveAsync(string groupName, string targetWave)
        {
            
            await MoveToWaveDropdown(groupName).ClickAsync();
            var groupTitle = _page.Locator("p.mud-typography-subtitle1").Filter(new() { HasText = groupName });
            var groupCard = groupTitle.Locator("xpath=ancestor::div[contains(@class,'mud-paper') " + "or contains(@class,'mud-card')][1]");
            await groupCard.Locator("input[placeholder='Select wave...']").ClickAsync();
            await _page.WaitForSelectorAsync(".mud-popover .mud-list-item");
            await _page.Locator(".mud-list-item").Filter(new() { HasText = targetWave}).ClickAsync();    
        }

        public async Task RollbackGroupToTargetWaveAsync(string targetWave)
        {
            await _page.GetByRole(AriaRole.Textbox,new() {Name="Move to..."}).ClickAsync();
            await _page.GetByText(targetWave, new() { Exact = true }).ClickAsync();
            //await _page.Locator($"div[role='option']:has-text('{targetWave}')").ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

         public async Task RollbackGroupAsync()
        {
            await RollbackButton.ClickAsync();
            await Task.Delay(100);
        }

        public string GetAncestorXPath(int levels)
        {
            return "xpath=" + string.Join("/", Enumerable.Repeat("..", levels));
        }

        public async Task BacklogGroupAsync(string groupName)
        {
            await BacklogCheckbox(groupName).CheckAsync();
            await MoveToBackLogButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }
    }
}