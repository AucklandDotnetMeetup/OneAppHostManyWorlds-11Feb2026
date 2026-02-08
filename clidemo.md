# Initial Setup
- `dotnet --list-sdks`
- `dotnet --version`
- `dotnet tool list -g`
    - check if we have `aspire.cli`
    - check if we have `dotnet-ef`
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
- `dotnet add package Aspire.Microsoft.EntityFrameworkCore.SqlServer --project ./AspireCrud.ApiService/AspireCrud.ApiService.csproj`
- `dotnet add package Microsoft.EntityFrameworkCore.Design --project ./AspireCrud.ApiService/AspireCrud.ApiService.csproj`
- adapt model
- create db context
- get into api project via `cd AspireCrud.ApiService`
    - `dotnet ef migrations add InitialWeatherForecast`
    - `dotnet ef database update`
- update endpoints and http file for testing

