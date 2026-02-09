# OneAppHostManyWorlds-11Feb2026
hands on aspire presentation hold on auckland dotnet user group at Microsoft Auckland, on 11th Feb 2026

# Sections
- Aspire CLI (init, run, etc...)
- Aspire with Api and Blazor hands on
- Aspire with Azure Functions
- Aspire with Angular
- Aspire with Github Models and Extensions.AI etc


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
- run `aspire add azure-storage` 
- check `func --version` to see if azure functions cli is installed
- run `func init AspireCrud.Function --worker-runtime dotnet-isolated` to create a new azure function project
- add azure function project to solution via `dotnet sln add AspireCrud.Function/AspireCrud_Function.csproj`
- aspire integration as `aspire add azure-functions`
- create a new azure function called `WeatherSummaryEnricher` via calling this
    func new   --name WeatherSummaryEnricher   --template "Timer trigger"

<!-- # Open API to Client Generation
- check `https://localhost:7494/openapi/v1.json`
- install openapi generator via `dotnet tool install --global Microsoft.dotnet-openapi`
- verify installation via `dotnet openapi --help` -->

# GitHub Models Integration
- `aspire add github-models`
- `dotnet add package Aspire.Azure.AI.Inference --prerelease`


# References
- [Aspire | SQL Server Integration](https://aspire.dev/integrations/databases/sql-server/sql-server-get-started/?lang=csharp)
- [Aspire | JavaScript Integration](https://aspire.dev/integrations/frameworks/javascript/)
- [Aspire | Azure Functions Integration](https://aspire.dev/integrations/cloud/azure/azure-functions/?environment=vscode)
- [Aspire | Azure Storage Blobs Integration](https://aspire.dev/integrations/cloud/azure/azure-storage-blobs/)
- [Aspire | GitHub Models Integration](https://aspire.dev/integrations/ai/github-models/)
- [Aspire Samples | Javascript Integration](https://github.com/dotnet/aspire-samples/tree/main/samples/aspire-with-javascript)
- [Aspire Samples | Azure Functions Integration](https://github.com/dotnet/aspire-samples/tree/main/samples/aspire-with-azure-functions)
- [Angular | Installation](https://angular.dev/installation)
- [Angular | ng new](https://angular.dev/cli/new)
- [Azure Function | Local Development](https://learn.microsoft.com/en-nz/azure/azure-functions/functions-run-local?tabs=macos%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp#install-the-azure-functions-core-tools)
- [Azure Function | .NET Aspire Integration](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-aspire-integration)
- [Tech Comm | Github Model Catalog](https://techcommunity.microsoft.com/blog/educatordeveloperblog/github-model-catalog---getting-started/4212711)
- [Github Models](https://github.com/marketplace?type=models)