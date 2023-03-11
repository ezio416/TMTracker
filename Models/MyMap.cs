// c 2023-03-09
// m 2023-03-09

namespace TMT.Models {
    public class MyMap : Map {
        public override float authorTime { get; set; }
        public override float bronzeTime { get; set; }
        public override Uri downloadUrl { get; set; }
        public override float goldTime { get; set; }
        [JsonPropertyName("uid")] public override string mapUid { get; set; }
        public override float silverTime { get; set; }
        public override string uploadedIsoUtc { get; set; }
    }
}