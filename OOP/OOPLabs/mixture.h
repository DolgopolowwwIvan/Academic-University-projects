
#ifndef MIXTURE_H
#define MIXTURE_H

#include "pearson_distribution.h"

class Mixture {
private:
    pearson_distribution d1;  // первый компонент (композиция)
    pearson_distribution d2;  // второй компонент (композиция)
    double p;                 // параметр смеси (0 < p < 1)

public:
    // Конструктор 
    Mixture(const pearson_distribution& prim1, 
            const pearson_distribution& prim2, 
            double p0);
    
    // Функции доступа к компонентам (требуется по заданию)
    pearson_distribution& component1() { return d1; }
    const pearson_distribution& component1() const { return d1; }
    pearson_distribution& component2() { return d2; }
    const pearson_distribution& component2() const { return d2; }
    
    
    double density(double x) const;
    double expectation() const;
    double variance() const;
    double skewness() const;
    double kurtosis() const;
    double generate() const;
    bool is_valid() const;
    
    // Get-функция для параметра смеси
    double get_mixture_param() const { return p; }

    static double mixture_density_static(double x, const Mixture& m);
    static void mixture_numeric_traits_static(const Mixture& m,
        double* mean, double* variance,
        double* skewness, double* kurtosis);
    static double mixture_rand_value_static(const Mixture& m);
};

#endif