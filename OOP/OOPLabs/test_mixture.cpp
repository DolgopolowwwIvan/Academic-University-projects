// test_mixture.cpp
#include "mixture.h"
#include <iostream>
#include <iomanip>

void test_mixture_class() {
    std::cout << "=== Testing Mixture Class ===" << std::endl;

    std::cout << "\n1. Testing constructors:" << std::endl;
    
    // Создаем базовые распределения
    pearson_distribution p1(0.0, 1.0, 3.0);
    pearson_distribution p2(2.0, 1.5, 4.0);
    
    // Создаем смесь
    Mixture mix1(p1, p2, 0.3);
    std::cout << "Mixture created with p=" << mix1.get_mixture_param() << std::endl;
    std::cout << "Component 1 location: " << mix1.component1().get_location() << std::endl;
    std::cout << "Component 2 scale: " << mix1.component2().get_scale() << std::endl;

    // Тестирование исключений
    std::cout << "\n2. Testing exceptions:" << std::endl;
    try {
        Mixture bad_mix(p1, p2, 1.5); // неверный параметр
        std::cout << "ERROR: Should have thrown exception!" << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "Caught exception (expected): " << e.what() << std::endl;
    }

    std::cout << "\n3. Testing set/get functions for components:" << std::endl;
    // Доступ и изменение компонентов
    mix1.component1().set_location(0.5);
    mix1.component2().set_scale(1.2);
    
    std::cout << "After modification:" << std::endl;
    std::cout << "Component 1 location: " << mix1.component1().get_location() << std::endl;
    std::cout << "Component 2 scale: " << mix1.component2().get_scale() << std::endl;

    std::cout << "\n4. Testing density function:" << std::endl;
    std::cout << "Density at x=0.0: " << mix1.density(0.0) << std::endl;
    std::cout << "Density at x=1.0: " << mix1.density(1.0) << std::endl;
    std::cout << "Density at x=2.0: " << mix1.density(2.0) << std::endl;

    std::cout << "\n5. Testing numerical characteristics:" << std::endl;
    std::cout << "Expectation: " << mix1.expectation() << std::endl;
    std::cout << "Variance: " << mix1.variance() << std::endl;
    std::cout << "Skewness: " << mix1.skewness() << std::endl;
    std::cout << "Kurtosis: " << mix1.kurtosis() << std::endl;

    std::cout << "\n6. Testing random generation:" << std::endl;
    std::cout << "Generated samples: ";
    for (int i = 0; i < 5; ++i) {
        std::cout << mix1.generate() << " ";
    }
    std::cout << std::endl;

    std::cout << "\n7. Testing validity:" << std::endl;
    std::cout << "mix1 is valid: " << (mix1.is_valid() ? "true" : "false") << std::endl;
    
    // Тестирование граничных значений - ОБЕРНУТЬ В TRY-CATCH
    std::cout << "\n7.1 Testing boundary values:" << std::endl;
    try {
        Mixture mix2(p1, p2, 0.0001); // близко к 0
        std::cout << "mix2 with p=0.0001 is valid: " << (mix2.is_valid() ? "true" : "false") << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "Caught exception for p=0.0001: " << e.what() << std::endl;
    }
    
    try {
        Mixture mix3(p1, p2, 0.9999); // близко к 1
        std::cout << "mix3 with p=0.9999 is valid: " << (mix3.is_valid() ? "true" : "false") << std::endl;
    } catch (const distribution_exception& e) {
        std::cout << "Caught exception for p=0.9999: " << e.what() << std::endl;
    }

    std::cout << "\n8. Testing static functions for backward compatibility:" << std::endl;
    double mean, variance, skewness, kurtosis;
    Mixture::mixture_numeric_traits_static(mix1, &mean, &variance, &skewness, &kurtosis);
    std::cout << "Static function results:" << std::endl;
    std::cout << "  Mean: " << mean << std::endl;
    std::cout << "  Variance: " << variance << std::endl;
    
    std::cout << "Density via static function at x=1.0: " 
              << Mixture::mixture_density_static(1.0, mix1) << std::endl;
    
    std::cout << "Random value via static function: " 
              << Mixture::mixture_rand_value_static(mix1) << std::endl;
}