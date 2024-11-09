# Command Tips

## Deployment

### Publish SmartMatrix.WebApi

```powershell
PS C:\GitRepos\GitHub\jackyykk\SmartMatrix\src\SmartMatrix.WebApi>
dotnet publish --configuration Release --output ../publish/SmartMatrix.WebApi
```

- The files will be created at C:\GitRepos\GitHub\jackyykk\SmartMatrix\src\publish\SmartMatrix.WebApi.
- Use Azure Entension to deploy the files to App Service.

### Navigate to the solution directory

```powershell
cd path/to/your/solution
```
e.g.
```powershell
cd C:\GitRepos\GitHub\jackyykk\SmartMatrix\src
```

### Create the class library project

```powershell
dotnet new classlib -n SmartMatrix.Application -f net8.0
```

### Add the class library project to the solution

```powershell
dotnet sln add SmartMatrix.Application/SmartMatrix.Application.csproj
```

### Add a project reference (if needed)

```powershell
dotnet add YourOtherProject reference SmartMatrix.Application/SmartMatrix.Application.csproj
```
e.g.
```powershell
PS C:\GitRepos\GitHub\jackyykk\SmartMatrix\src\SmartMatrix.Infrastructure>
dotnet add reference ..\SmartMatrix.Application/SmartMatrix.Application.csproj
```

### Add the Dapper package

```powershell
dotnet add package Dapper
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```