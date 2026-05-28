#pragma once
#include <expected>
#include <glad/gl.h>
#include <string>
#include <string_view>

enum class ShaderError {
    CompileError,
    LinkError,
    FileNotFound,
};

class Shader {
public:
    Shader() = default;
    ~Shader();
    Shader(Shader&& other) noexcept;
    Shader& operator=(Shader&& other) noexcept;

    Shader(const Shader&) = delete;
    Shader& operator=(const Shader&) = delete;

    static auto from_files(std::string_view vertex_path,
                           std::string_view fragment_path)
        -> std::expected<Shader, ShaderError>;

    auto use() const -> void;

private:
    explicit Shader(GLuint id) : id_{id} {}
    GLuint id_ = 0;
};
