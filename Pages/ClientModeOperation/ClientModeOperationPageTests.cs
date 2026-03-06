using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.TestHooks;
using WiseMigrateTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseMigrateTests.Pages.ClientModeOperation
{
    [Collection("Playwright collection")]
    [AllureSuite("Client Mode Operation Tests")]
    public class ClientModeOperationPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;

        private ProjectListPage _projectList = null!;

        private BasicSetup _basicSetup = null!;
        private ClientModeOperationPage _cmoPage = null!;

        public async new Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
            _cmoPage = new ClientModeOperationPage(Page);

            await _loginPage.ValidateMigrateAdminAccount(Page);
            await _projectList.ClickCreateNewProjectAsync();
            await _basicSetup.CreateProjectAsync();
            _createdProjectName= _basicSetup.projectName;
            Logger.Info("Logged in as Super Admin and created a basic project");
        }

        #region Smoke Tests
        [Fact]
        [AllureOwner("TC_001_CMO_Page_UI_Should_Load")]
        [AllureTag("smoke")]
        public async Task TC_001_CMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await Assertions.Expect(Page.GetByText("Current Mode of Operation - Architectural View")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_001_CMO_Page_UI");
                Logger.Info("CMO Page UI loaded successfully");
            }, "TC_001_CMO_Page_UI_Should_Load");
        }

        [Fact]
        [AllureOwner("TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("smoke")]
        public async Task TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons");
                Logger.Info("CMO Page has clickable navigation buttons");
            }, "TC_002_CMO_Page_Should_Have_Clickable_Navigation_Buttons");
        } 

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_Verify_CMO_Architecture_Display")]
        [AllureTag("Smoke")]
        public async Task TC_CMO_003_Verify_CMO_Architecture_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_003_Verify_CMO_Architecture_Display");
                Logger.Info("CMO architecture displayed correctly");
            }, "TC_CMO_003_Verify_CMO_Architecture_Display");
        }

        #endregion

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Verify_Pan_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_CMO_004_Verify_Pan_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
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
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_CMO_004_Pan_Functionality");
                Logger.Info("Pan functionality verified");
            }, "TC_CMO_004_Verify_Pan_Functionality");
        }

        #region Zoom In

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Verify_ZoomIn_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_005_Verify_ZoomIn_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomInWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_005_Verify_ZoomIn_MouseScroll");
                Logger.Info("Zoom In via mouse scroll verified");
            }, "TC_005_Verify_ZoomIn_MouseScroll");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_ZoomIn_Button")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_ZoomIn_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _basicSetup.WaitForPageAsync(1);
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_006_Verify_ZoomIn_Button");
                Logger.Info("Zoom In via button verified");
            }, "TC_006_Verify_ZoomIn_Button");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Verify_Max_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_007_Verify_Max_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.ZoomInToMaximumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_007_Verify_Max_Zoom_Limit");
                Logger.Info("Maximum zoom in level verified");
            }, "TC_007_Verify_Max_Zoom_Limit");
        
        }

        #endregion

        #region Zoom Out

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_ZoomOut_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_ZoomOut_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                    await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomOutWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight < initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_008_Verify_ZoomOut_MouseScroll");
                Logger.Info("Zoom Out via mouse scroll verified");
            }, "TC_008_Verify_ZoomOut_MouseScroll");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Verify_ZoomOut_Button")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_ZoomOut_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {                          
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalWidth < initialWidth);
                Assert.True(finalHeight < initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_009_Verify_ZoomOut_Button");
                Logger.Info("Zoom Out via button verified");
            }, "TC_009_Verify_ZoomOut_Button");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Verify_Min_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_010_Verify_Min_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async ()=>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.ZoomOutToMinimumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_010_Verify_Min_Zoom_Limit");
                Logger.Info("Minimum zoom out limit verified");
            }, "TC_010_Verify_Min_Zoom_Limit");
            
        }

        #endregion

        #region Reset

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_ResetOnZoom_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_012_Verify_ResetOnZoom_Functionality");
                Logger.Info("Reset functionality verified");
            }, "TC_012_Verify_ResetOnZoom_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Verify_ResetOnZoomOut_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_013_Verify_ResetOnZoomOut_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_013_Verify_ResetOnZoomOut_Functionality");
                Logger.Info("Reset functionality verified");
            }, "TC_013_Verify_ResetOnZoomOut_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_ResetOnZoom_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
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
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_014_Verify_ResetOnZoomCombination_Functionality");
                Logger.Info("Reset functionality verified");
            }, "TC_014_Verify_ResetOnZoomCombination_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Verify_Reset_Does_Not_Impact_Scale")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_Reset_Does_Not_Impact_Scale()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.ClickResetAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_015_Verify_Reset_Does_Not_Impact_Scale");
                Logger.Info("No error on multiple reset clicks");
            }, "TC_015_Verify_Reset_Does_Not_Impact_Scale");
        }

        #endregion

        #region Fullscreen

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Verify_Enter_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_016_Verify_Enter_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.EnterFullscreenModeAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.True(finalWidth > initialWidth);
                Assert.True(finalHeight > initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_016_Verify_Enter_Fullscreen");
                Logger.Info("Entered fullscreen mode successfully");
            }, "TC_016_Verify_Enter_Fullscreen");
        }

        // [Fact]
        // [Trait("Category", "Regression")]
        // [AllureOwner("TC_017_Verify_Exit_Fullscreen_ESC")]
        // [AllureTag("Regression")]
        // public async Task TC_017_Verify_Exit_Fullscreen_ESC()
        // {
        //     await _attachmentHelper.RunWithTracingAsync(async () =>
        //     {
        //         await _cmoPage.NavigateToCMOPageAsync();
        //         var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
        //         await _cmoPage.EnterFullscreenModeAsync();  
        //         await _basicSetup.WaitForPageAsync(2);
        //         await Page.Keyboard.PressAsync("Escape");    
        //         await _basicSetup.WaitForPageAsync(3);      
        //         var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
        //         Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
        //         Assert.Equal(finalHeight, initialHeight);
        //         await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_017_Verify_Exit_Fullscreen_ESC");
        //         Logger.Info("Verified exit from fullscreen mode");
        //     },"TC_017_Verify_Exit_Fullscreen_ESC");
        // }

         [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Verify_Exit_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_018_Verify_Exit_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.EnterFullscreenModeAsync();  
                await _basicSetup.WaitForPageAsync(1);
                await _cmoPage.ExitFullscreenModeAsync();      
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_018_Verify_Exit_Fullscreen");
                Logger.Info("Verified exit from fullscreen mode");
            },"TC_018_Verify_Exit_Fullscreen");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Verify_UI_Scaling_In_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_019_Verify_UI_Scaling_In_Fullscreen()
        {
            await _cmoPage.NavigateToCMOPageAsync();
            var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
            await _cmoPage.EnterFullscreenModeAsync();
            await _basicSetup.WaitForPageAsync(1);
            var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();    
            Assert.True(finalWidth > initialWidth);
            Assert.True(finalHeight > initialHeight);
            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_019_Verify_UI_Scaling_In_Fullscreen");
            Logger.Info("UI elements scale correctly in fullscreen");
        }

        #endregion

        #region Download

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Verify_Download_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_020_Verify_Download_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var download = await _cmoPage.TriggerDownloadAsync();
                var fileName= "CMO_"+_createdProjectName+"_"+DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")+".svg";    
                Console.WriteLine($"Expected filename: {fileName}");            
                Assert.NotNull(download);
                Assert.Equal(fileName, download.SuggestedFilename);
            }, "TC_020_Verify_Download_Functionality");
        }

         [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_Verify_Download_OnFullScreenMode_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_021_Verify_Download_OnFullScreenMode_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.EnterFullscreenModeAsync();
                var download = await _cmoPage.TriggerDownloadAsync();
                var fileName= "CMO_"+_createdProjectName+"_"+DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")+".svg";    
                Console.WriteLine($"Expected filename: {fileName}");            
                Assert.NotNull(download);
                Assert.Equal(fileName, download.SuggestedFilename);
            }, "TC_021_Verify_Download_OnFullScreenMode_Functionality");
        }
        #endregion

        #region Legend

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_022_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_022_Verify_Legend_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_022_Verify_Legend_Display");
                Logger.Info("Legend displayed successfully");
            },"TC_022_Verify_Legend_Display");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_023_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_023_Verify_Legend_Display_OnFullScreenMode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.EnterFullscreenModeAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_023_Verify_Legend_Display");
                Logger.Info("Legend displayed successfully");
            },"TC_023_Verify_Legend_Display");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_024_Verify_ToggleLegend_Operation")]
        [AllureTag("Regression")]
        public async Task TC_024_Verify_ToggleLegend_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync( async()=>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-wrapper")).Not.ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_024_Verify_ToggleLegend_Operation");
                Logger.Info("Legend toggle operation verified successfully");   
            },"TC_024_Verify_ToggleLegend_Operation");
        }

        #endregion

        #region Search Functionality

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_025_Verify_Search_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_025_Verify_Search_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var searchInputText= "SRVTEST0"+new Random().Next(10,53);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).ToBeVisibleAsync();
                await Task.Delay(100);
                await Assertions.Expect(Page.GetByText("Found 1 match")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_025_Verify_Search_Functionality");
            },"TC_025_Verify_Search_Functionality");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_026_Verify_SearchNavigation_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_026_Verify_SearchNavigation_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var searchInputText= "SRVTEST";
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator(".search-results")).ToBeVisibleAsync();
                await _basicSetup.WaitForPageAsync(1);
                var searchResultsCount = await Page.Locator(".search-results .search-item").CountAsync();
                await Assertions.Expect(Page.Locator("#fullscreen-container .search-container .search-results.show > span")).ToContainTextAsync($"Found {searchResultsCount} matches");
                 await _basicSetup.WaitForPageAsync(1);
                await _cmoPage.PerformSearchNavigationAsync();
                 await _basicSetup.WaitForPageAsync(1);
                var initalSearchItem = await Page.Locator(".search-results .search-item.selected-node.keyboard-selected").InnerTextAsync();
                await _basicSetup.WaitForPageAsync(1);
                var activeSearchItem = initalSearchItem;
                Console.WriteLine($"Initial active search item: {initalSearchItem}");
                for (int i=0;i<new Random().Next(1,searchResultsCount-1);i++)
                {
                    await _cmoPage.PerformSearchNavigationAsync();
                    await _basicSetup.WaitForPageAsync(1);
                    activeSearchItem = await Page.Locator(".search-results .search-item.selected-node.keyboard-selected").InnerTextAsync();
                }
                Assert.NotEqual(initalSearchItem, activeSearchItem);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_026_Verify_SearchNavigation_Functionality");
            },"TC_026_Verify_SearchNavigation_Functionality");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_027_Verify_Search_RandomSelectedNode")]
        [AllureTag("Regression")]
        public async Task TC_027_Verify_Search_RandomSelectedNode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.PerformSearchAsync("SRVTEST");
                await Assertions.Expect(Page.Locator(".search-results")).ToBeVisibleAsync();
                var NodeId=new Random().Next(10,53);
                var searchInputText= "SRVTEST0"+NodeId;
                Console.WriteLine($"Randomly selected search item: {searchInputText}");
                await _basicSetup.WaitForPageAsync(1);
                await Page.Locator($".search-results .search-item:has-text('{searchInputText}')").ClickAsync();
                //await Assertions.Expect(Page.Locator($"#node{NodeId}.title")).ToContainTextAsync(searchInputText);
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_027_Verify_Search_RandomSelectedNode");
                Logger.Info("Random search item selection and navigation verified");
            },"TC_027_Verify_Search_RandomSelectedNode");
        }
    
        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_028_Verify_Search_NoResults")]
        [AllureTag("Regression")]
        public async Task TC_028_Verify_Search_NoResults()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var searchInputText= "SRVTEST000"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).Not.ToBeVisibleAsync();
                await Assertions.Expect(Page.GetByText("No results found")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_028_Verify_Search_NoResults");
                Logger.Info("Search with no results verified");
            },"TC_028_Verify_Search_NoResults");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_029_Verify_CloseSearch")]
        [AllureTag("Regression")]
        public async Task TC_029_Verify_CloseSearch()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                var searchInputText= "SRVTEST0"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await _cmoPage.CloseSearchBarAsync();
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_029_Verify_CloseSearch");
            }, "TC_029_Verify_CloseSearch");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_030_Verify_Search_EmptyInput")]
        [AllureTag("Regression")]
        public async Task TC_030_Verify_Search_EmptyInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _cmoPage.NavigateToCMOPageAsync();
                await _cmoPage.PerformSearchAsync("");
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                await Assertions.Expect(Page.Locator("role=alert")).ToHaveTextAsync("Please enter a search term.");
                await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_030_Verify_Search_EmptyInput");
                Logger.Info("Search with empty input verified");
            }, "TC_030_Verify_Search_EmptyInput");
        }

        #endregion
                
            
    }
}