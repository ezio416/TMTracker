// c 2023-02-28
// m 2023-03-05

using TMT.ViewModels;

namespace TMT {
    public partial class HomePage : ContentPage {
        public HomePage(HomeViewModel vm) {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}