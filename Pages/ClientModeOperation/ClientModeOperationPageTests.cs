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
        protected override bool UseClassLevelGrouping => true;

        public async new Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _basicSetup = new BasicSetup(Page);
            _projectList = new ProjectListPage(Page);
            _cmoPage = new ClientModeOperationPage(Page);            

            // await _loginPage.ValidateMigrateAdminAccount(Page);
            // await _projectList.ClickCreateNewProjectAsync();
            // await _basicSetup.CreateProjectAsync();
            // _createdProjectName= _basicSetup.projectName;
            Logger.Info("Logged in as Super Admin and created a basic project");
        }

        #region Smoke Tests
        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_075_CMO_Page_UI_Should_Load")]
        [AllureTag("Smoke")]
        public async Task TC_075_CMO_Page_UI_Should_Load()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.GetByText("Current Mode of Operation - Architectural View")).ToBeVisibleAsync();
                Logger.Info("CMO Page UI loaded successfully");
            }, "TC_075_CMO_Page_UI_Should_Load");
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_076_CMO_Page_Should_Have_Clickable_Navigation_Buttons")]
        [AllureTag("Smoke")]
        public async Task TC_076_CMO_Page_Should_Have_Clickable_Navigation_Buttons()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).First).ToBeEnabledAsync();
                await Assertions.Expect(Page.Locator(".mr-3").GetByRole(AriaRole.Button).Last).ToBeEnabledAsync();                Logger.Info("CMO Page has clickable navigation buttons");
            }, "TC_076_CMO_Page_Should_Have_Clickable_Navigation_Buttons");
        } 

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_077_Verify_CMO_Architecture_Display")]
        [AllureTag("Smoke")]
        public async Task TC_077_Verify_CMO_Architecture_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                Logger.Info("CMO architecture displayed correctly");
            }, "TC_077_Verify_CMO_Architecture_Display");
        }

        #endregion

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_078_Verify_Pan_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_078_Verify_Pan_Functionality()
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
            }, "TC_078_Verify_Pan_Functionality");
        }

        #region Zoom In

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_079_Verify_ZoomIn_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_079_Verify_ZoomIn_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomInWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Zoom In via mouse scroll verified");
            }, "TC_079_Verify_ZoomIn_MouseScroll");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_080_Verify_ZoomIn_Button")]
        [AllureTag("Regression")]
        public async Task TC_080_Verify_ZoomIn_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _basicSetup.WaitForPageAsync(1);
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Zoom In via button verified");
            }, "TC_080_Verify_ZoomIn_Button");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_081_Verify_Max_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_081_Verify_Max_Zoom_Limit()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.ZoomInToMaximumAsync();
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("Maximum zoom in level verified");
            }, "TC_081_Verify_Max_Zoom_Limit");
        
        }

        #endregion

        #region Zoom Out

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_082_Verify_ZoomOut_MouseScroll")]
        [AllureTag("Regression")]
        public async Task TC_082_Verify_ZoomOut_MouseScroll()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                    
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.ZoomOutWithMouseAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalHeight < initialHeight);
                Logger.Info("Zoom Out via mouse scroll verified");
            }, "TC_082_Verify_ZoomOut_MouseScroll");
        }
 
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_083_Verify_ZoomOut_Button")]
        [AllureTag("Regression")]
        public async Task TC_083_Verify_ZoomOut_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {                          
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Console.WriteLine($"Initial dimensions: {initialWidth}x{initialHeight}, Final dimensions: {finalWidth}x{finalHeight}");
                Assert.True(finalWidth < initialWidth);
                Assert.True(finalHeight < initialHeight);
                Logger.Info("Zoom Out via button verified");
            }, "TC_083_Verify_ZoomOut_Button");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_084_Verify_Min_Zoom_Limit")]
        [AllureTag("Regression")]
        public async Task TC_084_Verify_Min_Zoom_Limit()
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
            }, "TC_084_Verify_Min_Zoom_Limit");
            
        }

        #endregion

        #region Reset

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_085_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_085_Verify_ResetOnZoom_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomInAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                Logger.Info("Reset functionality verified");
            }, "TC_085_Verify_ResetOnZoom_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_086_Verify_ResetOnZoomOut_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_086_Verify_ResetOnZoomOut_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.PerformZoomOutAsync();
                await _cmoPage.ClickResetAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                await Assertions.Expect(_cmoPage.CMOArchitectureContainer).ToBeVisibleAsync();
                Logger.Info("Reset functionality verified");
            }, "TC_086_Verify_ResetOnZoomOut_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_087_Verify_ResetOnZoom_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_087_Verify_ResetOnZoom_Functionality()
        {
            
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
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
            }, "TC_087_Verify_ResetOnZoomCombination_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_088_Verify_Reset_Does_Not_Impact_Scale")]
        [AllureTag("Regression")]
        public async Task TC_088_Verify_Reset_Does_Not_Impact_Scale()
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
            }, "TC_088_Verify_Reset_Does_Not_Impact_Scale");
        }

        #endregion

        #region Fullscreen

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_089_Verify_Enter_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_089_Verify_Enter_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.EnterFullscreenModeAsync();
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.True(finalWidth > initialWidth);
                Assert.True(finalHeight > initialHeight);
                Logger.Info("Entered fullscreen mode successfully");
            }, "TC_089_Verify_Enter_Fullscreen");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_090_Verify_Exit_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_090_Verify_Exit_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.EnterFullscreenModeAsync();  
                await _basicSetup.WaitForPageAsync(1);
                await _cmoPage.ExitFullscreenModeAsync();      
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();
                Assert.Equal(finalWidth, initialWidth);
                Assert.Equal(finalHeight, initialHeight);
                Logger.Info("Verified exit from fullscreen mode");
            },"TC_090_Verify_Exit_Fullscreen");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_091_Verify_UI_Scaling_In_Fullscreen")]
        [AllureTag("Regression")]
        public async Task TC_091_Verify_UI_Scaling_In_Fullscreen()
        {
            await _attachmentHelper.RunWithTracingAsync(async ()=>
            {
                
                var (initialWidth, initialHeight) = await _cmoPage.RetrieveDimensionsAsync();
                await _cmoPage.EnterFullscreenModeAsync();
                await _basicSetup.WaitForPageAsync(1);
                var (finalWidth, finalHeight) = await _cmoPage.RetrieveDimensionsAsync();    
                Assert.True(finalWidth > initialWidth);
                Assert.True(finalHeight > initialHeight);
                Logger.Info("UI elements scale correctly in fullscreen");
            },"TC_091_Verify_UI_Scaling_In_Fullscreen");
        }

        #endregion

        #region Download

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_092_Verify_Download_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_092_Verify_Download_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var download = await _cmoPage.TriggerDownloadAsync();
                var fileName= "CMO_"+_createdProjectName+"_"+DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")+".svg";    
                Console.WriteLine($"Expected filename: {fileName}");            
                Assert.NotNull(download);
                Assert.Equal(fileName, download.SuggestedFilename);
            }, "TC_092_Verify_Download_Functionality");
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_093_Verify_Download_OnFullScreenMode_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_093_Verify_Download_OnFullScreenMode_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.EnterFullscreenModeAsync();
                var download = await _cmoPage.TriggerDownloadAsync();
                var fileName= "CMO_"+_createdProjectName+"_"+DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")+".svg";    
                Console.WriteLine($"Expected filename: {fileName}");            
                Assert.NotNull(download);
                Assert.Equal(fileName, download.SuggestedFilename);
            }, "TC_093_Verify_Download_OnFullScreenMode_Functionality");
        }
        #endregion

        #region Legend

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_094_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_094_Verify_Legend_Display()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                Logger.Info("Legend displayed successfully");
            },"TC_094_Verify_Legend_Display");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_095_Verify_Legend_Display")]
        [AllureTag("Regression")]
        public async Task TC_095_Verify_Legend_Display_OnFullScreenMode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.EnterFullscreenModeAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                Logger.Info("Legend displayed successfully");
            },"TC_095_Verify_Legend_Display_OnFullScreenMode");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_096_Verify_ToggleLegend_Operation")]
        [AllureTag("Regression")]
        public async Task TC_096_Verify_ToggleLegend_Operation()
        {
            await _attachmentHelper.RunWithTracingAsync( async()=>
            {
                
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-container")).ToBeVisibleAsync();
                await _cmoPage.ClickLegendIconAsync();
                await Assertions.Expect(Page.Locator(" .legend-wrapper")).Not.ToBeVisibleAsync();
                Logger.Info("Legend toggle operation verified successfully");   
            },"TC_096_Verify_ToggleLegend_Operation");
        }

        #endregion

        #region Search Functionality

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_097_Verify_Search_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_097_Verify_Search_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST0"+new Random().Next(10,53);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).ToBeVisibleAsync();
                await Task.Delay(100);
                await Assertions.Expect(Page.GetByText("Found 1 match")).ToBeVisibleAsync();
            },"TC_097_Verify_Search_Functionality");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_098_Verify_SearchNavigation_Functionality")]
        [AllureTag("Regression")]
        public async Task TC_098_Verify_SearchNavigation_Functionality()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
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
            },"TC_098_Verify_SearchNavigation_Functionality");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_099_Verify_Search_RandomSelectedNode")]
        [AllureTag("Regression")] 
        public async Task TC_099_Verify_Search_RandomSelectedNode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.PerformSearchAsync("SRVTEST");
                await Assertions.Expect(Page.Locator(".search-results")).ToBeVisibleAsync();
                var NodeId=new Random().Next(10,53);
                var searchInputText= "SRVTEST0"+NodeId;
                Console.WriteLine($"Randomly selected search item: {searchInputText}");
                await _basicSetup.WaitForPageAsync(1);
                await Page.Locator($".search-results .search-item:has-text('{searchInputText}')").ClickAsync();
                Logger.Info("Random search item selection and navigation verified");
            },"TC_099_Verify_Search_RandomSelectedNode");
        }
    
        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_100_Verify_Search_NoResults")]
        [AllureTag("Regression")]
        public async Task TC_100_Verify_Search_NoResults()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST000"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await Assertions.Expect(Page.Locator("div.search-results.show")).Not.ToBeVisibleAsync();
                // await Assertions.Expect(Page.GetByText("No results found")).ToBeVisibleAsync();
                Logger.Info("Search with no results verified");
            },"TC_100_Verify_Search_NoResults");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_101_Verify_CloseSearch")]
        [AllureTag("Regression")]
        public async Task TC_101_Verify_CloseSearch()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                var searchInputText= "SRVTEST0"+new Random().Next(0,10);
                await _cmoPage.PerformSearchAsync(searchInputText);
                await _cmoPage.CloseSearchBarAsync();
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
            }, "TC_101_Verify_CloseSearch");
        }

        [Fact]
        [Trait("Category","Regression")]
        [AllureOwner("TC_102_Verify_Search_EmptyInput")]
        [AllureTag("Regression")]
        public async Task TC_102_Verify_Search_EmptyInput()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _cmoPage.PerformSearchAsync("");
                await Assertions.Expect(Page.GetByPlaceholder("Search Server...")).ToBeVisibleAsync();
                Logger.Info("Search with empty input verified");
            }, "TC_102_Verify_Search_EmptyInput");
        }

        #endregion
                
            
    }
}