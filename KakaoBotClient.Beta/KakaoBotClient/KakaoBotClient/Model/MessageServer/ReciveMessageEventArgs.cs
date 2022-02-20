using System;

namespace KakaoBotClient.Model.MessageServer
{
    public class ReciveMessageEventArgs : EventArgs
    {
        public string Room { get; set; }

        public string Content { get; set; }

        public ReciveMessageEventArgs(string room, string content)
        {
            Room = room;
            Content = content;
        }
    }
}
