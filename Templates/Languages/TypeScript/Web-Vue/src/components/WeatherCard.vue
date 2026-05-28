<script setup lang="ts">
import { ref } from 'vue';

interface WeatherData {
  current_weather?: { temperature: number };
  error?: string;
}

const data = ref<WeatherData | null>(null);
const loading = ref<boolean>(false);

async function fetchWeather(): Promise<void> {
  loading.value = true;
  try {
    const res = await fetch(
      'https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true'
    );
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    data.value = await res.json() as WeatherData;
  } catch (err) {
    data.value = { error: (err as Error).message };
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
