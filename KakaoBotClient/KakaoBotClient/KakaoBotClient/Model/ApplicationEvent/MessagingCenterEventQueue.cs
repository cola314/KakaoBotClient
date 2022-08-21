using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KakaoBotClient.Model.ApplicationEvent
{
    public class MessagingCenterEventQueue : IApplicationEventQueue
    {
        public void Send<T>(T e)
            where T : class, IBaseApplicationEvent
        {
            MessagingCenter.Send(e, EventKeyAttribute.GetKeyName(typeof(T)));
        }

        public void Subscribe<T>(object subscriber, Action<T> callback)
            where T : class, IBaseApplicationEvent
        {
            MessagingCenter.Subscribe(subscriber, EventKeyAttribute.GetKeyName(typeof(T)), callback);
        }

        public void UnSubscribe<T>(object subscriber)
            where T : class, IBaseApplicationEvent
        {
            MessagingCenter.Unsubscribe<T>(subscriber, EventKeyAttribute.GetKeyName(typeof(T)));
        }
    }
}
