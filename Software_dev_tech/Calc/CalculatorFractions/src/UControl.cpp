// UControl.cpp - Реализация класса "Управление калькулятором"
// Original: Реализация УправлениеКалькуляторомПростыхДробей

#include "UControl.h"
#include <sstream>

// Конструктор
TCtrl::TCtrl() : FState(cStart), FClipboard("") {
    SetInitialState();
}

// Деструктор
TCtrl::~TCtrl() {}

// Установка начального состояния
void TCtrl::SetInitialState() {
    FState = cStart;
    FPrevState = cStart;
    FEditor.Clear();
    FProcessor.Reset();
    FMemory.Clear();
    FNumber = TFrac(0, 1);
    FDisplayString = "0/1";
}

// Преобразование дроби в строку
std::string TCtrl::FractionToString(const TFrac& frac) {
    return frac.ToString();
}

// Преобразование команды в операцию
TOprtn TCtrl::CommandToOperation(int cmd) {
    switch (cmd) {
        case cmdAdd: return opAdd;
        case cmdSub: return opSub;
        case cmdMul: return opMul;
        case cmdDiv: return opDiv;
        default: return opNone;
    }
}

// Получить текущую строку отображения
std::string TCtrl::GetDisplayString() const {
    return FDisplayString;
}

// Установить строку отображения
void TCtrl::SetDisplayString(const std::string& str) {
    FDisplayString = str;
}

// Для отладки - получить состояние процессора
std::string TCtrl::GetProcessorState() const {
    return "Lop_Res=" + FProcessor.GetLop_Res().ToString() + 
           " Rop=" + FProcessor.GetRop().ToString() +
           " Op=" + OprtnToString(FProcessor.GetOperation());
}

// Получить текущую дробь
TFrac TCtrl::GetCurrentFraction() const {
    return FEditor.ToFraction();
}

// Выполнить команду калькулятора
std::string TCtrl::ExecuteCommand(int command, std::string& clipboard, std::string& MState) {
    std::string result;

    // Сначала обрабатываем команды буфера обмена
    if (command == cmdCopy || command == cmdPaste) {
        result = ExecuteClipboardCommand(command, clipboard);
        MState = FMemory.ReadState();
        return result;
    }

    // Команды памяти
    if (command >= cmdMC && command <= cmdMPlus) {
        result = ExecuteMemoryCommand(command, MState);
        return result;
    }

    // Команда сброса C
    if (command == cmdClear) {
        result = SetInitialCalculatorState();
        MState = FMemory.ReadState();
        return result;
    }

    // Команда CE (очистить текущий ввод)
    if (command == cmdClearEntry) {
        result = FEditor.Clear();
        if (FState == cValDone || FState == cExpDone) {
            FProcessor.SetRop(TFrac(0, 1));
        }
        FState = cEditing;
        MState = FMemory.ReadState();
        return result;
    }

    // Backspace
    if (command == cmdBackspace) {
        result = FEditor.Backspace();
        MState = FMemory.ReadState();
        return result;
    }

    // Ввод цифр и разделителя
    if (command >= cmdDigit0 && command <= cmdSeparator) {
        // Сохраняем предыдущее состояние для контекста только при переходе в редактирование
        if (FState == cValDone || FState == cOpChange) {
            FPrevState = FState;
            FState = cEditing;
            FEditor.Clear();
            FDisplayString = FEditor.GetString();
        } else if (FState == cStart || FState == cExpDone || FState == cFunDone) {
            FPrevState = FState;
            FState = cEditing;
            FEditor.Clear();
            FDisplayString = FEditor.GetString();
        }
        
        result = ExecuteEditorCommand(command);
        FDisplayString = result;
        MState = FMemory.ReadState();
        return result;
    }

    // Операции (+, -, *, /)
    if (command >= cmdAdd && command <= cmdDiv) {
        result = ExecuteOperation(command);
        FDisplayString = result;
        MState = FMemory.ReadState();
        return result;
    }

    // Функции (Sqr, Rev)
    if (command == cmdSqr || command == cmdRev) {
        result = ExecuteFunction(command);
        FDisplayString = result;
        MState = FMemory.ReadState();
        return result;
    }

    // Равно (=)
    if (command == cmdEqual) {
        result = CalculateExpression();
        FDisplayString = result;
        MState = FMemory.ReadState();
        return result;
    }

    MState = FMemory.ReadState();
    return FEditor.GetString();
}

// Выполнить команду редактора
std::string TCtrl::ExecuteEditorCommand(int command) {
    std::string result = FEditor.Edit(command);
    FDisplayString = result;
    FState = cEditing;
    return result;
}

// Выполнить операцию
std::string TCtrl::ExecuteOperation(int command) {
    TOprtn newOp = CommandToOperation(command);
    if (newOp == opNone) return FEditor.GetString();

    TFrac currentFrac = FEditor.ToFraction();

    switch (FState) {
        case cStart:
        case cEditing:
            // Проверяем предыдущее состояние - если было cValDone/cOpChange, выполняем операцию
            if (FPrevState == cValDone || FPrevState == cOpChange) {
                // Есть левый операнд и операция, currentFrac - правый операнд
                if (FProcessor.IsOperationSet()) {
                    FProcessor.SetRop(currentFrac);
                    FProcessor.OprtnRun();
                }
                // Устанавливаем новую операцию
                FProcessor.SetOperation(newOp);
                FState = cValDone;
            } else {
                // Первый операнд введён
                FProcessor.SetLop_Res(currentFrac);
                FProcessor.SetOperation(newOp);
                FState = cValDone;
            }
            break;

        case cValDone:
            // Меняем операцию без выполнения
            FProcessor.SetOperation(newOp);
            FState = cOpChange;
            break;

        case cOpChange:
            // Меняем операцию без выполнения
            FProcessor.SetOperation(newOp);
            FState = cOpChange;
            break;

        case cFunDone:
            // Функция выполнена, FNumber содержит результат функции
            // Если есть предыдущая операция - выполняем её с FNumber как правым операндом
            if (FProcessor.IsOperationSet()) {
                FProcessor.SetRop(FNumber);
                FProcessor.OprtnRun();
            } else {
                FProcessor.SetLop_Res(FNumber);
            }
            FProcessor.SetOperation(newOp);
            FState = cValDone;
            break;

        case cExpDone:
            // Выражение вычислено, начинаем новое с результатом
            FProcessor.SetLop_Res(FNumber);
            FProcessor.SetOperation(newOp);
            FState = cValDone;
            break;

        default:
            break;
    }

    // Очищаем редактор для ввода следующего числа
    FEditor.Clear();
    // Устанавливаем строку отображения - левый операнд + операция
    std::string displayStr = FProcessor.GetLop_Res().ToString() + " " + OprtnToString(newOp);
    FDisplayString = displayStr;
    
    return displayStr;
}

// Выполнить функцию
std::string TCtrl::ExecuteFunction(int command) {
    TFrac currentFrac = FEditor.ToFraction();
    TFunc func = (command == cmdSqr) ? fnSqr : fnRev;

    try {
        FProcessor.SetRop(currentFrac);
        FProcessor.FuncRun(func);
        FNumber = FProcessor.GetRop();
        FEditor.SetString(FNumber.ToString());
        FDisplayString = FNumber.ToString();
        FState = cFunDone;
        return FNumber.ToString();
    } catch (...) {
        FState = cError;
        FDisplayString = "Error";
        return "Error";
    }
}

// Вычислить выражение
std::string TCtrl::CalculateExpression() {
    TFrac currentFrac = FEditor.ToFraction();

    switch (FState) {
        case cValDone:
        case cOpChange:
            // Есть левый операнд и операция, применяем к текущему значению
            if (FProcessor.IsOperationSet()) {
                FProcessor.SetRop(currentFrac);
                FProcessor.OprtnRun();
                FNumber = FProcessor.GetLop_Res();
            } else {
                FNumber = currentFrac;
            }
            FEditor.SetString(FNumber.ToString());
            FDisplayString = FNumber.ToString();
            FState = cExpDone;
            break;

        case cEditing:
            // Ввод числа после операции - выполняем операцию с currentFrac
            if (FPrevState == cValDone || FPrevState == cOpChange) {
                if (FProcessor.IsOperationSet()) {
                    FProcessor.SetRop(currentFrac);
                    FProcessor.OprtnRun();
                    FNumber = FProcessor.GetLop_Res();
                } else {
                    FNumber = currentFrac;
                }
            } else if (FPrevState == cExpDone || FPrevState == cFunDone) {
                // После результата - начинаем новое вычисление
                FNumber = currentFrac;
            } else {
                FNumber = currentFrac;
            }
            FEditor.SetString(FNumber.ToString());
            FDisplayString = FNumber.ToString();
            FState = cExpDone;
            break;

        case cExpDone:
            // Повторное выполнение - используем последний результат и операцию
            if (FProcessor.IsOperationSet()) {
                FProcessor.SetRop(FNumber);  // Используем предыдущий результат как правый операнд
                FProcessor.OprtnRun();
                FNumber = FProcessor.GetLop_Res();
                FEditor.SetString(FNumber.ToString());
                FDisplayString = FNumber.ToString();
            }
            break;

        case cFunDone:
            // Функция выполнена, применяем операцию если есть
            if (FProcessor.IsOperationSet()) {
                FProcessor.SetRop(FNumber);
                FProcessor.OprtnRun();
                FNumber = FProcessor.GetLop_Res();
            }
            FEditor.SetString(FNumber.ToString());
            FDisplayString = FNumber.ToString();
            FState = cExpDone;
            break;

        default:
            break;
    }

    return FNumber.ToString();
}

// Установить начальное состояние калькулятора
std::string TCtrl::SetInitialCalculatorState() {
    SetInitialState();
    FDisplayString = "0/1";
    return "0/1";
}

// Выполнить команду памяти
std::string TCtrl::ExecuteMemoryCommand(int command, std::string& MState) {
    TFrac currentFrac = FEditor.ToFraction();

    switch (command) {
        case cmdMC:  // Memory Clear
            FMemory.Clear();
            break;

        case cmdMS:  // Memory Store
            FMemory.Store(currentFrac);
            break;

        case cmdMR:  // Memory Recall
            if (FMemory.IsOn()) {
                TFrac memVal = FMemory.Recall();
                FEditor.SetString(memVal.ToString());
                FDisplayString = memVal.ToString();
                FState = cEditing;
            }
            break;

        case cmdMPlus:  // Memory Plus
            FMemory.Add(currentFrac);
            break;

        default:
            break;
    }

    MState = FMemory.ReadState();
    FDisplayString = FEditor.GetString();
    return FEditor.GetString();
}

// Выполнить команду буфера обмена
std::string TCtrl::ExecuteClipboardCommand(int command, std::string& clipboard) {
    switch (command) {
        case cmdCopy:
            clipboard = FEditor.GetString();
            break;

        case cmdPaste:
            if (!clipboard.empty()) {
                FEditor.SetString(clipboard);
                FDisplayString = clipboard;
                FState = cEditing;
            }
            break;

        default:
            break;
    }

    return clipboard;
}
