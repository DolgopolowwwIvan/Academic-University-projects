cmake_minimum_required(VERSION 3.10)
project(PearsonDistributionLab4)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Флаги компиляции
if(MSVC)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /W4")
else()
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -Wextra -pedantic")
endif()

# Интерфейсы
add_library(idistribution INTERFACE)
target_include_directories(idistribution INTERFACE ${CMAKE_CURRENT_SOURCE_DIR})

add_library(ipersistent INTERFACE)
target_include_directories(ipersistent INTERFACE ${CMAKE_CURRENT_SOURCE_DIR})

# Основные классы
add_library(pearson_distribution pearson_distribution.cpp)
target_link_libraries(pearson_distribution idistribution ipersistent)

add_library(empiric empiric.cpp)
target_link_libraries(empiric idistribution ipersistent)

# Шаблон mixture находится только в заголовочном файле

# Тестирующие программы
add_executable(test_lab4 test_lab4.cpp)
target_link_libraries(test_lab4 pearson_distribution empiric)

add_executable(main main.cpp)
target_link_libraries(main pearson_distribution empiric)