namespace SurrealDB.NET.Models;

public class SurrealDbConfiguration
{
    public Uri Server { get; init; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Namespace { get; set; } = default!;
    public string Database { get; set; } = default!;
}
