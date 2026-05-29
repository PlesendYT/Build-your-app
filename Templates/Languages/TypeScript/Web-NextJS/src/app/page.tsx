// Next.js home page (App Router) — counter and weather fetch demo (client component)
'use client';

import { useState, type FC } from 'react';

interface WeatherData {
  current_weather?: { temperature: number };
  error?: string;
}

const Home: FC = () => {
  const [count, setCount] = useState<number>(0);
  const [weather, setWeather] = useState<WeatherData | null>(null);
  const [loading, setLoading] = useState<boolean>(false);

  // Fetch current weather data from the Open-Meteo API
  async function fetchWeather(): Promise<void> {
    setLoading(true);
    try {
      const res = await fetch(
        'https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true'
      );
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      setWeather(await res.json());
    } catch (err) {
      setWeather({ error: (err as Error).message });
    } finally {
      setLoading(false);
    }
  }

  return (
    <main style={{ maxWidth: 640, margin: '0 auto' }}>
      <header style={{ textAlign: 'center', padding: '2rem 0' }}>
        <h1>Next.js + TS</h1>
        <p>Next.js TypeScript Template</p>
      </header>

      <section style={{ background: '#fff', borderRadius: 8, padding: '1rem', marginBottom: '1rem', boxShadow: '0 2px 8px rgba(0,0,0,0.1)' }}>
        <h2>Counter</h2>
        <p style={{ fontSize: '3rem', fontWeight: 'bold' }}>{count}</p>
        <button onClick={() => setCount(c => c + 1)}>+1</button>
        <button onClick={() => setCount(c => c - 1)}>-1</button>
        <button onClick={() => setCount(0)}>Reset</button>
      </section>

      <section style={{ background: '#fff', borderRadius: 8, padding: '1rem', boxShadow: '0 2px 8px rgba(0,0,0,0.1)' }}>
        <h2>Wetter / Weather</h2>
        <button onClick={fetchWeather} disabled={loading}>
          {loading ? 'Lade…' : 'Daten laden'}
        </button>
        {weather && <pre>{JSON.stringify(weather, null, 2)}</pre>}
      </section>
    </main>
  );
};

export default Home;
