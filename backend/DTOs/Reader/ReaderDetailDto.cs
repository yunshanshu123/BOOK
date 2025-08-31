using System.Text.Json.Serialization;

namespace backend.DTOs.Reader
{
    public class ReaderDetailDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("nickName")]
        public string? NickName { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        [JsonPropertyName("creditScore")]
        public int CreditScore { get; set; }

        [JsonPropertyName("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonPropertyName("permission")]
        public string Permission { get; set; }
    }
}
