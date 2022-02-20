using Xamarin.Forms;

namespace KakaoBotClient.Model.Storage
{
    public class ApplicationStorage
    {
        private T GetValue<T>(string key, T defaultValue = default)
        {
            if (Application.Current.Properties.ContainsKey(key) 
                && Application.Current.Properties[key] is T val)
            {
                return val;
            }
            return defaultValue;
        }

        private void SetValue<T>(string key, T value)
        {
            Application.Current.Properties[key] = value;
        }

        public string ServerAddress
        {
            get => GetValue<string>(nameof(ServerAddress));
            set => SetValue<string>(nameof(ServerAddress), value);
        }

        public string ApiKey
        {
            get => GetValue<string>(nameof(ApiKey));
            set => SetValue<string>(nameof(ApiKey), value);
        }
    }
}
