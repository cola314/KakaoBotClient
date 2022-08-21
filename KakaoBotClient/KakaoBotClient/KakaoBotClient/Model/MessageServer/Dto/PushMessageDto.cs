using System.Text.Json.Serialization;

namespace KakaoBotClient.Model.MessageServer.Dto
{
    public class PushMessageDto
    {
        [JsonPropertyName("room")]
        public string Room { get; set; }

        [JsonPropertyName("msg")]
        public string Content { get; set; }
    }
}
