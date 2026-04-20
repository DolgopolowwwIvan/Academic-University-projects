// UEditor.cpp - Реализация класса "Ввод и редактирование простых дробей"
// Original: Реализация РедакторПростыхДробей

#include "UEditor.h"
#include "UFrac.h"
#include <cctype>

// Инициализация констант
const std::string TEditor::FSeparator = "/";
const std::string TEditor::FZeroString = "0/1";

// Конструктор
TEditor::TEditor() : FString(FZeroString) {}

// Деструктор
TEditor::~TEditor() {}

// Проверка: можно ли добавить цифру
bool TEditor::CanAddDigit() const {
    if (FString.empty()) return true;
    
    size_t sepPos = FString.find(FSeparator);
    if (sepPos == std::string::npos) {
        // Ещё нет разделителя - добавляем в числитель
        // Проверяем, не слишком ли длинный числитель
        return FString.length() < 10;
    } else {
        // Есть разделитель - добавляем в знаменатель
        return (FString.length() - sepPos - 1) < 10;
    }
}

// Проверка: можно ли добавить разделитель
bool TEditor::CanAddSeparator() const {
    // Разделитель можно добавить только если есть числитель и ещё нет разделителя
    if (FString.empty() || FString == "-" || FString == "-0") return false;
    
    // Если это "0/1" - нельзя добавить ещё один разделитель
    if (FString == "0/1" || FString == "-0/1") return false;
    
    // Проверяем, нет ли уже разделителя
    return FString.find(FSeparator) == std::string::npos;
}

// Проверка: дробь полностью введена
bool TEditor::IsComplete() const {
    size_t sepPos = FString.find(FSeparator);
    if (sepPos == std::string::npos) return false;
    if (sepPos == 0) return false;  // Нет числителя
    if (sepPos == FString.length() - 1) return false;  // Нет знаменателя
    return true;
}

// Дробь есть ноль
bool TEditor::IsFractionZero() const {
    try {
        TFrac frac = ToFraction();
        return frac.IsZero();
    } catch (...) {
        return false;
    }
}

// Добавить знак
std::string TEditor::AddSign() {
    if (FString.empty()) {
        FString = "-";
    } else if (FString[0] == '-') {
        FString = FString.substr(1);
    } else if (FString != "0" && !FString.empty() && FString.find(FSeparator) == std::string::npos) {
        FString = "-" + FString;
    }
    return FString;
}

// Добавить цифру
std::string TEditor::AddDigit(int digit) {
    if (digit < 0 || digit > 9) return FString;
    
    char digitChar = '0' + digit;
    
    // Если текущее значение "0/1" или "-0/1" - начинаем новый ввод
    if (FString == "0/1" || FString == "-0/1" || FString == "0" || FString == "-0") {
        // Заменяем начальное значение
        if (FString[0] == '-') {
            FString = "-" + std::string(1, digitChar);
        } else {
            FString = std::string(1, digitChar);
        }
        return FString;
    }
    
    if (!CanAddDigit()) return FString;
    
    if (FString.empty()) {
        FString = std::string(1, digitChar);
    } else {
        FString += digitChar;
    }
    
    return FString;
}

// Добавить ноль
std::string TEditor::AddZero() {
    return AddDigit(0);
}

// Добавить разделитель
std::string TEditor::AddSeparator() {
    if (!CanAddSeparator()) return FString;
    
    // Если строка пустая или только "-", добавляем 0 перед разделителем
    if (FString.empty() || FString == "-") {
        FString += "0";
    }
    
    FString += FSeparator;
    return FString;
}

// Забой символа
std::string TEditor::Backspace() {
    if (FString.empty()) return FString;
    
    // Удаляем последний символ
    FString.pop_back();
    
    // Если строка стала пустой или "-", устанавливаем 0/1
    if (FString.empty() || FString == "-") {
        FString = FZeroString;
    }
    
    // Если удалили разделитель и осталась только "-" или пусто
    if (FString == "-" || FString.empty()) {
        FString = FZeroString;
    }
    
    return FString;
}

// Очистить
std::string TEditor::Clear() {
    FString = FZeroString;
    return FString;
}

// Редактировать по команде
std::string TEditor::Edit(int command) {
    switch (command) {
        case ecDigit0: return AddZero();
        case ecDigit1: return AddDigit(1);
        case ecDigit2: return AddDigit(2);
        case ecDigit3: return AddDigit(3);
        case ecDigit4: return AddDigit(4);
        case ecDigit5: return AddDigit(5);
        case ecDigit6: return AddDigit(6);
        case ecDigit7: return AddDigit(7);
        case ecDigit8: return AddDigit(8);
        case ecDigit9: return AddDigit(9);
        case ecSign: return AddSign();
        case ecSeparator: return AddSeparator();
        case ecBackspace: return Backspace();
        case ecClear: return Clear();
        default: return FString;
    }
}

// Свойство: читать строку
std::string TEditor::GetString() const {
    return FString;
}

// Свойство: писать строку
void TEditor::SetString(const std::string& value) {
    // Простая валидация
    if (value.empty()) {
        FString = FZeroString;
        return;
    }
    
    // Проверяем формат: [-]<числитель>/<знаменатель>
    size_t sepPos = value.find(FSeparator);
    if (sepPos == std::string::npos) {
        // Нет разделителя - добавляем
        FString = value + FSeparator + "1";
    } else {
        FString = value;
    }
    
    // Проверяем корректность
    try {
        ToFraction();
    } catch (...) {
        FString = FZeroString;
    }
}

// Получить дробь как TFrac
TFrac TEditor::ToFraction() const {
    size_t sepPos = FString.find(FSeparator);
    if (sepPos == std::string::npos) {
        // Нет разделителя - считаем как целое число
        return TFrac(std::stoi(FString), 1);
    }
    
    std::string numStr = FString.substr(0, sepPos);
    std::string denomStr = FString.substr(sepPos + 1);
    
    if (numStr.empty() || numStr == "-") numStr = "0";
    if (denomStr.empty()) denomStr = "1";
    
    int num = std::stoi(numStr);
    int denom = std::stoi(denomStr);
    
    if (denom == 0) denom = 1;  // Защита от деления на ноль
    
    return TFrac(num, denom);
}

// Получить позицию ввода
int TEditor::GetInputPosition() const {
    size_t sepPos = FString.find(FSeparator);
    if (sepPos == std::string::npos) return 0;  // Числитель
    return 1;  // Знаменатель
}
