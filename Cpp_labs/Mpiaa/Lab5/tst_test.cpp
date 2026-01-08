#define CATCH_CONFIG_MAIN
#include "../catch.hpp"

#include "tsp.h"

#include <algorithm>

using namespace std;

// For a circilar path get its reverse.
vector<int> reversed(const vector<int>& path) {
    if (path.empty()) {
        return vector<int> {};
    }
    vector<int> result = { path[0] }; // first item is a start vertex
    result.insert(result.end(), path.rbegin(), path.rend()); // add vertices in reverse order
    result.pop_back(); // remove duplicated start vertex
    return result;
}

// Rotate path so it starts with the given vertex.
vector<int> start_with(const vector<int>& path, int vertex) {
    auto it = find(path.begin(), path.end(), vertex);
    if (it == path.end()) {
        return path;
    }
    vector<int> result(it, path.end()); // from start vertex to the path's end
    result.insert(result.end(), path.begin(), it); // add the rest of the path
    return result;
}

// From two possible directions for a circlular path choose one with min second vertex.
vector<int> min_dir(const vector<int>& path) {
    if (path.size() <= 1) {
        return path;
    }
    const auto reversed_path = reversed(path);
    return (path[1] <= reversed_path[1] ? path : reversed_path);
}

vector<int> aligned(const vector<int>& path, int start_vertex) {
    return min_dir(start_with(path, start_vertex));
}

TEST_CASE("[tsp_bruteforce] Empty graph", "[tsp_bruteforce]") {
    Graph g{};
    CHECK(tsp_bruteforce(g) == vector<int> {});
}

TEST_CASE("[tsp_bruteforce] Single vertex", "[tsp_bruteforce]") {
    Graph g{};
    g.add_vertex(0);
    CHECK(tsp_bruteforce(g) == vector<int> {});
}

TEST_CASE("[tsp_bruteforce] One edge", "[tsp_bruteforce]") {
    Graph g{ {0, 1, 2.5} };
    const auto result = tsp_bruteforce(g);
    const auto expected = vector<int>{ 0, 1 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_bruteforce] Three vertices, three edges", "[tsp_bruteforce]") {
    Graph g{ {0, 1, 2.5}, {0, 2, 0.5}, {1, 2, 1.0} };
    const auto result = tsp_bruteforce(g);
    const auto expected = vector<int>{ 0, 1, 2 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_bruteforce] Four vertices", "[tsp_bruteforce]") {
    Graph g{ {0, 1, 6.0}, {0, 2, 4.0}, {0, 3, 1.0},
             {1, 2, 3.5}, {1, 3, 2.0},
             {2, 3, 5.0} };
    const auto result = tsp_bruteforce(g);
    const auto expected = vector<int>{ 0, 2, 1, 3 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_bruteforce] Five vertices", "[tsp_bruteforce]") {
    Graph g{ {0, 1, 2.0}, {0, 2, 4.0}, {0, 3, 1.0}, {0, 4, 2.5},
             {1, 2, 3.6}, {1, 3, 6.0}, {1, 4, 3.0},
             {2, 3, 7.0}, {2, 4, 5.0},
             {3, 4, 9.0} };
    const auto result = tsp_bruteforce(g);
    const auto expected = vector<int>{ 0, 3, 2, 1, 4 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_bruteforce] Six vertices", "[tsp_bruteforce]") {
    Graph g{ {0, 1, 2.0}, {0, 2, 4.0}, {0, 3, 1.0}, {0, 4, 2.5}, {0, 5, 3.2},
             {1, 2, 3.6}, {1, 3, 6.0}, {1, 4, 3.0}, {1, 5, 0.1},
             {2, 3, 7.0}, {2, 4, 5.0}, {2, 5, 9},
             {3, 4, 9.0}, {3, 5, 0.5},
             {4, 5, 1.0} };
    const auto result = tsp_bruteforce(g);
    const auto expected = vector<int>{ 0, 3, 5, 1, 2, 4 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_BnB] Empty graph", "[tsp_BnB]") {
    Graph g{};
    CHECK(tsp_BnB(g) == vector<int> {});
}

TEST_CASE("[tsp_BnB] Single vertex", "[tsp_BnB]") {
    Graph g{};
    g.add_vertex(0);
    CHECK(tsp_BnB(g) == vector<int> {});
}

TEST_CASE("[tsp_BnB] One edge", "[tsp_BnB]") {
    Graph g{ {0, 1, 2.5} };
    const auto result = tsp_BnB(g);
    const auto expected = vector<int>{ 0, 1 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_BnB] Three vertices, three edges", "[tsp_BnB]") {
    Graph g{ {0, 1, 2.5}, {0, 2, 0.5}, {1, 2, 1.0} };
    const auto result = tsp_BnB(g);
    const auto expected = vector<int>{ 0, 1, 2 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_BnB] Four vertices", "[tsp_BnB]") {
    Graph g{ {0, 1, 6.0}, {0, 2, 4.0}, {0, 3, 1.0},
             {1, 2, 3.5}, {1, 3, 2.0},
             {2, 3, 5.0} };
    const auto result = tsp_BnB(g);
    const auto expected = vector<int>{ 0, 2, 1, 3 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_BnB] Five vertices", "[tsp_BnB]") {
    Graph g{ {0, 1, 2.0}, {0, 2, 4.0}, {0, 3, 1.0}, {0, 4, 2.5},
             {1, 2, 3.6}, {1, 3, 6.0}, {1, 4, 3.0},
             {2, 3, 7.0}, {2, 4, 5.0},
             {3, 4, 9.0} };
    const auto result = tsp_BnB(g);
    const auto expected = vector<int>{ 0, 3, 2, 1, 4 };
    CHECK(aligned(result, 0) == expected);
}

TEST_CASE("[tsp_BnB] Six vertices", "[tsp_BnB]") {
    Graph g{ {0, 1, 2.0}, {0, 2, 4.0}, {0, 3, 1.0}, {0, 4, 2.5}, {0, 5, 3.2},
             {1, 2, 3.6}, {1, 3, 6.0}, {1, 4, 3.0}, {1, 5, 0.1},
             {2, 3, 7.0}, {2, 4, 5.0}, {2, 5, 9},
             {3, 4, 9.0}, {3, 5, 0.5},
             {4, 5, 1.0} };
    const auto result = tsp_BnB(g);
    const auto expected = vector<int>{ 0, 3, 5, 1, 2, 4 };
    CHECK(aligned(result, 0) == expected);
}