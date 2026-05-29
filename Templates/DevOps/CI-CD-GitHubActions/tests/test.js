// Basic integration test for the CI/CD demo Express app
// Uses Node's built-in assert module and http to avoid extra dependencies

const assert = require('assert');
const http = require('http');

// Import the Express app instance
const app = require('../app/app.js');

// Track the number of passed and failed assertions
let passed = 0;
let failed = 0;

// Helper: create a test case with a descriptive name
function test(name, fn) {
  try {
    fn();
    passed++;
    console.log(`  PASS: ${name}`);
  } catch (err) {
    failed++;
    console.error(`  FAIL: ${name} — ${err.message}`);
  }
}

// Helper: make an HTTP request to the app and return the parsed response
function request(method, path, body) {
  return new Promise((resolve, reject) => {
    // Create an HTTP server using the app to test in-process
    const server = app.listen(0, () => {
      const port = server.address().port;
      const options = {
        hostname: 'localhost',
        port,
        path,
        method,
        headers: { 'Content-Type': 'application/json' },
      };

      const req = http.request(options, (res) => {
        let data = '';
        res.on('data', (chunk) => (data += chunk));
        res.on('end', () => {
          server.close();
          resolve({ status: res.statusCode, body: JSON.parse(data || 'null') });
        });
      });

      req.on('error', (err) => {
        server.close();
        reject(err);
      });

      if (body) req.write(JSON.stringify(body));
      req.end();
    });
  });
}

// ----- Run the tests -----
console.log('Running tests...\n');

test('GET /health returns ok', async () => {
  const res = await request('GET', '/health');
  assert.strictEqual(res.status, 200);
  assert.strictEqual(res.body.status, 'ok');
});

test('GET /api/items returns empty array initially', async () => {
  const res = await request('GET', '/api/items');
  assert.strictEqual(res.status, 200);
  assert.deepStrictEqual(res.body, []);
});

test('POST /api/items creates a new item', async () => {
  const res = await request('POST', '/api/items', { name: 'test-item' });
  assert.strictEqual(res.status, 201);
  assert.strictEqual(res.body.name, 'test-item');
  assert.ok(res.body.id > 0);
});

test('POST /api/items rejects missing name', async () => {
  const res = await request('POST', '/api/items', {});
  assert.strictEqual(res.status, 400);
  assert.strictEqual(res.body.error, 'Name is required');
});

// Print summary
const total = passed + failed;
console.log(`\nResults: ${passed}/${total} passed, ${failed}/${total} failed`);

// Exit with non-zero code if any test failed
process.exit(failed > 0 ? 1 : 0);
