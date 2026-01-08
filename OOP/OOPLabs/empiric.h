// empiric.h
#ifndef EMPIRIC_H
#define EMPIRIC_H

#include "pearson_distribution.h"
#include "mixture.h"  
#include <string>

class Empiric {
private:
    int n;           // объем выборки
    int k;           // количество интервалов
    double* data;    // массив данных
    double* pdf;     // массив значений плотности
    double min_val;
    double max_val;

    void calculate_histogram();

public:
    // Конструкторы
    Empiric(int n0, const pearson_distribution& prim, int k0 = 0);
    Empiric(int n0, const Mixture& mixt, int k0 = 0);
    Empiric(int n0, const Empiric& emp, int k0 = 0);
    Empiric(const std::string& filename, int k0 = 0);
    
    // Конструктор копирования
    Empiric(const Empiric& other);
    
    // Оператор присваивания
    Empiric& operator=(const Empiric& other);
    
    // Деструктор
    ~Empiric();
    
    // Интерфейс
    double density(double x) const;
    double expectation() const;
    double variance() const;
    double skewness() const;
    double kurtosis() const;
    double generate() const;
    bool is_valid() const;
    
    // Get-функции
    int get_sample_size() const { return n; }
    int get_intervals_count() const { return k; }
    const double* get_data() const { return data; }
    double get_min_value() const { return min_val; }
    double get_max_value() const { return max_val; }
    
    // Статические функции
    static double empirical_density_static(double x, const Empiric& emp);
    static void empirical_numeric_traits_static(const Empiric& emp,
        double* mean, double* variance,
        double* skewness, double* kurtosis);
    static double empirical_rand_value_static(const Empiric& emp);
    static Empiric* create_empirical_distribution_static(const double* sample, int n, int k);
    static void free_empirical_data_static(Empiric* emp);
};

#endif