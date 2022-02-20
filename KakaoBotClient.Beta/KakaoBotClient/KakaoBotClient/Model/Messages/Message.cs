using System;

namespace KakaoBotClient.Model.Messages
{
    public class Message
    {
        public Message(bool isGroupChat, string senderName, string roomName, string content)
        {
            this.IsGroupChat = isGroupChat;
            this.Sender = senderName;
            this.Room = roomName;
            this.Content = content;
        }

        public bool IsGroupChat { get; }

        public string Sender { get; }

        public string Room { get; }

        public string Content { get; }
    }
}
