/**
 * Immutable data record representing a circular game entity with position,
 * velocity, radius, and colour for the LibGDX sample.
 */
package game.libgdx;

public record Entity(float x, float y, float vx, float vy, float radius, float color) {}
