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
    public class FutureModeOpertionPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private ProjectListPage _projectList = null!;
        private FutureModeOperationPage _fmoPage = null!;
        private ClientModeOperationPage _cmoPage = null!;
         protected override bool UseClassLevelGrouping => true;

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _attachmentHelper=new AttachmentHelper(Context);
            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
             _cmoPage = new ClientModeOperationPage(Page);
            _fmoPage = new FutureModeOperationPage(Page);

            await NavigateToPageAsync("/fmoarch");
            _createdProjectName = _basicSetup.projectName;

            Logger.Info("Logged in as Super Admin and created a basic project");

        }

        #region Smoke Tests

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_FMO_Page_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_001_FMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                Logger.Info("FMO Page UI loaded successfully");
            }, nameof(TC_001_FMO_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("Smoke")]
        public async Task TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
                Logger.Info("FMO Page has clickable navigation buttons");
            }, nameof(TC_002_FMO_Page_Should_Have_Clickable_Navigation_Buttons));
        }

        #endregion

        #region Regression Test 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_Verify_Pan_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_003_CMO_Verify_Pan_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialX, initialY) = await _cmoPage.IntializeCoordinates();
                await _cmoPage.PerformPanAsync();
                var mousePosition = await Page.EvaluateAsync<dynamic>(@"() => 
                {
                    return {
                        x: window.mouseX || 0,  // fallback to 0 if not tracked
                        y: window.mouseY || 0
                    };
                }");
                Assert.NotEqual(initialX, (int)mousePosition.x);
                Assert.NotEqual(initialY, (int)mousePosition.y);
                Logger.Info("Pan functionality verified");
            }, nameof(TC_003_CMO_Verify_Pan_Functionality));
        }

        #region Zoom In

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Verify_ZoomIn_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_004_Verify_ZoomIn_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomInWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Zoom In via mouse scroll verified");
            }, nameof(TC_004_Verify_ZoomIn_MouseScroll));
        }
        
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_FMO_Zoom_In_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_005_FMO_Zoom_In_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(_fmoPage.Diagram).ToBeVisibleAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _fmoPage.ZoomInButton.ClickAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);

                Logger.Info("Zoom In button clicked successfully");
            }, nameof(TC_005_FMO_Zoom_In_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_Max_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_Max_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.ZoomInToMaximumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_006_Verify_Max_Zoom_Limit");
                Logger.Info("Maximum zoom in level verified");
            }, "TC_006_Verify_Max_Zoom_Limit");
        
        }

        #endregion

        #region Zoom Out

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Verify_ZoomOut_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_007_Verify_ZoomOut_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomOutWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight < initialHeight);
                Logger.Info("Zoom Out via mouse scroll verified");
            }, nameof (TC_007_Verify_ZoomOut_MouseScroll));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_FMO_Zoom_Out_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_008_FMO_Zoom_Out_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(_fmoPage.ZoomOutButton).ToBeEnabledAsync();
                await _fmoPage.ZoomInButton.ClickAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _fmoPage.ZoomOutButton.ClickAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight < initialHeight);

                Logger.Info("Zoom Out button clicked successfully");
            }, nameof(TC_008_FMO_Zoom_Out_Should_Work));
        }

        // [Fact]
        // [Trait("Category", "Regression")]
        // [AllureOwner("TC_009_Verify_Min_Zoom_Limit")]
        // [AllureTag("Regression")]
        // public async Task TC_009_Verify_Min_Zoom_Limit()
        // {
        //     await _attachmentHelper.RunWithTracingAsync(async ()=>
        //     {
        //         
        //         await _cmoPage.ZoomOutToMinimumAsync();
        //         var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
        //         await _cmoPage.PerformZoomOutAsync();
        //         var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
        //         Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
        //         Assert.Equal(finalHeight, initialHeight);
        //         Logger.Info("Minimum zoom out limit verified");
        //     }, nameof(TC_009_Verify_Min_Zoom_Limit));
            
        // }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Verify_Min_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_Min_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async ()=>
            {
                
                await _cmoPage.ZoomOutToMinimumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_009_Verify_Min_Zoom_Limit");
                Logger.Info("Minimum zoom out limit verified");
            }, "TC_009_Verify_Min_Zoom_Limit");
            
        }

        #endregion

        #region Reset


        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_FMO_Reset_Zoom_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_010_FMO_Reset_Zoom_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await Assertions.Expect(_fmoPage.ResetZoomButton).ToBeEnabledAsync();
                await _fmoPage.ResetZoomButton.ClickAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();

                Logger.Info("Reset Zoom button clicked successfully");
            }, nameof(TC_010_FMO_Reset_Zoom_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_011_Verify_ResetOnZoom_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                ;
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Random random = new Random();
                int zoomActionCount = random.Next(0,4);
                for (int i = 0; i < zoomActionCount; i++)
                {
                    await _cmoPage.PerformZoomInAsync();
                }
                zoomActionCount = random.Next(0,4);
                for (int i = 0; i < zoomActionCount; i++)
                {
                    await _cmoPage.PerformZoomInAsync();
                }
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                Logger.Info("Reset functionality verified");
            }, nameof(TC_011_Verify_ResetOnZoom_Functionality));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Verify_Reset_Does_Not_Impact_Scale")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_Reset_Does_Not_Impact_Scale()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.ClickResetAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("No error on multiple reset clicks");
            }, nameof(TC_012_Verify_Reset_Does_Not_Impact_Scale));
        }

        #endregion

        #region Fullscreen

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_FMO_Fullscreen_Should_Activate")]
        [AllureTag("Regression")]
        public async Task TC_013_FMO_Fullscreen_Should_Activate()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await Assertions.Expect(_fmoPage.FullScreenButton).ToBeEnabledAsync();
                await _fmoPage.FullScreenButton.ClickAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.True(finalWidth > initialWidth);
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Full screen activated");
            }, nameof(TC_013_FMO_Fullscreen_Should_Activate));
        }

         [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Verify_Exit_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_Exit_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _fmoPage.FullScreenButton.ClickAsync();
                await _basicSetup.WaitForPageAsync(1);
                await _cmoPage.ExitFullscreenModeAsync();      
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("Verified exit from fullscreen mode");
            },nameof(TC_014_Verify_Exit_Fullscreen));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Verify_UI_Scaling_In_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_UI_Scaling_In_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async()=>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _fmoPage.FullScreenButton.ClickAsync();
                await _basicSetup.WaitForPageAsync(1);
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();    
                Assert.True(finalWidth > initialWidth);
                Assert.True(finalHeight > initialHeight);
                Logger.Info("UI elements scale correctly in fullscreen");
            },nameof(TC_015_Verify_UI_Scaling_In_Fullscreen));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_FMO_Fullscreen_Should_Exit_On_ESC")]
        [AllureTag("Regression")]
        public async Task TC_016_FMO_Fullscreen_Should_Exit_On_ESC()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.FullScreenButton.ClickAsync();

                // Press ESC to exit fullscreen
                await Page.Keyboard.PressAsync("Escape");

                Logger.Info("Exited full screen using ESC key");
            }, nameof(TC_016_FMO_Fullscreen_Should_Exit_On_ESC));
        }

        
        #endregion

         #region Download 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Verify_Download_OnFullScreenMode_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_017_Verify_Download_OnFullScreenMode_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.EnterFullscreenModeAsync();
                var download = await _cmoPage.TriggerDownloadAsync();        
                Assert.NotNull(download);
                Assert.EndsWith(".svg", download.SuggestedFilename);
            }, nameof(TC_017_Verify_Download_OnFullScreenMode_Functionality));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Verify_Download_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_018_Verify_Download_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var download = await _cmoPage.TriggerDownloadAsync();           
                Assert.NotNull(download);
                Assert.EndsWith(".svg", download.SuggestedFilename);
            }, nameof(TC_018_Verify_Download_Functionality));
        }

        #endregion

        #region Legend 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_FMO_Show_Legend_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_019_FMO_Show_Legend_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ShowLegendButton.ClickAsync();

                await Assertions.Expect(_fmoPage.CloseLegendButton).ToBeVisibleAsync();

                Logger.Info("Legend opened successfully");
            }, nameof(TC_019_FMO_Show_Legend_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_FMO_Close_Legend_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_019_FMO_Close_Legend_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _fmoPage.ShowLegendButton.ClickAsync();
                await _fmoPage.CloseLegendButton.ClickAsync();

                Logger.Info("Legend closed successfully");
            }, nameof(TC_019_FMO_Close_Legend_Should_Work));
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_019_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_019_Verify_Legend_Display_OnFullScreenMode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _fmoPage.FullScreenButton.ClickAsync();
                await _fmoPage.ShowLegendButton.ClickAsync();
                await Assertions.Expect(Page.Locator("#fullscreen-container div").Filter(new() { HasText = "Legend" }).Nth(3)).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_019_Verify_Legend_Display");
                Logger.Info("Legend displayed successfully");
            },"TC_019_Verify_Legend_Display");
        }

        #endregion

        #region Search Function 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_FMO_Search_Box_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_020_FMO_Search_Box_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.SearchButton.ClickAsync();

                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                Logger.Info("Search textbox displayed");
            }, nameof(TC_020_FMO_Search_Box_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_FMO_Search_Should_Accept_Input")]
        [AllureTag("Regression")]
        public async Task TC_021_FMO_Search_Should_Accept_Input()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.SearchButton.ClickAsync();
                await _fmoPage.SearchTextbox.FillAsync("svr");

                await Assertions.Expect(_fmoPage.SearchTextbox).ToHaveValueAsync("svr");

                Logger.Info("Search input accepted successfully");
            }, nameof(TC_021_FMO_Search_Should_Accept_Input));
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_022_Verify_Search_NoResults")]
        [AllureTag("Regression")]
        public async Task TC_022_Verify_Search_NoResults()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST000"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).Not.ToBeVisibleAsync();
                // await Assertions.Expect(Page.GetByText("No results found")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_022_Verify_Search_NoResults");
                Logger.Info("Search with no results verified");
            },"TC_022_Verify_Search_NoResults");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_023_Verify_CloseSearch")]
        [AllureTag("Regression")]
        public async Task TC_023_Verify_CloseSearch()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST0"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await _cmoPage.CloseSearchBarAsync();
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_023_Verify_CloseSearch");
            }, "TC_023_Verify_CloseSearch");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_024_Verify_Search_EmptyInput")]
        [AllureTag("Regression")]
        public async Task TC_024_Verify_Search_EmptyInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.PerformSearchAsync("");
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                // await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Please enter a search term.");
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_024_Verify_Search_EmptyInput");
                Logger.Info("Search with empty input verified");
            }, "TC_024_Verify_Search_EmptyInput");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_FMO_Search_And_Select_Random_Server_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_025_FMO_Search_And_Select_Random_Server_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.SearchButton.ClickAsync();

                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                await _fmoPage.SearchTextbox.FillAsync("SRV");
                
                // await Page.Locator("#fullscreen-container div").Filter(new() { HasText = "Foun" }).Nth(3).ClickAsync();
                var container = Page.Locator("#fullscreen-container div")
                .Filter(new() { HasText = "Found" })
                .Nth(3);

            await Assertions.Expect(container).ToBeVisibleAsync();
            await _fmoPage.RandomSelector();

                Logger.Info("Search filter applied and random server selected");
            }, nameof(TC_025_FMO_Search_And_Select_Random_Server_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_026_FMO_Search_Invalid_Value_Should_Show_No_Results")]
        [AllureTag("Regression")]
        public async Task TC_026_FMO_Search_Invalid_Value_Should_Show_No_Results()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _fmoPage.SearchButton.ClickAsync();
                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                string invalidSearch = "uisiy43265125748ugfbhjfdhjvbj";
                await _fmoPage.SearchTextbox.FillAsync(invalidSearch);

                var options = Page.Locator(".mud-list-item:not(.mud-disabled)");
                await Page.WaitForTimeoutAsync(1000);
                int count = await options.CountAsync();

                Assert.True(count == 0, 
                    $"Expected no search results, but found {count} results.");

                Logger.Info("Invalid search correctly returned no results");
            }, nameof(TC_026_FMO_Search_Invalid_Value_Should_Show_No_Results));
        }

        #endregion

        

        #region Export to Excel 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_027_FMO_Export_To_Excel_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_027_FMO_Export_To_Excel_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await Assertions.Expect(_fmoPage.ExportToExcelButton).ToBeEnabledAsync();
                await _fmoPage.ExportToExcelButton.ClickAsync();

                Logger.Info("Export to Excel button clicked");
            }, nameof(TC_027_FMO_Export_To_Excel_Should_Work));
        }

        #endregion

        #region Sync Data

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_028_FMO_Sync_Data_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_028_FMO_Sync_Data_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await Assertions.Expect(_fmoPage.SyncDataButton).ToBeEnabledAsync();
                await _fmoPage.SyncDataButton.ClickAsync();

                Logger.Info("Sync data button clicked");
            }, nameof(TC_028_FMO_Sync_Data_Should_Work));
        }

        #endregion

        #region Time Period 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_029_FMO_Select_Time_Period_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_029_FMO_Select_Time_Period_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                await _fmoPage.ChooseTimePeriod.ClickAsync();

                // selecting random dropdown option
                await _basicSetup.RandomSelector();

                Logger.Info("Time Period filter selected successfully");
            }, nameof(TC_029_FMO_Select_Time_Period_Should_Work));
        } 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_030_FMO_View_Migration_Cost_Should_Open_Modal")]
        [AllureTag("Regression")]
        public async Task TC_030_FMO_View_Migration_Cost_Should_Open_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                Logger.Info("Migration cost modal opened");
            }, nameof(TC_030_FMO_View_Migration_Cost_Should_Open_Modal));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_031_FMO_One_Year_Reserved_Should_Work")]
        [AllureTag("sRegression")]
        public async Task TC_031_FMO_One_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.OneYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.OneYearReserved).ToBeVisibleAsync();

                Logger.Info("1 Year Reserved selected");
            }, nameof(TC_031_FMO_One_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("  ")]
        [AllureTag("Regression")]
        public async Task TC_032_FMO_Three_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.ThreeYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.ThreeYearReserved).ToBeVisibleAsync();

                Logger.Info("3 Year Reserved selected");
            }, nameof(TC_032_FMO_Three_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_033_FMO_Close_Migration_Cost_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_033_FMO_Close_Migration_Cost_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.CloseMigrationCostButton.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).Not.ToBeVisibleAsync();

                Logger.Info("Migration cost modal closed successfully");
            }, nameof(TC_033_FMO_Close_Migration_Cost_Should_Work));
        } 
        
        #endregion

        

        // private async Task SelectTimePeriodAsync(string period)
        // {
        //     await _fmoPage.TimePeriodDropdown.ClickAsync();

        //     switch (period)
        //     {
        //         case "Hour":
        //             await _fmoPage.PerHour.ClickAsync();
        //             break;

        //         case "Day":
        //             await _fmoPage.PerDay.ClickAsync();
        //             break;

        //         case "Month":
        //             await _fmoPage.PerMonth.ClickAsync();
        //             break;

        //         case "Year":
        //             await _fmoPage.PerYear.ClickAsync();
        //             break;
        //     }
        // }

        // private async Task VerifyCostCalculationAsync(ILocator totalCostLocator, ILocator columnLocator)
        // {
        //     string totalText = await totalCostLocator.InnerTextAsync();

        //     decimal uiTotal = decimal.Parse(
        //         totalText.Replace("$", "").Replace(",", "")
        //     );

        //     var values = await columnLocator.AllInnerTextsAsync();

        //     decimal calculatedTotal = values
        //         .Select(x => decimal.Parse(x.Replace("$", "").Replace(",", "")))
        //         .Sum();

        //     Logger.Info($"UI Total: {uiTotal}");
        //     Logger.Info($"Calculated Total: {calculatedTotal}");

        //     Assert.True(Math.Abs(uiTotal - calculatedTotal) < 1,
        //         "Cost mismatch between UI and row sum");
        // }

        #endregion

        #region Migration Cost 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_034_FMO_OnDemand_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_034_FMO_OnDemand_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_034_FMO_OnDemand_PerHour_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_035_FMO_OnDemand_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_035_FMO_OnDemand_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_035_FMO_OnDemand_PerDay_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_036_FMO_OnDemand_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_036_FMO_OnDemand_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_036_FMO_OnDemand_PerMonth_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_037_FMO_OnDemand_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_037_FMO_OnDemand_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_037_FMO_OnDemand_PerYear_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_038_FMO_OneYear_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_038_FMO_OneYear_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_038_FMO_OneYear_PerHour_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_039_FMO_OneYear_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_039_FMO_OneYear_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_039_FMO_OneYear_PerDay_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_040_FMO_OneYear_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_040_FMO_OneYear_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_040_FMO_OneYear_PerMonth_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_041_FMO_OneYear_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_041_FMO_OneYear_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_041_FMO_OneYear_PerYear_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_042_FMO_ThreeYear_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_042_FMO_ThreeYear_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_042_FMO_ThreeYear_PerHour_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_043_FMO_ThreeYear_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_043_FMO_ThreeYear_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_043_FMO_ThreeYear_PerDay_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_044_FMO_ThreeYear_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_044_FMO_ThreeYear_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_044_FMO_ThreeYear_PerMonth_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_045_FMO_ThreeYear_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_045_FMO_ThreeYear_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_045_FMO_ThreeYear_PerYear_CostValidation));
        }  
    }
}
         #endregion
