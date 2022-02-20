using KakaoBotClient.Model.Messages;
using KakaoBotClient.Model.MessageServer.Dto;
using SocketIOClient;
using System;
using System.Threading.Tasks;

namespace KakaoBotClient.Model.MessageServer
{
    internal class SocketIOMessageServerClient : IMessageServerClient
    {
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public event EventHandler<ReciveMessageEventArgs> OnReceiveMessage;

        private SocketIO _client;
        private string _apiKey;

        public Task ConnectAsync(string address, string apiKey)
        {
            _apiKey = apiKey;
            _client = new SocketIO(address);

            _client.OnConnected += async (s, e) =>
            {
                await _client.EmitAsync("register client", new RegisterClientDto(_apiKey));
                OnConnected?.Invoke(this, EventArgs.Empty);
            };
            _client.OnDisconnected += (s, e) =>
            {
                OnDisconnected?.Invoke(this, EventArgs.Empty);
            };

            _client.On("push message", res =>
            {
                var dto = res.GetValue<PushMessageDto>();
                OnReceiveMessage?.Invoke(this, new ReciveMessageEventArgs(dto.Room, dto.Content));
            });

            return _client.ConnectAsync();
        }

        public Task DisconnectAsync()
        {
            return _client.DisconnectAsync();
        }

        public void SendMessage(Message message)
        {
            _client.EmitAsync("request message", new ClientMessageDto(message));
        }
    }
}
