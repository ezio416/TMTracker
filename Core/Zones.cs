// c 2023-01-13
// m 2023-03-06

namespace TMT.Core {
    class Zones {
        public class Zone {
            public string name { get; set; }
            public string parentId { get; set; }
            public string zoneId { get; set; }
        }


        // using L1
        static readonly Dictionary<string, string> _zones = new();
        public static async Task<Dictionary<string, string>> Get() {
            if (_zones.Count > 0)
                return _zones;

            int ignoreLevel = 1;
            Dictionary<string, Dictionary<string, string>> parentLookup = new();

            HttpClient[] clients = await Auth.GetClients();

            Various.ApiWait();
            using HttpResponseMessage response = await clients[0].GetAsync("zones");
            string responseString = await response.Content.ReadAsStringAsync();
            Zone[] responseZones = JsonSerializer.Deserialize<Zone[]>(responseString);

            foreach (Zone zone in responseZones)
                parentLookup.Add(
                    zone.zoneId,
                    new Dictionary<string, string>() {
                        {"name", zone.name},
                        {"parentId", zone.parentId}
                    }
                );

            foreach (KeyValuePair<string, Dictionary<string, string>> zone in new Dictionary<string, Dictionary<string, string>>(parentLookup)) {
                string zoneId = zone.Key;
                List<string> zoneParts = new() { parentLookup[zoneId]["name"] };

                string parentId = parentLookup[zoneId]["parentId"];
                if (parentId != null) {
                    zoneParts.Add(parentLookup[parentId]["name"]);
                    string g0ParentId = parentLookup[parentId]["parentId"];
                    if (g0ParentId != null) {
                        zoneParts.Add(parentLookup[g0ParentId]["name"]);
                        string g1ParentId = parentLookup[g0ParentId]["parentId"];
                        if (g1ParentId != null) {
                            zoneParts.Add(parentLookup[g1ParentId]["name"]);
                            string g2ParentId = parentLookup[g1ParentId]["parentId"];
                            if (g2ParentId != null) {
                                zoneParts.Add(parentLookup[g2ParentId]["name"]);
                                string g3ParentId = parentLookup[g2ParentId]["parentId"];
                                if (g3ParentId != null)
                                    zoneParts.Add(parentLookup[g3ParentId]["name"]);
                            }
                        }
                    }
                }
                _zones[zoneId] = string.Join(',', zoneParts.Take(zoneParts.Count - ignoreLevel));
            }
            return _zones;
        }
    }
}