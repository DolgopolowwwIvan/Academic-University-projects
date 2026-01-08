#ifndef MIXTURE_H
#define MIXTURE_H

#include "idistribution.h"
#include "ipersistent.h"
#include <string>
#include <exception>

template<class Distribution1, class Distribution2>
class Mixture : public IDistribution, public IPersistent {
private:
    Distribution1 d1;
    Distribution2 d2;
    double p;

public:
    // Конструктор
    Mixture(const Distribution1& prim1, const Distribution2& prim2, double p0)
        : d1(prim1), d2(prim2), p(p0) {
        if (p0 <= 0.0 || p0 >= 1.0) {
            throw std::invalid_argument("Mixture parameter p must be between 0 and 1");
        }
    }
    
    // Доступ к компонентам
    Distribution1& component1() { return d1; }
    Distribution2& component2() { return d2; }
    const Distribution1& component1() const { return d1; }
    const Distribution2& component2() const { return d2; }
    
    // Реализация IDistribution
    double density(double x) const override {
        return (1.0 - p) * d1.density(x) + p * d2.density(x);
    }
    
    double expectation() const override {
        return (1.0 - p) * d1.expectation() + p * d2.expectation();
    }
    
    double variance() const override {
        double m1 = d1.expectation();
        double m2 = d2.expectation();
        double v1 = d1.variance();
        double v2 = d2.variance();
        double mean = expectation();
        return (1.0 - p) * (v1 + m1 * m1) + p * (v2 + m2 * m2) - mean * mean;
    }
    
    double skewness() const override {
        double mean = expectation();
        double variance_val = variance();
        
        if (variance_val <= 0) return 0.0;
        
        double m1 = d1.expectation();
        double m2 = d2.expectation();
        double v1 = d1.variance();
        double v2 = d2.variance();
        double s1 = d1.skewness();
        double s2 = d2.skewness();
        
        double delta1 = m1 - mean;
        double delta2 = m2 - mean;
        
        double numerator = (1.0 - p) * (delta1*delta1*delta1 + 3*delta1*v1 + v1*sqrt(v1)*s1) +
                          p * (delta2*delta2*delta2 + 3*delta2*v2 + v2*sqrt(v2)*s2);
        
        return numerator / (variance_val * sqrt(variance_val));
    }
    
    double kurtosis() const override {
        double mean = expectation();
        double variance_val = variance();
        
        if (variance_val <= 0) return 0.0;
        
        double m1 = d1.expectation();
        double m2 = d2.expectation();
        double v1 = d1.variance();
        double v2 = d2.variance();
        double s1 = d1.skewness();
        double s2 = d2.skewness();
        double k1 = d1.kurtosis();
        double k2 = d2.kurtosis();
        
        double delta1 = m1 - mean;
        double delta2 = m2 - mean;
        
        double numerator = (1.0 - p) * (pow(delta1, 4) + 6*delta1*delta1*v1 + 
                         4*delta1*v1*sqrt(v1)*s1 + v1*v1*(k1 + 3)) +
                         p * (pow(delta2, 4) + 6*delta2*delta2*v2 + 
                         4*delta2*v2*sqrt(v2)*s2 + v2*v2*(k2 + 3));
        
        return numerator / (variance_val * variance_val) - 3.0;
    }
    
    double generate() const override {
        double r = static_cast<double>(rand()) / RAND_MAX;
        if (r < (1.0 - p)) {
            return d1.generate();
        } else {
            return d2.generate();
        }
    }
    
    bool is_valid() const override {
        return d1.is_valid() && d2.is_valid() && (p > 0.0 && p < 1.0);
    }
    
    // Реализация IPersistent
    void save_to_file(const std::string& filename) const override {
        // Сохраняем параметры смеси
        std::ofstream file(filename);
        if (!file.is_open()) {
            throw std::runtime_error("Cannot open file for writing: " + filename);
        }
        file << p << std::endl;
        file.close();
        
        // Сохраняем компоненты в отдельные файлы
        d1.save_to_file(filename + "_component1.txt");
        d2.save_to_file(filename + "_component2.txt");
    }
    
    void load_from_file(const std::string& filename) override {
        std::ifstream file(filename);
        if (!file.is_open()) {
            throw std::runtime_error("Cannot open file for reading: " + filename);
        }
        file >> p;
        file.close();
        
        d1.load_from_file(filename + "_component1.txt");
        d2.load_from_file(filename + "_component2.txt");
    }
    
    // Дополнительные методы
    double get_mixture_param() const { return p; }
    void set_mixture_param(double new_p) {
        if (new_p > 0.0 && new_p < 1.0) {
            p = new_p;
        }
    }
};

#endif