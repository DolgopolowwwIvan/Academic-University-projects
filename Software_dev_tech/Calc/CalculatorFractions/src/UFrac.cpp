// UFrac.cpp - Реализация абстрактного типа данных "Простая дробь"
// Original: Реализация типа данные простые дроби

#include "UFrac.h"
#include <cmath>

// Сокращение дроби
void TFrac::Reduce() {
    if (FDenom == 0) {
        throw std::invalid_argument("Denominator cannot be zero");
    }
    
    // Знаменатель всегда положительный
    if (FDenom < 0) {
        FNum = -FNum;
        FDenom = -FDenom;
    }
    
    // Сокращение на НОД
    int gcd = std::gcd(std::abs(FNum), std::abs(FDenom));
    if (gcd != 0) {
        FNum /= gcd;
        FDenom /= gcd;
    }
}

// Конструктор
TFrac::TFrac(int num, int denom) : FNum(num), FDenom(denom) {
    Reduce();
}

// Копирующий конструктор
TFrac::TFrac(const TFrac& other) : FNum(other.FNum), FDenom(other.FDenom) {}

// Деструктор
TFrac::~TFrac() {}

// Сеттер числителя
void TFrac::SetNumerator(int num) {
    FNum = num;
    Reduce();
}

// Сеттер знаменателя
void TFrac::SetDenominator(int denom) {
    if (denom == 0) {
        throw std::invalid_argument("Denominator cannot be zero");
    }
    FDenom = denom;
    Reduce();
}

// Установка значения дроби
void TFrac::Set(int num, int denom) {
    if (denom == 0) {
        throw std::invalid_argument("Denominator cannot be zero");
    }
    FNum = num;
    FDenom = denom;
    Reduce();
}

// Проверка на ноль
bool TFrac::IsZero() const {
    return FNum == 0;
}

// Сложение
TFrac TFrac::Add(const TFrac& other) const {
    int num = FNum * other.FDenom + other.FNum * FDenom;
    int denom = FDenom * other.FDenom;
    return TFrac(num, denom);
}

// Вычитание
TFrac TFrac::Sub(const TFrac& other) const {
    int num = FNum * other.FDenom - other.FNum * FDenom;
    int denom = FDenom * other.FDenom;
    return TFrac(num, denom);
}

// Умножение
TFrac TFrac::Mul(const TFrac& other) const {
    int num = FNum * other.FNum;
    int denom = FDenom * other.FDenom;
    return TFrac(num, denom);
}

// Деление
TFrac TFrac::Div(const TFrac& other) const {
    if (other.FNum == 0) {
        throw std::invalid_argument("Division by zero fraction");
    }
    int num = FNum * other.FDenom;
    int denom = FDenom * other.FNum;
    return TFrac(num, denom);
}

// Возведение в квадрат
TFrac TFrac::Sqr() const {
    return TFrac(FNum * FNum, FDenom * FDenom);
}

// Обратное значение
TFrac TFrac::Rev() const {
    if (FNum == 0) {
        throw std::invalid_argument("Cannot reverse zero fraction");
    }
    return TFrac(FDenom, FNum);
}

// Преобразование в строку
std::string TFrac::ToString() const {
    return std::to_string(FNum) + "/" + std::to_string(FDenom);
}

// Преобразование в десятичное число
double TFrac::ToDouble() const {
    return static_cast<double>(FNum) / static_cast<double>(FDenom);
}

// Оператор присваивания
TFrac& TFrac::operator=(const TFrac& other) {
    if (this != &other) {
        FNum = other.FNum;
        FDenom = other.FDenom;
    }
    return *this;
}

// Оператор равенства
bool TFrac::operator==(const TFrac& other) const {
    return FNum == other.FNum && FDenom == other.FDenom;
}

// Оператор неравенства
bool TFrac::operator!=(const TFrac& other) const {
    return !(*this == other);
}
