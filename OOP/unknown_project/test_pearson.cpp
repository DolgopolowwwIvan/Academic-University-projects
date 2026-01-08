#include "pearson_distribution.h"
#include <iostream>
#include <iomanip>

void testpearson_distribution() {
    std::cout << "=== Testing pearson_distribution Class ===" << std::endl;

    std::cout << "\n1. Testing constructors:" << std::endl;
    pearson_distribution dist1;
    std::cout << "Default constructor: mu=" << dist1.get_location()
        << ", lambda=" << dist1.get_scale()
        << ", nu=" << dist1.get_shape() << std::endl;

    pearson_distribution dist2(1.0, 2.0, 4.0);
    std::cout << "Parameterized constructor: mu=" << dist2.get_location()
        << ", lambda=" << dist2.get_scale()
        << ", nu=" << dist2.get_shape() << std::endl;

    // “естируем с невалидными параметрами
    try {
        pearson_distribution dist3(1.0, -1.0, 0.3);
        
    }
    catch (const distribution_exception& e) {
        std::cout << "Caught exception: " << e.what() << std::endl;
    }


    std::cout << "\n2. Testing set/get functions:" << std::endl;
    dist1.set_location(5.0);
    dist1.set_scale(3.0);
    dist1.set_shape(6.0);
    std::cout << "After modification: mu=" << dist1.get_location()
        << ", lambda=" << dist1.get_scale()
        << ", nu=" << dist1.get_shape() << std::endl;

    std::cout << "\n3. Testing density function:" << std::endl;
    double x = 0.0;
    std::cout << "Density at x=" << x << ": " << dist2.density(x) << std::endl;
    std::cout << "Density at x=" << dist2.get_location() << ": "
        << dist2.density(dist2.get_location()) << std::endl;

    std::cout << "\n4. Testing numerical characteristics:" << std::endl;
    std::cout << "Expectation: " << dist2.expectation() << std::endl;
    std::cout << "Variance: " << dist2.variance() << std::endl;
    std::cout << "Skewness: " << dist2.skewness() << std::endl;
    std::cout << "Kurtosis: " << dist2.kurtosis() << std::endl;

    std::cout << "\n5. Testing random generation:" << std::endl;
    double samples[5];
    for (int i = 0; i < 5; ++i) {
        samples[i] = dist2.generate();
    }
    std::cout << "Generated samples: ";
    for (int i = 0; i < 5; ++i) {
        std::cout << samples[i] << " ";
    }
    std::cout << std::endl;

    std::cout << "\n6. Testing validity:" << std::endl;
    std::cout << "dist1 is valid: " << (dist1.is_valid() ? "true" : "false") << std::endl;
    std::cout << "dist2 is valid: " << (dist2.is_valid() ? "true" : "false") << std::endl;
   
}

