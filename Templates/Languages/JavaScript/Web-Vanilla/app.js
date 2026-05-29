// Web-Vanilla — main application
// Demonstrates a counter and a weather fetch API demo using vanilla DOM manipulation

/* ── Counter ── */

const counterValue = document.getElementById('counter-value');
const btnIncrement = document.getElementById('btn-increment');
const btnDecrement = document.getElementById('btn-decrement');
const btnReset = document.getElementById('btn-reset');

let count = 0;

/** Updates the counter display in the DOM */
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

/** Fetches current weather data from the Open-Meteo API and displays it */
async function fetchWeather() {
  try {
    weatherOutput.textContent = 'Loading…';
    const res = await fetch('https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true');
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    const data = await res.json();
    weatherOutput.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    weatherOutput.textContent = `Error: ${err.message}`;
  }
}

btnFetch.addEventListener('click', fetchWeather);

/* ── Initialize ── */

updateCounter();
