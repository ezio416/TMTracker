// c 2023-01-12
// m 2023-03-27

namespace TMT.Core;

class Various {
    public static string FormatSeconds(double num) {
        int Num = (int)(num * 1000);
        int thou = Num % 1000;
        int seconds = Num / 1000;
        int minutes = seconds / 60;
        seconds %= 60;
        int hours = minutes / 60;
        minutes %= 60;

        if (hours > 0)
            return $"{hours}:{minutes:D2}:{seconds:D2}.{thou:D3}";
        if (minutes > 0)
            return $"{minutes}:{seconds:D2}.{thou:D3}";
        return $"{seconds}.{thou:D3}";
    }

    public static bool IsCharHex(char ch) {
        if ('0' <= ch && ch <= '9' || 'A' <= ch && ch <= 'F' || 'a' <= ch && ch <= 'f')
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
        sw.WriteLine($"{Now(true)}  {category}  {msg}");
    }

    public static string Now(bool timezone = false) {
        string now = DateTime.Now.ToString("s").Replace("T", " ");
        if (timezone)
            now += $" {TimeZoneInfo.Local.ToString().Substring(4, 6)}";
        return now;
    }

    public static string UnixToIso(double unixTimestamp) {
        DateTime date = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return date.AddSeconds(unixTimestamp).ToUniversalTime().ToString("u").Replace(" ", "T");
    }
}