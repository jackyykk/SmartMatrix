Please use Node.js v20.18.01 with long-term support

*** Publish smartmatrix-client-nextjs ***

1. Install packages
npm install

2. Build the project, export the packages to ./out directory

npm run build

3. Since Azure Static Web App - SmartMatrixWeb had been setup as deployed by GitHub, It will trigger GitHub Action once we pushed the commits to main branch.

4. After GitHub Action runs successfully, it will deploy the packages to Azure Static Web App


*** Update Outdated Packages ***

1. Install npm-check-updates globally
npm install -g npm-check-updates

2. Update package.json with the latest versions
npx npm-check-updates -u

3. Install the updated dependencies
npm install