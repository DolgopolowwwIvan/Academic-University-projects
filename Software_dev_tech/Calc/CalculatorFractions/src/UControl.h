// UControl.h - Класс "Управление калькулятором чисел"
// Original: УправлениеКалькуляторомПростыхДробей (тип TCtrl)

#ifndef UCONTROL_H
#define UCONTROL_H

#include <string>
#include "UFrac.h"
#include "UEditor.h"
#include "UMemory.h"
#include "UProc.h"

// Состояния калькулятора
// Original: TCtrlState = (cStart, cEditing, FunDone, cValDone, cExpDone, cOpChange, cError)
enum TCtrlState {
    cStart,      // Начальное
    cEditing,    // Ввод и редактирование
    cFunDone,    // Функция выполнена
    cValDone,    // Значение введено
    cExpDone,    // Выражение вычислено
    cOpChange,   // Операция изменена
    cError       // Ошибка
};

// Преобразование состояния в строку
inline std::string CtrlStateToString(TCtrlState state) {
    switch (state) {
        case cStart: return "Start";
        case cEditing: return "Editing";
        case cFunDone: return "FunDone";
        case cValDone: return "ValDone";
        case cExpDone: return "ExpDone";
        case cOpChange: return "OpChange";
        case cError: return "Error";
        default: return "Unknown";
    }
}

// Команды калькулятора
enum TCalcCommand {
    cmdDigit0 = 0,
    cmdDigit1 = 1,
    cmdDigit2 = 2,
    cmdDigit3 = 3,
    cmdDigit4 = 4,
    cmdDigit5 = 5,
    cmdDigit6 = 6,
    cmdDigit7 = 7,
    cmdDigit8 = 8,
    cmdDigit9 = 9,
    cmdSign = 10,
    cmdSeparator = 11,
    cmdBackspace = 12,
    cmdClearEntry = 13,   // CE
    cmdClear = 14,        // C
    cmdAdd = 15,          // +
    cmdSub = 16,          // -
    cmdMul = 17,          // *
    cmdDiv = 18,          // /
    cmdEqual = 19,        // =
    cmdSqr = 20,          // Sqr
    cmdRev = 21,          // Rev
    cmdMC = 22,           // Memory Clear
    cmdMS = 23,           // Memory Store
    cmdMR = 24,           // Memory Recall
    cmdMPlus = 25,        // Memory Plus
    cmdCopy = 26,         // Копировать в буфер
    cmdPaste = 27         // Вставить из буфера
};

// Класс управления калькулятором
// Обязанность: управление выполнением команд калькулятора
class TCtrl {
private:
    TCtrlState FState;           // Состояние калькулятора
    TEditor FEditor;             // Редактор
    TMemory<TFrac> FMemory;      // Память
    TProc<TFrac> FProcessor;     // Процессор
    TFrac FNumber;               // Текущее число (результат)
    std::string FClipboard;      // Буфер обмена
    std::string FDisplayString;  // Строка для отображения
    TCtrlState FPrevState;       // Предыдущее состояние (для контекста)

    // Вспомогательные методы
    void SetInitialState();
    std::string FractionToString(const TFrac& frac);
    TOprtn CommandToOperation(int cmd);

public:
    // Конструктор
    TCtrl();

    // Деструктор
    ~TCtrl();

    // Выполнить команду калькулятора
    // Возвращает строку результата, обновляет MState (состояние памяти)
    std::string ExecuteCommand(int command, std::string& clipboard, std::string& MState);

    // Выполнить команду редактора
    std::string ExecuteEditorCommand(int command);

    // Выполнить операцию
    std::string ExecuteOperation(int command);

    // Выполнить функцию
    std::string ExecuteFunction(int command);

    // Вычислить выражение (команда =)
    std::string CalculateExpression();

    // Установить начальное состояние калькулятора
    std::string SetInitialCalculatorState();

    // Выполнить команду памяти
    std::string ExecuteMemoryCommand(int command, std::string& MState);

    // Выполнить команду буфера обмена
    std::string ExecuteClipboardCommand(int command, std::string& clipboard);

    // Свойство: состояние калькулятора
    TCtrlState GetState() const { return FState; }
    void SetState(TCtrlState state) { FState = state; }

    // Получить текущую строку редактора
    std::string GetDisplayString() const;

    // Установить строку отображения
    void SetDisplayString(const std::string& str);

    // Получить дробь из текущей строки
    TFrac GetCurrentFraction() const;
    
    // Для отладки - получить состояние процессора
    std::string GetProcessorState() const;
};

#endif // UCONTROL_H
