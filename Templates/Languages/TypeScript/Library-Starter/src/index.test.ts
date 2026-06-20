import { describe, it, expect } from "vitest";
import { greet, VERSION } from "./index.js";

describe("greet", () => {
  it("returns a greeting", () => {
    expect(greet("World")).toBe("Hello, World!");
  });
});

describe("VERSION", () => {
  it("is a semver-ish string", () => {
    expect(VERSION).toMatch(/^\d+\.\d+\.\d+/);
  });
});