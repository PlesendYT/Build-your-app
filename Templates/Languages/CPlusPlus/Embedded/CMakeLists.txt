cmake_minimum_required(VERSION 3.25)
project(Embedded VERSION 1.0.0 LANGUAGES CXX)
set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
add_executable(embedded src/main.cpp)
# For ARM cross-compilation:
# set(CMAKE_C_COMPILER arm-none-eabi-gcc)
# set(CMAKE_CXX_COMPILER arm-none-eabi-g++)
# target_compile_options(embedded PRIVATE -mcpu=cortex-m4 -mthumb -nostdlib)
