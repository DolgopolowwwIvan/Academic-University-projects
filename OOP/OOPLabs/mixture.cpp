#include "mixture.h"
#include <cmath>
#include <limits>
#include <random>

Mixture::Mixture(const pearson_distribution& prim1, 
                 const pearson_distribution& prim2, 
                 double p0)
    : d1(prim1), d2(prim2), p(p0) {
    if (p0 <= 0.0 || p0 >= 1.0) {
        throw distribution_exception("Mixture parameter p must be between 0 and 1");
    }
}

double Mixture::density(double x) const {
    double pdf1 = d1.density(x);
    double pdf2 = d2.density(x);
    return (1.0 - p) * pdf1 + p * pdf2;
}

double Mixture::expectation() const {
    double m1 = d1.expectation();
    double m2 = d2.expectation();
    return (1.0 - p) * m1 + p * m2;
}

double Mixture::variance() const {
    double m1 = d1.expectation();
    double m2 = d2.expectation();
    double v1 = d1.variance();
    double v2 = d2.variance();
    
    if (v1 == std::numeric_limits<double>::infinity() || 
        v2 == std::numeric_limits<double>::infinity()) {
        return std::numeric_limits<double>::infinity();
    }
    
    double mean = expectation();
    return (1.0 - p) * (v1 + m1 * m1) + p * (v2 + m2 * m2) - mean * mean;
}

double Mixture::skewness() const {
    double mean = expectation();
    double variance_val = variance();
    
    if (variance_val <= 0 || !std::isfinite(variance_val)) {
        return 0.0;
    }
    
    double m1 = d1.expectation();
    double m2 = d2.expectation();
    double v1 = d1.variance();
    double v2 = d2.variance();
    double a1 = d1.skewness();
    double a2 = d2.skewness();
    
    double delta1 = m1 - mean;
    double delta2 = m2 - mean;
    
    double numerator = (1.0 - p) * (std::pow(delta1, 3) + 3.0 * delta1 * v1 + 
                     std::pow(v1, 1.5) * a1) +
                     p * (std::pow(delta2, 3) + 3.0 * delta2 * v2 + 
                     std::pow(v2, 1.5) * a2);
    
    return numerator / std::pow(variance_val, 1.5);
}

double Mixture::kurtosis() const {
    double mean = expectation();
    double variance_val = variance();
    
    if (variance_val <= 0 || !std::isfinite(variance_val)) {
        return 0.0;
    }
    
    double m1 = d1.expectation();
    double m2 = d2.expectation();
    double v1 = d1.variance();
    double v2 = d2.variance();
    double a1 = d1.skewness();
    double a2 = d2.skewness();
    double e1 = d1.kurtosis();
    double e2 = d2.kurtosis();
    
    double delta1 = m1 - mean;
    double delta2 = m2 - mean;
    
    double numerator = (1.0 - p) * (std::pow(delta1, 4) + 6.0 * std::pow(delta1, 2) * v1 +
                     4.0 * delta1 * std::pow(v1, 1.5) * a1 + std::pow(v1, 2) * (e1 + 3.0)) +
                     p * (std::pow(delta2, 4) + 6.0 * std::pow(delta2, 2) * v2 +
                     4.0 * delta2 * std::pow(v2, 1.5) * a2 + std::pow(v2, 2) * (e2 + 3.0));
    
    return numerator / std::pow(variance_val, 2) - 3.0;
}

double Mixture::generate() const {
    static std::random_device rd;
    static std::mt19937 gen(rd());
    std::uniform_real_distribution<double> dis(0.0, 1.0);
    
    double r = dis(gen);
    if (r < (1.0 - p)) {
        return d1.generate();
    } else {
        return d2.generate();
    }
}

bool Mixture::is_valid() const {
    return d1.is_valid() && d2.is_valid() && (p > 0.0 && p < 1.0);
}

// Статические функции для обратной совместимости
double Mixture::mixture_density_static(double x, const Mixture& m) {
    return m.density(x);
}

void Mixture::mixture_numeric_traits_static(const Mixture& m,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    *mean = m.expectation();
    *variance = m.variance();
    *skewness = m.skewness();
    *kurtosis = m.kurtosis();
}

double Mixture::mixture_rand_value_static(const Mixture& m) {
    return m.generate();
}