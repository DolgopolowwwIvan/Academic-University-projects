#ifndef EMPIRIC_H
#define EMPIRIC_H

#include "idistribution.h"
#include "ipersistent.h"
#include <string>
#include <vector>

class Empiric : public IDistribution, public IPersistent {
private:
    int n;
    int k;
    double* data;
    double* pdf;
    double min_val;
    double max_val;
    
    void calculate_histogram();

public:
    // Упрощенный конструктор (только через интерфейс)
    Empiric(int n0, IDistribution& dist, int k0 = 0);
    
    // Конструктор из файла
    Empiric(const std::string& filename, int k0 = 0);
    
    // Конструктор копирования
    Empiric(const Empiric& other);
    
    // Оператор присваивания
    Empiric& operator=(const Empiric& other);
    
    // Деструктор
    ~Empiric();
    
    // Реализация IDistribution
    double density(double x) const override;
    double expectation() const override;
    double variance() const override;
    double skewness() const override;
    double kurtosis() const override;
    double generate() const override;
    bool is_valid() const override;
    
    // Реализация IPersistent
    void save_to_file(const std::string& filename) const override;
    void load_from_file(const std::string& filename) override;
    
    // Геттеры
    int get_sample_size() const { return n; }
    int get_intervals_count() const { return k; }
    const double* get_data() const { return data; }
    double get_min_value() const { return min_val; }
    double get_max_value() const { return max_val; }
};

#endif