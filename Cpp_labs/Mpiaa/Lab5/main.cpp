#define CATCH_CONFIG_RUNNER

#include "../catch.hpp"

#include <iostream>
#include <vector>   // Для std::vector
#include <limits>
#include <random>
#include <chrono>
#include <string>
#include "graph.h"  // Ваш файл с классом Graph
#include "tsp.h"

Graph generateRandomGraph(int size) {
    Graph graph;
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_real_distribution<> distrib(1.0, 100.0);

    // Добавляем вершины
    for (int i = 0; i < size; ++i) {
        graph.add_vertex(i);
    }

    // Шаг 1: Создаем связное остовное дерево
    std::vector<int> vertices(size);
    std::iota(vertices.begin(), vertices.end(), 0); // [0, 1, 2, ..., size-1]
    std::shuffle(vertices.begin(), vertices.end(), gen);

    for (int i = 1; i < size; ++i) {
        int u = vertices[i - 1];
        int v = vertices[i];
        double weight = distrib(gen);
        graph.add_edge(u, v, weight);
    }

    // Шаг 2: Добавляем дополнительные случайные рёбра
    for (int i = 0; i < size; ++i) {
        for (int j = i + 1; j < size; ++j) {
            if (!graph.has_edge(i, j)) { // Добавляем только если ребра ещё нет
                double weight = distrib(gen);
                graph.add_edge(i, j, weight);
            }
        }
    }

    return graph;
}

// Функция для вывода пути, его веса и времени работы алгоритма
void measureTime(const Graph& graph, std::function<std::vector<int>(const Graph&)> func, const std::string& name) {
    // Запуск замера времени
    auto start = std::chrono::high_resolution_clock::now();

    // Запускаем алгоритм
    auto result = func(graph);

    // Завершаем замер времени
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;

    // Вычисляем вес пути с использованием calculate_path_length
    double weight = calculate_path_length(graph, result);

    std::cout << "\n" << name << " Path Weight: " << weight << std::endl;
    std::cout << name << " Time: " << duration.count() << " seconds\n";
}

void measureTimeForGA(const Graph& graph, int P, int N, int max_iter, double Pm, const std::string& name) {
    try {
        auto start = std::chrono::high_resolution_clock::now(); // Начало замера времени

        // Запуск генетического алгоритма
        auto best_solution = tsp_GA(graph, P, N, max_iter, Pm);

        auto end = std::chrono::high_resolution_clock::now(); // Конец замера времени
        std::chrono::duration<double> duration = end - start;

        // Вычисление веса пути
        double total_weight = route_length(graph, best_solution);

        // Вывод результатов
        std::cout << "\n" << name << " Results:" << std::endl;
        std::cout << "Best Path: ";
        for (size_t i = 0; i < best_solution.size(); ++i) {
            std::cout << best_solution[i] << (i + 1 < best_solution.size() ? " -> " : "");
        }
        std::cout << "\nTotal Weight: " << total_weight << std::endl;
        std::cout << "Execution Time: " << duration.count() << " seconds\n";
    } catch (const std::exception& e) {
        std::cerr << "Error in " << name << ": " << e.what() << std::endl;
    }
}

int main(int argc, char* argv[]) {
    setlocale(LC_ALL, "RU");
    int result = Catch::Session().run(argc, argv);

    // Список размеров графов для тестирования
    std::vector<int> graph_sizes = { 3, 4, 5, 6, 7, 8, 9, 10, 100, 1000};

    // Тестирование для каждого размера графа
    for (int size : graph_sizes) {
        std::cout << "\n--- Тестирование графа с " << size << " вершинами ---\n";

        // Генерация случайного графа
        Graph graph = generateRandomGraph(size);

        // Замеры времени для каждого алгоритма
        if (size <= 10) {
            measureTime(graph, tsp_BnB, "tsp_BnB");
            
        }
        if(size <= 100){
             measureTime(graph, tsp_bruteforce, "tsp_bruteforce");
             measureTime(graph, tsp_local, "tsp_local");
        }
        measureTime(graph, tsp_greed, "tsp_greed");
        
        measureTimeForGA(graph, 10, 5, 100, 0.2, "Genetic Algorithm");
    }

    return result;
}
