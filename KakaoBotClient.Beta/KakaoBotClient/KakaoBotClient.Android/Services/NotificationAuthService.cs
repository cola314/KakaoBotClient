using Android.Content;
using AndroidX.Core.App;
using KakaoBotClient.Model.ApplicationService;
using KakaoBotClient.Model.Messages;

namespace KakaoBotClient.Droid.Services
{
    internal class NotificationAuthService : INotificationAuthService
    {
        private Context _context;

        public NotificationAuthService(Context context)
        {
            this._context = context;
        }

        public bool IsAuthenticated()
        {
            var sets = NotificationManagerCompat.GetEnabledListenerPackages(_context);
            return sets?.Contains(_context.PackageName) ?? false;
        }

        public void RequestAuthentication()
        {
            var intent = new Intent("android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS");
            intent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(intent);
        }
    }
}