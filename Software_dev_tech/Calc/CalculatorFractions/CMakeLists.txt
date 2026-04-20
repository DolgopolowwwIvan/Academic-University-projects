# CMakeLists.txt - Файл сборки для Linux
# Для Windows используйте C++ Builder 2022

cmake_minimum_required(VERSION 3.10)
project(CalculatorFractions)

# Стандарт C++17 (требуется для std::gcd)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Исходные файлы
set(SOURCES
    src/UFrac.cpp
    src/UEditor.cpp
    src/UControl.cpp
    src/main.cpp
)

# Заголовочные файлы
set(HEADERS
    src/UFrac.h
    src/UEditor.h
    src/UMemory.h
    src/UProc.h
    src/UControl.h
    src/UClcPnl.h
)

# Создаём исполняемый файл
add_executable(CalculatorFractions ${SOURCES} ${HEADERS})

# Включаем директорию с заголовками
target_include_directories(CalculatorFractions PRIVATE ${CMAKE_SOURCE_DIR}/src)

# Компиляция с предупреждениями
if(CMAKE_CXX_COMPILER_ID MATCHES "GNU|Clang")
    target_compile_options(CalculatorFractions PRIVATE -Wall -Wextra)
endif()
