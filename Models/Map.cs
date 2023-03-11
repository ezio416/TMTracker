// c 2023-03-09
// m 2023-03-11

namespace TMT.Models {
    public abstract class Map {
        public string authorName { get; set; }
        [JsonPropertyName("author")] public string authorId { get; set; }
        public abstract float authorTime { get; set; }
        public bool badUploadTime { get; set; }
        public abstract float bronzeTime { get; set; }
        public abstract Uri downloadUrl { get; set; }
        public abstract float goldTime { get; set; }
        public string mapId { get; set; }
        [JsonPropertyName("name")] public string mapName { get; set; }
        [JsonPropertyName("uid")] public abstract string mapUid { get; set; }
        [JsonPropertyName("medal")] public int personalMedal { get; set; }
        public float personalTime { get; set; }
        public List<Record> records { get; set; }
        public abstract float silverTime { get; set; }
        public Uri thumbnailUrl { get; set; }
        [JsonPropertyName("updateTimestamp")] public int updatedUnix { get; set; }
        public abstract string uploadedIsoUtc { get; set; }
        [JsonPropertyName("uploadTimestamp")] public int uploadedUnix { get; set; }
    }
}