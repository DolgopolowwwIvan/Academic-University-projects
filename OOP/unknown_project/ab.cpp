#include <cmath>
#include <iostream>
#include <cstdlib>
#include <ctime>
#include <malloc.h>
#include <fstream>
#include <algorithm>
#include <windows.h>
#include "pearson_distribution.h"
#include "data_exporter.h"  

using namespace std;

const double pi = 3.14159265358979323846;

// Объявление функции тестирования из test_pearson.cpp
void testpearson_distribution();

// Функциональный подход для смесей распределений
struct mixture_param {
    pearson_distribution dist1;  // Первое распределение (класс)
    pearson_distribution dist2;  // Второе распределение (класс)
    double p;                    // Параметр смеси (0 < p < 1)
};

struct empirical_data {
    double* sample;      // Указатель на массив с выборкой
    int sample_size;     // Объем выборки (n)
    double* pdf;         // Указатель на массив значений плотности (гистограмма)
    int k;               // Количество интервалов
    double min_val;      // Минимальное значение в выборке
    double max_val;      // Максимальное значение в выборке
};

////////////////////////////////////////////////////////////////////////////////////

// Функции для работы с распределением Пирсона (используют класс)
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

// Функциональный подход для смесей распределений
double mixture_density(double x, const mixture_param& m) {
    double pdf1 = pierson_density(x, m.dist1);
    double pdf2 = pierson_density(x, m.dist2);
    return (1 - m.p) * pdf1 + (m.p) * pdf2;
}

void mixture_numeric_traits(const mixture_param& m,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    double m1, v1, e1, a1;
    double m2, v2, e2, a2;

    numeric_traits(m.dist1, &m1, &v1, &e1, &a1);
    numeric_traits(m.dist2, &m2, &v2, &e2, &a2);

    if (v1 == std::numeric_limits<double>::infinity() || v2 == std::numeric_limits<double>::infinity()) {
        *variance = std::numeric_limits<double>::infinity();
    }
    else {
        *mean = (1 - m.p) * m1 + (m.p) * m2;
        *variance = (1 - m.p) * (v1 + m1 * m1) + m.p * (v2 + m2 * m2) - (*mean) * (*mean);
    }

    // Проверяем, что дисперсия положительная и конечная
    if (*variance <= 0 || !isfinite(*variance)) {
        *skewness = 0.0;
        *kurtosis = 0.0;
        return;
    }

    double delta1 = m1 - *mean;
    double delta2 = m2 - *mean;

    *skewness = ((1 - m.p) * (pow(delta1, 3) + 3 * delta1 * v1 + pow(v1, 1.5) * a1) +
        m.p * (pow(delta2, 3) + 3 * delta2 * v2 + pow(v2, 1.5) * a2))
        / pow(*variance, 1.5);

    double numerator = (1 - m.p) * (pow(delta1, 4) + 6 * pow(delta1, 2) * v1 +
        4 * delta1 * pow(v1, 1.5) * a1 + pow(v1, 2) * (e1 + 3)) +
        m.p * (pow(delta2, 4) + 6 * pow(delta2, 2) * v2 +
            4 * delta2 * pow(v2, 1.5) * a2 + pow(v2, 2) * (e2 + 3));

    *kurtosis = numerator / pow(*variance, 2) - 3;
}

double mixture_rand_value(const mixture_param& m) {
    double r = (double)rand() / RAND_MAX;
    if (r < (1 - m.p)) {
        return rand_value_gen(m.dist1);
    }
    else {
        return rand_value_gen(m.dist2);
    }
}

////////////////////////////////////////////////////////////////////////////////////

// Функции для эмпирических распределений (функциональный подход)
empirical_data* create_empirical_distribution(const double* sample, int n, int k) {
    empirical_data* emp = (empirical_data*)malloc(sizeof(empirical_data));
    if (emp == NULL) return NULL;

    double min_val = sample[0];
    double max_val = sample[0];
    for (int i = 1; i < n; i++) {
        if (sample[i] < min_val) min_val = sample[i];
        if (sample[i] > max_val) max_val = sample[i];
    }

    if (k < 2) {
        k = (int)(log(n) / log(2)) + 1;
        if (k < 2) k = 2; // Минимум 2 интервала
    }

    double* pdf = (double*)calloc(k, sizeof(double));
    if (pdf == NULL) {
        free(emp);
        return NULL;
    }

    double interval_length = (max_val - min_val) / k;

    for (int i = 0; i < n; i++) {
        int index = (int)((sample[i] - min_val) / interval_length);
        if (index == k) index = k - 1;
        else if (index > k - 1) index = k - 1;
        pdf[index] += 1.0;
    }

    for (int i = 0; i < k; i++) {
        pdf[i] /= (n * interval_length);
    }

    double* sample_copy = (double*)malloc(n * sizeof(double));
    if (sample_copy == NULL) {
        free(pdf);
        free(emp);
        return NULL;
    }
    for (int i = 0; i < n; i++) {
        sample_copy[i] = sample[i];
    }

    emp->sample = sample_copy;
    emp->sample_size = n;
    emp->pdf = pdf;
    emp->k = k;
    emp->min_val = min_val;
    emp->max_val = max_val;

    return emp;
}

double empirical_density(double x, const empirical_data* emp) {
    if (x < emp->min_val || x > emp->max_val) return 0.0;

    double interval_length = (emp->max_val - emp->min_val) / emp->k;
    int index = (int)((x - emp->min_val) / interval_length);
    if (index == emp->k) index = emp->k - 1;
    else if (index > emp->k - 1) index = emp->k - 1;

    return emp->pdf[index];
}

void empirical_numeric_traits(const empirical_data* emp,
    double* mean, double* variance,
    double* skewness, double* kurtosis) {
    double sum = 0.0;
    for (int i = 0; i < emp->sample_size; i++) {
        sum += emp->sample[i];
    }
    *mean = sum / emp->sample_size;

    double sum_sq = 0.0;
    for (int i = 0; i < emp->sample_size; i++) {
        double diff = emp->sample[i] - *mean;
        sum_sq += diff * diff;
    }
    *variance = sum_sq / (emp->sample_size);

    // Проверяем, что дисперсия положительная
    if (*variance <= 0) {
        *skewness = 0.0;
        *kurtosis = 0.0;
        return;
    }

    double sum_cube = 0.0;
    for (int i = 0; i < emp->sample_size; i++) {
        double diff = emp->sample[i] - *mean;
        sum_cube += diff * diff * diff;
    }
    *skewness = sum_cube / (emp->sample_size * pow(*variance, 1.5));

    double sum_fourth = 0.0;
    for (int i = 0; i < emp->sample_size; i++) {
        double diff = emp->sample[i] - *mean;
        sum_fourth += diff * diff * diff * diff;
    }
    *kurtosis = (sum_fourth / (emp->sample_size * (*variance) * (*variance))) - 3;
}

double empirical_rand_value(const empirical_data* emp) {
    double total_length = emp->max_val - emp->min_val;
    double interval_length = total_length / emp->k;

    double r1 = (double)rand() / RAND_MAX;
    double cumulative = 0.0;
    int selected_interval = emp->k - 1;

    for (int i = 0; i < emp->k; i++) {
        double prob = emp->pdf[i] * interval_length;
        cumulative += prob;
        if (r1 < cumulative) {
            selected_interval = i;
            break;
        }
    }

    double left_bound = emp->min_val + selected_interval * interval_length;
    double right_bound = left_bound + interval_length;
    double r2 = (double)rand() / RAND_MAX;
    if (selected_interval == emp->k - 1) {
        right_bound = emp->max_val;
    }
    return left_bound + r2 * (right_bound - left_bound);
}

void free_empirical_data(empirical_data* emp) {
    if (emp != NULL) {
        free(emp->sample);
        free(emp->pdf);
        free(emp);
    }
}

////////////////////////////////////////////////////////////////////////////////////

void prepare_and_export_data(const pearson_distribution& p,
    const mixture_param& mixture,
    const empirical_data* emp_primary,
    const empirical_data* emp_mixture,
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

void test_sampling_and_empirical(const pearson_distribution& p, const mixture_param& mixture, int n) {
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

    empirical_data* emp_primary = create_empirical_distribution(primary_sample, n, 0);
    empirical_data* emp_mixture = create_empirical_distribution(mixture_sample, n, 0);

    if (emp_primary == NULL || emp_mixture == NULL) {
        printf("Error creating empirical distributions!\n");
        if (emp_primary) free_empirical_data(emp_primary);
        if (emp_mixture) free_empirical_data(emp_mixture);
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

    prepare_and_export_data(p, mixture, emp_primary, emp_mixture, primary_sorted, n, mixture_sorted, n);

    // Освобождаем память
    free_empirical_data(emp_primary);
    free_empirical_data(emp_mixture);
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

    cout << "\n=== MAIN PROGRAM ===" << endl;

    const int n = 1000;
    double p_mix = 0.5;

    try {
        pearson_distribution p1("C:/Users/Виктор/Downloads/test.txt");
        pearson_distribution p2("C:/Users/Виктор/Downloads/test2.txt");

        cout << "Distribution 1 loaded from test.txt: mu=" << p1.get_location()
            << ", lambda=" << p1.get_scale() << ", nu=" << p1.get_shape() << endl;
        cout << "Distribution 2 loaded from test2.txt: mu=" << p2.get_location()
            << ", lambda=" << p2.get_scale() << ", nu=" << p2.get_shape() << endl;

        mixture_param mixture = { p1, p2, p_mix };

        // Основная логика программы - тестирование смесей и эмпирических распределений
        test_sampling_and_empirical(p1, mixture, n);

        cout << "\nProgram completed successfully!" << endl;

    }
    catch (const distribution_exception& e) {
        cerr << "Error: " << e.what() << endl;
        return 1;
    }

    // Пауза перед завершением
    cout << "Press Enter to exit...";
    cin.get();

    return 0;
}