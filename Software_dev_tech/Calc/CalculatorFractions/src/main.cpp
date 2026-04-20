// main.cpp - Главный файл проекта
// Калькулятор простых дробей
// Для компиляции в C++ Builder используйте проект .cbproj

#include <iostream>
#include <string>
#include "UFrac.h"
#include "UEditor.h"
#include "UMemory.h"
#include "UProc.h"
#include "UControl.h"

// Тестирование TFrac
void TestTFrac() {
    std::cout << "=== Тестирование TFrac ===" << std::endl;
    
    TFrac f1(2, 3);
    TFrac f2(3, 4);
    
    std::cout << "f1 = " << f1.ToString() << std::endl;
    std::cout << "f2 = " << f2.ToString() << std::endl;
    
    TFrac sum = f1.Add(f2);
    std::cout << "f1 + f2 = " << sum.ToString() << std::endl;
    
    TFrac diff = f1.Sub(f2);
    std::cout << "f1 - f2 = " << diff.ToString() << std::endl;
    
    TFrac prod = f1.Mul(f2);
    std::cout << "f1 * f2 = " << prod.ToString() << std::endl;
    
    TFrac quot = f1.Div(f2);
    std::cout << "f1 / f2 = " << quot.ToString() << std::endl;
    
    TFrac sqr = f1.Sqr();
    std::cout << "f1^2 = " << sqr.ToString() << std::endl;
    
    TFrac rev = f1.Rev();
    std::cout << "1/f1 = " << rev.ToString() << std::endl;
    
    std::cout << std::endl;
}

// Тестирование TEditor
void TestTEditor() {
    std::cout << "=== Тестирование TEditor ===" << std::endl;
    
    TEditor editor;
    std::cout << "Начало: " << editor.GetString() << std::endl;
    
    editor.Edit(ecDigit2);
    std::cout << "После '2': " << editor.GetString() << std::endl;
    
    editor.Edit(ecSeparator);
    std::cout << "После '/': " << editor.GetString() << std::endl;
    
    editor.Edit(ecDigit3);
    std::cout << "После '3': " << editor.GetString() << std::endl;
    
    TFrac frac = editor.ToFraction();
    std::cout << "Как дробь: " << frac.ToString() << std::endl;
    
    editor.Edit(ecClear);
    std::cout << "После Clear: " << editor.GetString() << std::endl;
    
    std::cout << std::endl;
}

// Тестирование TMemory
void TestTMemory() {
    std::cout << "=== Тестирование TMemory ===" << std::endl;
    
    TMemory<TFrac> memory;
    std::cout << "Состояние памяти: " << memory.ReadState() << std::endl;
    
    TFrac f(5, 2);
    memory.Store(f);
    std::cout << "После Store(5/2): " << memory.ReadState() << std::endl;
    std::cout << "Значение: " << memory.ReadNumber().ToString() << std::endl;
    
    TFrac f2(3, 2);
    memory.Add(f2);
    std::cout << "После Add(3/2): " << memory.ReadNumber().ToString() << std::endl;
    
    memory.Clear();
    std::cout << "После Clear: " << memory.ReadState() << std::endl;
    
    std::cout << std::endl;
}

// Тестирование TProc
void TestTProc() {
    std::cout << "=== Тестирование TProc ===" << std::endl;
    
    TProc<TFrac> proc;
    std::cout << "Начальное состояние" << std::endl;
    
    proc.SetLop_Res(TFrac(2, 1));
    proc.SetOperation(opAdd);
    std::cout << "Установлен левый операнд 2/1 и операция +" << std::endl;
    
    proc.SetRop(TFrac(3, 1));
    std::cout << "Установлен правый операнд 3/1" << std::endl;
    
    proc.OprtnRun();
    std::cout << "После выполнения: " << proc.GetLop_Res().ToString() << std::endl;
    
    std::cout << std::endl;
}

// Тестирование TCtrl
void TestTCtrl() {
    std::cout << "=== Тестирование TCtrl ===" << std::endl;
    
    TCtrl ctrl;
    std::string clipboard = "";
    std::string mstate = "";
    
    // Тест: 2/1 + 3/1 = 5/1
    std::cout << "Тест: 2/1 + 3/1 =" << std::endl;
    
    // Ввод 2/1
    ctrl.ExecuteCommand(cmdDigit2, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "Введено: " << ctrl.GetDisplayString() << std::endl;
    
    // Операция +
    ctrl.ExecuteCommand(cmdAdd, clipboard, mstate);
    std::cout << "После +: " << ctrl.GetDisplayString() << std::endl;
    
    // Ввод 3/1
    ctrl.ExecuteCommand(cmdDigit3, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "Введено: " << ctrl.GetDisplayString() << std::endl;
    
    // Равно
    ctrl.ExecuteCommand(cmdEqual, clipboard, mstate);
    std::cout << "Результат: " << ctrl.GetDisplayString() << std::endl;
    
    std::cout << std::endl;
    
    // Тест: Sqr
    std::cout << "Тест: 5/1 Sqr" << std::endl;
    ctrl.ExecuteCommand(cmdClear, clipboard, mstate);
    
    ctrl.ExecuteCommand(cmdDigit5, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "Введено: " << ctrl.GetDisplayString() << std::endl;
    
    ctrl.ExecuteCommand(cmdSqr, clipboard, mstate);
    std::cout << "После Sqr: " << ctrl.GetDisplayString() << std::endl;
    
    std::cout << std::endl;
}

// Пример вычисления выражения из задания
void TestExpression() {
    std::cout << "=== Тест выражения: 6/1 Sqr + 2/1 Sqr / 10/1 + 6/1 ===" << std::endl;
    
    TCtrl ctrl;
    std::string clipboard = "";
    std::string mstate = "";
    
    // 6/1
    ctrl.ExecuteCommand(cmdDigit6, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "6/1: " << ctrl.GetDisplayString() << std::endl;
    
    // Sqr
    ctrl.ExecuteCommand(cmdSqr, clipboard, mstate);
    std::cout << "Sqr: " << ctrl.GetDisplayString() << std::endl;
    
    // +
    ctrl.ExecuteCommand(cmdAdd, clipboard, mstate);
    std::cout << "+: " << ctrl.GetDisplayString() << std::endl;
    
    // 2/1
    ctrl.ExecuteCommand(cmdDigit2, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "2/1: " << ctrl.GetDisplayString() << std::endl;
    
    // Sqr
    ctrl.ExecuteCommand(cmdSqr, clipboard, mstate);
    std::cout << "Sqr: " << ctrl.GetDisplayString() << std::endl;
    
    // /
    ctrl.ExecuteCommand(cmdDiv, clipboard, mstate);
    std::cout << "/: " << ctrl.GetDisplayString() << std::endl;
    
    // 10/1
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit0, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "10/1: " << ctrl.GetDisplayString() << std::endl;
    
    // +
    ctrl.ExecuteCommand(cmdAdd, clipboard, mstate);
    std::cout << "+: " << ctrl.GetDisplayString() << std::endl;
    
    // 6/1
    ctrl.ExecuteCommand(cmdDigit6, clipboard, mstate);
    ctrl.ExecuteCommand(cmdSeparator, clipboard, mstate);
    ctrl.ExecuteCommand(cmdDigit1, clipboard, mstate);
    std::cout << "6/1: " << ctrl.GetDisplayString() << std::endl;
    
    // =
    ctrl.ExecuteCommand(cmdEqual, clipboard, mstate);
    std::cout << "= Результат: " << ctrl.GetDisplayString() << std::endl;
    
    std::cout << std::endl;
}

int main() {
    std::cout << "========================================" << std::endl;
    std::cout << "Калькулятор простых дробей" << std::endl;
    std::cout << "Тестирование логики (Linux консоль)" << std::endl;
    std::cout << "========================================" << std::endl;
    std::cout << std::endl;
    
    TestTFrac();
    TestTEditor();
    TestTMemory();
    TestTProc();
    TestTCtrl();
    TestExpression();
    
    std::cout << "========================================" << std::endl;
    std::cout << "Все тесты завершены!" << std::endl;
    std::cout << "Для GUI версии компилируйте в C++ Builder" << std::endl;
    std::cout << "========================================" << std::endl;
    
    return 0;
}
