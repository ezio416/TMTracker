// c 2023-03-05
// m 2023-03-05

using TMT.ViewModels;

namespace TMT {
	public partial class QueryPage : ContentPage {
		public QueryPage(QueryViewModel vm) {
			InitializeComponent();
			BindingContext = vm;
		}
	}
}