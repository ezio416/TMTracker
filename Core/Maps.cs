// c 2023-01-12
// m 2023-03-27

namespace TMT.Core;

class Maps {
    record _Campaign(JsonElement playlist) { public CampaignMap[] maps { get; set; } }
    record _CampaignList(JsonElement campaignList);
    record _CampaignRecord(int time);
    record _MapList(JsonElement mapList);

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
            using HttpResponseMessage response = await clients[0].GetAsync($"mapRecords/?accountIdList={Config.accountId}&mapIdList={group}");
            string responseString = await response.Content.ReadAsStringAsync();
            Record[] records = JsonSerializer.Deserialize<Record[]>(responseString);
            foreach (Record record in records) {
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

        using HttpResponseMessage response = await clients[1].GetAsync("api/token/campaign/official?length=1000&offset=0");
        string responseString = await response.Content.ReadAsStringAsync();
        _Campaign[] campaigns = JsonSerializer.Deserialize<_Campaign[]>(JsonSerializer.Deserialize<_CampaignList>(responseString).campaignList);

        foreach (_Campaign campaign in campaigns)
            campaign.maps = JsonSerializer.Deserialize<CampaignMap[]>(campaign.playlist);

        List<string> uids = new();
        for (int i = campaigns.Length; i > 0; i--)
            foreach (CampaignMap map in campaigns[i].maps)
                uids.Add(map.mapUid);

        return uids.ToArray();
    }

    // using L2
    public static async Task<MyMap[]> GetMyMaps(int count = 1000) {
        HttpClient[] clients = await Auth.GetClients();

        using HttpResponseMessage response = await clients[1].GetAsync($"api/token/map?length={count}");
        string responseString = await response.Content.ReadAsStringAsync();
        List<MyMap> myMaps = JsonSerializer.Deserialize<List<MyMap>>(JsonSerializer.Deserialize<_MapList>(responseString).mapList);

        foreach (MyMap map in myMaps) {
            if (map.uploadedUnix < 1_600_000_000) {
                map.badUploadTime = true;
                map.uploadedUnix = map.updatedUnix;
            }
            map.authorTime /= 1000;
            map.bronzeTime /= 1000;
            map.goldTime /= 1000;
            map.mapName = new StyledString(map.mapNameRaw);
            map.silverTime /= 1000;
            map.uploadedIsoUtc = Various.UnixToIso(map.uploadedUnix);
        }

        myMaps.RemoveAll(map => Config.myMaps.ignoreMapIds.Contains(map.mapId));

        return (from entry in myMaps orderby entry.uploadedUnix descending select entry).ToArray();
    }
}