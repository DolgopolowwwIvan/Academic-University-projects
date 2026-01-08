#pragma once

#include <string>

// Find the longest common subsequence of two strings.
std::string lcs_brute_force(const std::string &first, const std::string &second);
std::string lcs_dynamic(const std::string &first, const std::string &second);