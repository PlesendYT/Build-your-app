/**
 * Main entry point for the Vue ECommerce application.
 * Configures Vue, Pinia state management, Vue Router, and mounts the app.
 */
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './style.css'

// Route definitions for shop and cart views
const routes = [
  { path: '/', component: () => import('./views/Home.vue') },
  { path: '/cart', component: () => import('./views/Cart.vue') },
]

const router = createRouter({ history: createWebHistory(), routes })

// Bootstrap app with Pinia and router
createApp(App).use(createPinia()).use(router).mount('#app')
