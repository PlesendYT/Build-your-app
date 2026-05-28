// Web-Vanilla — Hauptapplikation

/* ── Counter ── */

const counterValue = document.getElementById('counter-value');
const btnIncrement = document.getElementById('btn-increment');
const btnDecrement = document.getElementById('btn-decrement');
const btnReset = document.getElementById('btn-reset');

let count = 0;

function updateCounter() {
  counterValue.textContent = count;
}

btnIncrement.addEventListener('click', () => {
  count++;
  updateCounter();
});

btnDecrement.addEventListener('click', () => {
  count--;
  updateCounter();
});

btnReset.addEventListener('click', () => {
  count = 0;
  updateCounter();
});

/* ── Weather (Fetch API Demo) ── */

const btnFetch = document.getElementById('btn-fetch');
const weatherOutput = document.getElementById('weather-output');

async function fetchWeather() {
  try {
    weatherOutput.textContent = 'Lade… / Loading…';
    const res = await fetch('https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true');
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    const data = await res.json();
    weatherOutput.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    weatherOutput.textContent = `Fehler / Error: ${err.message}`;
  }
}

btnFetch.addEventListener('click', fetchWeather);

/* ── Initialisieren ── */

updateCounter();
