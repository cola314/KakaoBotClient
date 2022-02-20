using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using KakaoBotClient.Droid.Services;
using KakaoBotClient.Model.Messages.Messages;
using KakaoBotClient.Model.ApplicationService;
using Microsoft.Extensions.DependencyInjection;
using Android.Content;

namespace KakaoBotClient.Droid
{
    [Activity(Label = "KakaoBotClient", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App(AddServices));
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<ActionStorage>();
            services.AddSingleton(_ => ApplicationContext);
            services.AddSingleton<IMessageSendService, MessageSendService>();
            services.AddSingleton<INotificationAuthService, NotificationAuthService>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}