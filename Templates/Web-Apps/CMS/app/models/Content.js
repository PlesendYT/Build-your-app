// Content model - in-memory store for CMS pages and articles
// Replace with a database adapter in production
const contents = [];
let nextId = 1;

const Content = {
  // Get all content items (admin view)
  findAll() {
    return [...contents].reverse();
  },

  // Get only published content (public view)
  findAllPublished() {
    return contents.filter(c => c.published).reverse();
  },

  // Find content by ID
  findById(id) {
    return contents.find(c => c.id === Number(id)) || null;
  },

  // Find content by URL slug
  findBySlug(slug) {
    return contents.find(c => c.slug === slug) || null;
  },

  // Create a new content item
  create({ title, body, published }) {
    const slug = title.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/^-|-$/g, '');
    const content = { id: nextId++, title, slug, body, published: !!published, createdAt: new Date(), updatedAt: new Date() };
    contents.push(content);
    return content;
  },

  // Update an existing content item
  update(id, { title, body, published }) {
    const content = contents.find(c => c.id === Number(id));
    if (!content) return null;
    content.title = title || content.title;
    content.body = body || content.body;
    content.published = published !== undefined ? published : content.published;
    content.updatedAt = new Date();
    return content;
  },

  // Delete a content item
  delete(id) {
    const idx = contents.findIndex(c => c.id === Number(id));
    if (idx !== -1) contents.splice(idx, 1);
  }
};

module.exports = Content;
