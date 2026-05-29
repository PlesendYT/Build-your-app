// Shader class: loads, compiles, and links OpenGL shader programs.
#pragma once
#include <expected>
#include <glad/gl.h>
#include <string>
#include <string_view>

// Errors that can occur during shader loading.
enum class ShaderError {
    CompileError,
    LinkError,
    FileNotFound,
};

// RAII wrapper for an OpenGL shader program.
class Shader {
public:
    Shader() = default;
    ~Shader();
    Shader(Shader&& other) noexcept;
    Shader& operator=(Shader&& other) noexcept;

    Shader(const Shader&) = delete;
    Shader& operator=(const Shader&) = delete;

    // Load shaders from files, compile and link them.
    static auto from_files(std::string_view vertex_path,
                           std::string_view fragment_path)
        -> std::expected<Shader, ShaderError>;

    // Bind this shader program for rendering.
    auto use() const -> void;

private:
    explicit Shader(GLuint id) : id_{id} {}
    GLuint id_ = 0;
};
