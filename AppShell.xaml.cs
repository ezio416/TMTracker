// c 2023-02-28
// m 2023-03-05

namespace TMT {
	public partial class AppShell : Shell {
		public AppShell() {
			InitializeComponent();

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(MyMapsPage), typeof(MyMapsPage));
            Routing.RegisterRoute(nameof(MyRecordsPage), typeof(MyRecordsPage));
            Routing.RegisterRoute(nameof(QueryPage), typeof(QueryPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
	}
}