#include <iostream>
#include <string>
#include <cstdlib> // для system()
#include <vector>
#include <limits>

using namespace std;

// Функция для Программы 1 (матричные вычисления)
void run_program1() {
    cout << "\n=== Запуск Программы 1 ===\n";
    system("./ScalarSolver"); // или system("g++ program1.cpp -o program1 && ./program1");
}

// Функция для Программы 2 (вычисление произведения дробей)
void run_program2() {
    cout << "\n=== Запуск Программы 2 ===\n";
    system("./Multiplier"); // или system("g++ program2.cpp -o program2 && ./program2");
}

// Функция для вывода справки
void show_help() {
    cout << "\n=== Справка ===\n"
         << "1. Программа 1 - вычисление матричного выражения (Cx·y)/(x·By) + Ax·By\n"
         << "2. Программа 2 - вычисление произведения дробей\n"
         << "3. Разработчик: Долгополов Иван\n"
         << "4. Для выбора программы введите соответствующую цифру в меню\n";
}

// Главное меню
void show_menu() {
    cout << "\n=== Главное меню ===\n"
         << "1. Запуск Программы 1\n"
         << "2. Запуск Программы 2\n"
         << "3. Справка\n"
         << "4. Выход\n"
         << "Выберите действие: ";
}

int main() {
    setlocale(LC_ALL, "RU");
    
    int choice;
    do {
        show_menu();
        while (!(cin >> choice) || choice < 1 || choice > 4) {
            cout << "Ошибка ввода! Введите число от 1 до 4: ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

        switch (choice) {
            case 1:
                run_program1();
                break;
            case 2:
                run_program2();
                break;
            case 3:
                show_help();
                break;
            case 4:
                cout << "Завершение работы...\n";
                break;
        }
    } while (choice != 4);

    return 0;
}
