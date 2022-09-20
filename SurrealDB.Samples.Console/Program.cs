using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SurrealDB.NET.API;
using SurrealDB.NET.Extensions;
using SurrealDB.NET.Models;

// Initialize a service collection
var services = new ServiceCollection();

// Create a configuration class
var sampleCredentials = new SurrealDbConfiguration
{
	Server = new Uri("http://localhost:8000"),
	Username = "root",
	Password = "root",
	Namespace = "test",
	Database = "test"
};

// Add services to the collection
services.AddSurrealDb(sampleCredentials);
services.AddSingleton(_ =>
{
	using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
	
	return loggerFactory.CreateLogger<Program>();
});

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Get the services from the DI container
var db = serviceProvider.GetRequiredService<Surreal>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

// Check if the connection is successful
var connected = await db.CheckConnection();
if (!connected) throw new Exception("Could not connect to SurrealDB");

// Globally change the current namespace
// await db.ChangeNamespace("my-namespace");

// Globally change the current database
// await db.ChangeDatabase("my-database");

// Run a query
var result = await db.Query(
	@"CREATE account SET
	name = 'ACME Inc',
	created_at = time::now();");

// Print out the response
var responseBody = JsonSerializer.Serialize(result[0].Result);
var rawResponse = JsonSerializer.Serialize(result);

logger.LogInformation("Query returned {Status} in {Time}.\r\nResponse body: {Response}\r\nRaw response: {RawResponse}",
	result[0].Status, result[0].Time, responseBody, rawResponse);
