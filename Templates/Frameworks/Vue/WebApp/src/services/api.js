/**
 * Axios HTTP client instance with configurable base URL and timeout.
 * Reads the API URL from VITE_API_URL environment variable, defaults to '/api'.
 */
import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  timeout: 10000,
})

export default api
