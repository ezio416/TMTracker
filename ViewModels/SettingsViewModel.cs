// c 2023-03-05
// m 2023-03-06

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TMT.ViewModels {
    public partial class SettingsViewModel : ObservableObject {
        const string accountIdTitleInitial = "Account ID";
        const string accountIdTitleChanged = accountIdTitleInitial + "*";
        [ObservableProperty]
        string accountIdTitle = accountIdTitleInitial;
        [ObservableProperty]
        string accountId = Config.accountId;
        string accountIdInitial = Config.accountId;

        const string agentTitleInitial = "API Agent";
        const string agentTitleChanged = agentTitleInitial + "*";
        [ObservableProperty]
        string agentTitle = agentTitleInitial;
        [ObservableProperty]
        string agent = Config.api.agent;
        string agentInitial = Config.api.agent;

        const string usernameTitleInitial = "API Username";
        const string usernameTitleChanged = usernameTitleInitial + "*";
        [ObservableProperty]
        string usernameTitle = usernameTitleInitial;
        [ObservableProperty]
        string username = Config.api.username;
        string usernameInitial = Config.api.username;

        const string passwordTitleInitial = "API Password";
        const string passwordTitleChanged = passwordTitleInitial + "*";
        [ObservableProperty]
        string passwordTitle = passwordTitleInitial;
        [ObservableProperty]
        string password = Config.api.password;
        string passwordInitial = Config.api.password;

        const string waitTimeTitleInitial = "API Wait Time (ms)";
        const string waitTimeTitleChanged = waitTimeTitleInitial + "*";
        [ObservableProperty]
        string waitTimeTitle = waitTimeTitleInitial;
        [ObservableProperty]
        string waitTime = Config.api.waitMilliseconds.ToString();
        string waitTimeInitial = Config.api.waitMilliseconds.ToString();

        [ObservableProperty]
        string[] ignoreMapIds = Config.myMaps.ignoreMapIds;
        string[] ignoreMapIdsInitial = Config.myMaps.ignoreMapIds;

        const string ignoreRegionsTitleInitial = "Ignore Regions";
        const string ignoreRegionsTitleChanged = ignoreRegionsTitleInitial + "*";
        [ObservableProperty]
        string ignoreRegionsTitle = ignoreRegionsTitleInitial;

        const string ignoreContinentTitleInitial = "Continent";
        const string ignoreContinentTitleChanged = ignoreContinentTitleInitial + "*";
        [ObservableProperty]
        string ignoreContinentTitle = ignoreContinentTitleInitial;
        [ObservableProperty]
        bool ignoreContinent = Config.myMaps.ignoreRegionContinent;
        bool ignoreContinentInitial = Config.myMaps.ignoreRegionContinent;

        const string ignoreWorldTitleInitial = "     World";
        const string ignoreWorldTitleChanged = ignoreWorldTitleInitial + "*";
        [ObservableProperty]
        string ignoreWorldTitle = ignoreWorldTitleInitial;
        [ObservableProperty]
        bool ignoreWorld = Config.myMaps.ignoreRegionWorld;
        bool ignoreWorldInitial = Config.myMaps.ignoreRegionWorld;

        [ObservableProperty]
        bool saveEnabled;

        [RelayCommand]
        async Task AccountIdClicked() {
            string userAccountId = await Application.Current.MainPage.DisplayPromptAsync(
                accountIdTitleInitial, "New account ID (UUID):", initialValue: AccountId, maxLength: 36, cancel: "CANCEL"
            );
            if (userAccountId != null) {
                AccountId = userAccountId;
                Config.accountId = AccountId;
                if (userAccountId == accountIdInitial) AccountIdTitle = accountIdTitleInitial;
                else {
                    AccountIdTitle = accountIdTitleChanged;
                    SaveEnabled = true;
                }
            }
        }

        [RelayCommand]
        async Task AgentClicked() {
            string userAgent = await Application.Current.MainPage.DisplayPromptAsync(
                agentTitleInitial, "New agent:", initialValue: Agent, cancel: "CANCEL"
            );
            if (userAgent != null) {
                Agent = userAgent;
                Config.api.agent = Agent;
                if (userAgent == agentInitial) AgentTitle = agentTitleInitial;
                else {
                    AgentTitle = agentTitleChanged;
                    SaveEnabled = true;
                }
            }
        }

        [RelayCommand]
        async Task UsernameClicked() {
            string userUsername = await Application.Current.MainPage.DisplayPromptAsync(
                usernameTitleInitial, "New Ubisoft username (email):", initialValue: Username, cancel: "CANCEL"
            );
            if (userUsername != null) {
                Username = userUsername;
                Config.api.username = Username;
                if (userUsername == usernameInitial) UsernameTitle = usernameTitleInitial;
                else {
                    UsernameTitle = usernameTitleChanged;
                    SaveEnabled = true;
                }
            }
        }

        [RelayCommand]
        async Task PasswordClicked() {
            string userPassword = await Application.Current.MainPage.DisplayPromptAsync(
                passwordTitleInitial, "New Ubisoft password:", initialValue: Password, cancel: "CANCEL"
            );
            if (userPassword != null) {
                Password = userPassword;
                Config.api.password = Password;
                if (userPassword == passwordInitial) PasswordTitle = passwordTitleInitial;
                else {
                    PasswordTitle = passwordTitleChanged;
                    SaveEnabled = true;
                }
            }
        }

        [RelayCommand]
        async Task WaitTimeClicked() {
            string userWaitTime = await Application.Current.MainPage.DisplayPromptAsync(
                waitTimeTitleInitial, "New wait time:", initialValue: WaitTime.ToString(), keyboard: Keyboard.Numeric, cancel: "CANCEL"
            );
            if (userWaitTime != null) {
                WaitTime = userWaitTime;
                Config.api.waitMilliseconds = Int32.Parse(WaitTime);
                if (userWaitTime == waitTimeInitial) WaitTimeTitle = waitTimeTitleInitial;
                else {
                    WaitTimeTitle = waitTimeTitleChanged;
                    SaveEnabled = true;
                }
            }
        }

        partial void OnIgnoreContinentChanged(bool value) {
            if (IgnoreContinent != ignoreContinentInitial) {
                Config.myMaps.ignoreRegionContinent = IgnoreContinent;
                IgnoreContinentTitle = ignoreContinentTitleChanged;
                SaveEnabled = true;
            }
            else IgnoreContinentTitle = ignoreContinentTitleInitial;
            CheckRegionsVsInitial();
        }

        partial void OnIgnoreWorldChanged(bool value) {
            if (IgnoreWorld != ignoreWorldInitial) {
                Config.myMaps.ignoreRegionWorld = IgnoreWorld;
                IgnoreWorldTitle = ignoreWorldTitleChanged;
                SaveEnabled = true;
            }
            else IgnoreWorldTitle = ignoreWorldTitleInitial;
            CheckRegionsVsInitial();
        }

        void CheckRegionsVsInitial() {
            if (IgnoreContinent == ignoreContinentInitial && IgnoreWorld == ignoreWorldInitial)
                IgnoreRegionsTitle = ignoreRegionsTitleInitial;
            else IgnoreRegionsTitle = ignoreRegionsTitleChanged;
        }

        [RelayCommand]
        async Task RevertClicked() {
            if (!SaveEnabled) {
                await Application.Current.MainPage.DisplayAlert("Success?", "Nothing to revert", "OK");
                return;
            }
            AccountId = accountIdInitial;
            AccountIdTitle = accountIdTitleInitial;

            Agent = agentInitial;
            AgentTitle = agentTitleInitial;

            Password = passwordInitial;
            PasswordTitle = passwordTitleInitial;

            Username = usernameInitial;
            UsernameTitle = usernameTitleInitial;

            WaitTime = waitTimeInitial;
            WaitTimeTitle = waitTimeTitleInitial;

            IgnoreContinent = ignoreContinentInitial;
            IgnoreWorld = ignoreWorldInitial;

            SaveEnabled = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Unsaved changes have been reverted", "OK");
        }

        [RelayCommand]
        async Task SaveClicked() {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Question", $"Are you sure you want to save settings to the file at {Config.configFile} ?", "YES", "CANCEL"
            );
            if (!confirm)
                return;

            _ = Config.Write();

            accountIdInitial = (string)AccountId.Clone();
            agentInitial = (string)Agent.Clone();
            passwordInitial = (string)Password.Clone();
            usernameInitial = (string)Username.Clone();
            waitTimeInitial = (string)WaitTime.Clone();
            ignoreContinentInitial = IgnoreContinent;
            ignoreWorldInitial = IgnoreWorld;

            AccountIdTitle = accountIdTitleInitial;
            AgentTitle = agentTitleInitial;
            PasswordTitle = passwordTitleInitial;
            UsernameTitle = usernameTitleInitial;
            WaitTimeTitle = waitTimeTitleInitial;
            IgnoreRegionsTitle = ignoreRegionsTitleInitial;
            IgnoreContinentTitle = ignoreContinentTitleInitial;
            IgnoreWorldTitle = ignoreWorldTitleInitial;

            SaveEnabled = false;
            await Application.Current.MainPage.DisplayAlert("Success", $"Settings saved", "OK");
        }
    }
}