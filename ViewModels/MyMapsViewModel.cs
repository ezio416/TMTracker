// c 2023-03-04
// m 2023-03-10

using System.Collections.ObjectModel;

namespace TMT.ViewModels {
    public partial class MyMapsViewModel : ObservableObject {
        [ObservableProperty]
        string status = Various.Now();

        [ObservableProperty]
        string mapCountLabelText = "map count: ";

        int i = 0;
        readonly HashSet<string> accountIds = new();
        readonly HashSet<string> mapIds = new();
        Dictionary<string, Records.Record> records = new();

        public ObservableCollection<MyMap> MyMaps { get; set; } = new();

        [RelayCommand]
        async Task RefreshMaps() {
            Status = $"getting my maps...";
            Storage.myMaps = Maps.SortMyMaps(await Maps.GetMyMaps());
            MapCountLabelText = $"map count: {Storage.myMaps.Length}";

            foreach (MyMap map in Storage.myMaps)
                MyMaps.Add(map);

            Status = $"updated: {Various.Now()}";
        }

        [RelayCommand]
        async Task RefreshRecords() {
            foreach (MyMap map in Storage.myMaps) {
                Status = $"({Storage.myMaps.Length - i++}) getting records: {map.mapName}";
                mapIds.Add(map.mapId);
                map.records = await Records.GetSingleMapRecords(map.mapUid);

                foreach (Records.Record record in map.records) {
                    record.mapId = map.mapId;
                    record.mapName = map.mapName;
                    accountIds.Add(record.accountId);
                    records.Add($"{record.mapId},{record.accountId}", record);
                }
            }

            //Status = "getting account info...";
            //Dictionary<string, Accounts.Account> accounts = await Accounts.GetAccounts(accountIds.ToArray());
            //Accounts.Account myAccount = accounts[Storage.myMaps[0].authorId];
            //foreach (MyMap map in Storage.myMaps)
            //    map.authorName = myAccount.accountName;
            //foreach (KeyValuePair<string, Records.Record> record in records)
            //    record.Value.accountName = accounts[record.Value.accountId].accountName;

            //Status = "inserting values...";
            //// add records to accounts - check each map to each if each account drove it
            //// split accounts to single-map / multi-map
            //List<string> singleRecordAccountIds = new();
            //HashSet<string> singleRecordMapIds = new();
            //List<string> multiRecordAccountIds = new();
            //foreach (KeyValuePair<string, Accounts.Account> account in accounts) {
            //    List<Records.Record> accountRecords = new();
            //    foreach (MyMap map in Storage.myMaps) {
            //        string key = $"{map.mapId},{account.Value.accountId}";
            //        if (records.TryGetValue(key, out Records.Record value))
            //            accountRecords.Add(value);
            //    }
            //    account.Value.records = accountRecords.ToArray();
            //    if (account.Value.records.Length == 1) {
            //        singleRecordAccountIds.Add(account.Value.accountId);
            //        singleRecordMapIds.Add(account.Value.records[0].mapId);
            //    }
            //    else
            //        multiRecordAccountIds.Add(account.Value.accountId);
            //}

            //Status = "getting more record info...";
            //string[][] singleRecordAccountGroups = Accounts.SplitAccountsToGroups(singleRecordAccountIds.ToArray(), singleRecordMapIds.Count);
            //records = await Records.GetMoreRecordInfo(singleRecordAccountGroups, singleRecordMapIds.ToArray(), records);
            //string[][] multiRecordAccountGroups = Accounts.SplitAccountsToGroups(multiRecordAccountIds.ToArray(), Storage.myMaps.Length);
            //records = await Records.GetMoreRecordInfo(multiRecordAccountGroups, mapIds.ToArray(), records);

            //Storage.myMapsRecentRecords = Records.SortRecords(records);
            Status = $"updated: {Various.Now()}";
        }
    }
}