// c 2023-03-11
// m 2023-03-11

namespace TMT.Pages {
    public partial class RecentRecordsPage : ContentPage {
        public RecentRecordsPage(MyMapsViewModel vm) {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}