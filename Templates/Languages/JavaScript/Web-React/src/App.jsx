import { useState, useEffect } from 'react';

function Header() {
  return (
    <header>
      <h1>Web-React</h1>
      <p>React SPA Template</p>
    </header>
  );
}

function Counter() {
  const [count, setCount] = useState(0);

  return (
    <section>
      <h2>Counter</h2>
      <p style={{ fontSize: '3rem', fontWeight: 'bold' }}>{count}</p>
      <button onClick={() => setCount(c => c + 1)}>+1</button>
      <button onClick={() => setCount(c => c - 1)}>-1</button>
      <button onClick={() => setCount(0)}>Reset</button>
    </section>
  );
}

function Weather({ lat, lon }) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);

  async function fetchWeather() {
    setLoading(true);
    try {
      const res = await fetch(
        `https://api.open-meteo.com/v1/forecast?latitude=${lat}&longitude=${lon}&current_weather=true`
      );
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const json = await res.json();
      setData(json);
    } catch (err) {
      setData({ error: err.message });
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
}

export default function App() {
  return (
    <>
      <Header />
      <main>
        <Counter />
        <Weather lat={52.52} lon={13.41} />
      </main>
    </>
  );
}
