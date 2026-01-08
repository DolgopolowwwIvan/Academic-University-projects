#ifndef PEARSON_DISTRIBUTION_H
#define PEARSON_DISTRIBUTION_H

#include "idistribution.h"
#include "ipersistent.h"
#include <string>
#include <exception>

class pearson_distribution : public IDistribution, public IPersistent {
private:
    double mu;
    double lambda;
    double nu;

public:
    // Конструкторы
    pearson_distribution();
    pearson_distribution(double mu, double lambda, double nu);
    pearson_distribution(const std::string& filename);
    
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
    
    // Сеттеры и геттеры
    void set_location(double mu);
    void set_scale(double lambda);
    void set_shape(double nu);
    double get_location() const;
    double get_scale() const;
    double get_shape() const;
};

#endif