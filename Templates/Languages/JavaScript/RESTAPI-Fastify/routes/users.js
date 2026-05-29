// User routes — CRUD operations for the users resource
// Implements GET, POST, PUT, DELETE with Fastify schema validation

const users = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob',   email: 'bob@example.com' },
];
let nextId = 3;

// Response schema for a single user
const userSchema = {
  type: 'object',
  properties: {
    id: { type: 'integer' },
    name: { type: 'string' },
    email: { type: 'string' },
  },
};

// Request/response schema for creating a user
const createUserSchema = {
  body: {
    type: 'object',
    required: ['name', 'email'],
    properties: {
      name: { type: 'string' },
      email: { type: 'string' },
    },
  },
  response: { 201: userSchema },
};

/** Registers all user CRUD routes on the given Fastify instance */
export default async function userRoutes(app) {
  // GET / — list all users
  app.get('/', async () => users);

  // GET /:id — get a single user by ID
  app.get('/:id', async (req, reply) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    if (!user) return reply.code(404).send({ error: 'User not found' });
    return user;
  });

  // POST / — create a new user (validated by schema)
  app.post('/', { schema: createUserSchema }, async (req, reply) => {
    const { name, email } = req.body;
    const user = { id: nextId++, name, email };
    users.push(user);
    return reply.code(201).send(user);
  });

  // PUT /:id — update an existing user's name and/or email
  app.put('/:id', async (req, reply) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    if (!user) return reply.code(404).send({ error: 'User not found' });
    const { name, email } = req.body;
    if (name)  user.name = name;
    if (email) user.email = email;
    return user;
  });

  // DELETE /:id — delete a user by ID
  app.delete('/:id', async (req, reply) => {
    const idx = users.findIndex(u => u.id === parseInt(req.params.id));
    if (idx === -1) return reply.code(404).send({ error: 'User not found' });
    users.splice(idx, 1);
    return reply.code(204).send();
  });
}
