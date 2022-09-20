using System.Net.Http.Headers;
using System.Net.Http.Json;
using SurrealDB.NET.Models;
using System.Text;

namespace SurrealDB.NET.API;

public class Surreal
{
    private readonly Uri _queryUri;
    private readonly HttpClient _httpClient;

    public Surreal(SurrealDbConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _queryUri = new Uri(configuration.Server, "/sql");
        _httpClient.BaseAddress = _queryUri;
        
        var authenticationString = $"{configuration.Username}:{configuration.Password}";
        var encodedAuthenticationString = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(authenticationString));
        
        _httpClient.DefaultRequestHeaders.Remove("Authorization");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", encodedAuthenticationString);

        ChangeNamespace(configuration.Namespace);
        ChangeDatabase(configuration.Database);
    }
    
    public async Task<bool> CheckConnection()
    {
        var content = new StringContent("INFO FOR DB;");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        try
        {
            var result = await _httpClient.PostAsync(_queryUri, content);
            return result.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
    
    public Task ChangeNamespace(string dbNamespace)
    {
        _httpClient.DefaultRequestHeaders.Remove("NS");
        _httpClient.DefaultRequestHeaders.Add("NS", dbNamespace);
        
        return Task.CompletedTask;
    }
    
    public Task ChangeDatabase(string database)
    {
        _httpClient.DefaultRequestHeaders.Remove("DB");
        _httpClient.DefaultRequestHeaders.Add("DB", database);
        
        return Task.CompletedTask;
    }
    
    public async Task<SurrealDbResponse[]> Query(string query)
    {
        var content = new StringContent(query);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var result = await _httpClient.PostAsync(_queryUri, content);
        var response = await result.Content.ReadFromJsonAsync<SurrealDbResponse[]>();
        return response ?? Array.Empty<SurrealDbResponse>();
    }
}
