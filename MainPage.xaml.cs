// c 2023-02-28
// m 2023-03-03

namespace TMT {
	public partial class MainPage : ContentPage {
        public MainPage() { InitializeComponent(); }

        static bool clicked;
		private void OnClicked(object sender, EventArgs e) {
            if (!clicked) {
                accountID.Text += Config.accountID;
                agent.Text += Config.api.agent;
                password.Text += Config.api.password;
                username.Text += Config.api.username;
                waitMilliseconds.Text += Config.api.waitMilliseconds;
                ignoreRegionContinent.Text += Config.myMaps.ignoreRegionContinent;
                ignoreRegionWorld.Text += Config.myMaps.ignoreRegionWorld;
                appDir.Text += Config.appDir;
                configFile.Text += Config.configFile;
                freshConfig.Text += Config.freshConfig;
                os.Text += Config.os;
                CounterBtn.Text = "loaded";
                clicked = true;
            }
        }
    }
}