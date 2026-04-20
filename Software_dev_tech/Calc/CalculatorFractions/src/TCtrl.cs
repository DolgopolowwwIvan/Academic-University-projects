// TCtrl.cs - Класс "Управление калькулятором чисел"
// Original: УправлениеКалькуляторомПростыхДробей (тип TCtrl)

using System;

namespace CalculatorFractions
{
    /// <summary>
    /// Состояния калькулятора
    /// Original: TCtrlState = (cStart, cEditing, FunDone, cValDone, cExpDone, cOpChange, cError)
    /// </summary>
    public enum TCtrlState
    {
        cStart,      // Начальное
        cEditing,    // Ввод и редактирование
        cFunDone,    // Функция выполнена
        cValDone,    // Значение введено
        cExpDone,    // Выражение вычислено
        cOpChange,   // Операция изменена
        cError       // Ошибка
    }

    /// <summary>
    /// Команды калькулятора
    /// </summary>
    public enum TCalcCommand
    {
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
    }

    /// <summary>
    /// Класс управления калькулятором
    /// Обязанность: управление выполнением команд калькулятора
    /// </summary>
    public class TCtrl
    {
        private TCtrlState FState;           // Состояние калькулятора
        private TEditor FEditor;             // Редактор
        private TMemory<TFrac> FMemory;      // Память
        private TProc<TFrac> FProcessor;     // Процессор
        private TFrac FNumber;               // Текущее число (результат)
        private string FClipboard;           // Буфер обмена
        private string FDisplayString;       // Строка для отображения
        private TCtrlState FPrevState;       // Предыдущее состояние
        private bool FShowAsFraction;        // Режим отображения: "дробь" или "число"

        /// <summary>
        /// Конструктор
        /// </summary>
        public TCtrl()
        {
            FState = TCtrlState.cStart;
            FClipboard = "";
            FShowAsFraction = false;  // по умолчанию режим "число"
            SetInitialState();
        }

        /// <summary>
        /// Режим отображения: "дробь" (true) или "число" (false)
        /// </summary>
        public bool ShowAsFraction
        {
            get => FShowAsFraction;
            set
            {
                FShowAsFraction = value;
                // Обновляем режим во всех объектах TFrac
                FNumber.ShowAsFraction = value;
                UpdateDisplayString();
            }
        }

        /// <summary>
        /// Установка начального состояния
        /// </summary>
        private void SetInitialState()
        {
            FState = TCtrlState.cStart;
            FPrevState = TCtrlState.cStart;
            FEditor = new TEditor();
            FProcessor = new TProc<TFrac>();
            FMemory = new TMemory<TFrac>();
            FNumber = new TFrac(0, 1);
            FNumber.ShowAsFraction = FShowAsFraction;
            FDisplayString = "0/1";
        }

        /// <summary>
        /// Обновить строку отображения с учётом режима
        /// </summary>
        private void UpdateDisplayString()
        {
            if (FState == TCtrlState.cStart || FState == TCtrlState.cEditing)
            {
                FDisplayString = FEditor.GetString();
            }
            else
            {
                FDisplayString = FNumber.ToString();
            }
        }

        /// <summary>
        /// Преобразование команды в операцию
        /// </summary>
        private TOprtn CommandToOperation(int cmd)
        {
            switch (cmd)
            {
                case (int)TCalcCommand.cmdAdd: return TOprtn.opAdd;
                case (int)TCalcCommand.cmdSub: return TOprtn.opSub;
                case (int)TCalcCommand.cmdMul: return TOprtn.opMul;
                case (int)TCalcCommand.cmdDiv: return TOprtn.opDiv;
                default: return TOprtn.opNone;
            }
        }

        /// <summary>
        /// Получить текущую строку отображения
        /// </summary>
        public string GetDisplayString()
        {
            UpdateDisplayString();
            return FDisplayString;
        }

        /// <summary>
        /// Получить текущее состояние
        /// </summary>
        public TCtrlState GetState() => FState;

        /// <summary>
        /// Выполнить команду калькулятора
        /// </summary>
        public string ExecuteCommand(int command, ref string clipboard, ref string MState)
        {
            string result;

            // Команды буфера обмена
            if (command == (int)TCalcCommand.cmdCopy || command == (int)TCalcCommand.cmdPaste)
            {
                result = ExecuteClipboardCommand(command, ref clipboard);
                MState = FMemory.ReadState();
                return result;
            }

            // Команды памяти
            if (command >= (int)TCalcCommand.cmdMC && command <= (int)TCalcCommand.cmdMPlus)
            {
                result = ExecuteMemoryCommand(command, ref MState);
                return result;
            }

            // Команда сброса C
            if (command == (int)TCalcCommand.cmdClear)
            {
                result = SetInitialCalculatorState();
                MState = FMemory.ReadState();
                return result;
            }

            // Команда CE (очистить текущий ввод)
            if (command == (int)TCalcCommand.cmdClearEntry)
            {
                // Сброс состояния ошибки
                if (FState == TCtrlState.cError)
                {
                    SetInitialState();
                    MState = FMemory.ReadState();
                    return FShowAsFraction ? "0/1" : "0";
                }
                
                result = FEditor.Clear();
                FDisplayString = result;
                if (FState == TCtrlState.cValDone || FState == TCtrlState.cExpDone)
                {
                    FProcessor.SetRop(new TFrac(0, 1));
                }
                FState = TCtrlState.cEditing;
                MState = FMemory.ReadState();
                return result;
            }

            // Backspace
            if (command == (int)TCalcCommand.cmdBackspace)
            {
                result = FEditor.Backspace();
                FDisplayString = result;
                MState = FMemory.ReadState();
                return result;
            }

            // Ввод цифр и разделителя
            if (command >= (int)TCalcCommand.cmdDigit0 && command <= (int)TCalcCommand.cmdSeparator)
            {
                // Сброс состояния ошибки при начале нового ввода
                if (FState == TCtrlState.cError)
                {
                    SetInitialState();
                }
                
                if (FState == TCtrlState.cValDone || FState == TCtrlState.cOpChange)
                {
                    FPrevState = FState;
                    FState = TCtrlState.cEditing;
                    FEditor.Clear();
                    FDisplayString = FEditor.GetString();
                }
                else if (FState == TCtrlState.cStart || FState == TCtrlState.cExpDone || FState == TCtrlState.cFunDone)
                {
                    FPrevState = FState;
                    FState = TCtrlState.cEditing;
                    FEditor.Clear();
                    FDisplayString = FEditor.GetString();
                }

                result = ExecuteEditorCommand(command);
                FDisplayString = result;
                MState = FMemory.ReadState();
                return result;
            }

            // Операции (+, -, *, /)
            if (command >= (int)TCalcCommand.cmdAdd && command <= (int)TCalcCommand.cmdDiv)
            {
                result = ExecuteOperation(command);
                FDisplayString = result;
                MState = FMemory.ReadState();
                return result;
            }

            // Функции (Sqr, Rev)
            if (command == (int)TCalcCommand.cmdSqr || command == (int)TCalcCommand.cmdRev)
            {
                result = ExecuteFunction(command);
                FDisplayString = result;
                MState = FMemory.ReadState();
                return result;
            }

            // Равно (=)
            if (command == (int)TCalcCommand.cmdEqual)
            {
                result = CalculateExpression();
                FDisplayString = result;
                MState = FMemory.ReadState();
                return result;
            }

            MState = FMemory.ReadState();
            return FEditor.GetString();
        }

        /// <summary>
        /// Выполнить команду редактора
        /// </summary>
        private string ExecuteEditorCommand(int command)
        {
            TEditCommand editCmd = (TEditCommand)command;
            string result = FEditor.Edit(editCmd);
            FDisplayString = result;
            FState = TCtrlState.cEditing;
            return result;
        }

        /// <summary>
        /// Выполнить операцию
        /// </summary>
        private string ExecuteOperation(int command)
        {
            // Сброс состояния ошибки при новой операции
            if (FState == TCtrlState.cError)
            {
                SetInitialState();
            }
            
            TOprtn newOp = CommandToOperation(command);
            if (newOp == TOprtn.opNone) return FEditor.GetString();

            TFrac currentFrac = FEditor.ToFraction();
            currentFrac.ShowAsFraction = FShowAsFraction;

            try
            {
                switch (FState)
                {
                    case TCtrlState.cStart:
                    case TCtrlState.cEditing:
                        if (FPrevState == TCtrlState.cValDone || FPrevState == TCtrlState.cOpChange)
                        {
                            if (FProcessor.IsOperationSet())
                            {
                                FProcessor.SetRop(currentFrac);
                                FProcessor.OprtnRun();
                            }
                            FProcessor.SetOperation(newOp);
                            FState = TCtrlState.cValDone;
                        }
                        else
                        {
                            FProcessor.SetLop_Res(currentFrac);
                            FProcessor.SetOperation(newOp);
                            FState = TCtrlState.cValDone;
                        }
                        break;

                    case TCtrlState.cValDone:
                        FProcessor.SetOperation(newOp);
                        FState = TCtrlState.cOpChange;
                        break;

                    case TCtrlState.cOpChange:
                        FProcessor.SetOperation(newOp);
                        FState = TCtrlState.cOpChange;
                        break;

                    case TCtrlState.cFunDone:
                        if (FProcessor.IsOperationSet())
                        {
                            FProcessor.SetRop(FNumber);
                            FProcessor.OprtnRun();
                        }
                        else
                        {
                            FProcessor.SetLop_Res(FNumber);
                        }
                        FProcessor.SetOperation(newOp);
                        FState = TCtrlState.cValDone;
                        break;

                    case TCtrlState.cExpDone:
                        FProcessor.SetLop_Res(FNumber);
                        FProcessor.SetOperation(newOp);
                        FState = TCtrlState.cValDone;
                        break;
                }

                FEditor.Clear();
                string displayStr = FProcessor.Lop_Res.ToString() + " " + newOp.ToStringValue();
                FDisplayString = displayStr;

                return displayStr;
            }
            catch
            {
                FState = TCtrlState.cError;
                FDisplayString = "Error";
                return "Error";
            }
        }

        /// <summary>
        /// Выполнить функцию
        /// </summary>
        private string ExecuteFunction(int command)
        {
            // Сброс состояния ошибки при новой функции
            if (FState == TCtrlState.cError)
            {
                SetInitialState();
            }
            
            TFrac currentFrac = FEditor.ToFraction();
            currentFrac.ShowAsFraction = FShowAsFraction;
            TFunc func = (command == (int)TCalcCommand.cmdSqr) ? TFunc.fnSqr : TFunc.fnRev;

            try
            {
                FProcessor.SetRop(currentFrac);
                FProcessor.FuncRun(func);
                FNumber = FProcessor.Rop;
                FNumber.ShowAsFraction = FShowAsFraction;
                FEditor.SetString(FNumber.ToString());
                FDisplayString = FNumber.ToString();
                FState = TCtrlState.cFunDone;
                return FNumber.ToString();
            }
            catch
            {
                FState = TCtrlState.cError;
                FDisplayString = "Error";
                return "Error";
            }
        }

        /// <summary>
        /// Вычислить выражение
        /// </summary>
        private string CalculateExpression()
        {
            // Сброс состояния ошибки при вычислении
            if (FState == TCtrlState.cError)
            {
                SetInitialState();
            }
            
            TFrac currentFrac = FEditor.ToFraction();
            currentFrac.ShowAsFraction = FShowAsFraction;

            try
            {
                switch (FState)
                {
                    case TCtrlState.cValDone:
                    case TCtrlState.cOpChange:
                        if (FProcessor.IsOperationSet())
                        {
                            FProcessor.SetRop(currentFrac);
                            FProcessor.OprtnRun();
                            FNumber = FProcessor.Lop_Res;
                        }
                        else
                        {
                            FNumber = currentFrac;
                        }
                        FNumber.ShowAsFraction = FShowAsFraction;
                        FEditor.SetString(FNumber.ToString());
                        FDisplayString = FNumber.ToString();
                        FState = TCtrlState.cExpDone;
                        break;

                    case TCtrlState.cEditing:
                        if (FPrevState == TCtrlState.cValDone || FPrevState == TCtrlState.cOpChange)
                        {
                            if (FProcessor.IsOperationSet())
                            {
                                FProcessor.SetRop(currentFrac);
                                FProcessor.OprtnRun();
                                FNumber = FProcessor.Lop_Res;
                            }
                            else
                            {
                                FNumber = currentFrac;
                            }
                        }
                        else
                        {
                            FNumber = currentFrac;
                        }
                        FNumber.ShowAsFraction = FShowAsFraction;
                        FEditor.SetString(FNumber.ToString());
                        FDisplayString = FNumber.ToString();
                        FState = TCtrlState.cExpDone;
                        break;

                    case TCtrlState.cExpDone:
                        if (FProcessor.IsOperationSet())
                        {
                            FProcessor.SetRop(FNumber);
                            FProcessor.OprtnRun();
                            FNumber = FProcessor.Lop_Res;
                            FNumber.ShowAsFraction = FShowAsFraction;
                            FEditor.SetString(FNumber.ToString());
                            FDisplayString = FNumber.ToString();
                        }
                        break;

                    case TCtrlState.cFunDone:
                        if (FProcessor.IsOperationSet())
                        {
                            FProcessor.SetRop(FNumber);
                            FProcessor.OprtnRun();
                            FNumber = FProcessor.Lop_Res;
                        }
                        FNumber.ShowAsFraction = FShowAsFraction;
                        FEditor.SetString(FNumber.ToString());
                        FDisplayString = FNumber.ToString();
                        FState = TCtrlState.cExpDone;
                        break;
                }

                return FNumber.ToString();
            }
            catch
            {
                FState = TCtrlState.cError;
                FDisplayString = "Error";
                return "Error";
            }
        }

        /// <summary>
        /// Установить начальное состояние калькулятора
        /// </summary>
        private string SetInitialCalculatorState()
        {
            SetInitialState();
            FDisplayString = FShowAsFraction ? "0/1" : "0";
            return FDisplayString;
        }

        /// <summary>
        /// Выполнить команду памяти
        /// </summary>
        private string ExecuteMemoryCommand(int command, ref string MState)
        {
            TFrac currentFrac = FEditor.ToFraction();
            currentFrac.ShowAsFraction = FShowAsFraction;

            switch (command)
            {
                case (int)TCalcCommand.cmdMC:
                    FMemory.Clear();
                    break;

                case (int)TCalcCommand.cmdMS:
                    FMemory.Store(currentFrac);
                    break;

                case (int)TCalcCommand.cmdMR:
                    if (FMemory.IsOn())
                    {
                        TFrac memVal = FMemory.Recall();
                        memVal.ShowAsFraction = FShowAsFraction;
                        FEditor.SetString(memVal.ToString());
                        FDisplayString = memVal.ToString();
                        FState = TCtrlState.cEditing;
                    }
                    break;

                case (int)TCalcCommand.cmdMPlus:
                    FMemory.Add(currentFrac);
                    break;
            }

            MState = FMemory.ReadState();
            FDisplayString = FEditor.GetString();
            return FEditor.GetString();
        }

        /// <summary>
        /// Выполнить команду буфера обмена
        /// </summary>
        private string ExecuteClipboardCommand(int command, ref string clipboard)
        {
            switch (command)
            {
                case (int)TCalcCommand.cmdCopy:
                    clipboard = FEditor.GetString();
                    break;

                case (int)TCalcCommand.cmdPaste:
                    if (!string.IsNullOrEmpty(clipboard))
                    {
                        FEditor.SetString(clipboard);
                        FDisplayString = clipboard;
                        FState = TCtrlState.cEditing;
                    }
                    break;
            }

            return clipboard;
        }
    }
}
