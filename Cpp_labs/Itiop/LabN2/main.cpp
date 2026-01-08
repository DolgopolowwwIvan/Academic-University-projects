#include <iostream>
#include <iomanip>
#include <cmath>
#include <cstdint>

using namespace std;

// Функция для кодирования float в шестнадцатеричное представление IEEE 754 (4 байта)
uint32_t encodeFloat(float num) {
    uint32_t result = 0;

    // Определяем знак, мантиссу и порядок
    int sign = (num < 0) ? 1 : 0;
    if (sign) num = -num; // Берем модуль числа

    int exponent = 0;
    float mantissa = frexp(num, &exponent);

    // Нормализуем мантиссу и порядок для формата IEEE 754
    exponent += 126; // Смещение для float: 127 - 1
    mantissa = (mantissa - 0.5f) * 2; // Приводим мантиссу к формату 1.xxxxx

    // Кодируем знак, порядок и мантиссу в 32-битное число
    result |= (sign << 31);
    result |= (exponent << 23);
    result |= (static_cast<uint32_t>(mantissa * (1 << 23)) & 0x7FFFFF);

    return result;
}

// Функция для кодирования double в шестнадцатеричное представление IEEE 754 (8 байт)
uint64_t encodeDouble(double num) {
    uint64_t result = 0;

    // Определяем знак, мантиссу и порядок
    int sign = (num < 0) ? 1 : 0;
    if (sign) num = -num; // Берем модуль числа

    int exponent = 0;
    double mantissa = frexp(num, &exponent);

    // Нормализуем мантиссу и порядок для формата IEEE 754
    exponent += 1022; // Смещение для double: 1023 - 1
    mantissa = (mantissa - 0.5) * 2; // Приводим мантиссу к формату 1.xxxxx

    // Кодируем знак, порядок и мантиссу в 64-битное число
    result |= (static_cast<uint64_t>(sign) << 63);
    result |= (static_cast<uint64_t>(exponent) << 52);
    result |= (static_cast<uint64_t>(mantissa * (1ULL << 52)) & 0xFFFFFFFFFFFFF);

    return result;
}

// Функция для декодирования шестнадцатеричного представления IEEE 754 в float
float decodeFloat(uint32_t hexValue) {
    // Извлекаем знак, порядок и мантиссу
    int sign = (hexValue >> 31) & 1;
    int exponent = ((hexValue >> 23) & 0xFF) - 127;
    uint32_t mantissaBits = hexValue & 0x7FFFFF;

    // Восстанавливаем мантиссу с учетом скрытого бита
    float mantissa = 1.0f + mantissaBits / static_cast<float>(1 << 23);

    // Вычисляем значение числа
    float result = ldexp(mantissa, exponent);
    if (sign) result = -result;

    return result;
}

// Функция для декодирования шестнадцатеричного представления IEEE 754 в double
double decodeDouble(uint64_t hexValue) {
    // Извлекаем знак, порядок и мантиссу
    int sign = (hexValue >> 63) & 1;
    int exponent = ((hexValue >> 52) & 0x7FF) - 1023;
    uint64_t mantissaBits = hexValue & 0xFFFFFFFFFFFFF;

    // Восстанавливаем мантиссу с учетом скрытого бита
    double mantissa = 1.0 + mantissaBits / static_cast<double>(1ULL << 52);

    // Вычисляем значение числа
    double result = ldexp(mantissa, exponent);
    if (sign) result = -result;

    return result;
}

int main() {

    setlocale(LC_ALL, "RU");
    // Пример чисел
    float floatNum = 0.05f;
    double doubleNum = 0.05;

// Кодирование в шестнадцатеричное представление
    uint32_t floatHex = encodeFloat(floatNum);
    uint64_t doubleHex = encodeDouble(doubleNum);

    cout << "Число (float): " << floatNum << " -> Шестнадцатеричное представление: 0x" << hex << setw(8) << setfill('0') << floatHex << endl;
    cout << "Число (double): " << doubleNum << " -> Шестнадцатеричное представление: 0x" << hex << setw(16) << setfill('0') << doubleHex << endl;

    float floatNum1 = -142.195f;
    double doubleNum1 = -142.195;

// Кодирование в шестнадцатеричное представление
    uint32_t floatHex1 = encodeFloat(floatNum1);
    uint64_t doubleHex1 = encodeDouble(doubleNum1);

    cout << "Число (float): " << floatNum1 << " -> Шестнадцатеричное представление: 0x" << hex << setw(8) << setfill('0') << floatHex1 << endl;
    cout << "Число (double): " << doubleNum1 << " -> Шестнадцатеричное представление: 0x" << hex << setw(16) << setfill('0') << doubleHex1 << endl;

    float floatNum2 = 445.125f;
    double doubleNum2 = 445.125;

// Кодирование в шестнадцатеричное представление
    uint32_t floatHex2 = encodeFloat(floatNum2);
    uint64_t doubleHex2 = encodeDouble(doubleNum2);

    cout << "Число (float): " << floatNum2 << " -> Шестнадцатеричное представление: 0x" << hex << setw(8) << setfill('0') << floatHex2 << endl;
    cout << "Число (double): " << doubleNum2 << " -> Шестнадцатеричное представление: 0x" << hex << setw(16) << setfill('0') << doubleHex2 << endl;

    float floatNum3 = 34.9609f;
    double doubleNum3 = 34.9609;

    // Кодирование в шестнадцатеричное представление
    uint32_t floatHex3 = encodeFloat(floatNum3);
    uint64_t doubleHex3 = encodeDouble(doubleNum3);

    cout << "Число (float): " << floatNum3 << " -> Шестнадцатеричное представление: 0x" << hex << setw(8) << setfill('0') << floatHex3 << endl;
    cout << "Число (double): " << doubleNum3 << " -> Шестнадцатеричное представление: 0x" << hex << setw(16) << setfill('0') << doubleHex3 << endl;


    uint32_t floatHex11 = 0xc2a0c000;
    uint64_t doubleHex11 = 0xC2A0C00000000000;

    uint32_t floatHex22 = 0x4595a800;
    uint64_t doubleHex22 = 0x4595a80000000000;

    uint32_t floatHex33 = 0xc07b1d60;
    uint64_t doubleHex33 = 0xc07b1d6000000000;

    uint32_t floatHex44 = 0x40758300;
    uint64_t doubleHex44 = 0x4075830000000000;

    // Декодирование из шестнадцатеричного представления обратно в число
    float decodedFloat11 = decodeFloat(floatHex11);
    double decodedDouble11 = decodeDouble(doubleHex11);

    cout << "\nПреобразование шестнадцатеричного float обратно в число: " << decodedFloat11 << endl;
    cout << "Преобразование шестнадцатеричного double обратно в число: " << decodedDouble11 << endl;

    float decodedFloat22 = decodeFloat(floatHex22);
    double decodedDouble22 = decodeDouble(doubleHex22);

    cout << "\nПреобразование шестнадцатеричного float обратно в число: " << decodedFloat22 << endl;
    cout << "Преобразование шестнадцатеричного double обратно в число: " << decodedDouble22 << endl;

    float decodedFloat33 = decodeFloat(floatHex33);
    double decodedDouble33 = decodeDouble(doubleHex33);

    cout << "\nПреобразование шестнадцатеричного float обратно в число: " << decodedFloat33 << endl;
    cout << "Преобразование шестнадцатеричного double обратно в число: " << decodedDouble33 << endl;

    float decodedFloat44 = decodeFloat(floatHex44);
    double decodedDouble44 = decodeDouble(doubleHex44);

    cout << "\nПреобразование шестнадцатеричного float обратно в число: " << decodedFloat44 << endl;
    cout << "Преобразование шестнадцатеричного double обратно в число: " << decodedDouble44 << endl;




    return 0;
}



