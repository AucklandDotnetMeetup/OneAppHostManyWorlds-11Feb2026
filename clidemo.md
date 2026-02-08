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

# Angular Integration
- remove blazor `Web` project and its references from `AppHost`
- check if angualar is installed via `ng version`
- install angular via `npm install -g @angular/cli`
- create angular project via `ng new aspire-angular --style css --routing true --ssr=no`
- `aspire add javascript` to add js integration to the app host
- adapt `AppHost.cs` to serve angular app
- change Angular App to call API

# Azure Function Integration
- check `func --version` to see if azure functions cli is installed
- run `func init AspireCrud.Function --worker-runtime dotnet-isolated` to create a new azure function project
- add azure function project to solution via `dotnet sln add AspireCrud.Function/AspireCrud_Function.csproj`
- create a new azure function called `WeatherSummaryEnricher` via calling this
    func new   --name WeatherSummaryEnricher   --template "Timer trigger"
