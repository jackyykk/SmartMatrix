Command Tips

# Navigate to the solution directory
cd path/to/your/solution

# Create the class library project
dotnet new classlib -n SmartMatrix.Application -f net8.0

# Add the class library project to the solution
dotnet sln add SmartMatrix.Application/SmartMatrix.Application.csproj

# Add a project reference (if needed)
dotnet add YourOtherProject reference SmartMatrix.Application/SmartMatrix.Application.csproj

e.g.
PS C:\GitRepos\GitHub\jackyykk\SmartMatrix\src\SmartMatrix.Infrastructure>
dotnet add reference ..\SmartMatrix.Application/SmartMatrix.Application.csproj

# Add the Dapper package
dotnet add package Dapper