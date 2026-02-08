# Initial Setup
- `dotnet --list-sdks`
- `dotnet --version`
- `dotnet tool list -g`
- `dotnet tool uninstall -g aspire.cli`
- `dotnet tool install -g aspire.cli`
- `aspire new`
    - name: Aspire Crud
    - Blazor and Api
    - No Tests
    - No Redis
    - No devtunnels
- `cd AspireCrud`
- `dotnet watch run --project ./AspireCrud.AppHost/AspireCrud.AppHost.csproj`
- `aspire run`

# Integrate with SQL Server
- `aspire add sqlserver`
- `dotnet add package Aspire.Microsoft.Data.SqlClient --project ./AspireCrud.ApiService/AspireCrud.ApiService.csproj`
