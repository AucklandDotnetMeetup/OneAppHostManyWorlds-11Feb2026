using Aspire.Hosting.GitHub;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();

var sql = builder.AddSqlServer("sql");
var sqldb = sql.AddDatabase("sqldb");

var model = GitHubModel.OpenAI.OpenAIGpt5ChatPreview;
var chat = builder.AddGitHubModel("chat", model);

var apiService = builder.AddProject<Projects.AspireCrud_ApiService>("apiservice")
    .WithReference(sqldb).WaitFor(sqldb)
    .WithReference(chat).WaitFor(chat)
    .WithHttpHealthCheck("/health");

var function = builder.AddAzureFunctionsProject<Projects.AspireCrud_Function>("functions")
    .WithHostStorage(storage)
    .WithReference(chat).WaitFor(chat)
    .WithReference(apiService).WaitFor(apiService);

var spa = builder.AddJavaScriptApp("spa", "../AspiredAngular", runScriptName: "start")
    .WithNpm(installCommand: "install")
    .WithReference(apiService).WaitFor(apiService)
    .WithUrl("http://localhost:4200")
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
