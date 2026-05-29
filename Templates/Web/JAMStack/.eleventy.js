// Eleventy (11ty) configuration for the JAMStack template
module.exports = function(eleventyConfig) {
  // Copy static assets directly to the output directory
  eleventyConfig.addPassthroughCopy("src/style.css");
  eleventyConfig.addPassthroughCopy("src/script.js");

  return {
    // Directory structure configuration
    dir: {
      input: "src",
      output: "_site",
      includes: "_includes",
      layouts: "_layouts"
    },
    // Supported template formats and template engines
    templateFormats: ["html", "njk", "md"],
    markdownTemplateEngine: "njk",
    htmlTemplateEngine: "njk"
  };
};
