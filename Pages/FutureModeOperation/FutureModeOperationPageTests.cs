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
        [AllureOwner("TC_103_FMO_Page_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_103_FMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                Logger.Info("FMO Page UI loaded successfully");
            }, nameof(TC_103_FMO_Page_UI_Should_Load));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_104_FMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("Smoke")]
        public async Task TC_104_FMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.GetByText("Future Mode of Operation - Architectural View")).ToBeVisibleAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
                Logger.Info("FMO Page has clickable navigation buttons");
            }, nameof(TC_104_FMO_Page_Should_Have_Clickable_Navigation_Buttons));
        }

        #endregion

        #region Regression Test 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_105_Verify_Pan_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_105_Verify_Pan_Functionality()
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
            }, nameof(TC_105_Verify_Pan_Functionality));
        }

        #region Zoom In

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_106_Verify_ZoomIn_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_106_Verify_ZoomIn_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomInWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Zoom In via mouse scroll verified");
            }, nameof(TC_106_Verify_ZoomIn_MouseScroll));
        }
        
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_107_FMO_Zoom_In_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_107_FMO_Zoom_In_Should_Work()
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
            }, nameof(TC_107_FMO_Zoom_In_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_108_Verify_Max_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_108_Verify_Max_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.ZoomInToMaximumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("Maximum zoom in level verified");
            }, "TC_108_Verify_Max_Zoom_Limit");
        
        }

        #endregion

        #region Zoom Out

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_109_Verify_ZoomOut_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_109_Verify_ZoomOut_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomOutWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight < initialHeight);
                Logger.Info("Zoom Out via mouse scroll verified");
            }, nameof (TC_109_Verify_ZoomOut_MouseScroll));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_110_FMO_Zoom_Out_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_110_FMO_Zoom_Out_Should_Work()
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
            }, nameof(TC_110_FMO_Zoom_Out_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_111_Verify_Min_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_111_Verify_Min_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async ()=>
            {
                
                await _cmoPage.ZoomOutToMinimumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("Minimum zoom out limit verified");
            }, "TC_111_Verify_Min_Zoom_Limit");
            
        }

        #endregion

        #region Reset

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_112_FMO_Reset_Zoom_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_112_FMO_Reset_Zoom_Should_Work()
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
            }, nameof(TC_112_FMO_Reset_Zoom_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_113_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_113_Verify_ResetOnZoom_Functionality()
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
            }, nameof(TC_113_Verify_ResetOnZoom_Functionality));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_114_Verify_Reset_Does_Not_Impact_Scale")]
        [AllureTag("Regression")]
        public async Task TC_114_Verify_Reset_Does_Not_Impact_Scale()
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
            }, nameof(TC_114_Verify_Reset_Does_Not_Impact_Scale));
        }

        #endregion

        #region Fullscreen

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_115_FMO_Fullscreen_Should_Activate")]
        [AllureTag("Regression")]
        public async Task TC_115_FMO_Fullscreen_Should_Activate()
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
            }, nameof(TC_115_FMO_Fullscreen_Should_Activate));
        }

         [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_116_Verify_Exit_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_116_Verify_Exit_Fullscreen()
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
            },nameof(TC_116_Verify_Exit_Fullscreen));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_117_Verify_UI_Scaling_In_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_117_Verify_UI_Scaling_In_Fullscreen()
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
            },nameof(TC_117_Verify_UI_Scaling_In_Fullscreen));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_118_FMO_Fullscreen_Should_Exit_On_ESC")]
        [AllureTag("Regression")]
        public async Task TC_118_FMO_Fullscreen_Should_Exit_On_ESC()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.FullScreenButton.ClickAsync();

                // Press ESC to exit fullscreen
                await Page.Keyboard.PressAsync("Escape");

                Logger.Info("Exited full screen using ESC key");
            }, nameof(TC_118_FMO_Fullscreen_Should_Exit_On_ESC));
        }

        
        #endregion

         #region Download 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_119_Verify_Download_OnFullScreenMode_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_119_Verify_Download_OnFullScreenMode_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.EnterFullscreenModeAsync();
                var download = await _cmoPage.TriggerDownloadAsync();        
                Assert.NotNull(download);
                Assert.EndsWith(".svg", download.SuggestedFilename);
            }, nameof(TC_119_Verify_Download_OnFullScreenMode_Functionality));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_120_Verify_Download_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_120_Verify_Download_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var download = await _cmoPage.TriggerDownloadAsync();           
                Assert.NotNull(download);
                Assert.EndsWith(".svg", download.SuggestedFilename);
            }, nameof(TC_120_Verify_Download_Functionality));
        }

        #endregion

        #region Legend 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_121_FMO_Show_Legend_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_121_FMO_Show_Legend_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ShowLegendButton.ClickAsync();

                await Assertions.Expect(_fmoPage.CloseLegendButton).ToBeVisibleAsync();

                Logger.Info("Legend opened successfully");
            }, nameof(TC_121_FMO_Show_Legend_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_122_FMO_Close_Legend_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_122_FMO_Close_Legend_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _fmoPage.ShowLegendButton.ClickAsync();
                await _fmoPage.CloseLegendButton.ClickAsync();

                Logger.Info("Legend closed successfully");
            }, nameof(TC_122_FMO_Close_Legend_Should_Work));
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_123_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_123_Verify_Legend_Display_OnFullScreenMode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _fmoPage.FullScreenButton.ClickAsync();
                await _fmoPage.ShowLegendButton.ClickAsync();
                await Assertions.Expect(Page.Locator("#fullscreen-container div").Filter(new() { HasText = "Legend" }).Nth(3)).ToBeVisibleAsync();
                Logger.Info("Legend displayed successfully");
            },"TC_123_Verify_Legend_Display");
        }

        #endregion

        #region Search Function 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_124_FMO_Search_Box_Should_Open")]
        [AllureTag("Regression")]
        public async Task TC_124_FMO_Search_Box_Should_Open()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.SearchButton.ClickAsync();

                await Assertions.Expect(_fmoPage.SearchTextbox).ToBeVisibleAsync();

                Logger.Info("Search textbox displayed");
            }, nameof(TC_124_FMO_Search_Box_Should_Open));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_125_FMO_Search_Should_Accept_Input")]
        [AllureTag("Regression")]
        public async Task TC_125_FMO_Search_Should_Accept_Input()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.SearchButton.ClickAsync();
                await _fmoPage.SearchTextbox.FillAsync("svr");

                await Assertions.Expect(_fmoPage.SearchTextbox).ToHaveValueAsync("svr");

                Logger.Info("Search input accepted successfully");
            }, nameof(TC_125_FMO_Search_Should_Accept_Input));
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_126_Verify_Search_NoResults")]
        [AllureTag("Regression")]
        public async Task TC_126_Verify_Search_NoResults()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST000"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).Not.ToBeVisibleAsync();
                // await Assertions.Expect(Page.GetByText("No results found")).ToBeVisibleAsync();
                Logger.Info("Search with no results verified");
            },"TC_126_Verify_Search_NoResults");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_127_Verify_CloseSearch")]
        [AllureTag("Regression")]
        public async Task TC_127_Verify_CloseSearch()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST0"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await _cmoPage.CloseSearchBarAsync();
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
            }, "TC_127_Verify_CloseSearch");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_128_Verify_Search_EmptyInput")]
        [AllureTag("Regression")]
        public async Task TC_128_Verify_Search_EmptyInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.PerformSearchAsync("");
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                // await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Please enter a search term.");
                Logger.Info("Search with empty input verified");
            }, "TC_128_Verify_Search_EmptyInput");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_129_FMO_Search_And_Select_Random_Server_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_129_FMO_Search_And_Select_Random_Server_Should_Work()
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
            }, nameof(TC_129_FMO_Search_And_Select_Random_Server_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_130_FMO_Search_Invalid_Value_Should_Show_No_Results")]
        [AllureTag("Regression")]
        public async Task TC_130_FMO_Search_Invalid_Value_Should_Show_No_Results()
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
            }, nameof(TC_130_FMO_Search_Invalid_Value_Should_Show_No_Results));
        }

        #endregion

        

        #region Export to Excel 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_131_FMO_Export_To_Excel_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_131_FMO_Export_To_Excel_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await Assertions.Expect(_fmoPage.ExportToExcelButton).ToBeEnabledAsync();
                await _fmoPage.ExportToExcelButton.ClickAsync();

                Logger.Info("Export to Excel button clicked");
            }, nameof(TC_131_FMO_Export_To_Excel_Should_Work));
        }

        #endregion

        #region Sync Data

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_132_FMO_Sync_Data_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_132_FMO_Sync_Data_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await Assertions.Expect(_fmoPage.SyncDataButton).ToBeEnabledAsync();
                await _fmoPage.SyncDataButton.ClickAsync();

                Logger.Info("Sync data button clicked");
            }, nameof(TC_132_FMO_Sync_Data_Should_Work));
        }

        #endregion

        #region Time Period 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_133_FMO_Select_Time_Period_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_133_FMO_Select_Time_Period_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                await _fmoPage.ChooseTimePeriod.ClickAsync();

                // selecting random dropdown option
                await _basicSetup.RandomSelector();

                Logger.Info("Time Period filter selected successfully");
            }, nameof(TC_133_FMO_Select_Time_Period_Should_Work));
        } 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_134_FMO_View_Migration_Cost_Should_Open_Modal")]
        [AllureTag("Regression")]
        public async Task TC_134_FMO_View_Migration_Cost_Should_Open_Modal()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).ToBeVisibleAsync();

                Logger.Info("Migration cost modal opened");
            }, nameof(TC_134_FMO_View_Migration_Cost_Should_Open_Modal));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_135_FMO_One_Year_Reserved_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_135_FMO_One_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.OneYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.OneYearReserved).ToBeVisibleAsync();

                Logger.Info("1 Year Reserved selected");
            }, nameof(TC_135_FMO_One_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_136_FMO_Three_Year_Reserved_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_136_FMO_Three_Year_Reserved_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.ThreeYearReserved.ClickAsync();

                await Assertions.Expect(_fmoPage.ThreeYearReserved).ToBeVisibleAsync();

                Logger.Info("3 Year Reserved selected");
            }, nameof(TC_136_FMO_Three_Year_Reserved_Should_Work));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_137_FMO_Close_Migration_Cost_Should_Work")]
        [AllureTag("Regression")]
        public async Task TC_137_FMO_Close_Migration_Cost_Should_Work()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();
                await _fmoPage.CloseMigrationCostButton.ClickAsync();

                await Assertions.Expect(_fmoPage.MigrationCostHeader).Not.ToBeVisibleAsync();

                Logger.Info("Migration cost modal closed successfully");
            }, nameof(TC_137_FMO_Close_Migration_Cost_Should_Work));
        } 
        
        #endregion

        #endregion

        #region Migration Cost 

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_138_FMO_OnDemand_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_138_FMO_OnDemand_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_138_FMO_OnDemand_PerHour_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_139_FMO_OnDemand_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_139_FMO_OnDemand_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_139_FMO_OnDemand_PerDay_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_140_FMO_OnDemand_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_140_FMO_OnDemand_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_140_FMO_OnDemand_PerMonth_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_141_FMO_OnDemand_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_141_FMO_OnDemand_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OnDemandTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OnDemandColumn);

            }, nameof(TC_141_FMO_OnDemand_PerYear_CostValidation));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_142_FMO_OneYear_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_142_FMO_OneYear_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_142_FMO_OneYear_PerHour_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_143_FMO_OneYear_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_143_FMO_OneYear_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_143_FMO_OneYear_PerDay_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_144_FMO_OneYear_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_144_FMO_OneYear_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_144_FMO_OneYear_PerMonth_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_145_FMO_OneYear_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_145_FMO_OneYear_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.OneYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.OneYearReservedColumn);

            }, nameof(TC_145_FMO_OneYear_PerYear_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_146_FMO_ThreeYear_PerHour_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_146_FMO_ThreeYear_PerHour_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Hour");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_146_FMO_ThreeYear_PerHour_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_147_FMO_ThreeYear_PerDay_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_147_FMO_ThreeYear_PerDay_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Day");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_147_FMO_ThreeYear_PerDay_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_148_FMO_ThreeYear_PerMonth_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_148_FMO_ThreeYear_PerMonth_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                

                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Month");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_148_FMO_ThreeYear_PerMonth_CostValidation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_149_FMO_ThreeYear_PerYear_CostValidation")]
        [AllureTag("Regression")]
        public async Task TC_149_FMO_ThreeYear_PerYear_CostValidation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _fmoPage.ViewMigrationCostButton.First.ClickAsync();

                await _fmoPage.ThreeYearTab.ClickAsync();

                await _fmoPage.SelectTimePeriodAsync("Year");

                await _fmoPage.VerifyCostCalculationAsync(
                    _fmoPage.TotalCost,
                    _fmoPage.ThreeYearReservedColumn);

            }, nameof(TC_149_FMO_ThreeYear_PerYear_CostValidation));
        }  
    }
}
         #endregion
