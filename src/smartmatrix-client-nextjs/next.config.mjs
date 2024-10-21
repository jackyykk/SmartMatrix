/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'export',
  trailingSlash: true,  
  webpack: (config, options) => {
    if (options.dev) {
      config.devtool = 'eval-source-map';
    }
    return config;
  },  
  // Add other configurations if needed
};

export default nextConfig;