using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.ProjectCreation;
using WiseMigrateTests.Pages.InfrastructureUpload;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.ClientModeOperation
{
    public class ClientModeOperationPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        private readonly InfrastructureUploadPage _infrastructureUpload;


        public ILocator NextButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

        public ILocator CancelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
        private ILocator ZoomInButton => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(0);
        private ILocator ZoomOutButton => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(1);
        private ILocator ResetButton => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(2);

        private ILocator DownloadButton => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(3);
        private ILocator FullscreenButton => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(4);
        private ILocator LegendIcon => ViewControlToolBar.GetByRole(AriaRole.Button).Nth(5);
         public ILocator SearchButton =>_page.Locator(".search-container > .mud-tooltip-root > .mud-button-root");
        private ILocator SearchNavigateButton =>_page.Locator("#fullscreen-container .search-container > div:nth-child(4) > button > span > svg");
        public ILocator SearchInputBar => _page.GetByPlaceholder("Search Server...");
        private ILocator CloseSearchButton => _page.Locator(".mud-input-adornment-icon-button");
        private ILocator ViewControlToolBar => _page.Locator(".main-content:has([role='toolbar'])");
        public ILocator CMOArchitectureContainer => _page.Locator("#diagram");
   

        public ClientModeOperationPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
            _infrastructureUpload= new InfrastructureUploadPage(page, _basicSetup);
        }

        public async Task NavigateToCMOPageAsync()
        {
            await _basicSetup.InfrastructureUploadPageSetupAsync();            
            await _basicSetup.WaitForPageAsync(2);                         
        }


        public async Task ClickNavigationNextAsync()
        {
            await _page.Locator(".mr-3").GetByRole(AriaRole.Button).Last.ClickAsync();
        }

        public async Task PerformZoomInAsync()
        {
            await ZoomInButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task PerformZoomOutAsync()
        {
            await ZoomOutButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task<(int, int)> IntializeCoordinates()
        {
            var boundingBox = await CMOArchitectureContainer.BoundingBoxAsync();
            if (boundingBox == null)
                return(0, 0);
            int startX = (int)(boundingBox.X + boundingBox.Width / 2);
            int startY = (int)(boundingBox.Y + boundingBox.Height / 2);
            return(startX, startY);
        }
        public async Task PerformPanAsync()
        {
            var (startX, startY) = await IntializeCoordinates();
            await _page.Mouse.MoveAsync(startX, startY);
            await _page.Mouse.DownAsync();
            var random = new Random();
            await _page.Mouse.MoveAsync(startX + random.Next(100), startY + random.Next(100), new MouseMoveOptions { Steps = 10 });
            await _page.Mouse.UpAsync();
        }

        public async Task ZoomInWithMouseAsync()
        {
            var (startX, startY) = await IntializeCoordinates();
            await _page.Mouse.MoveAsync(startX, startY);
            await _page.Mouse.WheelAsync(0, -120);
        }

        public async Task ZoomOutWithMouseAsync()
        {
            var (startX, startY) = await IntializeCoordinates();
            await _page.Mouse.MoveAsync(startX, startY);
            await _page.Mouse.WheelAsync(0, +120);
        }
        public async Task<double> ZoomInToMaximumAsync()
        {
            double initialHeight = CMOArchitectureContainer.BoundingBoxAsync().Result?.Height ?? 0;
            double previousHeight = initialHeight;
            int zoomCount = 0;

            for(int i=0;i<100;i++)
            {
                await ZoomInButton.ClickAsync();
                await _basicSetup.WaitForPageAsync(1);
                double currentHeight = CMOArchitectureContainer.BoundingBoxAsync().Result?.Height ?? 0;

                if (Math.Abs(currentHeight - previousHeight) < 0.001)
                {
                    return currentHeight;
                }
                previousHeight = currentHeight;
                zoomCount++;
                
            }
            Console.WriteLine("Warning: Max attempts reached — could not determine limit");
            return previousHeight;
        }

        public async Task<double> ZoomOutToMinimumAsync()
        {
            double initialHeight = CMOArchitectureContainer.BoundingBoxAsync().Result?.Height ?? 0;
            double previousHeight = initialHeight;
            int zoomCount = 0;

            for(int i=0;i<100;i++)
            {
                await ZoomOutButton.ClickAsync();
                await _basicSetup.WaitForPageAsync(1);
                double currentHeight = CMOArchitectureContainer.BoundingBoxAsync().Result?.Height ?? 0;

                if (Math.Abs(currentHeight - previousHeight) < 0.001)
                {
                    return currentHeight;
                }
                previousHeight = currentHeight;
                zoomCount++;
                
            }
            Console.WriteLine("Warning: Max attempts reached — could not determine limit");
            return previousHeight;
        }

        public async Task ClickResetAsync()
        {
            await ResetButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task EnterFullscreenModeAsync()
        {
            await FullscreenButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task ExitFullscreenModeAsync()
        {
            await FullscreenButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }


        public async Task<(float, float)> RetrieveDimensionsAsync()
        {
            var container = await CMOArchitectureContainer.BoundingBoxAsync();
            if (container == null)
                return(0, 0);
            return (container.Width, container.Height);
        }        
        public async Task<IDownload> TriggerDownloadAsync()
        {
            var downloadTask = _page.WaitForDownloadAsync();
            await DownloadButton.ClickAsync(); 
            return await downloadTask;         
        }

        public async Task ClickLegendIconAsync()
        {
            await LegendIcon.ClickAsync();
            await _basicSetup.WaitForPageAsync(1);
        }

        public async Task PerformSearchAsync(string searchTerm)
        {
            await SearchButton.ClickAsync();
            await SearchInputBar.FillAsync(searchTerm);            
        }

        public async Task PerformSearchNavigationAsync()
        {
            if(SearchNavigateButton == null)
            {
                throw new Exception("Search Navigate Button not found.");
            }   
            await SearchNavigateButton.ClickAsync();
            await Task.Delay(100);     
        }

        public async Task CloseSearchBarAsync()
        {
            await CloseSearchButton.ClickAsync();
            await Task.Delay(100);
        }        
    }
}
