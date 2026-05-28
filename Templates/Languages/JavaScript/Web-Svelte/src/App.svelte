<script>
  let count = $state(0);
  let weather = $state(null);
  let loading = $state(false);

  function increment() { count++ }
  function decrement() { count-- }
  function reset()     { count = 0 }

  async function fetchWeather() {
    loading = true;
    try {
      const res = await fetch(
        'https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true'
      );
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      weather = await res.json();
    } catch (err) {
      weather = { error: err.message };
    } finally {
      loading = false;
    }
  }
</script>

<header>
  <h1>Web-Svelte</h1>
  <p>Svelte 5 Template</p>
</header>

<main>
  <section>
    <h2>Counter</h2>
    <p class="count">{count}</p>
    <button onclick={increment}>+1</button>
    <button onclick={decrement}>-1</button>
    <button onclick={reset}>Reset</button>
  </section>

  <section>
    <h2>Wetter / Weather</h2>
    <button onclick={fetchWeather} disabled={loading}>
      {loading ? 'Lade…' : 'Daten laden'}
    </button>
    {#if weather}
      <pre>{JSON.stringify(weather, null, 2)}</pre>
    {/if}
  </section>
</main>

<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body { font-family: system-ui, sans-serif; background: #f5f5f5; padding: 1rem; }
  header { text-align: center; padding: 2rem 0; }
  main { max-width: 640px; margin: 0 auto; display: flex; flex-direction: column; gap: 2rem; }
  section { background: #fff; border-radius: 0.5rem; padding: 1rem; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
  h2 { margin-bottom: 1rem; }
  .count { font-size: 3rem; font-weight: bold; }
  button { background: #4a90d9; color: #fff; border: none; border-radius: 0.25rem; padding: 0.5rem 1rem; cursor: pointer; margin-right: 0.5rem; }
  button:hover { background: #357abd; }
  pre { margin-top: 1rem; white-space: pre-wrap; }
</style>
