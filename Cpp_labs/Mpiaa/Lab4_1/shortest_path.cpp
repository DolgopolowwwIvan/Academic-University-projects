#include <queue>
#include <unordered_map>
#include <vector>
#include <limits>
#include <algorithm>
#include "graph.h"


using namespace std;

vector<int> build_path(const unordered_map<int, int>& parent, int start, int end) {
    vector<int> path;
    int current = end;
    while (current != start) {
        path.push_back(current);
        auto it = parent.find(current);
        if (it == parent.end()) {
            return {}; // Путь не существует
        }
        current = it->second;
    }
    path.push_back(start);
    reverse(path.begin(), path.end());
    return path;
}


vector<int> shortest_path(const Graph& G, int start, int end) {
    if (start == end) {
        return {};  // Возвращаем пустой путь, если стартовая и конечная вершины совпадают
    }

    unordered_map<int, double> distance;
    unordered_map<int, int> parent;
    const double INF = numeric_limits<double>::infinity();

    for (int v : G.get_vertices()) {
        distance[v] = INF;
    }
    distance[start] = 0.0;

    using QueueElem = pair<double, int>;
    priority_queue<QueueElem, vector<QueueElem>, greater<>> queue;
    queue.push({ 0.0, start });

    while (!queue.empty()) {
        int u = queue.top().second;
        double dist_u = queue.top().first;
        queue.pop();

        if (u == end) {
            return build_path(parent, start, end);
        }

        if (dist_u > distance[u]) continue;

        for (const auto& edge : G.get_adjacent_edges(u)) {
            int v = edge.first;
            double weight = edge.second;
            double new_distance = distance[u] + weight;
            if (new_distance < distance[v]) {
                distance[v] = new_distance;
                parent[v] = u;
                queue.push({ new_distance, v });
            }
        }
    }

    return {}; // Путь не найден
}
