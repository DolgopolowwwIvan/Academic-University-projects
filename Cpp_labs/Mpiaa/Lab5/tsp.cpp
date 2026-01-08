#include "tsp.h"
#include <vector>
#include <algorithm>
#include <limits>
#include <random>
#include <numeric>
#include <cmath>
#include <iostream>
#include <stdexcept>
#include <unordered_set>
#include <unordered_map>
#include <functional>
#include <utility>

using namespace std;
//Полный перебор

vector<int> tsp_bruteforce(const Graph& graph) {
    vector<int> vertices = graph.get_vertices();

    // Если граф пуст или содержит только одну вершину
    if (vertices.empty() || vertices.size() == 1) {
        return {};
    }

    int start = vertices[0]; // Фиксируем стартовую вершину
    vector<int> best_path;
    double min_cost = numeric_limits<double>::infinity();

    // Убираем стартовую вершину из множества для перестановок
    vertices.erase(remove(vertices.begin(), vertices.end(), start), vertices.end());

    // Перебор всех перестановок вершин, кроме стартовой
    do {
        double current_cost = 0;
        bool valid_path = true;
        vector<int> current_path = { start };

        // Проверяем путь и считаем его стоимость
        int current_vertex = start;
        for (int next_vertex : vertices) {
            if (!graph.has_edge(current_vertex, next_vertex)) {
                valid_path = false;
                break;
            }
            current_cost += graph.edge_weight(current_vertex, next_vertex);
            current_path.push_back(next_vertex);
            current_vertex = next_vertex;
        }

        // Проверяем замыкание цикла, если это нужно
        if (valid_path && graph.has_edge(current_vertex, start)) {
            current_cost += graph.edge_weight(current_vertex, start);
        }

        // Обновляем минимальную стоимость, если нашли валидный путь
        if (valid_path && current_cost < min_cost) {
            min_cost = current_cost;
            best_path = current_path;
        }
    } while (next_permutation(vertices.begin(), vertices.end()));

    return best_path; // Возвращаем путь без двойного включения начальной вершины
}

//Ветвей и границ 

// Функция для вычисления длины пути
double length(const vector<int>& path, const Graph& graph) {
    double total_length = 0;
    for (size_t i = 1; i < path.size(); ++i) {
        if (!graph.has_edge(path[i - 1], path[i])) {
            return numeric_limits<double>::infinity(); // Если ребро отсутствует, возвращаем бесконечность
        }
        total_length += graph.edge_weight(path[i - 1], path[i]);
    }

    // Добавляем обратно в стартовую вершину
    if (graph.has_edge(path.back(), path[0])) {
        total_length += graph.edge_weight(path.back(), path[0]);
    }
    else {
        return numeric_limits<double>::infinity(); // Если путь не замкнут, возвращаем бесконечность
    }

    return total_length;
}

// Функция для вычисления нижней границы для подмножества вершин
double lower_bound(const Graph& graph, const vector<int>& visited) {
    unordered_set<int> visited_set(visited.begin(), visited.end());
    double bound = 0;

    for (int vertex : graph.get_vertices()) {
        if (visited_set.count(vertex)) continue; // Пропускаем уже посещенные вершины

        // Находим два минимальных рёбер для каждой вершины
        vector<pair<int, double>> edges = graph.get_adjacent_edges(vertex);
        double first_min = numeric_limits<double>::infinity();
        double second_min = numeric_limits<double>::infinity();

        for (const auto& edge : edges) {
            int neighbor = edge.first;
            double weight = edge.second;
            if (visited_set.count(neighbor)) continue;

            // Находим два минимальных рёбер
            if (weight < first_min) {
                second_min = first_min;
                first_min = weight;
            }
            else if (weight < second_min) {
                second_min = weight;
            }
        }


        // Для каждой вершины добавляем два минимальных рёбер
        if (first_min < numeric_limits<double>::infinity()) {
            bound += first_min;
        }
        if (second_min < numeric_limits<double>::infinity()) {
            bound += second_min;
        }
    }

    return bound / 2.0; // Делим на 2, так как каждое ребро учитывается дважды
}

// Функция для нахождения минимального пути из двух
vector<int> min_path(const vector<int>& path1, const vector<int>& path2, const Graph& graph) {
    return length(path1, graph) < length(path2, graph) ? path1 : path2;
}

// Основной рекурсивный метод ветвей и границ
vector<int> bnb(const Graph& graph, vector<int> visited, vector<int> best_path, double& best_length) {
    if (visited.size() == graph.get_vertices().size()) {
        // Если все вершины посещены, то возвращаем минимальный путь
        double current_length = length(visited, graph);
        if (current_length < best_length) {
            best_length = current_length;
            best_path = visited; // Обновляем лучший путь
        }
        return best_path;
    }

    for (int next_vertex : graph.get_vertices()) {
        if (find(visited.begin(), visited.end(), next_vertex) != visited.end()) continue;

        vector<int> next_visited = visited;
        next_visited.push_back(next_vertex);

        // Вычисляем нижнюю границу для нового подмножества
        double bound = lower_bound(graph, next_visited);

        if (bound < best_length) {
            // Если нижняя граница меньше текущей длины самого короткого пути, продолжаем рекурсию
            best_path = bnb(graph, next_visited, best_path, best_length);
        }
    }

    return best_path;
}

// Главная функция для решения задачи коммивояжера с использованием метода ветвей и границ
vector<int> tsp_BnB(const Graph& graph) {
    int start = 0;
    vector<int> best_path;
    double best_length = numeric_limits<double>::infinity();

    // Инициализируем путь с начальной вершины
    vector<int> visited = { start };

    // Вызываем рекурсивный метод ветвей и границ
    return bnb(graph, visited, best_path, best_length);
}


//Жадный

vector<int> tsp_greed(const Graph& graph) {
    int start = 0;  // Стартовая вершина
    auto vertices = graph.get_vertices();

    // Если граф пуст или имеет только одну вершину
    if (vertices.empty() || vertices.size() == 1) {
        return {};
    }

    vector<int> path;               // Список для хранения пути
    unordered_set<int> visited;     // Множество для отслеживания посещенных вершин
    visited.insert(start);          // Добавляем стартовую вершину в посещенные
    path.push_back(start);          // Стартовая вершина - начало пути

    int current = start;            // Текущая вершина для начала алгоритма

    // Пока не пройдено все вершины
    while (visited.size() < graph.get_vertices().size()) {
        double min_weight = numeric_limits<double>::infinity(); // Минимальный вес
        int next = -1; // Следующая вершина для посещения

        // Перебираем смежные вершины
        for (const auto& edge : graph.get_adjacent_edges(current)) {
            int neighbor = edge.first;    // Соседняя вершина
            double weight = edge.second;  // Вес ребра

            if (visited.count(neighbor) == 0 && weight < min_weight) {
                min_weight = weight;
                next = neighbor;           // Обновляем следующую вершину
            }
        }

        // Если есть следующая вершина
        if (next != -1) {
            path.push_back(next);         // Добавляем найденную вершину в путь
            visited.insert(next);         // Помечаем вершину как посещенную
            current = next;               // Переходим к следующей вершине
        }
        else {
            break;  // Если не удается найти следующую вершину, завершить
        }
    }


    // Проверяем, можно ли замкнуть цикл (возвращаемся в стартовую вершину)
    for (const auto& edge : graph.get_adjacent_edges(current)) {
        if (edge.first == start) {
            path.push_back(start);  // Замыкаем путь только если есть ребро к старту
            break;
        }
    }

    // Убираем повторное появление вершины 0, если она добавилась из-за логики выше
    if (path.size() > 1 && path.front() == path.back()) {
        path.pop_back();
    }

    return path;  // Возвращаем сформированный путь
}

//Локальный поиск

double calculate_path_length(const Graph& graph, const vector<int>& path) {
    if (path.size() < 2) return 0.0;
    double length = 0.0;
    for (size_t i = 0; i < path.size() - 1; ++i) {
        length += graph.edge_weight(path[i], path[i + 1]);
    }
    length += graph.edge_weight(path.back(), path.front()); // Close the cycle
    return length;
}

// Пользовательская хэш-функция для std::pair
struct pair_hash {
    template <class T1, class T2>
    std::size_t operator()(const std::pair<T1, T2>& pair) const {
        auto hash1 = std::hash<T1>{}(pair.first);
        auto hash2 = std::hash<T2>{}(pair.second);
        return hash1 ^ (hash2 << 1); // Комбинируем два хэша
    }
};

// Используем мемоизацию для хранения весов рёбер
unordered_map<pair<int, int>, double, pair_hash> edge_weight_cache;

double edge_weight_with_cache(const Graph& graph, int u, int v) {
    if (u > v) swap(u, v);
    auto edge = make_pair(u, v);
    if (edge_weight_cache.find(edge) == edge_weight_cache.end()) {
        edge_weight_cache[edge] = graph.edge_weight(u, v);
    }
    return edge_weight_cache[edge];
}

vector<int> transform_path(const vector<int>& path, size_t i, size_t k) {
    vector<int> new_path(path.begin(), path.begin() + i);
    new_path.insert(new_path.end(), path.rbegin() + (path.size() - k - 1), path.rend() - i);
    new_path.insert(new_path.end(), path.begin() + k + 1, path.end());
    return new_path;
}

vector<int> two_opt_improve(const Graph& graph, const vector<int>& path) {
    double best_length = calculate_path_length(graph, path);
    vector<int> best_path = path;

    // Пробуем оптимизировать путь, ограничивая количество проверок
    size_t iterations_without_improvement = 0;
    size_t max_iterations_without_improvement = 10; // Можно настроить

    for (size_t i = 1; i < path.size() - 1 && iterations_without_improvement < max_iterations_without_improvement; ++i) {
        for (size_t k = i + 1; k < path.size(); ++k) {
            vector<int> new_path = transform_path(path, i, k);
            double new_length = calculate_path_length(graph, new_path);
            if (new_length < best_length) {
                best_length = new_length;
                best_path = new_path;
                iterations_without_improvement = 0; // Сбросить счётчик улучшений
                break;
            }
        }

        if (iterations_without_improvement < max_iterations_without_improvement) {
            ++iterations_without_improvement;
        }
    }

    return best_path;
}

vector<int> tsp_local(const Graph& graph) {
    vector<int> vertices = graph.get_vertices();
    if (vertices.empty()) return {};

    // Случайный путь
    random_device rd;
    mt19937 g(rd());
    shuffle(vertices.begin(), vertices.end(), g);

    vector<int> current_path = vertices;
    double current_length = calculate_path_length(graph, current_path);

    while (true) {
        vector<int> improved_path = two_opt_improve(graph, current_path);
        double improved_length = calculate_path_length(graph, improved_path);

        if (improved_length < current_length) {
            current_path = improved_path;
            current_length = improved_length;
        }
        else {
            break;
        }
    }

    return current_path;
}

// GEN Генерация случайного числа в диапазоне [a, b)
double random_double(double a, double b) {
    static random_device rd;
    static mt19937 gen(rd());
    uniform_real_distribution<> dis(a, b);
    return dis(gen);
}

// Функция для вычисления длины маршрута
double route_length(const Graph& graph, const vector<int>& route) {
    double length = 0.0;
    for (size_t i = 0; i < route.size() - 1; ++i) {
        if (!graph.has_edge(route[i], route[i + 1])) {
            throw std::runtime_error("Ошибка: Ребро отсутствует между " +
                                     std::to_string(route[i]) + " и " + std::to_string(route[i + 1]));
        }
        length += graph.edge_weight(route[i], route[i + 1]);
    }
    if (!graph.has_edge(route.back(), route.front())) {
        throw std::runtime_error("Ошибка: Ребро отсутствует между " +
                                 std::to_string(route.back()) + " и " + std::to_string(route.front()));
    }
    length += graph.edge_weight(route.back(), route.front());
    return length;
}

// Проверка корректности маршрута
bool isValidRoute(const vector<int>& route, const Graph& graph) {
    unordered_set<int> visited;
    for (size_t i = 0; i < route.size(); ++i) {
        int u = route[i];
        int v = route[(i + 1) % route.size()]; // Циклический маршрут
        if (!graph.has_edge(u, v)) {
            cerr << "Ребро отсутствует между " << u << " и " << v << endl;
            return false;
        }
        visited.insert(u);
    }
    return visited.size() == graph.get_vertices().size();
}

// Функция для вычисления веса хромосомы
double fitness(const Graph& graph, const vector<int>& route, double max_length, double min_length) {
    double length = route_length(graph, route);
    if (max_length == min_length) {
        max_length += 1.0; // Избегаем деления на 0
    }
    return (max_length - length) / (max_length - min_length);
}

// Операция скрещивания (CrossoverER)
vector<int> crossover_ER(const vector<int>& parent1, const vector<int>& parent2, const Graph& graph) {
    vector<int> offspring;
    vector<bool> visited(graph.get_vertices().size(), false);

    int current_vertex = parent1[0];
    visited[current_vertex] = true;
    offspring.push_back(current_vertex);

    while (offspring.size() < parent1.size()) {
        vector<int> candidates;
        for (int vertex : graph.get_adjacent_vertices(current_vertex)) {
            if (!visited[vertex]) {
                candidates.push_back(vertex);
            }
        }

        if (!candidates.empty()) {
            current_vertex = candidates[static_cast<int>(random_double(0, candidates.size()))];
        } else {
            current_vertex = parent2[static_cast<int>(random_double(0, parent2.size()))];
        }

        if (visited[current_vertex]) {
            throw runtime_error("No valid candidates during crossover. Check graph connectivity.");
        }

        visited[current_vertex] = true;
        offspring.push_back(current_vertex);
    }

    if (!isValidRoute(offspring, graph)) {
        throw runtime_error("Некорректный маршрут после кроссовера.");
    }

    return offspring;
}

// Мутация хромосомы (TwoOptImprovement)
vector<int> mutate(const vector<int>& route) {
    vector<int> mutated_route = route;
    int i = static_cast<int>(random_double(0, route.size()));
    int j = static_cast<int>(random_double(0, route.size()));
    while (i == j) {
        j = static_cast<int>(random_double(0, route.size()));
    }
    if (i > j) swap(i, j); // Убедимся, что i <= j
    reverse(mutated_route.begin() + i, mutated_route.begin() + j);

    return mutated_route;
}

// Операция селекции SUS (Stochastic Universal Sampling)
vector<vector<int>> SUS(const vector<vector<int>>& population, const vector<double>& fitness_scores, int n) {
    double total_fitness = accumulate(fitness_scores.begin(), fitness_scores.end(), 0.0);
    if (total_fitness == 0.0) {
        throw runtime_error("Total fitness is zero. Check fitness function or population diversity.");
    }

    double distance = total_fitness / n;
    double start = random_double(0, distance);

    vector<vector<int>> selected;
    size_t current_index = 0;
    double cumulative_fitness = fitness_scores[current_index];

    for (int i = 0; i < n; ++i) {
        double point = start + i * distance;
        while (cumulative_fitness < point) {
            current_index++;
            if (current_index >= fitness_scores.size()) {
                throw runtime_error("Index out of bounds during SUS.");
            }
            cumulative_fitness += fitness_scores[current_index];
        }
        selected.push_back(population[current_index]);
    }
    return selected;
}

// Генетический алгоритм для решения задачи коммивояжера
vector<int> tsp_GA(const Graph& graph, int P, int N, int max_iter, double Pm) {
    if (graph.get_vertices().empty()) {
        throw runtime_error("Graph has no vertices.");
    }

    double max_length = -numeric_limits<double>::max();
    double min_length = numeric_limits<double>::max();

    // Инициализация начального поколения
    vector<vector<int>> population(P);
    random_device rd;
    mt19937 gen(rd());

    for (int i = 0; i < P; ++i) {
        population[i] = graph.get_vertices();
        shuffle(population[i].begin(), population[i].end(), gen);
        if (!isValidRoute(population[i], graph)) {
            throw runtime_error("Некорректный маршрут в начальной популяции.");
        }
    }

    vector<int> best_solution;
    double best_length = numeric_limits<double>::max();

    // Главный цикл генетического алгоритма
    for (int iter = 0; iter < max_iter; ++iter) {
        vector<double> fitness_scores(P);

        // Вычисление максимальной и минимальной длины маршрута
        for (int i = 0; i < P; ++i) {
            double length = route_length(graph, population[i]);
            max_length = max(max_length, length);
            min_length = min(min_length, length);
        }

        // Вычисление значений фитнес-функции для каждой хромосомы
        for (int i = 0; i < P; ++i) {
            fitness_scores[i] = fitness(graph, population[i], max_length, min_length);
        }

        // Выбор родителей через SUS
        auto selected_parents = SUS(population, fitness_scores, N);

        // Создание нового поколения
        vector<vector<int>> offspring;
        for (int i = 0; i < N / 2; ++i) {
            auto parent1 = selected_parents[static_cast<int>(random_double(0, selected_parents.size()))];
            auto parent2 = selected_parents[static_cast<int>(random_double(0, selected_parents.size()))];
            auto child = crossover_ER(parent1, parent2, graph);

            if (random_double(0, 1) < Pm) {
                child = mutate(child);
            }
            offspring.push_back(child);
        }

        population = offspring;

        // Обновление лучшего решения
        for (const auto& route : population) {
            double length = route_length(graph, route);
            if (length < best_length) {
                best_solution = route;
                best_length = length;
            }
        }

        // Отладочная информация
        cout << "Итерация " << iter + 1 << ": Лучшая длина маршрута = " << best_length << endl;
    }

    return best_solution;
}
