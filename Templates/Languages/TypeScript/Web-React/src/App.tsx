// Root React component — composes Header, Counter, and Weather sections
import { useState, useEffect, type FC } from 'react';

interface WeatherData {
  current_weather?: { temperature: number };
  error?: string;
}

// Static header component
const Header: FC = () => (
  <header>
    <h1>Web-React + TS</h1>
    <p>React TypeScript Template</p>
  </header>
);

// Counter section with increment, decrement, and reset buttons
const Counter: FC = () => {
  const [count, setCount] = useState<number>(0);

  return (
    <section>
      <h2>Counter</h2>
      <p style={{ fontSize: '3rem', fontWeight: 'bold' }}>{count}</p>
      <button onClick={() => setCount(c => c + 1)}>+1</button>
      <button onClick={() => setCount(c => c - 1)}>-1</button>
      <button onClick={() => setCount(0)}>Reset</button>
    </section>
  );
};

// Weather section — fetches and displays current weather for the given coordinates
const Weather: FC<{ lat: number; lon: number }> = ({ lat, lon }) => {
  const [data, setData] = useState<WeatherData | null>(null);
  const [loading, setLoading] = useState<boolean>(false);

  async function fetchWeather(): Promise<void> {
    setLoading(true);
    try {
      const res = await fetch(
        `https://api.open-meteo.com/v1/forecast?latitude=${lat}&longitude=${lon}&current_weather=true`
      );
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const json: WeatherData = await res.json();
      setData(json);
    } catch (err) {
      setData({ error: (err as Error).message });
    } finally {
      setLoading(false);
    }
  }

  return (
    <section>
      <h2>Wetter / Weather</h2>
      <button onClick={fetchWeather} disabled={loading}>
        {loading ? 'Lade…' : 'Daten laden'}
      </button>
      {data && <pre>{JSON.stringify(data, null, 2)}</pre>}
    </section>
  );
};

// Main app layout
const App: FC = () => (
  <>
    <Header />
    <main>
      <Counter />
      <Weather lat={52.52} lon={13.41} />
    </main>
  </>
);

export default App;
