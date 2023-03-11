// c 2023-01-13
// m 2023-03-09

namespace TMT.Core {
    public class Records {
        private record _Tops(JsonElement tops);
        private record _Top(JsonElement top);
        public class Record {
            public string accountId { get; set; }
            public string accountName { get; set; }
            [JsonPropertyName("url")] public Uri ghostUrl { get; set; }
            public string mapId { get; set; }
            public string mapName { get; set; }
            public string mapUid { get; set; }
            public int medal { get; set; }
            public int position { get; set; }
            [JsonPropertyName("mapRecordId")] public string recordId { get; set; }
            public object recordScore { get; set; }
            [JsonPropertyName("score")] public float time { get; set; }
            [JsonPropertyName("timestamp")] public string timestampIsoUtc { get; set; }
            public int timestampUnix { get; set; }
            public string zoneId { get; set; }
            public string zoneName { get; set; }
        }


        public static async Task<Dictionary<string, Record>> GetMoreRecordInfo(string[][] accountGroups, string[] mapIds, Dictionary<string, Record> records) {
            foreach (string[] group in accountGroups) {
                Dictionary<string, Record> recordsLookup = await GetMultiMapRecords(group, mapIds);
                foreach (KeyValuePair<string, Record> record in recordsLookup) {
                    try {
                        records[record.Key].ghostUrl = record.Value.ghostUrl;
                        records[record.Key].recordId = record.Value.recordId;
                        records[record.Key].timestampIsoUtc = record.Value.timestampIsoUtc;
                        records[record.Key].timestampUnix = Various.IsoToUnix(record.Value.timestampIsoUtc);
                    } catch {}
                }
            }
            return records;
        }

        // using L1
        public static async Task<Dictionary<string, Record>> GetMultiMapRecords(string[] accountIds, string[] mapIds) {
            // Maximum 207 total IDs (account + map)
            HttpClient[] clients = await Auth.GetClients();

            string accountIdsString = string.Join("%2C", accountIds);
            string mapIdsString = string.Join("%2C", mapIds);
            Various.ApiWait();
            using HttpResponseMessage response = await clients[0].GetAsync($"mapRecords/?accountIdList={accountIdsString}&mapIdList={mapIdsString}");
            string responseString = await response.Content.ReadAsStringAsync();
            Record[] records = JsonSerializer.Deserialize<Record[]>(responseString);

            Dictionary<string, Record> recordsLookup = new();
            foreach (Record record in records) {
                record.timestampIsoUtc = record.timestampIsoUtc.Replace("+00:00", "Z");
                recordsLookup.Add($"{record.mapId},{record.accountId}", record);
            }

            return recordsLookup;
        }

        // using L2
        public static async Task<Record[]> GetSingleMapRecords(string mapUid, int count = 100) {
            HttpClient[] clients = await Auth.GetClients();
            Dictionary<string, string> zones = await Zones.Get();

            string url = $"api/token/leaderboard/group/Personal_Best/map/{mapUid}/top";
            url += $"?audience=NadeoLiveServices&length={count}&onlyWorld=True";
            Various.ApiWait();
            using HttpResponseMessage response = await clients[1].GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            JsonElement tops = JsonSerializer.Deserialize<_Tops>(responseString).tops[0];
            JsonElement top = JsonSerializer.Deserialize<_Top>(tops).top;
            Record[] records = JsonSerializer.Deserialize<Record[]>(top);
            foreach (Record record in records) {
                record.mapUid = mapUid;
                record.time /= 1000;
                record.zoneName = zones[record.zoneId];
            }

            return records;
        }

        public static Record[] SortRecords(Dictionary<string, Record> records) {
            var sortedRecordsEnum = from entry in records orderby entry.Value.timestampUnix descending select entry.Value;
            return sortedRecordsEnum.ToArray();
        }
    }
}