// Redis caching pattern example using ioredis
// Run: npm install && npm start

const Redis = require("ioredis");

// Create a Redis client with authentication
const redis = new Redis({
  host: process.env.REDIS_HOST || "localhost",
  port: process.env.REDIS_PORT || 6379,
  password: process.env.REDIS_PASSWORD || "",
});

/** Simulate fetching data from a slow database */
async function fetchFromDatabase(key) {
  console.log(`  [DB] Fetching ${key} from database...`);
  return `Value for ${key} (from DB)`;
}

/** Get data with caching: check Redis first, fall back to DB */
async function getCachedData(key) {
  // Try to fetch from Redis cache first
  const cached = await redis.get(key);
  if (cached) {
    console.log(`  [Cache] HIT for ${key}`);
    return cached;
  }

  // Cache miss: fetch from the database
  console.log(`  [Cache] MISS for ${key}`);
  const data = await fetchFromDatabase(key);

  // Store in Redis with a 60-second expiration (TTL)
  await redis.setex(key, 60, data);
  return data;
}

async function main() {
  console.log("Connected to Redis\n");

  // First call: cache miss, fetches from database
  console.log("Request 1:");
  const result1 = await getCachedData("user:1001");
  console.log("  Result:", result1, "\n");

  // Second call: cache hit, returns from Redis
  console.log("Request 2:");
  const result2 = await getCachedData("user:1001");
  console.log("  Result:", result2, "\n");

  // Demonstrate other Redis data structures: a sorted set
  await redis.zadd("leaderboard", 100, "Alice", 85, "Bob", 72, "Charlie");
  const top = await redis.zrevrange("leaderboard", 0, 2, "WITHSCORES");
  console.log("Leaderboard:", top);

  await redis.quit();
}

main().catch(console.error);
