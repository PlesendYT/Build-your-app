<script setup>
import { ref } from 'vue';

const data = ref(null);
const loading = ref(false);

async function fetchWeather() {
  loading.value = true;
  try {
    const res = await fetch(
      'https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true'
    );
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    data.value = await res.json();
  } catch (err) {
    data.value = { error: err.message };
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <section>
    <h2>Wetter / Weather</h2>
    <button @click="fetchWeather" :disabled="loading">
      {{ loading ? 'Lade…' : 'Daten laden' }}
    </button>
    <pre v-if="data">{{ data }}</pre>
  </section>
</template>

<style scoped>
pre { white-space: pre-wrap; margin-top: 1rem; }
</style>
