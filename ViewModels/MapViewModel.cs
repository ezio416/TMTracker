// c 2023-03-27
// m 2023-03-27

namespace TMT.ViewModels;

[QueryProperty(nameof(Mapp), "Mapp")]
public partial class MapViewModel : ObservableObject {
    [ObservableProperty]
    MyMap mapp;

    [ObservableProperty]
    string uploaded;

    [ObservableProperty]
    string authorTime;

    [ObservableProperty]
    string goldTime;

    [ObservableProperty]
    string silverTime;

    [ObservableProperty]
    string bronzeTime;

    [RelayCommand]
    void Appearing() {
        Uploaded = $"{Mapp.uploadedIsoUtc.Replace('T', ' ').Replace("Z", " UTC")}";
        AuthorTime = $"A: {Various.FormatSeconds(Mapp.authorTime)}";
        GoldTime   = $"G: {Various.FormatSeconds(Mapp.goldTime)}";
        SilverTime = $"S: {Various.FormatSeconds(Mapp.silverTime)}";
        BronzeTime = $"B: {Various.FormatSeconds(Mapp.bronzeTime)}";
    }
}