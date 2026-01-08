#pragma once

#include "graph.h"

#include <vector>

using namespace std;

/// Return shortest path from start to end as array of vertices.
vector<int> shortest_path(const Graph& graph, int start_vertex, int end_vertex);