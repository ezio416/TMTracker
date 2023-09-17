// c 2023-03-04
// m 2023-07-04

using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace TMT.ViewModels;

public partial class MyMapsViewModel : ObservableObject {
    [ObservableProperty]
    string status = Various.Now();

    [ObservableProperty]
    string mapCountLabelText = "map count: ";

    [ObservableProperty]
    bool refreshRecordsEnabled = false;

    [ObservableProperty]
    bool viewRecordsEnabled = false;

    int i = 0;
    readonly HashSet<string> accountIds = new();
    Dictionary<string, Account> accounts;
    readonly HashSet<string> mapIds = new();
    Account myAccount;
    Dictionary<string, Record> records = new();

    public ObservableCollection<MyMap> MyMaps { get; set; } = new();
    public ObservableCollection<Record> RecentRecords { get; set; } = new();

    [RelayCommand]
    async Task RefreshMaps() {
        Status = $"getting my maps...";
        Storage.myMaps = await Maps.GetMyMaps();
        MapCountLabelText = $"map count: {Storage.myMaps.Count}";

        MyMaps.Clear();
        foreach (MyMap map in Storage.myMaps)
            MyMaps.Add(map);

        RefreshRecordsEnabled = true;

        Various.Log("refreshed my maps", "myma");
        Status = $"updated: {Various.Now()}";
    }

    [RelayCommand]
    async Task RefreshRecords() {
        foreach (MyMap map in Storage.myMaps) {
            Status = $"({Storage.myMaps.Count - i++}) getting records: {map.mapName.text}";
            mapIds.Add(map.mapId);
            map.records = await Records.GetSingleMapRecords(map.mapUid);
            map.accountIds = new();

            foreach (Record record in map.records) {
                map.accountIds.Add(record.accountId);
                record.mapId = map.mapId;
                record.mapName = map.mapName;
                accountIds.Add(record.accountId);
                records.Add($"{record.mapId},{record.accountId}", record);
            }

            if (map.records.Count == 0)
                continue;

            records = await Records.GetMoreRecordInfo(map.accountIds.ToArray(), map.mapId, records);
        }

        Status = "getting account info...";
        accounts = await Accounts.GetAccounts(accountIds.ToArray());
        myAccount = accounts[Storage.myMaps[0].authorId];
        foreach (MyMap map in Storage.myMaps)
            map.authorName = myAccount.accountName;
        foreach (KeyValuePair<string, Record> record in records)
            record.Value.accountName = accounts[record.Value.accountId].accountName;

        // add records to accounts - check each map to each if each account drove it
        // split accounts to single-map / multi-map
        //List<string> singleRecordAccountIds = new();
        //HashSet<string> singleRecordMapIds = new();
        //List<string> multiRecordAccountIds = new();
        //foreach (KeyValuePair<string, Account> account in accounts) {
        //    List<Record> accountRecords = new();
        //    foreach (MyMap map in Storage.myMaps) {
        //        string key = $"{map.mapId},{account.Value.accountId}";
        //        if (records.TryGetValue(key, out Record value))
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
        //string[][] multiRecordAccountGroups = Accounts.SplitAccountsToGroups(multiRecordAccountIds.ToArray(), Storage.myMaps.Count);
        //records = await Records.GetMoreRecordInfo(multiRecordAccountGroups, mapIds.ToArray(), records);

        Storage.myMapsRecentRecords = Records.SortRecords(records);
        RecentRecords.Clear();
        foreach (Record record in Storage.myMapsRecentRecords)
            RecentRecords.Add(record);

        ViewRecordsEnabled = true;

        Various.Log("refreshed records", "myma");
        Status = $"updated: {Various.Now()}";
    }

    [RelayCommand]
    async Task ViewMapPage(MyMap mapp) {
        await Shell.Current.GoToAsync(
            nameof(MapPage),
            new Dictionary<string, object> {
                { "Mapp", mapp }
            }
        );
    }

    [RelayCommand]
    async Task ViewRecordsPage() {
        await Shell.Current.GoToAsync(nameof(RecentRecordsPage));
    }
}