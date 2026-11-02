using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Boilerplate: start an MCP server that talks over standard input/output.
var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();   // logs would corrupt the MCP messages, so turn them off
builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly();
builder.Build().Run();