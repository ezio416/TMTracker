// c 2023-03-27
// m 2023-03-27

namespace TMT.Pages;

public partial class MapPage : ContentPage {
    public MapPage(MapViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}