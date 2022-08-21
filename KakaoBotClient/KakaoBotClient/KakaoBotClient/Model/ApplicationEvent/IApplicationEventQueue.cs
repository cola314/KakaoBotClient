using System;

namespace KakaoBotClient.Model.ApplicationEvent
{
    public interface IApplicationEventQueue
    {
        void Send<T>(T e) where T : class, IBaseApplicationEvent;
        void Subscribe<T>(object subscriber, Action<T> callback) where T : class, IBaseApplicationEvent;
        void UnSubscribe<T>(object subscriber) where T : class, IBaseApplicationEvent;
    }
}