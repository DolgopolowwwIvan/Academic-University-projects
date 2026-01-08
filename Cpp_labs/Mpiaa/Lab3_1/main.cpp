#define CATCH_CONFIG_MAIN

#include "/home/billiejean/Documents/Proga/Cpp_labs/Mpiaa/catch.hpp"

#include "lcs.h"

// Вспомогательная функция для замера времени выполнения
template <typename Func>
void measureTime(const std::string& X, const std::string& Y, Func func, const std::string& label) {
    auto start = std::chrono::high_resolution_clock::now();
    std::string result = func(X, Y);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = end - start;
    std::cout << label << " LCS: " << result << " (length: " << result.size() << ") "
              << "Time: " << duration.count() << " seconds\n";
}

int main(int argc, char* argv[])
 {
    int result = Catch::Session().run(argc, argv);
    return result;


    std::string X = "abcbdab";
    std::string Y = "bdcaba";

    // Замер времени для полного перебора
    measureTime(X, Y, lcs_brute_force, "Brute-force");

    // Замер времени для динамического программирования
    measureTime(X, Y, lcs_dynamic, "Dynamic programming");

    return 0;
}



