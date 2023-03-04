// c 2023-02-28
// m 2023-03-03

namespace TMT {
	public static class MauiProgram {
		public static MauiApp CreateMauiApp() {
			Config.Init();
			return MauiApp.CreateBuilder()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.Build();
		}
    }
}