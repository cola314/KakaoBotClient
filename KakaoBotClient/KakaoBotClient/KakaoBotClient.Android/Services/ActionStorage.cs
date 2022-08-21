using Android.App;
using System.Collections.Generic;

namespace KakaoBotClient.Droid.Services
{
    internal class ActionStorage
    {
        private readonly object lockObj = new object();
        private readonly Dictionary<string, Notification.Action> Actions = new Dictionary<string, Notification.Action>();

        public void Store(string roomName, Notification.Action ac)
        {
            lock (lockObj)
            {
                Actions[roomName] = ac;
            }
        }

        public Notification.Action GetAction(string roomName)
        {
            lock (lockObj)
            {
                return Actions[roomName];
            }
        }
    }
}