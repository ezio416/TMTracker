// c 2023-03-05
// m 2023-03-05

using TMT.ViewModels;

namespace TMT {
	public partial class MyRecordsPage : ContentPage {
		public MyRecordsPage(MyRecordsViewModel vm) {
			InitializeComponent();
			BindingContext = vm;
		}
	}
}