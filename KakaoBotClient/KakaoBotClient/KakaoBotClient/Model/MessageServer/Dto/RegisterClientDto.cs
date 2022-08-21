using System.Text.Json.Serialization;

namespace KakaoBotClient.Model.MessageServer.Dto
{
    public class RegisterClientDto
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }

        public RegisterClientDto(string password)
        {
            Password = password;
        }
    }
}
