#include "lcs.h"

using namespace std;

#include <iostream>
#include <string>
#include <vector>
#include <chrono>
#include <algorithm>

// Функция для проверки, является ли строка `subseq` подпоследовательностью строки `str`
bool isSubsequence(const std::string& subseq, const std::string& str) {
    size_t i = 0, j = 0;
    while (i < subseq.size() && j < str.size()) {
        if (subseq[i] == str[j]) ++i;
        ++j;
    }
    return i == subseq.size();
}

// Полный перебор для поиска LCS
std::string lcs_brute_force(const std::string& X, const std::string& Y) {
    size_t maxLen = 0;
    std::string best;

    for (size_t i = 0; i < (1 << X.size()); ++i) {
        std::string subseq;
        for (size_t j = 0; j < X.size(); ++j) {
            if (i & (1 << j)) subseq += X[j];
        }
        if (isSubsequence(subseq, Y) && subseq.size() > maxLen) {
            best = subseq;
            maxLen = subseq.size();
        }
    }
    return best;
}

// Динамическое программирование для поиска LCS
std::string lcs_dynamic(const std::string& X, const std::string& Y) {
    size_t N = X.size(), M = Y.size();
    std::vector<std::vector<int>> LCS(N + 1, std::vector<int>(M + 1, 0));

    // Заполнение таблицы LCS
    for (size_t i = 1; i <= N; ++i) {
        for (size_t j = 1; j <= M; ++j) {
            if (X[i - 1] == Y[j - 1]) {
                LCS[i][j] = LCS[i - 1][j - 1] + 1;
            } else {
                LCS[i][j] = std::max(LCS[i - 1][j], LCS[i][j - 1]);
            }
        }
    }

    // Восстановление LCS по таблице
    std::string result;
    size_t i = N, j = M;
    while (i > 0 && j > 0) {
        if (X[i - 1] == Y[j - 1]) {
            result = X[i - 1] + result;
            --i;
            --j;
        } else if (LCS[i - 1][j] > LCS[i][j - 1]) {
            --i;
        } else {
            --j;
        }
    }
    return result;
}

