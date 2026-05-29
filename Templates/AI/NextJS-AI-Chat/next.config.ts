import type { NextConfig } from "next";

// Next.js configuration for the AI chat application
const nextConfig: NextConfig = {
  // Allow remote images if needed for avatars or bot icons
  images: {
    remotePatterns: [],
  },
};

export default nextConfig;
