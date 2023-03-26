﻿// c 2023-03-25
// m 2023-03-26

namespace TMT.Models {
    public class Account {
        public string   accountId       { get; set; }
        [JsonPropertyName("displayName")]
        public string   accountName     { get; set; }
        public ClubTag  clubTag         { get; set; }
        public Record[] records         { get; set; }
        [JsonPropertyName("timestamp")]
        public string   timestampIsoUtc { get; set; }
        public int      timestampUnix   { get; set; }
    }
}