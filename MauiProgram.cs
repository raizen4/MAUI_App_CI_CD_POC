using CommunityToolkit.Maui;
using DotNet.Meteor.HotReload.Plugin;
using MauiAppCiCd.ViewModels;
using MauiAppCiCd.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif
namespace MauiAppCiCd;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
#if DEBUG
            .EnableHotReload()
#endif
            .UseMauiCommunityToolkit()
            .UseMauiMicroMvvm<AppShell>(
                "Resources/Styles/Colors.xaml",
                "Resources/Styles/Styles.xaml")
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterViewModels()
            .RegisterFirebaseServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((_,__) => {
                CrossFirebase.Initialize();
                return false;
            }));
#elif ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) =>
                CrossFirebase.Initialize(activity)));
#endif
        });
        
        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
        return builder;
    }
    
    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.MapView<MainPage, MainPageViewModel>();
        
        //Register popups
        builder.Services.AddTransientPopup<PopupTestPage, PopupTestPageViewModel>();

        return builder;
    }
}