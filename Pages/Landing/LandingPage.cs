using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using Microsoft.Playwright;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace WiseMigrateTests.Pages.Landing
{
    public class LandingPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;
        public LandingPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
        }

        
        public async Task NavigateToLandingPageAsync()
        {
            await _page.GotoAsync(_basicSetup.AppUrl);
        
        }

        public async Task ClickGenerateMigrationPlanAsync()
        {
            await _page.ClickAsync("text=Generate Migration Plan for Your Enterprise");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
        
        public async Task ClickMigrationAssessmentAsync()
        {
           await _page.ClickAsync("text=Start Your Migration Assessment");
           await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public BasicSetup Setup => _basicSetup;

    }
}

