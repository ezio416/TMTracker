// c 2023-03-09
// m 2023-03-26

namespace TMT.Models {
    public class CampaignMap : Map {
        [JsonPropertyName("authorScore")]
        public override float  authorTime     { get; set; }
        [JsonPropertyName("bronzeScore")]
        public override float  bronzeTime     { get; set; }
        [JsonPropertyName("fileUrl")]
        public override Uri    downloadUrl    { get; set; }
        [JsonPropertyName("goldScore")]
        public override float  goldTime       { get; set; }
        public override string mapUid         { get; set; }
        [JsonPropertyName("silverScore")]
        public override float  silverTime     { get; set; }
        [JsonPropertyName("timestamp")]
        public override string uploadedIsoUtc { get; set; }
    }
}