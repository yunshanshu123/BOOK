using System.Text.Json.Serialization;

namespace backend.DTOs.Web
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }


        [JsonConstructor]//序列化特性
        LoginDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
