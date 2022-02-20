using KakaoBotClient.Model.Messages;
using KakaoBotClient.Model.MessageServer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KakaoBotClient.Model.MessageServer
{
    public interface IMessageServerClient
    {
        event EventHandler OnConnected;
        event EventHandler OnDisconnected;
        event EventHandler<ReciveMessageEventArgs> OnReceiveMessage;

        Task ConnectAsync(string address, string apiKey);
        Task DisconnectAsync();

        void SendMessage(Message message);   
    }
}
