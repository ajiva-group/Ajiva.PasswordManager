﻿using Microsoft.Maui.LifecycleEvents;
using Ajiva.PasswordManager.Ui.Maui.Services;

namespace Ajiva.PasswordManager.Ui.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                });
            builder.ConfigureLifecycleEvents(lifecycle => {
#if WINDOWS
        lifecycle
            .AddWindows(windows =>
                windows.OnNativeMessage((app, args) => {
                    if (WindowExtensions.Hwnd == IntPtr.Zero)
                    {
                        WindowExtensions.Hwnd = args.Hwnd;
                        WindowExtensions.SetIcon("Platforms/Windows/trayicon.ico");
                    }
                    app.ExtendsContentIntoTitleBar = false;
                }));
#endif
            });

            var services = builder.Services;
#if WINDOWS
            services.AddSingleton<ITrayService, WinUI.TrayService>();
            services.AddSingleton<INotificationService, WinUI.NotificationService>();
#elif MACCATALYST
            services.AddSingleton<ITrayService, MacCatalyst.TrayService>();
            services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#endif

            return builder.Build();
        }
    }
}
