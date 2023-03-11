// c 2023-03-04
// m 2023-03-10

using System.Collections.ObjectModel;

namespace TMT.ViewModels {
    public partial class MyMapsViewModel : ObservableObject {
        [ObservableProperty]
        string status;

        //int i = 0;
        //readonly HashSet<string> accountIds = new();
        //readonly HashSet<string> mapIds = new();
        //readonly Dictionary<string, Records.Record> records = new();

        public ObservableCollection<MyMap> MyMaps { get; set; } = new();

        public MyMapsViewModel() {
            status = "get my maps";
        }

        [RelayCommand]
        async Task Btn() {
            Storage.myMaps = await Maps.GetMyMaps();

            //foreach (MyMap map in Storage.myMaps) {
            //    Status = $"({Storage.myMaps.Length - i++}) getting records: {map.mapName}";
            //    mapIds.Add(map.mapId);
            //    map.records = await Records.GetSingleMapRecords(map.mapUid);

            //    foreach (Records.Record record in map.records) {
            //        record.mapId = map.mapId;
            //        record.mapName = map.mapName;
            //        accountIds.Add(record.accountId);
            //        records.Add($"{record.mapId},{record.accountId}", record);
            //    }
            //}

            foreach (MyMap map in Storage.myMaps)
                MyMaps.Add(map);
        }
    }
}