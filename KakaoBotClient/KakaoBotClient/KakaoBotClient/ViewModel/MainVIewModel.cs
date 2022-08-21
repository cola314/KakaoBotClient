using KakaoBotClient.Model.ApplicationService;
using KakaoBotClient.Model.Messages;
using KakaoBotClient.Model.MessageServer;
using KakaoBotClient.Model.Storage;
using KakaoBotClient.Resources;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KakaoBotClient.ViewModel
{
    public class MainViewModel : BindableObject
    {
        private readonly ClientMessageFacade _clientMessageFacade;
        private readonly INotificationAuthService _notificationAuthService;
        private readonly ApplicationStorage _applicationStorage;

        public MainViewModel(
            INotificationAuthService notificationAuthService,
            ClientMessageFacade clientMessageFacade,
            ApplicationStorage applicationStorage)
        {
            this._notificationAuthService = notificationAuthService ?? throw new ArgumentNullException(nameof(notificationAuthService));
            this._clientMessageFacade = clientMessageFacade ?? throw new ArgumentNullException(nameof(clientMessageFacade));
            this._applicationStorage = applicationStorage ?? throw new ArgumentNullException(nameof(applicationStorage));

            clientMessageFacade.OnMessageReceived += ClientMessageFacade_OnMessageReceived;
        }

        private IMessageServerClient _client;

        public ICommand RequestAuth => new Command(_ =>
            {
                _notificationAuthService.RequestAuthentication();
            });

        public string ServerAddress
        {
            get => _applicationStorage.ServerAddress;
            set
            {
                _applicationStorage.ServerAddress = value;
                OnPropertyChanged(nameof(ServerAddress));
                OnPropertyChanged(nameof(ConnectServer));
            }
        }

        public string ApiKey
        {
            get => _applicationStorage.ApiKey;
            set
            {
                _applicationStorage.ApiKey = value;
                OnPropertyChanged(nameof(ApiKey));
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
                OnPropertyChanged(nameof(ConnectServer));
                OnPropertyChanged(nameof(DisconnectServer));
            }
        }

        public ICommand ConnectServer => new Command(async _ =>
            {
                _client = App.ServiceProvider.GetService<IMessageServerClient>();

                _client.OnConnected += async (s, e) =>
                {
                    await Device.InvokeOnMainThreadAsync(() => IsConnected = true);
                };
                _client.OnDisconnected += async (s, e) =>
                {
                    await Device.InvokeOnMainThreadAsync(() => IsConnected = false);
                };
                _client.OnReceiveMessage += (s, message) =>
                {
                    Task.Run(() =>
                    {
                        _clientMessageFacade.SendMessage(message.Room, message.Content);
                    });
                };

                try
                {
                    await _client.ConnectAsync(ServerAddress, ApiKey);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException ||
                        ex is UriFormatException)
                    {
                        await App.Current.MainPage.DisplayAlert(Resource.ERROR, Resource.ERROR_SERVER_ADDRESS_FORMAT, Resource.CONFIRM);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(Resource.ERROR, Resource.ERROR_CONNECT_SERVER, Resource.CONFIRM);
                    }
                }
            },
            _ => !string.IsNullOrEmpty(ServerAddress) && !IsConnected);

        private void ClientMessageFacade_OnMessageReceived(object sender, Message e)
        {
            _client?.SendMessage(e);
        }

        public ICommand DisconnectServer => new Command(async _ =>
            {
                await _client.DisconnectAsync();
                _client = null;
            }, _ => _client != null && IsConnected);
    }
}
