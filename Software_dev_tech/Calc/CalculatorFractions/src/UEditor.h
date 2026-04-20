// UEditor.h - Класс "Ввод и редактирование простых дробей"
// Original: РедакторПростыхДробей

#ifndef UEDITOR_H
#define UEDITOR_H

#include <string>

// Класс TEditor - Ввод и редактирование простых дробей
// Обязанность: ввод, хранение и редактирование строкового представления простых дробей
class TEditor {
private:
    std::string FString;  // Строковое представление редактируемой простой дроби

    // Константы формата
    static const std::string FSeparator;  // Разделитель числителя и знаменателя
    static const std::string FZeroString; // Строковое представление нуля

    // Проверка допустимости добавления символа
    bool CanAddDigit() const;
    bool CanAddSeparator() const;
    bool IsComplete() const;  // Дробь полностью введена (есть числитель и знаменатель)

public:
    // Конструктор
    TEditor();

    // Деструктор
    ~TEditor();

    // Дробь есть ноль - возвращает true, если строка содержит 0/1
    bool IsFractionZero() const;

    // Добавить знак - добавляет или удаляет знак "-" из строки
    std::string AddSign();

    // Добавить цифру - добавляет цифру к строке, если формат позволяет
    std::string AddDigit(int digit);

    // Добавить ноль - добавляет ноль к строке, если формат позволяет
    std::string AddZero();

    // Добавить разделитель - добавляет '/' к строке
    std::string AddSeparator();

    // Забой символа - удаляет крайний правый символ
    std::string Backspace();

    // Очистить - устанавливает строку в 0/1
    std::string Clear();

    // Редактировать - выполняет команду редактирования по номеру
    std::string Edit(int command);

    // Свойство: читать строку в формате строки
    std::string GetString() const;

    // Свойство: писать строку в формате строки
    void SetString(const std::string& value);

    // Получить дробь как TFrac
    class TFrac ToFraction() const;

    // Получить текущую позицию ввода (0 - числитель, 1 - знаменатель)
    int GetInputPosition() const;
};

// Команды редактирования
enum TEditCommand {
    ecDigit0 = 0,
    ecDigit1 = 1,
    ecDigit2 = 2,
    ecDigit3 = 3,
    ecDigit4 = 4,
    ecDigit5 = 5,
    ecDigit6 = 6,
    ecDigit7 = 7,
    ecDigit8 = 8,
    ecDigit9 = 9,
    ecSign = 10,
    ecSeparator = 11,
    ecBackspace = 12,
    ecClear = 13
};

#endif // UEDITOR_H
