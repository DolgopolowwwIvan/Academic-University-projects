// UClcPnl.cpp - Реализация класса "Интерфейс калькулятора"
// Original: Реализация ИнтерфейсКалькулятораПростыхДробей

#include "UClcPnl.h"

#pragma package(smart_init)
#pragma resource "*.dfm"

TClcPnl* ClcPnl;  // Глобальная переменная формы

// Конструктор
__fastcall TClcPnl::TClcPnl(TComponent* Owner)
    : TForm(Owner), FNumberMode(false)
{
    FCtrl = new TCtrl();
}

// Деструктор
__fastcall TClcPnl::~TClcPnl()
{
    delete FCtrl;
}

// Создание формы
void __fastcall TClcPnl::FormCreate(TObject* Sender)
{
    FCtrl = new TCtrl();
    FNumberMode = false;  // По умолчанию режим дроби
    
    UpdateDisplay();
    UpdateMemoryStatus();
    
    // Установка всплывающих подсказок
    DisplayLabel->Hint = "Отображение текущей дроби";
    Btn0->Hint = "Ввод цифры 0";
    Btn1->Hint = "Ввод цифры 1";
    Btn2->Hint = "Ввод цифры 2";
    Btn3->Hint = "Ввод цифры 3";
    Btn4->Hint = "Ввод цифры 4";
    Btn5->Hint = "Ввод цифры 5";
    Btn6->Hint = "Ввод цифры 6";
    Btn7->Hint = "Ввод цифры 7";
    Btn8->Hint = "Ввод цифры 8";
    Btn9->Hint = "Ввод цифры 9";
    BtnAdd->Hint = "Сложение (+)";
    BtnSub->Hint = "Вычитание (-)";
    BtnMul->Hint = "Умножение (*)";
    BtnDiv->Hint = "Деление (/)";
    BtnEqual->Hint = "Равно (=) - вычислить выражение";
    BtnSqr->Hint = "Квадрат (Sqr) - возведение в квадрат";
    BtnRev->Hint = "Обратное (Rev) - 1/x";
    BtnSign->Hint = "Сменить знак (+/-)";
    BtnSeparator->Hint = "Разделитель дроби (/)";
    BtnBackspace->Hint = "Удалить символ (Backspace)";
    BtnCE->Hint = "Очистить текущий ввод (CE)";
    BtnC->Hint = "Сбросить калькулятор (C)";
    BtnMC->Hint = "Очистить память (MC)";
    BtnMS->Hint = "Сохранить в память (MS)";
    BtnMR->Hint = "Восстановить из памяти (MR)";
    BtnMPlus->Hint = "Добавить к памяти (M+)";
}

// Обработка нажатия клавиши
void __fastcall TClcPnl::FormKeyPress(TObject* Sender, char& Key)
{
    int command = -1;
    
    // Цифры
    if (Key >= '0' && Key <= '9') {
        command = cmdDigit0 + (Key - '0');
    }
    // Знак
    else if (Key == '+' || Key == '-') {
        if (Key == '+') command = cmdAdd;
        else if (Key == '-') command = cmdSub;
    }
    // Операции
    else if (Key == '*') command = cmdMul;
    else if (Key == '/') {
        Key = '/';  // Разрешаем ввод /
        command = cmdDiv;
    }
    // Равно
    else if (Key == '=' || Key == 13) {  // 13 = Enter
        command = cmdEqual;
    }
    // Разделитель дроби
    else if (Key == '/' || Key == '|') {
        command = cmdSeparator;
    }
    // Backspace
    else if (Key == 8) {  // 8 = Backspace
        command = cmdBackspace;
    }
    // Escape - сброс
    else if (Key == 27) {  // 27 = Escape
        command = cmdClear;
    }
    // S - Sqr
    else if (Key == 's' || Key == 'S') {
        command = cmdSqr;
    }
    // R - Rev
    else if (Key == 'r' || Key == 'R') {
        command = cmdRev;
    }
    
    if (command >= 0) {
        std::string clipboard = "";
        std::string mstate = "";
        FCtrl->ExecuteCommand(command, clipboard, mstate);
        UpdateDisplay();
        UpdateMemoryStatus();
    }
}

// Обработка нажатия кнопки
void __fastcall TClcPnl::ButtonClick(TObject* Sender)
{
    if (Sender->ClassName() != "TBitButton") return;
    
    TBitButton* button = dynamic_cast<TBitButton*>(Sender);
    if (!button) return;
    
    int command = GetCommandFromButton(button);
    if (command < 0) return;
    
    std::string clipboard = "";
    std::string mstate = "";
    
    // Обработка команд буфера обмена
    if (command == cmdCopy) {
        Clipboard()->AsText = WideString(FCtrl->GetDisplayString().c_str());
        return;
    }
    else if (command == cmdPaste) {
        WideString ws = Clipboard()->AsText;
        std::string text(ws.c_str());
        FCtrl->ExecuteCommand(cmdPaste, text, mstate);
        UpdateDisplay();
        return;
    }
    
    FCtrl->ExecuteCommand(command, clipboard, mstate);
    UpdateDisplay();
    UpdateMemoryStatus();
}

// Преобразование кнопки в команду
int TClcPnl::GetCommandFromButton(TBitButton* button)
{
    if (button == Btn0) return cmdDigit0;
    if (button == Btn1) return cmdDigit1;
    if (button == Btn2) return cmdDigit2;
    if (button == Btn3) return cmdDigit3;
    if (button == Btn4) return cmdDigit4;
    if (button == Btn5) return cmdDigit5;
    if (button == Btn6) return cmdDigit6;
    if (button == Btn7) return cmdDigit7;
    if (button == Btn8) return cmdDigit8;
    if (button == Btn9) return cmdDigit9;
    if (button == BtnAdd) return cmdAdd;
    if (button == BtnSub) return cmdSub;
    if (button == BtnMul) return cmdMul;
    if (button == BtnDiv) return cmdDiv;
    if (button == BtnEqual) return cmdEqual;
    if (button == BtnSqr) return cmdSqr;
    if (button == BtnRev) return cmdRev;
    if (button == BtnSign) return cmdSign;
    if (button == BtnSeparator) return cmdSeparator;
    if (button == BtnBackspace) return cmdBackspace;
    if (button == BtnCE) return cmdClearEntry;
    if (button == BtnC) return cmdClear;
    if (button == BtnMC) return cmdMC;
    if (button == BtnMS) return cmdMS;
    if (button == BtnMR) return cmdMR;
    if (button == BtnMPlus) return cmdMPlus;
    
    return -1;
}

// Обновление отображения
void TClcPnl::UpdateDisplay()
{
    std::string display = FCtrl->GetDisplayString();
    DisplayLabel->Caption = WideString(display.c_str());
}

// Обновление состояния памяти
void TClcPnl::UpdateMemoryStatus()
{
    // Состояние памяти отображается в MemoryStatusLabel
    // M - память включена, пусто - выключена
    if (FCtrl->GetState() != cStart) {
        // Проверяем состояние памяти через контроллер
        // Для упрощения - показываем M если память была использована
        MemoryStatusLabel->Caption = WideString("M");
    } else {
        MemoryStatusLabel->Caption = WideString(" ");
    }
}

// Форматирование результата
std::string TClcPnl::FormatResult(const TFrac& frac)
{
    if (FNumberMode && frac.GetDenominator() == 1) {
        // Режим числа, знаменатель = 1 - показываем как целое
        return std::to_string(frac.GetNumerator());
    }
    return frac.ToString();
}

// Копировать
void __fastcall TClcPnl::CopyClick(TObject* Sender)
{
    Clipboard()->AsText = WideString(FCtrl->GetDisplayString().c_str());
}

// Вставить
void __fastcall TClcPnl::PasteClick(TObject* Sender)
{
    WideString ws = Clipboard()->AsText;
    std::string text(ws.c_str());
    std::string mstate = "";
    FCtrl->ExecuteCommand(cmdPaste, text, mstate);
    UpdateDisplay();
}

// Режим дроби
void __fastcall TClcPnl::FractionModeClick(TObject* Sender)
{
    FNumberMode = false;
    FractionModeItem->Checked = true;
    NumberModeItem->Checked = false;
    UpdateDisplay();
}

// Режим числа
void __fastcall TClcPnl::NumberModeClick(TObject* Sender)
{
    FNumberMode = true;
    FractionModeItem->Checked = false;
    NumberModeItem->Checked = true;
    UpdateDisplay();
}

// О программе
void __fastcall TClcPnl::AboutClick(TObject* Sender)
{
    ShowMessage(L"Калькулятор простых дробей\nВерсия 1.0\n\nРазработано в рамках лабораторной работы\nпо объектно-ориентированному программированию C++");
}

