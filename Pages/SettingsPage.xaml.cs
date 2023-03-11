// c 2023-03-05
// m 2023-03-11

namespace TMT.Pages {
	public partial class SettingsPage : ContentPage {
		public SettingsPage(SettingsViewModel vm) {
			InitializeComponent();
			BindingContext = vm;
		}
	}
}