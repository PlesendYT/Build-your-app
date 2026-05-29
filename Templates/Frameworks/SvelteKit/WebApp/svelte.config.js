// Import the auto-detecting adapter (adapts to Vercel, Netlify, Cloudflare, etc.)
import adapter from '@sveltejs/adapter-auto'
// Import the Svelte Vite preprocessor for TypeScript and other transformations
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte'

/** @type {import('@sveltejs/kit').Config} */
export default {
  kit: { adapter: adapter() },
  preprocess: vitePreprocess(),
}
