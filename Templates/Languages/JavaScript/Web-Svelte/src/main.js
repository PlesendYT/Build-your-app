// Web-Svelte — application entry point
// Mounts the root Svelte component to the DOM

import { mount } from 'svelte';
import App from './App.svelte';

mount(App, { target: document.getElementById('app') });
