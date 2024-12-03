/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true, // Enable React Strict Mode
  productionBrowserSourceMaps: true, // Enable source maps in production
  images: {
    domains: [], // Add your image domains here
  },
  i18n: {
    locales: ['en'], // Add your supported locales here
    defaultLocale: 'en',
  },
  webpack: (config, options) => {    
    if (!options.dev) {
      config.devtool = options.isServer ? false : 'source-map'  // Enable source maps in production mode for client-side
    }    

    // Example of custom webpack configuration        
    if (!options.isServer) {
      config.resolve.fallback = {
        fs: false,
        path: false,
        os: false,
      };
    }

    return config;
  },
};

export default nextConfig;