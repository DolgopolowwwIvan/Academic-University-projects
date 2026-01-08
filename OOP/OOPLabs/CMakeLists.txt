cmake_minimum_required(VERSION 3.10)
project(PearsonDistribution)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Для Wine - не используем Windows-specific флаги
if(NOT WIN32)
    # Для Linux/Unix через Wine
    add_definitions(-D_USE_MATH_DEFINES)
endif()

# Включение предупреждений
if(MSVC)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /W4")
else()
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -Wextra -pedantic")
endif()

# Основная библиотека распределения Пирсона
add_library(pearson_distribution pearson_distribution.cpp)

# Библиотека для смеси распределений
add_library(mixture mixture.cpp)

# Библиотека для эмпирического распределения
add_library(empiric empiric.cpp)

# Библиотека для экспорта данных
add_library(data_exporter data_exporter.cpp)

# Установка свойств для библиотек
target_include_directories(pearson_distribution PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})
target_include_directories(mixture PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})
target_include_directories(empiric PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})
target_include_directories(data_exporter PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})

# Основная программа
add_executable(main main.cpp)
target_link_libraries(main pearson_distribution mixture empiric data_exporter)

# Тестирующая программа
add_executable(test_pearson test_pearson.cpp)
target_link_libraries(test_pearson pearson_distribution mixture empiric data_exporter)