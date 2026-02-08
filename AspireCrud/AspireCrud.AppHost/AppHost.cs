var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");
var sqldb = sql.AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.AspireCrud_ApiService>("apiservice")
    .WithReference(sqldb).WaitFor(sqldb)
    .WithHttpHealthCheck("/health");

var function = builder.AddProject<Projects.AspireCrud_Function>("functions")
    .WithReference(apiService).WaitFor(apiService);

var spa = builder.AddJavaScriptApp("spa", "../AspiredAngular", runScriptName: "start")
    .WithNpm(installCommand: "install")
    .WithReference(apiService).WaitFor(apiService)
    .WithUrl("http://localhost:4200")
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
