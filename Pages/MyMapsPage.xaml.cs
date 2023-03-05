// c 2023-03-04
// m 2023-03-05

using TMT.ViewModels;

namespace TMT {
	public partial class MyMapsPage : ContentPage {
		public MyMapsPage(MyMapsViewModel vm) {
			InitializeComponent();
			BindingContext = vm;
		}
	}
}