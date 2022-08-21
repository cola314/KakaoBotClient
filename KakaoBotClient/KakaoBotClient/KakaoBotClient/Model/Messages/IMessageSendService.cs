namespace KakaoBotClient.Model.Messages.Messages
{
    public interface IMessageSendService
    {
        void Send(string roomName, string content);
    }
}
