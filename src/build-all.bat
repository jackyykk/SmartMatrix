@echo off
REM Find all .csproj files and build them
for /r %%i in (*.csproj) do (
    dotnet build "%%i"
)