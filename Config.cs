// c 2023-01-15
// m 2023-03-04

namespace TMT {
    class Config {
        public class Settings {
            public string accountID { get; set; }
            public SettingsAPI api { get; set; }
            public SettingsMyMaps myMaps { get; set; }
        }

        public class SettingsAPI {
            public string agent { get; set; }
            public string password { get; set; }
            public string username { get; set; }
            public int waitMilliseconds { get; set; }
        }

        public class SettingsMyMaps {
            public List<string> ignoreMapIDs { get; set; }
            public bool ignoreRegionContinent { get; set; }
            public bool ignoreRegionWorld { get; set; }
        }


        public static string accountID;
        public static SettingsAPI api;
        public static string cacheDir = FileSystem.Current.CacheDirectory;
        public static string configFile;
        public static Settings defaultConfig;
        public static bool freshConfig;
        public static SettingsMyMaps myMaps;

#if ANDROID
        public static string appDir = "/storage/emulated/0/Android/data/com.ezio416.tmtracker";
        public static string os = "android";
#endif
#if WINDOWS
        public static string appDir = FileSystem.Current.AppDataDirectory;
        public static string os = "windows";
#endif


        static bool _init;
        public static async Task<bool> Init() {
            //static string DumpJson(Settings content) { return System.Text.Json.JsonSerializer.Serialize(content); }
            static Settings LoadJson(string content) { return System.Text.Json.JsonSerializer.Deserialize<Settings>(content); }
            if (!_init) {
                string internalFile = "appsettings.json";
                configFile = Path.Combine(appDir, internalFile);

                using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync(internalFile);
                using StreamReader reader = new(stream);
                string defaultContent = await reader.ReadToEndAsync();
                defaultConfig = LoadJson(defaultContent);

                if (!File.Exists(configFile)) {
                    freshConfig = true;
                    using FileStream outputStream = File.OpenWrite(configFile);
                    using StreamWriter streamWriter = new(outputStream);
                    await streamWriter.WriteAsync(defaultContent);
                    if (!File.Exists(configFile)) throw new FileNotFoundException("appsettings.json failed to write");
                }

                // defaultConfig should be cloned instead of reading from the new file
                Settings _config = LoadJson(File.ReadAllText(configFile));
                accountID = _config.accountID;
                api = _config.api;
                myMaps = _config.myMaps;

                _init = true;
            }
            return true;
        }

        //public static async Task<bool> Write() {
        //    return false;
        //}
    }
}