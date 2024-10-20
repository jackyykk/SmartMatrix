Publish SmartMatrix.WebApi

1. Build the project, export the packages to ./publish directory

dotnet publish --configuration Release --output ./publish

2. Upload the packages to Azure App Services

- Open VSCode/Azure Extension, Select App Services/smartmatric
- Right Click, Select 'Deploy to Web App'
- Choose the ./publish folder

