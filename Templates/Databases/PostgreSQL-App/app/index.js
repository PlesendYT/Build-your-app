// PostgreSQL CRUD example using the pg library
// Run: npm install && npm start

const { Pool } = require("pg");

// Create a connection pool to the PostgreSQL database
const pool = new Pool({
  user: "admin",
  host: "localhost",
  database: "myapp",
  password: "secret",
  port: 5432,
});

async function main() {
  let client;
  try {
    // Acquire a client from the pool
    client = await pool.connect();
    console.log("Connected to PostgreSQL");

    // INSERT: Create a new user record
    const insertRes = await client.query(
      "INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *",
      ["Alice", "alice@example.com"]
    );
    console.log("Inserted:", insertRes.rows[0]);

    // SELECT: Query all users from the table
    const selectRes = await client.query("SELECT * FROM users");
    console.log("All users:", selectRes.rows);

    // UPDATE: Modify an existing user's email
    const updateRes = await client.query(
      "UPDATE users SET email = $1 WHERE name = $2 RETURNING *",
      ["alice@newdomain.com", "Alice"]
    );
    console.log("Updated:", updateRes.rows[0]);

    // DELETE: Remove a user by name
    const deleteRes = await client.query(
      "DELETE FROM users WHERE name = $1 RETURNING *",
      ["Alice"]
    );
    console.log("Deleted:", deleteRes.rows[0]);
  } catch (err) {
    console.error("Database error:", err);
  } finally {
    // Always release the client back to the pool
    if (client) client.release();
    await pool.end();
  }
}

main();
