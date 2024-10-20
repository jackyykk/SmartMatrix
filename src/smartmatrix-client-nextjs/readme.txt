Publish smartmatrix-client-nextjs

1. Build the project, export the packages to ./out directory

npm run build

2. Upload the packages to Azure App Services

az login

az staticwebapp upload --name SmartMatrixWeb --resource-group smartmatrix-rg --source out


