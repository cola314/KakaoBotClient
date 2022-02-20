using KakaoBotClient.Core.Application.Messages;
using KakaoBotClient.Model.Messages;
using KakaoBotClient.Model.Messages.Messages;
using System;

namespace KakaoBotClient.Model.ApplicationService
{
    public class ClientMessageFacade : IMessageObserver
    {
        private readonly IMessageSendService _messageSendService;

        public event EventHandler<Message> OnMessageReceived;  
        
        public ClientMessageFacade(IMessageSendService messageSendService)
        {
            _messageSendService = messageSendService ?? throw new ArgumentNullException(nameof(messageSendService));
        }

        public void SendMessage(string room, string message)
        {
            _messageSendService.Send(room, message);
        }

        public void OnReceiveMessage(Message message)
        {
            OnMessageReceived?.Invoke(this, message);
        }
    }
}
