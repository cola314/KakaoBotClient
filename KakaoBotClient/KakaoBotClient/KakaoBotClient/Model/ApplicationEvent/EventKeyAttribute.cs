using System;

namespace KakaoBotClient.Model.ApplicationEvent
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventKeyAttribute : Attribute
    {
        public string Key { get; }

        public EventKeyAttribute(string key)
        {
            Key = key;
        }

        public static string GetKeyName(Type type)
        {
            var attr = GetCustomAttribute(type, typeof(EventKeyAttribute)) as EventKeyAttribute;
            return attr?.Key;
        }
    }
}
