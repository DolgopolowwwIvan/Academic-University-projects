#include <iostream>
#include <cmath>
#include <string>
#include <algorithm>

using namespace std;

// Функция перевода десятичного числа в любую систему счисления
string decimalToAnyBase(double num, int base, int precision = 6) {
    if (base < 2 || base > 36) return "Base not supported!";

    bool isNegative = (num < 0);
    if (isNegative) num = -num;

    string result;
    long long integerPart = static_cast<long long>(num);  // Целая часть
    double fractionalPart = num - integerPart;            // Дробная часть

    // Перевод целой части
    string intPartStr;
    while (integerPart > 0) {
        int remainder = integerPart % base;
        intPartStr += (remainder < 10) ? remainder + '0' : remainder - 10 + 'A';
        integerPart /= base;
    }
    if (intPartStr.empty()) intPartStr = "0";
    reverse(intPartStr.begin(), intPartStr.end());

    result = intPartStr;

    // Перевод дробной части
    if (fractionalPart > 0) {
        result += '.';
        while (precision-- > 0) {
            fractionalPart *= base;
            int fractionalIntPart = static_cast<int>(fractionalPart);
            result += (fractionalIntPart < 10) ? fractionalIntPart + '0' : fractionalIntPart - 10 + 'A';
            fractionalPart -= fractionalIntPart;
        }
    }

    return isNegative ? "-" + result : result;
}

// Функция перевода из любой системы счисления в десятичную
double anyBaseToDecimal(const string& numStr, int base) {
    if (base < 2 || base > 36) return -1;  // Unsupported base

    bool isNegative = (numStr[0] == '-');
    string number = isNegative ? numStr.substr(1) : numStr;

    int pointPos = number.find('.');
    if (pointPos == string::npos) pointPos = number.length();

    // Обработка целой части
    double decimalValue = 0;    
    int exponent = 0;
    for (int i = pointPos - 1; i >= 0; --i) {
        char digit = number[i];
        int value = (isdigit(digit)) ? digit - '0' : toupper(digit) - 'A' + 10;
        decimalValue += value * pow(base, exponent++);
    }

    // Обработка дробной части
    exponent = -1;
    for (int i = pointPos + 1; i < number.length(); ++i) {
        char digit = number[i];
        int value = (isdigit(digit)) ? digit - '0' : toupper(digit) - 'A' + 10;
        decimalValue += value * pow(base, exponent--);
    }

    return isNegative ? -decimalValue : decimalValue;
}

// Функция сложения и вычитания двух чисел в любой системе счисления
string addAndMultiplyInBase(const string& num1, const string& num2, int base, char operation) {
    double decimalNum1 = anyBaseToDecimal(num1, base);
    double decimalNum2 = anyBaseToDecimal(num2, base);

    double resultDecimal;
    if (operation == '+') {
        resultDecimal = decimalNum1 + decimalNum2;
    }
    else if (operation == '-') {
        resultDecimal = decimalNum1 - decimalNum2;
    }
    else if (operation == '*') {
        resultDecimal = decimalNum1 * decimalNum2;
    }
    else {
        return "Unsupported operation!";
    }

    return decimalToAnyBase(resultDecimal, base, 10); // Увеличиваем precision для точных дробных чисел
}

int main() {

    setlocale(LC_ALL, "RU");

    // Пункт a) Перевод десятичных чисел в разные системы счисления
    cout << "1. Перевод десятичных чисел:\n";
    double decimalNums[] = {390, 101, 1001.34375, 658.5, 138.625};
    for (double num : decimalNums) {
        cout << "Число " << num << " в двоичной системе: " << decimalToAnyBase(num, 2) << endl;
        cout << "Число " << num << " в восьмеричной системе: " << decimalToAnyBase(num, 8) << endl;
        cout << "Число " << num << " в шестнадцатеричной системе: " << decimalToAnyBase(num, 16) << endl;
        cout << endl;
    }

    // Пункт b) Перевод чисел из других систем в десятичную
    cout << "2. Перевод чисел в десятичную систему:\n";
    string baseNumbers[] = {"1011110", "101011100.101", "441.5", "11B.7"};
    int bases[] = {2, 2, 8, 16}; // Основания систем счисления для соответствующих чисел
    for (int i = 0; i < 4; ++i) {
        cout << "Число " << baseNumbers[i] << " из системы " << bases[i] << " в десятичную: " 
             << anyBaseToDecimal(baseNumbers[i], bases[i]) << endl;
    }


    cout << "\n3. Арифметические операции:\n";
    
    string num1 = "1000110";   
    string num2 = "100101101.11"; 
    cout << "1000110 + 100101101.11 в двоичной системе: " << addAndMultiplyInBase(num1, num2, 2, '+') << endl;

    num1 = "101010101.111";  
    num2 = "110100011.1";  
    cout << "101010101.111 - 110100011.1 в двоичной системе: " << addAndMultiplyInBase(num1, num2, 2, '-') << endl;

   
    num1 = "176.4";  
    num2 = "2.0";   
    cout << "176.4 + 2.0 в восьмеричной системе: " << addAndMultiplyInBase(num1, num2, 8, '+') << endl;

   
    num1 = "412.5";  
    num2 = "236.7";  
    cout << "412.5 - 236.7 в восьмеричной системе: " << addAndMultiplyInBase(num1, num2, 8, '-') << endl;

    
    num1 = "12B.4";  
    num2 = "165.0";  
    cout << "12B.4 + 165.0 в шестнадцатеричной системе: " << addAndMultiplyInBase(num1, num2, 16, '+') << endl;

    
    num1 = "1A8.8";  
    num2 = "DF.0";   
    cout << "1A8.8 - DF.0 в шестнадцатеричной системе: " << addAndMultiplyInBase(num1, num2, 16, '-') << endl;

    num1 = "101110001.111";
    num2 = "111111011.11";
    cout << "1101110001.111 *s 111111011.11 в двоичной системе: " << addAndMultiplyInBase(num1, num2, 2, '*') << endl;
    
    num1 = "454.1";
    num2 = "557.3";
    cout << "454.1 * 557.3 в восьмеричной системе: " << addAndMultiplyInBase(num1, num2, 8, '*') << endl;

    num1 = "60.C";
    num2 = "1A5.A";
    cout << "60.C * 1A5.A в шестнадцатеричной системе: " << addAndMultiplyInBase(num1, num2, 16, '*') << endl;


    return 0;
}
