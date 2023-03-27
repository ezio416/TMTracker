// c 2023-03-11
// m 2023-03-26

namespace TMT.Models {
    public class Record {
        public string       accountId       { get; set; }
        public string       accountName     { get; set; }
        [JsonPropertyName("url")]
        public Uri          ghostUrl        { get; set; }
        public string       mapId           { get; set; }
        public StyledString mapName         { get; set; }
        public string       mapUid          { get; set; }
        public int          medal           { get; set; }
        public int          position        { get; set; }
        [JsonPropertyName("mapRecordId")]
        public string       recordId        { get; set; }
        public object       recordScore     { get; set; }
        [JsonPropertyName("score")]
        public float        time            { get; set; }
        [JsonPropertyName("timestamp")]
        public string       timestampIsoUtc { get; set; }
        public int          timestampUnix   { get; set; }
        public string       zoneId          { get; set; }
        public string       zoneName        { get; set; }
    }
}