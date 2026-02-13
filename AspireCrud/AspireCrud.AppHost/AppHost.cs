
using Aspire.Hosting.GitHub;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var sql = builder.AddSqlServer("sql");
var sqldb = sql.AddDatabase("sqldb");

var model = GitHubModel.OpenAI.OpenAIGpt4o;
var chat = builder.AddGitHubModel("chat", model);

var apiService = builder.AddProject<Projects.AspireCrud_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(sqldb).WaitFor(sqldb);

var function = builder.AddAzureFunctionsProject<Projects.AspireCrud_Function>("function")
    .WithReference(apiService).WaitFor(apiService)
    .WithReference(chat).WaitFor(chat)
    .WithHostStorage(storage);

var spaWeb = builder.AddJavaScriptApp("spa", "../AspiredAngular", runScriptName: "start")
    .WithNpm(installCommand: "install")
    .WithReference(apiService).WaitFor(apiService)
    .WithUrl("http://localhost:4200")
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
