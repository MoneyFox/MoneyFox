namespace MoneyFox.Infrastructure.DbBackup.OneDriveModels
{
    using System.Text.Json.Serialization;

    internal class UserDto
    {
        [JsonPropertyName("userPrincipalName")]
        public string PrincipalName { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("mail")]
        public string Email { get; set; }
    }
}
