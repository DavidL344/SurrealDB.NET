using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurrealDB.NET.Models;

public class SurrealDbResponse
{
    /// <summary>
    /// Database response time in milliseconds.
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; } = default!;
    
    /// <summary>
    /// An HTTP status returned by the database.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
    
    /// <summary>
    /// The data returned by the database.
    /// </summary>
    [JsonPropertyName("result")]
    public object[] Result { get; set; } = default!;

    public override string ToString() => JsonSerializer.Serialize(this);

    public string ToFormattedString()
        => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
}
