// c 2023-03-04
// m 2023-03-27

namespace TMT.Pages;

public partial class MyMapsPage : ContentPage {
	public MyMapsPage(MyMapsViewModel vm) {
		InitializeComponent();
		BindingContext = vm;
	}
}