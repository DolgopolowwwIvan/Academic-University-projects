// UClcPnl.h - Класс "Интерфейс калькулятора"
// Original: ИнтерфейсКалькулятораПростыхДробей
// Наследник TForm для C++ Builder VCL

#ifndef UCLCPNL_H
#define UCLCPNL_H

// Для C++ Builder VCL
#ifdef __BORLANDC__
    #include <System.Classes.hpp>
    #include <System.SysUtils.hpp>
    #include <Vcl.Controls.hpp>
    #include <Vcl.StdCtrls.hpp>
    #include <Vcl.Buttons.hpp>
    #include <Vcl.ExtCtrls.hpp>
    #include <Vcl.Menus.hpp>
    #include <Vcl.Dialogs.hpp>
    #include <Vcl.ComCtrls.hpp>
    #include <Clipbrd.hpp>
#else
    // Заглушки для компиляции на Linux (не для GUI)
    #include <string>
#endif

#include "UControl.h"

// Класс формы калькулятора
// Наследуется от TForm в среде C++ Builder
#ifdef __BORLANDC__
class TClcPnl : public TForm
#else
class TClcPnl  // Заглушка для Linux
#endif
{
#ifdef __BORLANDC__
__published:
    // Визуальные компоненты
    TStaticText* DisplayLabel;      // Отображение текущей дроби
    TStaticText* MemoryStatusLabel; // Состояние памяти

    // Кнопки ввода цифр
    TBitButton* Btn0;
    TBitButton* Btn1;
    TBitButton* Btn2;
    TBitButton* Btn3;
    TBitButton* Btn4;
    TBitButton* Btn5;
    TBitButton* Btn6;
    TBitButton* Btn7;
    TBitButton* Btn8;
    TBitButton* Btn9;

    // Кнопки операций
    TBitButton* BtnAdd;
    TBitButton* BtnSub;
    TBitButton* BtnMul;
    TBitButton* BtnDiv;
    TBitButton* BtnEqual;

    // Кнопки функций
    TBitButton* BtnSqr;
    TBitButton* BtnRev;

    // Кнопки редактирования
    TBitButton* BtnSign;
    TBitButton* BtnSeparator;
    TBitButton* BtnBackspace;
    TBitButton* BtnCE;
    TBitButton* BtnC;

    // Кнопки памяти
    TBitButton* BtnMC;
    TBitButton* BtnMS;
    TBitButton* BtnMR;
    TBitButton* BtnMPlus;

    // Главное меню
    TMainMenu* MainMenu;
    TMenuItem* EditMenu;
    TMenuItem* CopyItem;
    TMenuItem* PasteItem;
    TMenuItem* ViewMenu;
    TMenuItem* FractionModeItem;
    TMenuItem* NumberModeItem;
    TMenuItem* HelpMenu;
    TMenuItem* AboutItem;

    // Обработчики событий
    void __fastcall FormCreate(TObject* Sender);
    void __fastcall FormKeyPress(TObject* Sender, char& Key);
    void __fastcall ButtonClick(TObject* Sender);

    // Команды меню
    void __fastcall CopyClick(TObject* Sender);
    void __fastcall PasteClick(TObject* Sender);
    void __fastcall FractionModeClick(TObject* Sender);
    void __fastcall NumberModeClick(TObject* Sender);
    void __fastcall AboutClick(TObject* Sender);

private:
    TCtrl* FCtrl;           // Контроллер калькулятора
    bool FNumberMode;       // Режим отображения: число или дробь

    // Преобразование нажатия кнопки в команду
    int GetCommandFromButton(TBitButton* button);

    // Обновление отображения
    void UpdateDisplay();
    void UpdateMemoryStatus();

    // Отображение результата в выбранном формате
    std::string FormatResult(const TFrac& frac);

public:
    // Конструктор
    __fastcall TClcPnl(TComponent* Owner);

    // Деструктор
    virtual __fastcall ~TClcPnl();
};

// Глобальная переменная формы (стандарт для C++ Builder)
extern PACKAGE TClcPnl* ClcPnl;

#else
// Заглушка для Linux
class TClcPnl {
private:
    TCtrl* FCtrl;
    bool FNumberMode;

public:
    TClcPnl() : FCtrl(new TCtrl()), FNumberMode(false) {}
    ~TClcPnl() { delete FCtrl; }

    // Методы-заглушки для тестирования логики
    std::string ExecuteCommand(int cmd) {
        std::string clipboard = "";
        std::string mstate = "";
        return FCtrl->ExecuteCommand(cmd, clipboard, mstate);
    }

    std::string GetDisplay() {
        return FCtrl->GetDisplayString();
    }
};
#endif

#endif // UCLCPNL_H
