#ifndef PEARSON_DISTRIBUTION_H
#define PEARSON_DISTRIBUTION_H

#include <string>
#include <exception>

class pearson_distribution {
private:
    double mu;      // сдвиг
    double lambda;  // масштаб  
    double nu;      // параметр формы

public:
    // Конструкторы
    pearson_distribution();
    pearson_distribution(double mu, double lambda, double nu);
    pearson_distribution(const std::string& filename); 

    // Set-функции
    void set_location(double mu);
    void set_scale(double lambda);
    void set_shape(double nu);

    // Get-функции
    double get_location() const;
    double get_scale() const;
    double get_shape() const;

    // Функция плотности
    double density(double x) const;

    // Функции для моментов и характеристик
    double expectation() const;
    double variance() const;
    double skewness() const;
    double kurtosis() const;

    // Моделирование случайной величины
    double generate() const;

    // Вспомогательные методы
    bool is_valid() const;
};

// Класс исключения для обработки ошибок
class distribution_exception : public std::exception {
private:
    std::string message;
public:
    explicit distribution_exception(const std::string& message) : message(message) {}
    const char* what() const noexcept override {
        return message.c_str();
    }
};

#endif