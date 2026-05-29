// Prisma ORM CRUD example with User and Post models
// Run: npm run dev

import { PrismaClient } from "@prisma/client";

const prisma = new PrismaClient();

async function main() {
  // CREATE: Insert a new user with a related post
  const user = await prisma.user.create({
    data: {
      name: "Alice",
      email: "alice@example.com",
      posts: {
        create: { title: "Hello World", content: "My first post!", published: true },
      },
    },
    include: { posts: true },
  });
  console.log("Created user with posts:", JSON.stringify(user, null, 2));

  // READ: Find all published posts with their author
  const posts = await prisma.post.findMany({
    where: { published: true },
    include: { author: true },
  });
  console.log("Published posts:", posts);

  // UPDATE: Publish all unpublished posts by Alice
  const updated = await prisma.post.updateMany({
    where: { author: { email: "alice@example.com" }, published: false },
    data: { published: true },
  });
  console.log(`Updated ${updated.count} posts`);

  // DELETE: Remove the user we created (cascades to posts)
  const deleted = await prisma.user.delete({ where: { email: "alice@example.com" } });
  console.log("Deleted user:", deleted.id);
}

main()
  .catch(console.error)
  .finally(() => prisma.$disconnect());
