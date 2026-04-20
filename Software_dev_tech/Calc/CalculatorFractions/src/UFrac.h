// UFrac.h - Абстрактный тип данных "Простая дробь"
// Original: Абстрактный тип данных простые дроби

#ifndef UFRAC_H
#define UFRAC_H

#include <string>
#include <numeric>
#include <stdexcept>

// ADT TFrac - Простая дробь (тип TFrac)
// Это пара целых чисел: числитель и знаменатель (a/b)
// Простые дроби изменяемые
class TFrac {
private:
    int FNum;       // Числитель (numerator)
    int FDenom;     // Знаменатель (denominator)

    // Сокращение дроби
    void Reduce();

public:
    // Конструктор по умолчанию: 0/1
    TFrac(int num = 0, int denom = 1);

    // Копирующий конструктор
    TFrac(const TFrac& other);

    // Деструктор
    ~TFrac();

    // Геттеры
    int GetNumerator() const { return FNum; }
    int GetDenominator() const { return FDenom; }

    // Сеттеры
    void SetNumerator(int num);
    void SetDenominator(int denom);

    // Установка значения дроби
    void Set(int num, int denom);

    // Проверка на ноль (0/1)
    bool IsZero() const;

    // Арифметические операции
    TFrac Add(const TFrac& other) const;
    TFrac Sub(const TFrac& other) const;
    TFrac Mul(const TFrac& other) const;
    TFrac Div(const TFrac& other) const;

    // Функции
    TFrac Sqr() const;      // Возведение в квадрат
    TFrac Rev() const;      // Обратное значение (1/x)

    // Преобразование в строку
    std::string ToString() const;

    // Преобразование в десятичное число
    double ToDouble() const;

    // Операторы присваивания
    TFrac& operator=(const TFrac& other);

    // Операторы сравнения
    bool operator==(const TFrac& other) const;
    bool operator!=(const TFrac& other) const;
};

#endif // UFRAC_H
