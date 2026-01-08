#include "empiric.h"
#include <fstream>
#include <cmath>
#include <algorithm>
#include <random>
#include <limits>

Empiric::Empiric(int n0, const pearson_distribution& prim, int k0)
    : n(n0), k(k0), data(new double[n]), pdf(nullptr),
      min_val(0.0), max_val(0.0) {
    
    if (n <= 1) {
        delete[] data;
        throw distribution_exception("Sample size must be greater than 1");
    }
    
    // Генерация выборки
    for (int i = 0; i < n; ++i) {
        data[i] = prim.generate();
    }
    
    calculate_histogram();
}

Empiric::Empiric(int n0, const Mixture& mixt, int k0)
    : n(n0), k(k0), data(new double[n]), pdf(nullptr),
      min_val(0.0), max_val(0.0) {
    
    if (n <= 1) {
        delete[] data;
        throw distribution_exception("Sample size must be greater than 1");
    }
    
    // Генерация выборки
    for (int i = 0; i < n; ++i) {
        data[i] = mixt.generate();
    }
    
    calculate_histogram();
}

Empiric::Empiric(int n0, const Empiric& emp, int k0)
    : n(n0), k(k0), data(new double[n]), pdf(nullptr),
      min_val(0.0), max_val(0.0) {
    
    if (n <= 1) {
        delete[] data;
        throw distribution_exception("Sample size must be greater than 1");
    }
    
    // Генерация выборки из эмпирического распределения
    for (int i = 0; i < n; ++i) {
        data[i] = emp.generate();
    }
    
    calculate_histogram();
}

Empiric::Empiric(const std::string& filename, int k0)
    : n(0), k(k0), data(nullptr), pdf(nullptr),
      min_val(0.0), max_val(0.0) {
    
    std::ifstream file(filename);
    if (!file.is_open()) {
        throw distribution_exception("Cannot open file: " + filename);
    }
    
    // Чтение количества данных
    file >> n;
    if (n <= 1) {
        throw distribution_exception("Sample size must be greater than 1");
    }
    
    data = new double[n];
    for (int i = 0; i < n; ++i) {
        file >> data[i];
        if (file.fail()) {
            delete[] data;
            throw distribution_exception("Error reading data from file");
        }
    }
    
    file.close();
    calculate_histogram();
}

Empiric::Empiric(const Empiric& other)
    : n(other.n), k(other.k), data(new double[other.n]),
      pdf(new double[other.k]), min_val(other.min_val),
      max_val(other.max_val) {
    
    // Глубокое копирование массивов
    for (int i = 0; i < n; ++i) {
        data[i] = other.data[i];
    }
    
    for (int i = 0; i < k; ++i) {
        pdf[i] = other.pdf[i];
    }
}

Empiric& Empiric::operator=(const Empiric& other) {
    if (this == &other) {
        return *this;
    }
    
    // Освобождаем старую память
    delete[] data;
    delete[] pdf;
    
    // Копируем данные
    n = other.n;
    k = other.k;
    min_val = other.min_val;
    max_val = other.max_val;
    
    // Выделяем новую память
    data = new double[n];
    pdf = new double[k];
    
    // Копируем массивы
    for (int i = 0; i < n; ++i) {
        data[i] = other.data[i];
    }
    
    for (int i = 0; i < k; ++i) {
        pdf[i] = other.pdf[i];
    }
    
    return *this;
}

Empiric::~Empiric() {
    delete[] data;
    delete[] pdf;
}

void Empiric::calculate_histogram() {
    // Находим min и max
    min_val = data[0];
    max_val = data[0];
    for (int i = 1; i < n; ++i) {
        if (data[i] < min_val) min_val = data[i];
        if (data[i] > max_val) max_val = data[i];
    }
    
    // Определяем количество интервалов (формула Стёрджеса)
    if (k <= 1) {
        k = (int)(log(n) / log(2.0)) + 1;
        if (k < 2) k = 2;
    }
    
    // Выделяем память для гистограммы
    if (pdf != nullptr) {
        delete[] pdf;
    }
    pdf = new double[k]();
    
    // Рассчитываем длину интервала
    double interval_length = (max_val - min_val) / k;
    
    // Строим гистограмму
    for (int i = 0; i < n; ++i) {
        int index = (int)((data[i] - min_val) / interval_length);
        if (index == k) index = k - 1;
        pdf[index] += 1.0;
    }
    
    // Нормализуем
    for (int i = 0; i < k; ++i) {
        pdf[i] /= (n * interval_length);
    }
}

double Empiric::density(double x) const {
    if (x < min_val || x > max_val || pdf == nullptr) {
        return 0.0;
    }
    
    double interval_length = (max_val - min_val) / k;
    int index = (int)((x - min_val) / interval_length);
    if (index == k) index = k - 1;
    else if (index > k - 1) index = k - 1;
    
    return pdf[index];
}

double Empiric::expectation() const {
    double sum = 0.0;
    for (int i = 0; i < n; ++i) {
        sum += data[i];
    }
    return sum / n;
}

double Empiric::variance() const {
    double mean = expectation();
    double sum_sq = 0.0;
    
    for (int i = 0; i < n; ++i) {
        double diff = data[i] - mean;
        sum_sq += diff * diff;
    }
    
    return sum_sq / n;
}

double Empiric::skewness() const {
    double mean = expectation();
    double var = variance();
    
    if (var <= 0) {
        return 0.0;
    }
    
    double sum_cube = 0.0;
    for (int i = 0; i < n; ++i) {
        double diff = data[i] - mean;
        sum_cube += diff * diff * diff;
    }
    
    return sum_cube / (n * std::pow(var, 1.5));
}

double Empiric::kurtosis() const {
    double mean = expectation();
    double var = variance();
    
    if (var <= 0) {
        return 0.0;
    }
    
    double sum_fourth = 0.0;
    for (int i = 0; i < n; ++i) {
        double diff = data[i] - mean;
        sum_fourth += diff * diff * diff * diff;
    }
    
    return (sum_fourth / (n * var * var)) - 3.0;
}

double Empiric::generate() const {
    if (pdf == nullptr) {
        return 0.0;
    }
    
    static std::random_device rd;
    static std::mt19937 gen(rd());
    std::uniform_real_distribution<double> dis(0.0, 1.0);
    
    double total_length = max_val - min_val;
    double interval_length = total_length / k;
    
    double r1 = dis(gen);
    double cumulative = 0.0;
    int selected_interval = k - 1;
    
    for (int i = 0; i < k; ++i) {
        double prob = pdf[i] * interval_length;
        cumulative += prob;
        if (r1 < cumulative) {
            selected_interval = i;
            break;
        }
    }
    
    double left_bound = min_val + selected_interval * interval_length;
    double right_bound = left_bound + interval_length;
    
    if (selected_interval == k - 1) {
        right_bound = max_val;
    }
    
    double r2 = dis(gen);
    return left_bound + r2 * (right_bound - left_bound);
}

bool Empiric::is_valid() const {
    return n > 1 && data != nullptr && pdf != nullptr;
}

// Статические функции для обратной совместимости
double Empiric::empirical_density_static(double x, const Empiric& emp) {
    return emp.density(x);
}

void Empiric::empirical_numeric_traits_static(const Empiric& emp,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    *mean = emp.expectation();
    *variance = emp.variance();
    *skewness = emp.skewness();
    *kurtosis = emp.kurtosis();
}

double Empiric::empirical_rand_value_static(const Empiric& emp) {
    return emp.generate();
}

Empiric* Empiric::create_empirical_distribution_static(const double* sample, int n, int k) {
    try {
        // Создаем временный файл для передачи данных
        std::string filename = "temp_empiric_data.txt";
        std::ofstream file(filename);
        
        if (!file.is_open()) {
            return nullptr;
        }
        
        file << n << "\n";
        for (int i = 0; i < n; ++i) {
            file << sample[i] << "\n";
        }
        
        file.close();
        
        // Создаем Empiric из файла
        Empiric* emp = new Empiric(filename, k);
        
        // Удаляем временный файл
        std::remove(filename.c_str());
        
        return emp;
    } catch (...) {
        return nullptr;
    }
}

void Empiric::free_empirical_data_static(Empiric* emp) {
    delete emp;
}