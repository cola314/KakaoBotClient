using KakaoBotClient.Model.Messages;
using System.Text.Json.Serialization;

namespace KakaoBotClient.Model.MessageServer.Dto
{
    public class ClientMessageDto
    {
        [JsonPropertyName("sender")]
        public string Sender { get; set; }

        [JsonPropertyName("room")]
        public string Room { get; set; }

        [JsonPropertyName("msg")]
        public string Content { get; set; }

        [JsonPropertyName("isGroupChat")]
        public bool IsGroupChat { get; set; }

        public ClientMessageDto(Message message)
        {
            this.Sender = message.Sender;
            this.Room = message.Room;
            this.Content = message.Content;
            this.IsGroupChat = message.IsGroupChat;
        }
    }
}
