// c 2023-03-05
// m 2023-03-27

namespace TMT.Pages;

public partial class QueryPage : ContentPage {
	public QueryPage(QueryViewModel vm) {
		InitializeComponent();
		BindingContext = vm;
	}
}