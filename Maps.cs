// c 2023-01-12
// m 2023-03-04

using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMT {
    class Maps {
        record _CampaignList(JsonElement campaignList);
        record _CampaignRecord(int time);
        record _MapList(JsonElement mapList);
        public abstract class Map {
            public string authorName { get; set; }
            [JsonPropertyName("author")] public string authorId { get; set; }
            public abstract float authorTime { get; set; }
            public abstract float bronzeTime { get; set; }
            public abstract Uri downloadUrl { get; set; }
            public abstract float goldTime { get; set; }
            public string mapId { get; set; }
            [JsonPropertyName("name")] public string mapName { get; set; }
            [JsonPropertyName("uid")] public abstract string mapUid { get; set; }
            [JsonPropertyName("medal")] public int personalMedal { get; set; }
            public float personalTime { get; set; }
            public Records.Record[] records { get; set; }
            public abstract float silverTime { get; set; }
            public Uri thumbnailUrl { get; set; }
            public abstract string uploadedIsoUtc { get; set; }
            [JsonPropertyName("uploadTimestamp")] public int uploadedUnix { get; set; }
        }
        public class CampaignMap : Map {
            [JsonPropertyName("authorScore")] public override float authorTime { get; set; }
            [JsonPropertyName("bronzeScore")] public override float bronzeTime { get; set; }
            [JsonPropertyName("fileUrl")] public override Uri downloadUrl { get; set; }
            [JsonPropertyName("goldScore")] public override float goldTime { get; set; }
            public override string mapUid { get; set; }
            [JsonPropertyName("silverScore")] public override float silverTime { get; set; }
            [JsonPropertyName("timestamp")] public override string uploadedIsoUtc { get; set; }
        }
        public class MyMap : Map {
            public override float authorTime { get; set; }
            public override float bronzeTime { get; set; }
            public override Uri downloadUrl { get; set; }
            public override float goldTime { get; set; }
            [JsonPropertyName("uid")] public override string mapUid { get; set; }
            public override float silverTime { get; set; }
            public override string uploadedIsoUtc { get; set; }
        }
        record _Campaign(JsonElement playlist) { public CampaignMap[] maps { get; set; } }


        // using L1+2
        public static async Task<CampaignMap[]> GetCampaignMaps() {
            const int maximumIdsUids = 200;
            List<string> ids = new();
            List<string> idGroups = new();
            List<string> uidGroups = new();

            HttpClient[] clients = await Auth.GetClients();

            string[] uids = await GetCampaignUids();
            string[] uidsLeft = new string[uids.Length];
            Array.Copy(uids, uidsLeft, uids.Length);
            while (true) {
                if (uidsLeft.Length <= maximumIdsUids) {
                    uidGroups.Add(string.Join("%2C", uidsLeft));
                    break;
                }
                uidGroups.Add(string.Join("%2C", uidsLeft.Take(maximumIdsUids)));
                uidsLeft = uidsLeft.Skip(maximumIdsUids).Take(uidsLeft.Length - maximumIdsUids).ToArray();
            }

            Dictionary<string, CampaignMap> mapsById = new();
            Dictionary<string, CampaignMap> mapsByUid = new();
            foreach (string uid in uids)
                mapsByUid.Add(uid, new CampaignMap());

            foreach (string group in uidGroups) {
                Various.ApiWait();
                using HttpResponseMessage response = await clients[0].GetAsync($"maps/?mapUidList={group}");
                string responseString = await response.Content.ReadAsStringAsync();
                CampaignMap[] maps = JsonSerializer.Deserialize<CampaignMap[]>(responseString);
                foreach (CampaignMap map in maps) {
                    map.authorName = "Nadeo";
                    map.authorTime /= 1000;
                    map.bronzeTime /= 1000;
                    map.goldTime /= 1000;
                    map.silverTime /= 1000;
                    map.uploadedIsoUtc = map.uploadedIsoUtc.Replace("+00:00", "Z");
                    map.uploadedUnix = Various.IsoToUnix(map.uploadedIsoUtc);
                    mapsById[map.mapId] = mapsByUid[map.mapUid] = map;
                    ids.Add(map.mapId);
                }
            }

            while (true) {
                if (ids.Count <= maximumIdsUids) {
                    idGroups.Add(string.Join("%2C", ids));
                    break;
                }
                idGroups.Add(string.Join("%2C", ids.Take(maximumIdsUids)));
                ids = ids.Skip(maximumIdsUids).Take(ids.Count - maximumIdsUids).ToList();
            }

            foreach (string group in idGroups) {
                Various.ApiWait();
                using HttpResponseMessage response = await clients[0].GetAsync($"mapRecords/?accountIdList={Config.accountId}&mapIdList={group}");
                string responseString = await response.Content.ReadAsStringAsync();
                Records.Record[] records = JsonSerializer.Deserialize<Records.Record[]>(responseString);
                foreach (Records.Record record in records) {
                    mapsById[record.mapId].personalMedal = record.medal;
                    mapsById[record.mapId].personalTime = (float)JsonSerializer.Deserialize<_CampaignRecord>(record.recordScore.ToString()).time / 1000;
                }
            }

            List<CampaignMap> mapsInOrder = new();
            foreach (string uid in uids)
                mapsInOrder.Add(mapsByUid[uid]);

            return mapsInOrder.ToArray();
        }

        // using L2
        public static async Task<string[]> GetCampaignUids() {
            HttpClient[] clients = await Auth.GetClients();

            Various.ApiWait();
            using HttpResponseMessage response = await clients[1].GetAsync("api/token/campaign/official?length=1000&offset=0");
            string responseString = await response.Content.ReadAsStringAsync();
            JsonElement maps = JsonSerializer.Deserialize<_CampaignList>(responseString).campaignList;
            _Campaign[] campaigns = JsonSerializer.Deserialize<_Campaign[]>(maps);

            foreach (_Campaign campaign in campaigns)
                campaign.maps = JsonSerializer.Deserialize<CampaignMap[]>(campaign.playlist);

            List<string> uids = new();
            for (int i = campaigns.Length - 1; i >= 0; i--)
                foreach (CampaignMap map in campaigns[i].maps)
                    uids.Add(map.mapUid);

            return uids.ToArray();
        }

        // using L2
        public static async Task<MyMap[]> GetMyMaps(int count = 1000) {
            HttpClient[] clients = await Auth.GetClients();

            Various.ApiWait();
            using HttpResponseMessage response = await clients[1].GetAsync($"api/token/map?length={count}");
            string responseString = await response.Content.ReadAsStringAsync();
            JsonElement maps = JsonSerializer.Deserialize<_MapList>(responseString).mapList;
            MyMap[] myMaps = JsonSerializer.Deserialize<MyMap[]>(maps);

            foreach (MyMap map in myMaps) {
                map.authorTime /= 1000;
                map.bronzeTime /= 1000;
                map.goldTime /= 1000;
                map.silverTime /= 1000;
                map.uploadedIsoUtc = Various.UnixToIso(map.uploadedUnix);
            }

            return myMaps;
        }
    }
}