/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true, // Enable React Strict Mode  
  images: {
    domains: [], // Add your image domains here
  },
  i18n: {
    locales: ['en'], // Add your supported locales here
    defaultLocale: 'en',
  },
  webpack: (config, { isServer }) => {
    // Example of custom webpack configuration
    if (!isServer) {
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