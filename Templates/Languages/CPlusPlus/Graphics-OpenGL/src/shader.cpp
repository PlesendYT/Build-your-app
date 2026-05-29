// Shader class implementation - compiles and links OpenGL shaders.
#include "shader.hpp"
#include <fstream>
#include <sstream>
#include <vector>

// Compile a single shader from source; returns shader ID or error.
static auto compile(GLuint type, std::string_view source) -> std::expected<GLuint, ShaderError> {
    auto id = glCreateShader(type);
    auto* src = source.data();
    auto len = static_cast<GLint>(source.size());
    glShaderSource(id, 1, &src, &len);
    glCompileShader(id);

    GLint success;
    glGetShaderiv(id, GL_COMPILE_STATUS, &success);
    if (!success) {
        GLint log_len;
        glGetShaderiv(id, GL_INFO_LOG_LENGTH, &log_len);
        std::vector<char> log(log_len);
        glGetShaderInfoLog(id, log_len, nullptr, log.data());
        glDeleteShader(id);
        return std::unexpected(ShaderError::CompileError);
    }
    return id;
}

// Read shader source from a file.
static auto read_file(std::string_view path) -> std::expected<std::string, ShaderError> {
    std::ifstream file{path.data()};
    if (!file) return std::unexpected(ShaderError::FileNotFound);
    std::stringstream ss;
    ss << file.rdbuf();
    return ss.str();
}

// Create a shader program from vertex and fragment shader files.
auto Shader::from_files(std::string_view vertex_path, std::string_view fragment_path)
    -> std::expected<Shader, ShaderError> {
    auto vs_src = read_file(vertex_path);
    if (!vs_src) return std::unexpected(vs_src.error());

    auto fs_src = read_file(fragment_path);
    if (!fs_src) return std::unexpected(fs_src.error());

    auto vs = compile(GL_VERTEX_SHADER, *vs_src);
    if (!vs) return std::unexpected(vs.error());

    auto fs = compile(GL_FRAGMENT_SHADER, *fs_src);
    if (!fs) {
        glDeleteShader(*vs);
        return std::unexpected(fs.error());
    }

    GLuint program = glCreateProgram();
    glAttachShader(program, *vs);
    glAttachShader(program, *fs);
    glLinkProgram(program);

    GLint success;
    glGetProgramiv(program, GL_LINK_STATUS, &success);
    glDeleteShader(*vs);
    glDeleteShader(*fs);

    if (!success) {
        glDeleteProgram(program);
        return std::unexpected(ShaderError::LinkError);
    }

    return Shader{program};
}

// Destructor: delete the shader program.
Shader::~Shader() {
    if (id_) glDeleteProgram(id_);
}

// Move constructor: transfer ownership.
Shader::Shader(Shader&& other) noexcept : id_{other.id_} {
    other.id_ = 0;
}

// Move assignment: transfer ownership, clean up old program.
auto Shader::operator=(Shader&& other) noexcept -> Shader& {
    if (this != &other) {
        if (id_) glDeleteProgram(id_);
        id_ = other.id_;
        other.id_ = 0;
    }
    return *this;
}

// Bind the shader program for use.
auto Shader::use() const -> void {
    glUseProgram(id_);
}
