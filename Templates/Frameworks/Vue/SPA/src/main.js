/**
 * Main entry point for the Vue SPA application.
 * Configures Vue Router with static and dynamic routes, then mounts the app.
 */
import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './style.css'

import Home from './views/Home.vue'
import Page from './views/Page.vue'

// Routes: static home page and dynamic page with :id parameter
const routes = [
  { path: '/', component: Home },
  { path: '/page/:id', component: Page },
]

const router = createRouter({ history: createWebHistory(), routes })
createApp(App).use(router).mount('#app')
