using Microsoft.Playwright;
using WiseMigrateTests.Utils;


namespace WiseMigrateTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;
        public string AppUrl => ConfigReader.Get("AppUrl");
        public string LoginPageUrl => ConfigReader.Get("LoginPageUrl");
        public string Username => ConfigReader.Get("Username");
        public string Password => ConfigReader.Get("Password");

        public BasicSetup(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAppAsync() => await _page.GotoAsync(AppUrl);
        public async Task LoginAsync()
        {
            await _page.FillAsync("input[name='username']", Username);
            await _page.FillAsync("input[name='password']", Password);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task WaitForPageAsync(int seconds) => await Task.Delay(seconds * 1000);
    }
}
