/**
 * Vite configuration for the Vue WebApp.
 * Enables the Vue plugin and sets dev server to port 3000 with auto-open.
 */
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: { port: 3000, open: true }
})
