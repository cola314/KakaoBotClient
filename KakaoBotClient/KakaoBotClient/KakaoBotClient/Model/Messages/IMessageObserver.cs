using KakaoBotClient.Model.Messages;

namespace KakaoBotClient.Core.Application.Messages
{
    public interface IMessageObserver
    {
        void OnReceiveMessage(Message message);
    }
}
