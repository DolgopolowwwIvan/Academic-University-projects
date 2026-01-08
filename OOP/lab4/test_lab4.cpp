#include <iostream>
#include <vector>
#include <memory>
#include "pearson_distribution.h"
#include "mixture.h"
#include "empiric.h"
#include "idistribution.h"

void test_polymorphism() {
    std::cout << "=== Тестирование полиморфизма и интерфейсов ===" << std::endl;
    
    // Создаем объекты разных типов
    pearson_distribution p1(0.0, 1.0, 3.0);
    pearson_distribution p2(2.0, 1.5, 4.0);
    
    // Шаблонная смесь
    Mixture<pearson_distribution, pearson_distribution> mixture(p1, p2, 0.3);
    
    // Эмпирическое распределение из pearson
    Empiric empiric1(1000, p1, 20);
    
    // Эмпирическое распределение из смеси (через интерфейс)
    Empiric empiric2(1000, mixture, 20);
    
    // Массив указателей на интерфейс распределения
    std::vector<IDistribution*> distributions;
    distributions.push_back(&p1);
    distributions.push_back(&p2);
    distributions.push_back(&mixture);
    distributions.push_back(&empiric1);
    distributions.push_back(&empiric2);
    
    // Демонстрация позднего связывания
    std::cout << "\n1. Позднее связывание - вызов density(0.0):" << std::endl;
    for (size_t i = 0; i < distributions.size(); ++i) {
        std::cout << "  Распределение " << i+1 << ": density(0.0) = " 
                  << distributions[i]->density(0.0) << std::endl;
    }
    
    // Демонстрация полиморфной генерации
    std::cout << "\n2. Полиморфная генерация (по 3 значения):" << std::endl;
    for (size_t i = 0; i < distributions.size(); ++i) {
        std::cout << "  Распределение " << i+1 << ": ";
        for (int j = 0; j < 3; ++j) {
            std::cout << distributions[i]->generate() << " ";
        }
        std::cout << std::endl;
    }
    
    // Массив указателей на персистентные объекты
    std::vector<IPersistent*> persistent_objects;
    persistent_objects.push_back(&p1);
    persistent_objects.push_back(&mixture);
    persistent_objects.push_back(&empiric1);
    
    std::cout << "\n3. Сохранение объектов (персистентность):" << std::endl;
    for (size_t i = 0; i < persistent_objects.size(); ++i) {
        std::string filename = "test_object_" + std::to_string(i) + ".txt";
        try {
            persistent_objects[i]->save_to_file(filename);
            std::cout << "  Объект " << i+1 << " сохранен в " << filename << std::endl;
        } catch (const std::exception& e) {
            std::cout << "  Ошибка сохранения объекта " << i+1 << ": " << e.what() << std::endl;
        }
    }
    
    // Тестирование сложных шаблонных смесей
    std::cout << "\n4. Тестирование шаблонных смесей:" << std::endl;
    
    // Смесь смесей (двуслойная смесь)
    Mixture<pearson_distribution, pearson_distribution> mixture1(p1, p2, 0.3);
    pearson_distribution p3(1.0, 0.5, 5.0);
    pearson_distribution p4(-1.0, 2.0, 6.0);
    Mixture<pearson_distribution, pearson_distribution> mixture2(p3, p4, 0.7);
    
    // Смесь смесей (шаблон с шаблоном)
    Mixture<Mixture<pearson_distribution, pearson_distribution>, 
            Mixture<pearson_distribution, pearson_distribution>> 
        deep_mixture(mixture1, mixture2, 0.5);
    
    std::cout << "  Глубокая смесь создана успешно" << std::endl;
    std::cout << "  Матожидание глубокой смеси: " << deep_mixture.expectation() << std::endl;
    
    // Эмпирическое распределение из глубокой смеси
    Empiric empiric_deep(2000, deep_mixture, 30);
    std::cout << "  Эмпирическое распределение из глубокой смеси создано" << std::endl;
    std::cout << "  Объем выборки: " << empiric_deep.get_sample_size() << std::endl;
    
    std::cout << "\n=== Тестирование завершено ===" << std::endl;
}

void test_template_mixtures() {
    std::cout << "\n=== Тестирование различных инстанциаций шаблона ===" << std::endl;
    
    pearson_distribution p1(0.0, 1.0, 3.0);
    pearson_distribution p2(1.0, 2.0, 4.0);
    Empiric emp1(500, p1, 10);
    
    // Разные типы смесей
    Mixture<pearson_distribution, pearson_distribution> mix1(p1, p2, 0.3);
    Mixture<pearson_distribution, Empiric> mix2(p1, emp1, 0.5);
    Mixture<Empiric, pearson_distribution> mix3(emp1, p2, 0.7);
    
    std::cout << "1. Смесь pearson+pearson:" << std::endl;
    std::cout << "   density(0.5) = " << mix1.density(0.5) << std::endl;
    std::cout << "   expectation = " << mix1.expectation() << std::endl;
    
    std::cout << "\n2. Смесь pearson+empiric:" << std::endl;
    std::cout << "   density(0.5) = " << mix2.density(0.5) << std::endl;
    std::cout << "   expectation = " << mix2.expectation() << std::endl;
    
    std::cout << "\n3. Смесь empiric+pearson:" << std::endl;
    std::cout << "   density(0.5) = " << mix3.density(0.5) << std::endl;
    std::cout << "   expectation = " << mix3.expectation() << std::endl;
}

int main() {
    try {
        test_polymorphism();
        test_template_mixtures();
        
        std::cout << "\nВсе тесты пройдены успешно!" << std::endl;
        return 0;
    } catch (const std::exception& e) {
        std::cerr << "Ошибка: " << e.what() << std::endl;
        return 1;
    }
}