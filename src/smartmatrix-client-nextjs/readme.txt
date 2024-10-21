Publish smartmatrix-client-nextjs

1. Install packages
npm install

2. Build the project, export the packages to ./out directory

npm run build

3. Since Azure Static Web App - SmartMatrixWeb had been setup as deployed by GitHub, It will trigger GitHub Action once we pushed the commits to main branch.

4. After GitHub Action runs successfully, it will deploy the packages to Azure Static Web App


