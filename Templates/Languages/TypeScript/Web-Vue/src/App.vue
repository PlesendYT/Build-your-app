<script setup lang="ts">
import { ref } from 'vue';
import WeatherCard from './components/WeatherCard.vue';

const count = ref<number>(0);

function increment(): void { count.value++ }
function decrement(): void { count.value-- }
function reset(): void     { count.value = 0 }
</script>

<template>
  <header>
    <h1>Web-Vue + TS</h1>
    <p>Vue 3 TypeScript Template</p>
  </header>

  <main>
    <section>
      <h2>Counter</h2>
      <p class="count">{{ count }}</p>
      <button @click="increment">+1</button>
      <button @click="decrement">-1</button>
      <button @click="reset">Reset</button>
    </section>

    <WeatherCard />
  </main>
</template>

<style>
* { box-sizing: border-box; margin: 0; padding: 0; }
body { font-family: system-ui, sans-serif; background: #f5f5f5; padding: 1rem; }
header { text-align: center; padding: 2rem 0; }
main { max-width: 640px; margin: 0 auto; display: flex; flex-direction: column; gap: 2rem; }
section { background: #fff; border-radius: 0.5rem; padding: 1rem; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.count { font-size: 3rem; font-weight: bold; }
button { background: #4a90d9; color: #fff; border: none; border-radius: 0.25rem; padding: 0.5rem 1rem; cursor: pointer; margin-right: 0.5rem; }
button:hover { background: #357abd; }
</style>
