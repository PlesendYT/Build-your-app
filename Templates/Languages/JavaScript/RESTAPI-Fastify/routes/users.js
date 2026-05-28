const users = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob',   email: 'bob@example.com' },
];
let nextId = 3;

const userSchema = {
  type: 'object',
  properties: {
    id: { type: 'integer' },
    name: { type: 'string' },
    email: { type: 'string' },
  },
};

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

export default async function userRoutes(app) {
  app.get('/', async () => users);

  app.get('/:id', async (req, reply) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    if (!user) return reply.code(404).send({ error: 'User not found' });
    return user;
  });

  app.post('/', { schema: createUserSchema }, async (req, reply) => {
    const { name, email } = req.body;
    const user = { id: nextId++, name, email };
    users.push(user);
    return reply.code(201).send(user);
  });

  app.put('/:id', async (req, reply) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    if (!user) return reply.code(404).send({ error: 'User not found' });
    const { name, email } = req.body;
    if (name)  user.name = name;
    if (email) user.email = email;
    return user;
  });

  app.delete('/:id', async (req, reply) => {
    const idx = users.findIndex(u => u.id === parseInt(req.params.id));
    if (idx === -1) return reply.code(404).send({ error: 'User not found' });
    users.splice(idx, 1);
    return reply.code(204).send();
  });
}
