// UMemory.h - Параметризованный абстрактный тип данных "Память"
// Original: ADT TMemory - Память

#ifndef UMEMORY_H
#define UMEMORY_H

#include <string>

// Состояния памяти
enum TMemoryState {
    msOff,  // Выключена
    msOn    // Включена
};

// Преобразование состояния в строку
inline std::string MemoryStateToString(TMemoryState state) {
    return (state == msOn) ? "M" : " ";
}

// TMemory - Параметризованный класс памяти для хранения одного числа типа T
// Шаблон класса для хранения объекта типа T
template <class T>
class TMemory {
private:
    T FNumber;        // Хранимое число
    TMemoryState FState;  // Состояние памяти

public:
    // Конструктор
    // Инициализирует поле FNumber объектом типа T со значением по умолчанию
    // Память устанавливается в состояние "Выключена" (msOff)
    TMemory() : FNumber(), FState(msOff) {}

    // Деструктор
    ~TMemory() {}

    // Записать - записывает объект E в память
    // Память устанавливается в состояние "Включена" (msOn)
    void Store(const T& E) {
        FNumber = E;
        FState = msOn;
    }

    // Взять - возвращает копию объекта из памяти
    T Recall() const {
        return FNumber;
    }

    // Добавить - добавляет объект E к хранимому значению
    void Add(const T& E) {
        FNumber = FNumber.Add(E);  // Предполагается, что у типа T есть метод Add
        FState = msOn;
    }

    // Очистить - сбрасывает память в значение по умолчанию
    // Память устанавливается в состояние "Выключена" (msOff)
    void Clear() {
        FNumber = T();  // Значение по умолчанию для типа T
        FState = msOff;
    }

    // Читать состояние памяти - возвращает строковое представление состояния
    std::string ReadState() const {
        return MemoryStateToString(FState);
    }

    // Читать число - возвращает хранимое значение
    T ReadNumber() const {
        return FNumber;
    }

    // Свойство: состояние памяти (геттер)
    TMemoryState GetState() const {
        return FState;
    }

    // Свойство: состояние памяти (сеттер)
    void SetState(TMemoryState state) {
        FState = state;
    }

    // Свойство: число (геттер)
    T GetNumber() const {
        return FNumber;
    }

    // Свойство: число (сеттер)
    void SetNumber(const T& value) {
        FNumber = value;
    }

    // Проверка: память включена
    bool IsOn() const {
        return FState == msOn;
    }
};

#endif // UMEMORY_H
