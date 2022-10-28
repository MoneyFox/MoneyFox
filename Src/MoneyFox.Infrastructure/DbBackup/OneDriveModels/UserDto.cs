namespace MoneyFox.Infrastructure.DbBackup.OneDriveModels;

using System.Text.Json.Serialization;
using Newtonsoft.Json;

/// <summary>
///     This class is annotated for JSON.net as well as System.Text.Json.
///     As soon as flurl updated to System.Text.Json the annotation for JSON.net can be removed.
/// </summary>
internal class UserDto
{
    [JsonPropertyName("userPrincipalName")]
    [JsonProperty("userPrincipalName")]
    public string? PrincipalName { get; set; }

    [JsonPropertyName("displayName")]
    [JsonProperty("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("mail")]
    [JsonProperty("mail")]
    public string? Email { get; set; }
}
