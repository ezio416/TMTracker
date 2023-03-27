// c 2023-02-28
// m 2023-03-27

namespace TMT;

public partial class AppShell : Shell {
	public AppShell() {
		InitializeComponent();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        Routing.RegisterRoute(nameof(MyMapsPage), typeof(MyMapsPage));
        Routing.RegisterRoute(nameof(MyRecordsPage), typeof(MyRecordsPage));
        Routing.RegisterRoute(nameof(QueryPage), typeof(QueryPage));
        Routing.RegisterRoute(nameof(RecentRecordsPage), typeof(RecentRecordsPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}