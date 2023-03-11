// c 2023-02-28
// m 2023-03-11

using CommunityToolkit.Maui;

namespace TMT {
	public static class MauiProgram {
		public static MauiApp CreateMauiApp() {
            Config.Init();
			var builder = MauiApp.CreateBuilder()
				.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("CascadiaCode.ttf", "CascadiaCode");
                });
				
			builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddSingleton<MyMapsPage>();
            builder.Services.AddSingleton<MyMapsViewModel>();

            builder.Services.AddSingleton<MyRecordsPage>();
            builder.Services.AddSingleton<MyRecordsViewModel>();

            builder.Services.AddSingleton<QueryPage>();
            builder.Services.AddSingleton<QueryViewModel>();

            builder.Services.AddSingleton<RecentRecordsPage>();

            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();

            return builder.Build();
		}
    }
}