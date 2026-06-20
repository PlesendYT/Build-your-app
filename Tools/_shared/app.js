// Shared helpers for the Build-your-app tools.
(function () {
  function el(html) {
    var t = document.createElement("template");
    t.innerHTML = html.trim();
    return t.content.firstElementChild;
  }
  function qs(sel, root) { return (root || document).querySelector(sel); }
  function qsa(sel, root) { return Array.prototype.slice.call((root || document).querySelectorAll(sel)); }

  function toast(message, kind) {
    var wrap = qs(".toast-wrap");
    if (!wrap) { wrap = el('<div class="toast-wrap"></div>'); document.body.appendChild(wrap); }
    var t = el('<div class="toast"></div>');
    if (kind) t.classList.add(kind);
    t.textContent = message;
    wrap.appendChild(t);
    setTimeout(function () { t.style.opacity = "0"; t.style.transition = "opacity .25s"; }, 1800);
    setTimeout(function () { if (t.parentNode) t.parentNode.removeChild(t); }, 2300);
  }

  function copyText(text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
      return navigator.clipboard.writeText(text).then(function () { return true; }).catch(function () { return fallback(text); });
    }
    return Promise.resolve(fallback(text));
  }
  function fallback(text) {
    var ta = document.createElement("textarea");
    ta.value = text;
    ta.style.position = "fixed"; ta.style.opacity = "0";
    document.body.appendChild(ta);
    ta.select();
    var ok = false;
    try { ok = document.execCommand("copy"); } catch (e) { ok = false; }
    document.body.removeChild(ta);
    return ok;
  }

  function download(filename, content, mime) {
    var blob = content instanceof Blob ? content : new Blob([content], { type: mime || "text/plain;charset=utf-8" });
    var url = URL.createObjectURL(blob);
    var a = el('<a></a>');
    a.href = url; a.download = filename;
    document.body.appendChild(a); a.click(); document.body.removeChild(a);
    setTimeout(function () { URL.revokeObjectURL(url); }, 500);
  }

  function readFile(file) {
    return new Promise(function (resolve, reject) {
      var fr = new FileReader();
      fr.onload = function () { resolve(fr.result); };
      fr.onerror = function () { reject(fr.error); };
      fr.readAsText(file);
    });
  }

  function debounce(fn, ms) {
    var h;
    return function () {
      var a = arguments, self = this;
      clearTimeout(h);
      h = setTimeout(function () { fn.apply(self, a); }, ms || 200);
    };
  }

  function escapeHtml(s) {
    return String(s).replace(/[&<>"']/g, function (c) {
      return ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[c];
    });
  }

  function mountToolbar(target, opts) {
    var root = typeof target === "string" ? qs(target) : target;
    if (!root) return;
    var bar = el('<div class="toolbar"></div>');
    var inToolsRoot = /\/Tools\/(index\.html)?$/i.test(location.pathname);
    var homeHref = inToolsRoot ? "./index.html" : "../index.html";
    bar.appendChild(el('<div class="brand"><a href="' + homeHref + '">Build-your-app / Tools</a></div>'));
    if (opts && opts.tool) {
      var span = document.createElement("span");
      span.className = "crumb";
      span.textContent = "› " + opts.tool;
      bar.appendChild(span);
    }
    var spacer = el('<span class="spacer"></span>'); bar.appendChild(spacer);
    var hub = el('<a class="btn ghost" href="' + homeHref + '">Hub</a>'); bar.appendChild(hub);
    bar.appendChild(el('<a class="btn ghost" href="https://github.com" target="_blank" rel="noopener">GitHub ↗</a>'));
    root.appendChild(bar);
  }

  window.Tools = { el: el, qs: qs, qsa: qsa, toast: toast, copyText: copyText, download: download, readFile: readFile, debounce: debounce, escapeHtml: escapeHtml, mountToolbar: mountToolbar };
})();