// c 2023-01-13
// m 2023-03-11

namespace TMT.Core {
    class Accounts {
        public class Account {
            public string accountId { get; set; }
            [JsonPropertyName("displayName")] public string accountName { get; set; }
            public ClubTag clubTag { get; set; }
            public Record[] records { get; set; }
            [JsonPropertyName("timestamp")] public string timestampIsoUtc { get; set; }
            public int timestampUnix { get; set; }
        }
        public class ClubTag {
            public string accountId { get; set; }
            public ClubTagLetter[] clubTagLetters { get; set; }
            [JsonPropertyName("clubTag")] public string clubTagRaw { get; set; }
            public string clubTagText { get; set; }
            [JsonPropertyName("timestamp")] public string timestampIsoUtc { get; set; }
            public int timestampUnix { get; set; }
        }
        public class ClubTagLetter {
            public string color3 { get; set; }
            public string color6 { get; set; }
            public bool isBold { get; set; }
            public bool isItalic { get; set; }
            public bool isNarrow { get; set; }
            public bool isShadow { get; set; }
            public bool isWide { get; set; }
            public char val { get; set; }
        }


        // using L1
        public static async Task<Dictionary<string, Account>> GetAccounts(string[] accountIds) {
            const int accountLimit = 50;
            Dictionary<string, Account> accounts = new();
            List<string> groups = new();

            HttpClient[] clients = await Auth.GetClients();

            while (true) {
                if (accountIds.Length <= accountLimit) {
                    groups.Add(string.Join("%2C", accountIds));
                    break;
                }
                groups.Add(string.Join("%2C", accountIds.Take(accountLimit)));
                accountIds = accountIds.Skip(accountLimit).ToArray();
            }

            foreach (string group in groups) {
                Various.ApiWait();
                using HttpResponseMessage response = await clients[0].GetAsync($"accounts/displayNames/?accountIdList={group}");
                string responseString = await response.Content.ReadAsStringAsync();
                Account[] responseAccounts = JsonSerializer.Deserialize<Account[]>(responseString);
                foreach (Account account in responseAccounts) {
                    account.timestampIsoUtc = account.timestampIsoUtc.Replace("+00:00", "Z");
                    account.timestampUnix = Various.IsoToUnix(account.timestampIsoUtc);
                    accounts.Add(account.accountId, account);
                }

                Various.ApiWait();
                using HttpResponseMessage responseClub = await clients[0].GetAsync($"accounts/clubTags/?accountIdList={group}");
                string responseClubString = await responseClub.Content.ReadAsStringAsync();
                ClubTag[] clubTags = JsonSerializer.Deserialize<ClubTag[]>(responseClubString);
                foreach (ClubTag clubTag in clubTags) {
                    List<char> letters = new();
                    clubTag.clubTagLetters = ParseClubTag(clubTag.clubTagRaw);
                    foreach (ClubTagLetter clubTagLetter in clubTag.clubTagLetters)
                        letters.Add(clubTagLetter.val);
                    clubTag.clubTagText = new string(letters.ToArray());
                    clubTag.timestampIsoUtc = clubTag.timestampIsoUtc.Replace("+00:00", "Z");
                    clubTag.timestampUnix = Various.IsoToUnix(clubTag.timestampIsoUtc);
                    if (accounts.TryGetValue(clubTag.accountId, out Account value))
                        value.clubTag = clubTag;
                }
            }

            return accounts;
        }

        public static ClubTagLetter[] ParseClubTag(string clubTagRaw) {
            bool isBold = false;
            List<char> code = new();
            string color3 = "FFF";
            int isColorCountdown = 0;
            bool isFlag = false;
            bool isVal = true;
            bool isItalic = false;
            bool isNarrow = false;
            bool isShadow = false;
            List<ClubTagLetter> letters = new();
            bool isWide = false;

            foreach (char ch in clubTagRaw) {
                bool isCharHex = Various.IsCharHex(ch);
                if (ch == '$') {
                    isFlag = true;
                    isVal = false;
                    continue;
                }
                if (isColorCountdown > 0) {
                    if (isCharHex) {
                        code.Add(ch);
                        isColorCountdown -= 1;
                        if (isColorCountdown > 0)
                            isFlag = false;
                        continue;
                    } else {
                        for (int i = 0; i < 3 - code.Count; i++)
                            code.Add('0');
                        isColorCountdown = 0;
                        isFlag = false;
                    }
                }
                if (isFlag) {
                    switch (ch) {
                        case 'G':  // reset color
                            color3 = "FFF";
                            break;
                        case 'I':
                            isItalic = true;
                            break;
                        case 'L':  // clickable link
                            continue;
                        case 'N':
                            isNarrow = true;
                            break;
                        case 'O':
                            isBold = true;
                            break;
                        case 'S':
                            isShadow = true;
                            isFlag = false;
                            break;
                        case 'T': // uppercase
                            continue;
                        case 'W':
                            isWide = true;
                            break;
                        case 'Z':  // reset styling
                            isBold = false;
                            isItalic = false;
                            isNarrow = false;
                            isShadow = false;
                            isWide = false;
                            break;
                        case '$':  // escaped '$'
                            isVal = true;
                            break;
                        default:
                            isFlag = false;
                            if (isCharHex) {
                                if (code.Count == 0)
                                    isColorCountdown += 2;
                                code.Add(ch);
                                isFlag = true;
                            }
                            break;
                    }
                }
                if (code.Count == 3) {
                    isVal = true;
                    color3 = new string(code.ToArray());
                    code.Clear();
                }
                if (isVal)
                    letters.Add(new ClubTagLetter {
                        color3 = color3,
                        isBold = isBold,
                        isItalic = isItalic,
                        isNarrow = isNarrow,
                        isShadow = isShadow,
                        isWide = isWide,
                        val = ch
                    });
            }

            return letters.ToArray();
        }

        public static string[][] SplitAccountsToGroups(string[] accountIds, int mapCount) {
            int maximumAccounts = 207 - mapCount;
            List<string[]> groups = new();
            while (true) {
                if (accountIds.Length <= maximumAccounts) {
                    groups.Add(accountIds);
                    return groups.ToArray();
                }
                groups.Add(accountIds.Take(maximumAccounts).ToArray());
                accountIds = accountIds.Skip(maximumAccounts).ToArray();
            }
        }
    }
}