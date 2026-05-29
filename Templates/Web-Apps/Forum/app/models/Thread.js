// Thread model - in-memory data store for forum threads and posts
// Replace with a database adapter in production
const threads = [];
const posts = [];
let nextThreadId = 1;
let nextPostId = 1;

const Thread = {
  // Return all threads ordered by newest first
  findAll() {
    return [...threads].reverse();
  },

  // Find a single thread by its ID
  findById(id) {
    return threads.find(t => t.id === Number(id)) || null;
  },

  // Create a new thread with title, content, and author info
  create({ title, content, userId, username }) {
    const thread = { id: nextThreadId++, title, content, userId, username, createdAt: new Date(), postCount: 0 };
    threads.push(thread);
    return thread;
  },

  // Get all posts belonging to a specific thread
  findPostsByThreadId(threadId) {
    return posts.filter(p => p.threadId === Number(threadId));
  },

  // Add a post to a thread
  addPost({ threadId, content, userId, username }) {
    const post = { id: nextPostId++, threadId: Number(threadId), content, userId, username, createdAt: new Date() };
    posts.push(post);
    const thread = threads.find(t => t.id === Number(threadId));
    if (thread) thread.postCount++;
    return post;
  }
};

module.exports = Thread;
