// c 2023-01-19
// m 2023-03-10

namespace TMT.Core {
    class Storage {
        #pragma warning disable 0649
        public static CampaignMap[] campaignMaps;
        public static MyMap[] myMaps;
        public static Accounts.Account[] myMapsAccounts;
        public static List<Records.Record> myMapsRecentRecords;
        #pragma warning restore 0649
    }
}