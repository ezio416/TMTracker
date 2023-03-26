// c 2023-03-25
// m 2023-03-26

namespace TMT.Models {
    public class ClubTag {
        public string       accountId       { get; set; }
        public StyledChar[] clubTagLetters  { get; set; }
        [JsonPropertyName("clubTag")]
        public string       clubTagRaw      { get; set; }
        public string       clubTagText     { get; set; }
        [JsonPropertyName("timestamp")]
        public string       timestampIsoUtc { get; set; }
        public int          timestampUnix   { get; set; }
    }
}