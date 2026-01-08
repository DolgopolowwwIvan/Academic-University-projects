#define CATCH_CONFIG_RUNNER

#include "../catch.hpp"

#include <random>
#include <set>
#include <tuple>
#include "graph.h"
#include "shortest_path.h"

using namespace std;

void measure_shortest_path_time(Graph& G, int start, int end, int iterations) {
    using namespace std::chrono;

    double total_duration = 0.0;

    for (int i = 0; i < iterations; ++i) {
        auto start_time = high_resolution_clock::now();

        vector<int> path = shortest_path(G, start, end);

        auto end_time = high_resolution_clock::now();
        duration<double, std::milli> duration = end_time - start_time;

        total_duration += duration.count();
    }

    double average_duration = total_duration / iterations;
    cout << "Среднее время: " << average_duration << " ms" << endl;
}

Graph generate_random_graph(int num_vertices, int num_edges, int min_weight, int max_weight) {
    Graph g;
    random_device rd;
    mt19937 gen(rd());
    uniform_int_distribution<> weight_dist(min_weight, max_weight);  
    uniform_int_distribution<> vertex_dist(0, num_vertices - 1);

    
    for (int i = 0; i < num_vertices; ++i) {
        g.add_vertex(i);
    }

    
    for (int i = 1; i < num_vertices; ++i) {
        int u = i;
        int v = vertex_dist(gen) % i;  
        int weight = weight_dist(gen);  
        g.add_edge(u, v, weight);
    }

    
    int added_edges = num_vertices - 1;  
    set<pair<int, int>> existing_edges;

    // Сохраняем добавленные рёбра, чтобы избежать дублирования
    for (int i = 0; i < num_vertices; ++i) {
        for (const auto& edge : g.get_adjacent_edges(i)) {
            existing_edges.insert({ min(i, edge.first), max(i, edge.first) });
        }
    }

    while (added_edges < num_edges) {
        int u = vertex_dist(gen);
        int v = vertex_dist(gen);
        if (u != v && existing_edges.find({ min(u, v), max(u, v) }) == existing_edges.end()) {
            int weight = weight_dist(gen);  //целочисленный вес
            g.add_edge(u, v, weight);
            existing_edges.insert({ min(u, v), max(u, v) });
            ++added_edges;
        }
    }

    return g;
}

int main(int argc, char* argv[]) {
     
    setlocale(LC_ALL, "RU");

    int result = Catch::Session().run(argc, argv);

    int num_vertices = 1000;  
    int num_edges = 1000;     
    int min_weight = 1;
    int max_weight = 50;

    Graph G = generate_random_graph(num_vertices, num_edges, min_weight, max_weight);

    int start = 0;
    int end = num_vertices - 1;

    // Запуск тестов времен
    measure_shortest_path_time(G, start, end, 100);  

    return result;

    
}
