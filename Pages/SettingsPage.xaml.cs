// c 2023-03-05
// m 2023-03-05

using TMT.ViewModels;

namespace TMT {
	public partial class SettingsPage : ContentPage {
		public SettingsPage(SettingsViewModel vm) {
			InitializeComponent();
			BindingContext = vm;
		}
	}
}