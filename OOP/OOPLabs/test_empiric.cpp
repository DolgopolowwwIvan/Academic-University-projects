// test_empiric.cpp
#include "empiric.h"
#include "mixture.h"
#include <iostream>
#include <iomanip>
#include <fstream>

void test_empiric_class() {
    std::cout << "=== Testing Empiric Class ===" << std::endl;

    std::cout << "\n1. Testing constructors:" << std::endl;
    
    // Создаем базовое распределение для тестов
    pearson_distribution p(0.0, 1.0, 3.0);
    
    // Создаем смесь в блоке try-catch
    Mixture* mix_ptr = nullptr;
    try {
        mix_ptr = new Mixture(p, pearson_distribution(2.0, 1.5, 4.0), 0.3);
    } catch (const distribution_exception& e) {
        std::cout << "Error creating mixture: " << e.what() << std::endl;
        return; // Не можем продолжать без mixture
    }
    
    // Ссылка на смесь для удобства
    Mixture& mix_ref = *mix_ptr;
    
    // Тестируем конструктор из pearson_distribution
    std::cout << "\n1.1 Constructor from pearson_distribution:" << std::endl;
    try {
        Empiric emp1(100, p, 10); // Уменьшил размер для скорости
        std::cout << "  Sample size: " << emp1.get_sample_size() << std::endl;
        std::cout << "  Intervals: " << emp1.get_intervals_count() << std::endl;
        std::cout << "  Data range: [" << emp1.get_min_value() 
                 << ", " << emp1.get_max_value() << "]" << std::endl;
        std::cout << "  Expectation: " << emp1.expectation() << std::endl;
        std::cout << "  Variance: " << emp1.variance() << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error: " << e.what() << std::endl;
    }

    // Тестируем конструктор из Mixture (передаем ссылку, а не указатель)
    std::cout << "\n1.2 Constructor from Mixture:" << std::endl;
    try {
        Empiric emp2(50, mix_ref, 8); // Уменьшил размер для скорости
        std::cout << "  Sample size: " << emp2.get_sample_size() << std::endl;
        std::cout << "  Intervals: " << emp2.get_intervals_count() << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error: " << e.what() << std::endl;
    }

    // Тестируем конструктор из файла
    std::cout << "\n1.3 Constructor from file:" << std::endl;
    try {
        // Сначала создаем тестовый файл
        std::ofstream test_file("test_data.txt");
        test_file << "10\n";
        for (int i = 0; i < 10; ++i) {
            test_file << i * 0.5 << "\n";
        }
        test_file.close();
        
        Empiric emp3("test_data.txt", 5);
        std::cout << "  Sample size from file: " << emp3.get_sample_size() << std::endl;
        std::cout << "  Expectation from file: " << emp3.expectation() << std::endl;
        
        // Удаляем тестовый файл
        std::remove("test_data.txt");
    } catch (const distribution_exception& e) {
        std::cout << "  Error: " << e.what() << std::endl;
    }

    std::cout << "\n2. Testing copy constructor and assignment operator:" << std::endl;
    
    // Сначала создаем оригинальный Empiric объект
    Empiric emp_original(200, p, 10);
    Empiric emp_copy = emp_original; // Конструктор копирования
    
    std::cout << "  Original expectation: " << emp_original.expectation() << std::endl;
    std::cout << "  Copy expectation: " << emp_copy.expectation() << std::endl;
    
    // Оператор присваивания - создаем временный объект из смеси
    std::cout << "\n2.1 Testing assignment operator:" << std::endl;
    try {
        Empiric temp_emp(50, mix_ref, 5);
        Empiric emp_assigned = temp_emp; // Используем конструктор копирования
        
        // Теперь тестируем оператор присваивания
        emp_assigned = emp_original;
        std::cout << "  Assigned sample size: " << emp_assigned.get_sample_size() << std::endl;
        std::cout << "  Assigned expectation: " << emp_assigned.expectation() << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in assignment test: " << e.what() << std::endl;
    }

    std::cout << "\n3. Testing density function:" << std::endl;
    try {
        Empiric emp_test(100, p, 10);
        std::cout << "  Min value: " << emp_test.get_min_value() << std::endl;
        std::cout << "  Max value: " << emp_test.get_max_value() << std::endl;
        std::cout << "  Density at min value: " << emp_test.density(emp_test.get_min_value()) << std::endl;
        std::cout << "  Density at middle range: " 
                  << emp_test.density((emp_test.get_min_value() + emp_test.get_max_value()) / 2) << std::endl;
        std::cout << "  Density outside range: " << emp_test.density(-100.0) << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in density test: " << e.what() << std::endl;
    }

    std::cout << "\n4. Testing numerical characteristics:" << std::endl;
    try {
        Empiric emp_test(100, p, 10);
        std::cout << "  Expectation: " << emp_test.expectation() << std::endl;
        std::cout << "  Variance: " << emp_test.variance() << std::endl;
        std::cout << "  Skewness: " << emp_test.skewness() << std::endl;
        std::cout << "  Kurtosis: " << emp_test.kurtosis() << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in numerical characteristics test: " << e.what() << std::endl;
    }

    std::cout << "\n5. Testing random generation:" << std::endl;
    try {
        Empiric emp_test(100, p, 10);
        std::cout << "  Generated samples: ";
        for (int i = 0; i < 5; ++i) {
            std::cout << emp_test.generate() << " ";
        }
        std::cout << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in random generation test: " << e.what() << std::endl;
    }

    std::cout << "\n6. Testing validity:" << std::endl;
    try {
        Empiric emp_test(100, p, 10);
        std::cout << "  emp_test is valid: " << (emp_test.is_valid() ? "true" : "false") << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in validity test: " << e.what() << std::endl;
    }

    std::cout << "\n7. Testing static functions:" << std::endl;
    try {
        Empiric emp_test(100, p, 10);
        double mean, variance, skewness, kurtosis;
        Empiric::empirical_numeric_traits_static(emp_test, &mean, &variance, &skewness, &kurtosis);
        std::cout << "  Static function results:" << std::endl;
        std::cout << "    Mean: " << mean << std::endl;
        std::cout << "    Variance: " << variance << std::endl;
        
        std::cout << "  Density via static function at x=0.5: " 
                  << Empiric::empirical_density_static(0.5, emp_test) << std::endl;
        
        std::cout << "  Random value via static function: " 
                  << Empiric::empirical_rand_value_static(emp_test) << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Error in static functions test: " << e.what() << std::endl;
    }

    std::cout << "\n8. Testing static creation function:" << std::endl;
    double sample_data[] = {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0};
    Empiric* emp_static = Empiric::create_empirical_distribution_static(sample_data, 10, 5);
    if (emp_static) {
        std::cout << "  Created via static function:" << std::endl;
        std::cout << "    Sample size: " << emp_static->get_sample_size() << std::endl;
        std::cout << "    Expectation: " << emp_static->expectation() << std::endl;
        Empiric::free_empirical_data_static(emp_static);
    } else {
        std::cout << "  Failed to create via static function" << std::endl;
    }

    std::cout << "\n9. Testing exceptions:" << std::endl;
    try {
        Empiric bad_emp(1, p, 10); // Слишком маленькая выборка
        std::cout << "  ERROR: Should have thrown exception!" << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "  Caught exception (expected): " << e.what() << std::endl;
    }

    // Очистка
    delete mix_ptr;
}