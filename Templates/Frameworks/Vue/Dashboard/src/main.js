/**
 * Main entry point for the Vue Dashboard application.
 * Sets up Vue, Vue Router, and mounts the root component.
 */
import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './style.css'

// Application route definitions with lazy-loaded views
const routes = [
  { path: '/', component: () => import('./views/Dashboard.vue') },
  { path: '/users', component: () => import('./views/Users.vue') },
  { path: '/settings', component: () => import('./views/Settings.vue') },
]

// Create router instance and mount app
const router = createRouter({ history: createWebHistory(), routes })
createApp(App).use(router).mount('#app')
