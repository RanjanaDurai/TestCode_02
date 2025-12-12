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
        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync("input[type='email']", username);
            await _page.FillAsync("input[type='Password']", password);
            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task LoginAsync()
        {
            var cred = ConfigReader.GetCredential("standard_user"); // or whatever default you want
            await LoginAsync(cred.Username, cred.Password);
        }
        public async Task WaitForPageAsync(int seconds) => await Task.Delay(seconds * 1000);
    }
}
