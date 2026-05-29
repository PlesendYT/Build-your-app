// Page model - in-memory store for wiki pages with version history
// Replace with a database adapter in production
const pages = [];
const history = [];
let nextId = 1;

const Page = {
  // Get all pages sorted by creation date descending
  findAll() {
    return [...pages].reverse();
  },

  // Find a page by its URL slug
  findBySlug(slug) {
    return pages.find(p => p.slug === slug) || null;
  },

  // Create a new page with slug generated from the title
  create({ title, content }) {
    const slug = title.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/^-|-$/g, '');
    const page = { id: nextId++, title, slug, content, createdAt: new Date(), updatedAt: new Date() };
    pages.push(page);
    // Save initial version in history
    history.push({ slug, content, version: 1, savedAt: new Date() });
    return page;
  },

  // Update page content and save a new history entry
  update(slug, content) {
    const page = pages.find(p => p.slug === slug);
    if (!page) return null;
    page.content = content;
    page.updatedAt = new Date();
    const version = history.filter(h => h.slug === slug).length + 1;
    history.push({ slug, content, version, savedAt: new Date() });
    return page;
  },

  // Get all historical versions for a page
  getHistory(slug) {
    return history.filter(h => h.slug === slug);
  }
};

module.exports = Page;
