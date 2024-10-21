/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'export',
  trailingSlash: true,
  webpack: (config, { dev, isServer }) => {
    if (dev) {
      config.devtool = 'source-map';
    }
    return config;
  },
  // Add other configurations if needed
};

export default nextConfig;