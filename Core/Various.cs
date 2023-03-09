// c 2023-01-12
// m 2023-03-08

namespace TMT.Core {
    class Various {
        public static void ApiWait() { Thread.Sleep(Config.api.waitMilliseconds); }

        public static bool IsCharHex(char ch) {
            if ('0' <= ch && ch <= '9' || 'A' <= ch && ch <= 'Z' || 'a' <= ch && ch <= 'z')
                return true;
            return false;
        }

        public static int IsoToUnix(string isoTimestamp) {
            DateTime date = DateTime.ParseExact(isoTimestamp, "yyyy-MM-dd'T'HH:mm:ss'Z'", null, System.Globalization.DateTimeStyles.RoundtripKind);
            return (int)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static void Log(string msg = "", string category = "misc", string file = "tmt.log") {
            category = category.ToUpper();
            using StreamWriter sw = File.AppendText($"{Config.dirLogs}/{file}");
            sw.WriteLine($"{DateTime.Now} {TimeZoneInfo.Local.ToString().Substring(4, 6)}  {category}  {msg}");
        }

        public static string UnixToIso(double unixTimestamp) {
            DateTime date = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return date.AddSeconds(unixTimestamp).ToUniversalTime().ToString("u").Replace(" ", "T");
        }
    }
}