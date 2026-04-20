// MainForm.cs - Форма калькулятора простых дробей
// Visual Studio Windows Forms C#

using System;
using System.Windows.Forms;

namespace CalculatorFractions
{
    public partial class MainForm : Form
    {
        private TCtrl FCtrl;
        private string FClipboard = "";

        public MainForm()
        {
            InitializeComponent();
            FCtrl = new TCtrl();
            // По умолчанию режим "число" (скрывать /1)
            FCtrl.ShowAsFraction = false;
            numberModeItem.Checked = true;
            fractionModeItem.Checked = false;
            UpdateDisplay();
            SetupToolTips();
        }

        private void SetupToolTips()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(DisplayLabel, "Отображение текущей дроби");
            toolTip.SetToolTip(Btn0, "Ввод цифры 0");
            toolTip.SetToolTip(Btn1, "Ввод цифры 1");
            toolTip.SetToolTip(Btn2, "Ввод цифры 2");
            toolTip.SetToolTip(Btn3, "Ввод цифры 3");
            toolTip.SetToolTip(Btn4, "Ввод цифры 4");
            toolTip.SetToolTip(Btn5, "Ввод цифры 5");
            toolTip.SetToolTip(Btn6, "Ввод цифры 6");
            toolTip.SetToolTip(Btn7, "Ввод цифры 7");
            toolTip.SetToolTip(Btn8, "Ввод цифры 8");
            toolTip.SetToolTip(Btn9, "Ввод цифры 9");
            toolTip.SetToolTip(BtnAdd, "Сложение (+)");
            toolTip.SetToolTip(BtnSub, "Вычитание (-)");
            toolTip.SetToolTip(BtnMul, "Умножение (*)");
            toolTip.SetToolTip(BtnDiv, "Деление (/)");
            toolTip.SetToolTip(BtnEqual, "Равно (=) - вычислить выражение");
            toolTip.SetToolTip(BtnSqr, "Квадрат (Sqr) - возведение в квадрат");
            toolTip.SetToolTip(BtnRev, "Обратное (Rev) - 1/x");
            toolTip.SetToolTip(BtnSign, "Сменить знак (+/-)");
            toolTip.SetToolTip(BtnSeparator, "Разделитель дроби (/)");
            toolTip.SetToolTip(BtnBackspace, "Удалить символ (Backspace)");
            toolTip.SetToolTip(BtnCE, "Очистить текущий ввод (CE)");
            toolTip.SetToolTip(BtnC, "Сбросить калькулятор (C)");
            toolTip.SetToolTip(BtnMC, "Очистить память (MC)");
            toolTip.SetToolTip(BtnMS, "Сохранить в память (MS)");
            toolTip.SetToolTip(BtnMR, "Восстановить из памяти (MR)");
            toolTip.SetToolTip(BtnMPlus, "Добавить к памяти (M+)");
        }

        private void UpdateDisplay()
        {
            DisplayLabel.Text = FCtrl.GetDisplayString();
            MemoryStatusLabel.Text = "   ";
        }

        private int GetCommandFromButton(Button button)
        {
            if (button == Btn0) return (int)TCalcCommand.cmdDigit0;
            if (button == Btn1) return (int)TCalcCommand.cmdDigit1;
            if (button == Btn2) return (int)TCalcCommand.cmdDigit2;
            if (button == Btn3) return (int)TCalcCommand.cmdDigit3;
            if (button == Btn4) return (int)TCalcCommand.cmdDigit4;
            if (button == Btn5) return (int)TCalcCommand.cmdDigit5;
            if (button == Btn6) return (int)TCalcCommand.cmdDigit6;
            if (button == Btn7) return (int)TCalcCommand.cmdDigit7;
            if (button == Btn8) return (int)TCalcCommand.cmdDigit8;
            if (button == Btn9) return (int)TCalcCommand.cmdDigit9;
            if (button == BtnAdd) return (int)TCalcCommand.cmdAdd;
            if (button == BtnSub) return (int)TCalcCommand.cmdSub;
            if (button == BtnMul) return (int)TCalcCommand.cmdMul;
            if (button == BtnDiv) return (int)TCalcCommand.cmdDiv;
            if (button == BtnEqual) return (int)TCalcCommand.cmdEqual;
            if (button == BtnSqr) return (int)TCalcCommand.cmdSqr;
            if (button == BtnRev) return (int)TCalcCommand.cmdRev;
            if (button == BtnSign) return (int)TCalcCommand.cmdSign;
            if (button == BtnSeparator) return (int)TCalcCommand.cmdSeparator;
            if (button == BtnBackspace) return (int)TCalcCommand.cmdBackspace;
            if (button == BtnCE) return (int)TCalcCommand.cmdClearEntry;
            if (button == BtnC) return (int)TCalcCommand.cmdClear;
            if (button == BtnMC) return (int)TCalcCommand.cmdMC;
            if (button == BtnMS) return (int)TCalcCommand.cmdMS;
            if (button == BtnMR) return (int)TCalcCommand.cmdMR;
            if (button == BtnMPlus) return (int)TCalcCommand.cmdMPlus;

            return -1;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                int command = GetCommandFromButton(button);
                if (command < 0) return;

                string mstate = "";
                
                if (command == (int)TCalcCommand.cmdCopy)
                {
                    Clipboard.SetText(FCtrl.GetDisplayString());
                    return;
                }
                else if (command == (int)TCalcCommand.cmdPaste)
                {
                    string text = Clipboard.GetText();
                    if (!string.IsNullOrEmpty(text))
                    {
                        FCtrl.ExecuteCommand((int)TCalcCommand.cmdPaste, ref text, ref mstate);
                        UpdateDisplay();
                    }
                    return;
                }

                FCtrl.ExecuteCommand(command, ref FClipboard, ref mstate);
                UpdateDisplay();
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            int command = -1;
            char key = e.KeyChar;

            if (key >= '0' && key <= '9')
            {
                command = (int)TCalcCommand.cmdDigit0 + (key - '0');
            }
            else if (key == '+')
            {
                command = (int)TCalcCommand.cmdAdd;
            }
            else if (key == '-')
            {
                command = (int)TCalcCommand.cmdSub;
            }
            else if (key == '*')
            {
                command = (int)TCalcCommand.cmdMul;
            }
            else if (key == '/')
            {
                command = (int)TCalcCommand.cmdDiv;
            }
            else if (key == '=' || key == 13)
            {
                command = (int)TCalcCommand.cmdEqual;
            }
            else if (key == '|' || key == '\\')
            {
                command = (int)TCalcCommand.cmdSeparator;
            }
            else if (key == 8)
            {
                command = (int)TCalcCommand.cmdBackspace;
            }
            else if (key == 27)
            {
                command = (int)TCalcCommand.cmdClear;
            }
            else if (key == 's' || key == 'S')
            {
                command = (int)TCalcCommand.cmdSqr;
            }
            else if (key == 'r' || key == 'R')
            {
                command = (int)TCalcCommand.cmdRev;
            }

            if (command >= 0)
            {
                string mstate = "";
                FCtrl.ExecuteCommand(command, ref FClipboard, ref mstate);
                UpdateDisplay();
            }
        }

        private void CopyClick(object sender, EventArgs e)
        {
            Clipboard.SetText(FCtrl.GetDisplayString());
        }

        private void PasteClick(object sender, EventArgs e)
        {
            string text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(text))
            {
                string mstate = "";
                FCtrl.ExecuteCommand((int)TCalcCommand.cmdPaste, ref text, ref mstate);
                UpdateDisplay();
            }
        }

        private void FractionModeClick(object sender, EventArgs e)
        {
            FCtrl.ShowAsFraction = true;
            fractionModeItem.Checked = true;
            numberModeItem.Checked = false;
            UpdateDisplay();
        }

        private void NumberModeClick(object sender, EventArgs e)
        {
            FCtrl.ShowAsFraction = false;
            fractionModeItem.Checked = false;
            numberModeItem.Checked = true;
            UpdateDisplay();
        }

        private void AboutClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Калькулятор простых дробей\nВерсия 1.0\n\n" +
                "Разработано в рамках лабораторной работы\n" +
                "по объектно-ориентированному программированию C#",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
