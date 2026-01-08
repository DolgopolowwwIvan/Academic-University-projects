# Минимальная версия CMake
cmake_minimum_required(VERSION 3.10)

# Название проекта
project(GraphProject)

# Укажите стандарт C++
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

# Флаги компиляции
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -Wextra -Wpedantic -Wshadow -Wformat=2 -Wcast-align -Wconversion -Wsign-conversion -Wnull-dereference -g3 -O0")

# Укажите исполняемый файл и исходные файлы
add_executable(GraphProject
    main.cpp
    graph.cpp
    shortest_path.cpp
    graph_test.cpp
    shortest_path_test.cpp
)
# Добавляем тест для shortest_path
add_executable(ShortestPathTest
    shortest_path_test.cpp
    graph.cpp
    shortest_path.cpp
)