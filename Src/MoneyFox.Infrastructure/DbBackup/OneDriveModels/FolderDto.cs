namespace MoneyFox.Infrastructure.DbBackup.OneDriveModels;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

internal class FolderSearchDto
{
    [JsonPropertyName("value")]
    [JsonProperty("value")]
    public List<FolderDto> Value { get; set; } = null!;
}

internal class FolderDto
{
    [JsonPropertyName("id")]
    [JsonProperty("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
}
