// c 2023-03-05
// m 2023-03-27

namespace TMT.Pages;

public partial class MyRecordsPage : ContentPage {
	public MyRecordsPage(MyRecordsViewModel vm) {
		InitializeComponent();
		BindingContext = vm;
	}
}