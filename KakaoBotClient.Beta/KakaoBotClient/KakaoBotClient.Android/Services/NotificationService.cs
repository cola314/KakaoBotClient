using Android.App;
using Android.Content;
using Android.OS;
using Android.Service.Notification;
using Android.Util;
using KakaoBotClient.Core.Application.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xamarin.Forms;
using Message = KakaoBotClient.Model.Messages.Message;

namespace KakaoBotClient.Droid.Services
{
    [Service(
        Name = "org.goodgoodgood.notification.NotificationListener",
        Label = "Mina Listener",
        Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE",
        Exported = true)]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    public class NotificationService : NotificationListenerService
    {
        private string Tag => nameof(NotificationService);

        private ActionStorage ActionStorage => App.ServiceProvider.GetService<ActionStorage>();
        private IMessageObserver MessageObserver => App.ServiceProvider.GetService<IMessageObserver>();

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);
            
            if (!IsKakaoNotification(sbn))
                return;

            try
            {
                var we = new Notification.WearableExtender(sbn.Notification);
                var acts = we.Actions
                    .Where(ac => IsMessageNotification(ac));

                if (!acts.Any())
                    return;

                var message = (Build.VERSION.SdkInt > (BuildVersionCodes)23)
                    ? CreateMessageAfterSdk23(sbn)
                    : CreateMessageUntilSdk23(sbn);

                foreach (var action in acts)
                {
                    ActionStorage.Store(message.Room, action);
                    MessageObserver.OnReceiveMessage(message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Tag, "Error on receive message {0}", ex.ToString());
            }
        }

        private bool IsKakaoNotification(StatusBarNotification sbn)
        {
            return sbn.PackageName == "com.kakao.talk";
        }

        private bool IsMessageNotification(Notification.Action ac)
        {
            return ac.GetRemoteInputs()?.Length > 0 &&
                ac.Title.ToString().ToLower().Contains("replay") || ac.Title.ToString().Contains("답장");
        }

        private Message CreateMessageAfterSdk23(StatusBarNotification sbn)
        {
            Bundle data = sbn.Notification.Extras;

            string roomName = data.GetString("android.summaryText");
            string senderName = data.Get("android.title").ToString();
            string content = data.Get("android.text").ToString();
            bool isGroupChat = roomName != null;


            return new Message(isGroupChat, senderName, roomName ?? senderName, content);
        }                

        private Message CreateMessageUntilSdk23(StatusBarNotification sbn)
        {
            Bundle data = sbn.Notification.Extras;

            string roomName = data.GetString("android.subText");
            string senderName = data.GetString("android.title");
            string content = data.GetString("android.text");
            bool isGroupChat = roomName != null;

            return new Message(isGroupChat, senderName, roomName ?? senderName, content);
        }
    }
}