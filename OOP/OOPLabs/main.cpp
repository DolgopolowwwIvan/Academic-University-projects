// main.cpp
#include <cmath>
#include <iostream>
#include <cstdlib>
#include <ctime>
#include <fstream>
#include <algorithm>
#include <windows.h>
#include "pearson_distribution.h"
#include "mixture.h"
#include "empiric.h"
#include "data_exporter.h"

using namespace std;

const double pi = 3.14159265358979323846;

// Объявления тестирующих функций
void testpearson_distribution();  // из test_pearson.cpp
void test_mixture_class();        // из test_mixture.cpp  
void test_empiric_class();        // из test_empiric.cpp

////////////////////////////////////////////////////////////////////////////////////

// Функции для работы с распределением Пирсона (обратная совместимость)
double pierson_density(double x, const pearson_distribution& p) {
    return p.density(x);
}

void numeric_traits(const pearson_distribution& p, double* mean, double* variance, double* excess, double* coef_asymm) {
    *mean = p.expectation();
    *variance = p.variance();
    *excess = p.kurtosis();
    *coef_asymm = p.skewness();
}

double rand_value_gen(const pearson_distribution& p) {
    return p.generate();
}

////////////////////////////////////////////////////////////////////////////////////

// Функции для работы со смесью распределений (обратная совместимость)
double mixture_density(double x, const Mixture& m) {
    return Mixture::mixture_density_static(x, m);
}

void mixture_numeric_traits(const Mixture& m,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    Mixture::mixture_numeric_traits_static(m, mean, variance, skewness, kurtosis);
}

double mixture_rand_value(const Mixture& m) {
    return Mixture::mixture_rand_value_static(m);
}

////////////////////////////////////////////////////////////////////////////////////

// Функции для работы с эмпирическим распределением (обратная совместимость)
double empirical_density(double x, const Empiric* emp) {
    if (emp == nullptr) return 0.0;
    return Empiric::empirical_density_static(x, *emp);
}

void empirical_numeric_traits(const Empiric* emp,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    if (emp == nullptr) return;
    Empiric::empirical_numeric_traits_static(*emp, mean, variance, skewness, kurtosis);
}

double empirical_rand_value(const Empiric* emp) {
    if (emp == nullptr) return 0.0;
    return Empiric::empirical_rand_value_static(*emp);
}

////////////////////////////////////////////////////////////////////////////////////

void prepare_and_export_data(const pearson_distribution& p,
    const Mixture& mixture,
    const Empiric* emp_primary,
    const Empiric* emp_mixture,
    const double* primary_sorted, int primary_size,
    const double* mixture_sorted, int mixture_size) {

    // Подготовка данных для основного распределения (берем каждое 10-е значение)
    int primary_points = primary_size / 10;
    if (primary_points < 1) primary_points = 1;

    double* primary_x_values = new double[primary_points];
    double* primary_theoretical_densities = new double[primary_points];
    double* primary_empirical_densities = new double[primary_points];

    for (int i = 0; i < primary_points; i++) {
        double x = primary_sorted[i * 10];
        primary_x_values[i] = x;
        primary_theoretical_densities[i] = pierson_density(x, p);
        primary_empirical_densities[i] = empirical_density(x, emp_primary);
    }

    // Подготовка данных для смеси распределений (берем каждое 10-е значение)
    int mixture_points = mixture_size / 10;
    if (mixture_points < 1) mixture_points = 1;

    double* mixture_x_values = new double[mixture_points];
    double* mixture_theoretical_densities = new double[mixture_points];
    double* mixture_empirical_densities = new double[mixture_points];

    for (int i = 0; i < mixture_points; i++) {
        double x = mixture_sorted[i * 10];
        mixture_x_values[i] = x;
        mixture_theoretical_densities[i] = mixture_density(x, mixture);
        mixture_empirical_densities[i] = empirical_density(x, emp_mixture);
    }

    // Экспортируем данные через data_exporter
    data_exporter exporter_data("data.txt");
    exporter_data.export_data(primary_x_values, primary_theoretical_densities, primary_points);

    data_exporter exporter_data1("data1.txt");
    exporter_data1.export_data(mixture_x_values, mixture_theoretical_densities, mixture_points);

    data_exporter exporter_data2("data2.txt");
    exporter_data2.export_data(primary_x_values, primary_empirical_densities, primary_points);

    data_exporter exporter_data3("data3.txt");
    exporter_data3.export_data(mixture_x_values, mixture_empirical_densities, mixture_points);

    cout << "Data exported to files: data.txt, data1.txt, data2.txt, data3.txt" << endl;

    // Освобождаем память
    delete[] primary_x_values;
    delete[] primary_theoretical_densities;
    delete[] primary_empirical_densities;
    delete[] mixture_x_values;
    delete[] mixture_theoretical_densities;
    delete[] mixture_empirical_densities;
}

void test_sampling_and_empirical(const pearson_distribution& p, const Mixture& mixture, int n) {
    printf("\n=== Testing Sampling and Empirical Distribution ===\n");

    double* primary_sample = (double*)malloc(n * sizeof(double));
    double* mixture_sample = (double*)malloc(n * sizeof(double));

    if (primary_sample == NULL || mixture_sample == NULL) {
        printf("Memory allocation failed!\n");
        if (primary_sample) free(primary_sample);
        if (mixture_sample) free(mixture_sample);
        return;
    }

    printf("Generating %d samples from distributions...\n", n);
    for (int i = 0; i < n; i++) {
        primary_sample[i] = rand_value_gen(p);
        mixture_sample[i] = mixture_rand_value(mixture);
    }

    // Сортировка массивов
    double* primary_sorted = (double*)malloc(n * sizeof(double));
    double* mixture_sorted = (double*)malloc(n * sizeof(double));

    if (primary_sorted == NULL || mixture_sorted == NULL) {
        printf("Memory allocation for sorted arrays failed!\n");
        free(primary_sample);
        free(mixture_sample);
        if (primary_sorted) free(primary_sorted);
        if (mixture_sorted) free(mixture_sorted);
        return;
    }

    for (int i = 0; i < n; i++) {
        primary_sorted[i] = primary_sample[i];
        mixture_sorted[i] = mixture_sample[i];
    }

    // Используем qsort для сортировки
    qsort(primary_sorted, n, sizeof(double), [](const void* a, const void* b) {
        double arg1 = *static_cast<const double*>(a);
        double arg2 = *static_cast<const double*>(b);
        if (arg1 < arg2) return -1;
        if (arg1 > arg2) return 1;
        return 0;
        });

    qsort(mixture_sorted, n, sizeof(double), [](const void* a, const void* b) {
        double arg1 = *static_cast<const double*>(a);
        double arg2 = *static_cast<const double*>(b);
        if (arg1 < arg2) return -1;
        if (arg1 > arg2) return 1;
        return 0;
        });

    Empiric* emp_primary = Empiric::create_empirical_distribution_static(primary_sample, n, 0);
    Empiric* emp_mixture = Empiric::create_empirical_distribution_static(mixture_sample, n, 0);

    if (emp_primary == nullptr || emp_mixture == nullptr) {
        printf("Error creating empirical distributions!\n");
        if (emp_primary) Empiric::free_empirical_data_static(emp_primary);
        if (emp_mixture) Empiric::free_empirical_data_static(emp_mixture);
        free(primary_sample);
        free(mixture_sample);
        free(primary_sorted);
        free(mixture_sorted);
        return;
    }

    int random_index1 = rand() % n;
    double random_point1 = primary_sample[random_index1];

    double primary_theoretical = pierson_density(random_point1, p);
    double primary_empirical = empirical_density(random_point1, emp_primary);

    printf("\n=== Density Comparison at Random Points ===\n");
    printf("Primary distribution at random point x = %.6f:\n", random_point1);
    printf("  Theoretical density: %.6f\n", primary_theoretical);
    printf("  Empirical density:   %.6f\n", primary_empirical);
    printf("  Difference:          %.6f\n", primary_empirical - primary_theoretical);

    int random_index2 = rand() % n;
    double random_point2 = mixture_sample[random_index2];

    double mixture_theoretical = mixture_density(random_point2, mixture);
    double mixture_empirical = empirical_density(random_point2, emp_mixture);

    printf("Mixture distribution at random point x = %.6f:\n", random_point2);
    printf("  Theoretical density: %.6f\n", mixture_theoretical);
    printf("  Empirical density:   %.6f\n", mixture_empirical);
    printf("  Difference:          %.6f\n", mixture_empirical - mixture_theoretical);

    prepare_and_export_data(p, mixture, emp_primary, emp_mixture, 
                           primary_sorted, n, mixture_sorted, n);

    // Освобождаем память
    Empiric::free_empirical_data_static(emp_primary);
    Empiric::free_empirical_data_static(emp_mixture);
    free(primary_sample);
    free(mixture_sample);
    free(primary_sorted);
    free(mixture_sorted);
}

////////////////////////////////////////////////////////////////////////////////////////////////

int main() {
    srand((unsigned)time(NULL));
    SetConsoleOutputCP(1251);

    cout << "=== TESTING PEARSON DISTRIBUTION CLASS ===" << endl;
    testpearson_distribution();
    
    cout << "\n=== TESTING MIXTURE CLASS ===" << endl;
    test_mixture_class();
    
    cout << "\n=== TESTING EMPIRIC CLASS ===" << endl;
    test_empiric_class();

    cout << "\n=== MAIN PROGRAM ===" << endl;

    const int n = 1000;
    double p_mix = 0.5;

    try {
        // Загружаем распределения из файлов (пути нужно адаптировать под вашу систему)
        pearson_distribution p1("/home/billiejean/Documents/Proga/University/OOP/OOPLabs/test.txt");
        pearson_distribution p2("/home/billiejean/Documents/Proga/University/OOP/OOPLabs/test1.txt");

        cout << "\nDistribution 1 loaded from test.txt:" << endl;
        cout << "  mu = " << p1.get_location() << endl;
        cout << "  lambda = " << p1.get_scale() << endl;
        cout << "  nu = " << p1.get_shape() << endl;
        
        cout << "\nDistribution 2 loaded from test2.txt:" << endl;
        cout << "  mu = " << p2.get_location() << endl;
        cout << "  lambda = " << p2.get_scale() << endl;
        cout << "  nu = " << p2.get_shape() << endl;

        // Создаем смесь распределений
        Mixture mixture(p1, p2, p_mix);
        
        cout << "\nMixture created with parameter p = " << mixture.get_mixture_param() << endl;
        
        // Демонстрация доступа к компонентам (требование методички)
        cout << "\nDemonstrating component access (methodology requirement):" << endl;
        cout << "Original component 1 location: " << mixture.component1().get_location() << endl;
        cout << "Original component 2 scale: " << mixture.component2().get_scale() << endl;
        
        // Изменение параметров компонентов
        mixture.component1().set_location(p1.get_location() + 0.5);
        mixture.component2().set_scale(p2.get_scale() * 1.1);
        
        cout << "Modified component 1 location: " << mixture.component1().get_location() << endl;
        cout << "Modified component 2 scale: " << mixture.component2().get_scale() << endl;
        
        // Восстанавливаем исходные значения для дальнейших тестов
        mixture.component1().set_location(p1.get_location());
        mixture.component2().set_scale(p2.get_scale());

        // Вычисляем характеристики смеси
        cout << "\nMixture characteristics:" << endl;
        cout << "  Expectation: " << mixture.expectation() << endl;
        cout << "  Variance: " << mixture.variance() << endl;
        cout << "  Skewness: " << mixture.skewness() << endl;
        cout << "  Kurtosis: " << mixture.kurtosis() << endl;

        // Основная логика программы - тестирование смесей и эмпирических распределений
        test_sampling_and_empirical(p1, mixture, n);

        // Дополнительное тестирование конструкторов Empiric
        cout << "\n=== Additional Empiric Class Testing ===" << endl;
        
        // Создаем эмпирическое распределение из pearson_distribution
        Empiric empiric_from_pearson(500, p1, 15);
        cout << "Empiric from pearson distribution:" << endl;
        cout << "  Sample size: " << empiric_from_pearson.get_sample_size() << endl;
        cout << "  Intervals: " << empiric_from_pearson.get_intervals_count() << endl;
        cout << "  Data range: [" << empiric_from_pearson.get_min_value() 
             << ", " << empiric_from_pearson.get_max_value() << "]" << endl;
        
        // Создаем эмпирическое распределение из смеси
        Empiric empiric_from_mixture(500, mixture, 15);
        cout << "\nEmpiric from mixture:" << endl;
        cout << "  Sample size: " << empiric_from_mixture.get_sample_size() << endl;
        
        // Тестируем глубокое копирование
        Empiric copy_of_empiric = empiric_from_pearson;
        cout << "\nTesting deep copy:" << endl;
        cout << "  Original expectation: " << empiric_from_pearson.expectation() << endl;
        cout << "  Copy expectation: " << copy_of_empiric.expectation() << endl;
        
        // Генерация из эмпирического распределения
        cout << "\nGenerating from empiric distribution:" << endl;
        cout << "  Generated values: ";
        for (int i = 0; i < 5; ++i) {
            cout << empiric_from_pearson.generate() << " ";
        }
        cout << endl;

        cout << "\n=== Program completed successfully! ===" << endl;

    } catch (const distribution_exception& e) {
        cerr << "\n=== ERROR ===" << endl;
        cerr << "Distribution exception: " << e.what() << endl;
        cerr << "Please check:" << endl;
        cerr << "1. Existence of files test.txt and test2.txt" << endl;
        cerr << "2. File format (three numbers per line)" << endl;
        cerr << "3. Valid parameter values (lambda > 0, nu > 0.5)" << endl;
        return 1;
    } catch (const exception& e) {
        cerr << "\n=== ERROR ===" << endl;
        cerr << "Standard exception: " << e.what() << endl;
        return 1;
    } catch (...) {
        cerr << "\n=== ERROR ===" << endl;
        cerr << "Unknown exception occurred!" << endl;
        return 1;
    }

    // Пауза перед завершением (только для Windows)
    #ifdef _WIN32
    cout << "\nPress Enter to exit...";
    cin.get();
    #endif

    return 0;
}