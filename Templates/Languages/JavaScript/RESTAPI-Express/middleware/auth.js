// Middleware utilities — error handler and async wrapper for Express

/** Global error handler middleware — logs the error and sends a JSON response */
export function errorHandler(err, _req, res, _next) {
  console.error(err.stack);
  res.status(err.status || 500).json({
    error: err.message || 'Internal Server Error',
  });
}

/** Wraps an async route handler to catch errors and forward to the error handler */
export function asyncHandler(fn) {
  return (req, res, next) => Promise.resolve(fn(req, res, next)).catch(next);
}
