// c 2023-01-13
// m 2023-03-26

namespace TMT.Core {
    class Accounts {
        class _ClubTag {
            public string accountId { get; set; }
            public string clubTag { get; set; }
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
                using HttpResponseMessage response = await clients[0].GetAsync($"accounts/displayNames/?accountIdList={group}");
                string responseString = await response.Content.ReadAsStringAsync();
                Account[] responseAccounts = JsonSerializer.Deserialize<Account[]>(responseString);
                foreach (Account account in responseAccounts) {
                    account.timestampIsoUtc = account.timestampIsoUtc.Replace("+00:00", "Z");
                    account.timestampUnix = Various.IsoToUnix(account.timestampIsoUtc);
                    accounts.Add(account.accountId, account);
                }

                using HttpResponseMessage responseClub = await clients[0].GetAsync($"accounts/clubTags/?accountIdList={group}");
                string responseClubString = await responseClub.Content.ReadAsStringAsync();
                _ClubTag[] tags = JsonSerializer.Deserialize<_ClubTag[]>(responseClubString);
                foreach (_ClubTag tag in tags)
                    accounts[tag.accountId].clubTag = new StyledString(tag.clubTag);
            }

            return accounts;
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