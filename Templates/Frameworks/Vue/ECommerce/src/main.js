import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './style.css'

const routes = [
  { path: '/', component: () => import('./views/Home.vue') },
  { path: '/cart', component: () => import('./views/Cart.vue') },
]

const router = createRouter({ history: createWebHistory(), routes })

createApp(App).use(createPinia()).use(router).mount('#app')
