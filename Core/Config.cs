// c 2023-01-15
// m 2023-03-08

namespace TMT.Core {
    class Config {
        public class Settings {
            public string accountId { get; set; }
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
            public string[] ignoreMapIds { get; set; }
            public bool ignoreRegionContinent { get; set; }
            public bool ignoreRegionWorld { get; set; }
        }


        public static string accountId;
        public static SettingsAPI api;
        public static string configFile;
        public static Settings defaultConfig;
        public static bool freshConfig;
        public static Settings initialSettings;
        public static SettingsMyMaps myMaps;

#if ANDROID
        public static string dirApp = "/storage/emulated/0/Android/data/com.ezio416.tmtracker";
        public static string dirCache = $"{dirApp}/cache";
        public static string dirFiles = $"{dirApp}/files";
        public static string dirLogs = $"{dirApp}/logs";
        public static string os = "android";
#elif WINDOWS
        public static string dirApp = Directory.GetParent(FileSystem.Current.AppDataDirectory).ToString();
        public static string dirCache = FileSystem.Current.CacheDirectory;
        public static string dirFiles = FileSystem.Current.AppDataDirectory;
        public static string dirLogs = $"{dirApp}/logs";
        public static string os = "windows";
#endif


        static bool _init;
        public static async void Init() {
            static Settings LoadJson(string content) { return JsonSerializer.Deserialize<Settings>(content); }

            if (!_init) {
                string internalFile = "appsettings.json";
                configFile = Path.Combine(dirFiles, internalFile);

                using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync(internalFile);
                using StreamReader reader = new(stream);
                string defaultContent = await reader.ReadToEndAsync();
                defaultConfig = LoadJson(defaultContent);

                Directory.CreateDirectory(dirCache);
                Directory.CreateDirectory(dirFiles);
                Directory.CreateDirectory(dirLogs);

                if (!File.Exists(configFile)) {
                    freshConfig = true;
                    using FileStream outputStream = File.OpenWrite(configFile);
                    using StreamWriter streamWriter = new(outputStream);
                    await streamWriter.WriteAsync(defaultContent);
                }

                Settings config = LoadJson(File.ReadAllText(configFile));
                initialSettings = config;
                accountId = config.accountId;
                api = config.api;
                myMaps = config.myMaps;

                _init = true;
            }
        }

        public static bool Write() {
            Settings config = new() { accountId = accountId, api = api, myMaps = myMaps };
            string content = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFile, content);
            Various.Log("settings updated", "cfg");
            return true;
        }
    }
}