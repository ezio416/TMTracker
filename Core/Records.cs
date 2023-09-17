// c 2023-01-13
// m 2023-07-05

namespace TMT.Core;

public class Records {
    record _Tops(JsonElement tops);
    record _Top(JsonElement top);

    public static async Task<Dictionary<string, Record>> GetMoreRecordInfo(string[] accountIds, string mapId, Dictionary<string, Record> records) {
        Dictionary<string, Record> recordsLookup = await GetMultiMapRecords(accountIds, mapId);
        foreach (KeyValuePair<string, Record> record in recordsLookup) {
            try {
                records[record.Key].ghostUrl = record.Value.ghostUrl;
                records[record.Key].recordId = record.Value.recordId;
                records[record.Key].timestampIsoUtc = record.Value.timestampIsoUtc;
                records[record.Key].timestampUnix = Various.IsoToUnix(record.Value.timestampIsoUtc);
            } catch {}
        }
        return records;
    }

    // using L1
    public static async Task<Dictionary<string, Record>> GetMultiMapRecords(string[] accountIds, string mapId) {
        // Maximum 207 total IDs (account + map)
        HttpClient[] clients = await Auth.GetClients();

        string accountIdsString = string.Join("%2C", accountIds);
        using HttpResponseMessage response = await clients[0].GetAsync($"mapRecords/?accountIdList={accountIdsString}&mapIdList={mapId}");
        string responseString = await response.Content.ReadAsStringAsync();
        List<Record> records = JsonSerializer.Deserialize<List<Record>>(responseString);

        Dictionary<string, Record> recordsLookup = new();
        foreach (Record record in records) {
            record.timeFormatted = Various.FormatSeconds(record.time);
            record.timestampIsoUtc = record.timestampIsoUtc.Replace("+00:00", "Z");
            recordsLookup.Add($"{record.mapId},{record.accountId}", record);
        }

        return recordsLookup;
    }

    // using L2
    public static async Task<List<Record>> GetSingleMapRecords(string mapUid, int count = 100) {
        HttpClient[] clients = await Auth.GetClients();
        Dictionary<string, string> zones = await Zones.Get();

        string url = $"api/token/leaderboard/group/Personal_Best/map/{mapUid}/top";
        url += $"?audience=NadeoLiveServices&length={count}&onlyWorld=True";
        using HttpResponseMessage response = await clients[1].GetAsync(url);
        string responseString = await response.Content.ReadAsStringAsync();
        JsonElement tops = JsonSerializer.Deserialize<_Tops>(responseString).tops[0];
        JsonElement top = JsonSerializer.Deserialize<_Top>(tops).top;
        List<Record> records = JsonSerializer.Deserialize<List<Record>>(top);
        foreach (Record record in records) {
            record.mapUid = mapUid;
            record.time /= 1000;
            record.timeFormatted = Various.FormatSeconds(record.time);
            record.zoneName = zones[record.zoneId];
        }

        return records;
    }

    public static List<Record> SortRecords(Dictionary<string, Record> records) {
        return (from entry in records orderby entry.Value.timestampUnix descending select entry.Value).ToList();
    }
}