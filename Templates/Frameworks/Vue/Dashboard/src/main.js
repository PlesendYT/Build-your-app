import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './style.css'

const routes = [
  { path: '/', component: () => import('./views/Dashboard.vue') },
  { path: '/users', component: () => import('./views/Users.vue') },
  { path: '/settings', component: () => import('./views/Settings.vue') },
]

const router = createRouter({ history: createWebHistory(), routes })
createApp(App).use(router).mount('#app')
