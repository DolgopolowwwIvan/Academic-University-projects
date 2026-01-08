#include "pearson_distribution.h"
#include <fstream>
#include <cmath>
#include <random>
#include <sstream>
#include <limits>

const double pi = 3.14159265358979323846;

// Конструкторы
pearson_distribution::pearson_distribution()
    : mu(0.0), lambda(1.0), nu(2.0) {
}

pearson_distribution::pearson_distribution(double mu, double lambda, double nu)
    : mu(mu), lambda(lambda), nu(nu) {
    if (lambda <= 0.0) {
        throw distribution_exception("lambda must be positive!");
    }
    if (nu <= 0.5) {
        throw distribution_exception("nu must be greater than 0.5!");
    }
}

pearson_distribution::pearson_distribution(const std::string& filename)
    : mu(0.0), lambda(1.0), nu(2.0) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        throw distribution_exception("Cannot open file for reading: " + filename);
    }

    double file_mu, file_lambda, file_nu;

    file >> file_mu >> file_lambda >> file_nu;

    if (file.fail()) {
        file.close();
        throw distribution_exception("Error reading parameters from file: " + filename);
    }

    file.close();

    // Проверяем валидность прочитанных параметров
    if (file_lambda <= 0.0) {
        throw distribution_exception("Scale parameter must be positive. Read value: " + std::to_string(file_lambda));
    }
    if (file_nu <= 0.5) {
        throw distribution_exception("Shape parameter must be greater than 0.5. Read value: " + std::to_string(file_nu));
    }

    // Устанавливаем параметры
    mu = file_mu;
    lambda = file_lambda;
    nu = file_nu;
}

// Set-функции
void pearson_distribution::set_location(double mu) {
    this->mu = mu;
}

void pearson_distribution::set_scale(double lambda) {
    if (lambda > 0.0) {
        this->lambda = lambda;
    }
}

void pearson_distribution::set_shape(double nu) {
    if (nu > 0.5) {
        this->nu = nu;
    }
}

// Get-функции
double pearson_distribution::get_location() const { return mu; }
double pearson_distribution::get_scale() const { return lambda; }
double pearson_distribution::get_shape() const { return nu; }

// Вспомогательная функция для бета-функции
static double log_beta(double x, double y) {
    return std::lgamma(x) + std::lgamma(y) - std::lgamma(x + y);
}

// Функция плотности
double pearson_distribution::density(double x) const {
    if (!is_valid()) {
        return 0.0;
    }

    double log_constant = -log(lambda) - log_beta(nu - 0.5, 0.5);
    double z = (x - mu) / lambda;
    double log_pdf = log_constant - nu * log(1.0 + z * z);
    return exp(log_pdf);
}

// Функции для моментов и характеристик
double pearson_distribution::expectation() const {
    return mu;
}

double pearson_distribution::variance() const {
    if (!is_valid() || nu <= 1.5) {
        return std::numeric_limits<double>::infinity();
    }
    return lambda * lambda / (2.0 * nu - 3.0);
}

double pearson_distribution::skewness() const {
    return 0.0; // Распределение симметрично
}

double pearson_distribution::kurtosis() const {
    if (!is_valid() || nu <= 2.5) {
        return std::numeric_limits<double>::infinity();
    }
    return 6.0 / (2.0 * nu - 5.0);
}

// Моделирование случайной величины
double pearson_distribution::generate() const {
    if (!is_valid()) {
        return 0.0;
    }

    static std::random_device rd;
    static std::mt19937 gen(rd());
    std::uniform_real_distribution<double> dis(0.0001, 0.9999);

    double r1 = dis(gen);
    double r2 = dis(gen);

    double base_value = sqrt(pow(r1, -1.0 / nu) - 1.0) * cos(2.0 * pi * r2);
    return mu + lambda * base_value;
}

bool pearson_distribution::is_valid() const {
    return lambda > 0.0 && nu > 0.5;
}