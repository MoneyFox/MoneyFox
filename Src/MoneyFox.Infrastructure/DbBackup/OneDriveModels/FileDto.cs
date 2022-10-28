namespace MoneyFox.Infrastructure.DbBackup.OneDriveModels;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

internal class FileSearchDto
{
    [JsonPropertyName("value")]
    [JsonProperty("value")]
    public List<FileDto> Files { get; set; } = null!;
}

internal class FileDto
{
    [JsonPropertyName("id")]
    [JsonProperty("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("lastModifiedDateTime")]
    [JsonProperty("lastModifiedDateTime")]
    public DateTimeOffset LastModifiedDateTime { get; set; }

    [JsonPropertyName("createdDateTime")]
    [JsonProperty("createdDateTime")]
    public DateTimeOffset CreatedDate { get; set; }
}
