// c 2023-01-10
// m 2023-03-25

namespace TMT.Core {
    class Auth {
        private record class _Ticket(string ticket);
        private record class _Token(string accessToken);

        static HttpClient[] _clients;
        public static async Task<HttpClient[]> GetClients() {
            if (_clients != null)
                return _clients;

            using HttpClient _client0 = new();
            HttpClient client1 = new();
            HttpClient client2 = new();

            _client0.BaseAddress = new Uri("https://public-ubiservices.ubi.com/v3/profiles/sessions/");
            client1.BaseAddress = new Uri("https://prod.trackmania.core.nadeo.online/");
            client2.BaseAddress = new Uri("https://live-services.trackmania.nadeo.live/");

            // using credentials to get L0
            string basic = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Config.api.username}:{Config.api.password}"));
            _client0.DefaultRequestHeaders.Add("Authorization", basic);
            _client0.DefaultRequestHeaders.Add("Ubi-AppId", "86263886-327a-4328-ac69-527f0d20a237");
            _client0.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"{Config.api.agent} / {Config.api.username}");
            using HttpResponseMessage response = await _client0.PostAsync("", new StringContent("", Encoding.UTF8, "application/json"));
            string responseString = await response.Content.ReadAsStringAsync();
            string token0 = "ubi_v1 t=" + JsonSerializer.Deserialize<_Ticket>(responseString).ticket;

            // using L0 to get L1
            client1.DefaultRequestHeaders.Add("Authorization", token0);
            using HttpResponseMessage response1 = await client1.PostAsync(
                "v2/authentication/token/ubiservices",
                new StringContent("", Encoding.UTF8, "application/json")
            );
            responseString = await response1.Content.ReadAsStringAsync();
            string token1 = "nadeo_v1 t=" + JsonSerializer.Deserialize<_Token>(responseString).accessToken;

            // using L1 to get L2
            client1.DefaultRequestHeaders.Clear();
            client1.DefaultRequestHeaders.Add("Authorization", token1);
            using HttpResponseMessage response2 = await client1.PostAsync(
                "v2/authentication/token/nadeoservices",
                new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("audience", "NadeoLiveServices") })
            );
            responseString = await response2.Content.ReadAsStringAsync();
            string token2 = "nadeo_v1 t=" + JsonSerializer.Deserialize<_Token>(responseString).accessToken;

            client2.DefaultRequestHeaders.Add("Authorization", token2);

            _clients = new[] { client1, client2 };
            return _clients;
        }
    }
}