#include "graph.h"

#include <vector>

/// Solve Travelling Salesman Problem (TSP) for the graph:
/// Find the shortest (with a minimal total weight) tour and return it as an array of vertices.
std::vector<int> tsp_local(const Graph& graph);
double calculate_path_length(const Graph& graph, const std::vector<int>& path);
std::vector<int> tsp_bruteforce(const Graph& graph);
std::vector<int> tsp_BnB(const Graph& graph);
std::vector<int> tsp_greed(const Graph& graph);
std::vector<int> tsp_GA(const Graph& graph, int P, int N, int max_iter, double Pm);
double route_length(const Graph& graph, const vector<int>& route);