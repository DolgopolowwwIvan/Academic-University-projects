namespace Calculator.Core;

// Управление калькулятором - распределяет команды между объектами
#nullable disable
public class TCtrl
{
    private TEditor _editor;
    private TProc _processor;
    private TMemory _memory;
    private TClipBoard _clipBoard;
    private TANumber _number;
    private TCtrlState _state;

    // Коды команд
    public const int CMD_DIGIT_0 = 0;
    public const int CMD_DIGIT_1 = 1;
    public const int CMD_DIGIT_2 = 2;
    public const int CMD_DIGIT_3 = 3;
    public const int CMD_DIGIT_4 = 4;
    public const int CMD_DIGIT_5 = 5;
    public const int CMD_DIGIT_6 = 6;
    public const int CMD_DIGIT_7 = 7;
    public const int CMD_DIGIT_8 = 8;
    public const int CMD_DIGIT_9 = 9;
    public const int CMD_DECIMAL_POINT = 10;
    public const int CMD_CHANGE_SIGN = 11;
    public const int CMD_BACKSPACE = 12;
    public const int CMD_CLEAR = 13;
    public const int CMD_CLEAR_ALL = 14;
    public const int CMD_ADD = 15;
    public const int CMD_SUBTRACT = 16;
    public const int CMD_MULTIPLY = 17;
    public const int CMD_DIVIDE = 18;
    public const int CMD_EQUALS = 19;
    public const int CMD_FUNC_SIN = 20;
    public const int CMD_FUNC_COS = 21;
    public const int CMD_FUNC_TAN = 22;
    public const int CMD_FUNC_SQRT = 23;
    public const int CMD_FUNC_LOG = 24;
    public const int CMD_FUNC_LN = 25;
    public const int CMD_FUNC_ABS = 26;
    public const int CMD_FUNC_EXP = 27;
    public const int CMD_MEMORY_STORE = 28;
    public const int CMD_MEMORY_RECALL = 29;
    public const int CMD_MEMORY_CLEAR = 30;
    public const int CMD_MEMORY_ADD = 31;
    public const int CMD_MEMORY_SUBTRACT = 32;
    public const int CMD_COPY = 33;
    public const int CMD_PASTE = 34;

    public TCtrl()
    {
        _editor = new TEditor();
        _processor = new TProc();
        _memory = new TMemory();
        _clipBoard = new TClipBoard();
        _number = new TANumber();
        _state = TCtrlState.cStart;
    }

    public TCtrlState State
    {
        get => _state;
        set => _state = value;
    }

    public TEditor Editor => _editor;
    public TProc Processor => _processor;
    public TMemory Memory => _memory;
    public TANumber Number => _number;

    // Выполнить команду калькулятора
    public string ExecuteCalculatorCommand(int command, ref string buffer, ref string memoryState)
    {
        string result = string.Empty;

        try
        {
            // Команды цифр
            if (command >= CMD_DIGIT_0 && command <= CMD_DIGIT_9)
            {
                result = ExecuteEditorCommand(command);
                _state = TCtrlState.cEditing;
            }
            // Десятичная точка
            else if (command == CMD_DECIMAL_POINT)
            {
                result = ExecuteEditorCommand(command);
                _state = TCtrlState.cEditing;
            }
            // Смена знака
            else if (command == CMD_CHANGE_SIGN)
            {
                result = ExecuteEditorCommand(command);
            }
            // Backspace
            else if (command == CMD_BACKSPACE)
            {
                result = ExecuteEditorCommand(command);
            }
            // Clear
            else if (command == CMD_CLEAR)
            {
                _editor.Clear();
                _number.Clear();
                result = "0";
                _state = TCtrlState.cStart;
            }
            // Clear All
            else if (command == CMD_CLEAR_ALL)
            {
                SetInitialCalculatorState(0);
                result = "0";
            }
            // Арифметические операции
            else if (command >= CMD_ADD && command <= CMD_DIVIDE)
            {
                result = ExecuteOperation(command);
            }
            // Равно
            else if (command == CMD_EQUALS)
            {
                result = CalculateExpression(command);
            }
            // Функции
            else if (command >= CMD_FUNC_SIN && command <= CMD_FUNC_EXP)
            {
                result = ExecuteFunction(command);
            }
            // Команды памяти
            else if (command >= CMD_MEMORY_STORE && command <= CMD_MEMORY_SUBTRACT)
            {
                result = ExecuteMemoryCommand(command, ref memoryState);
            }
            // Буфер обмена
            else if (command == CMD_COPY || command == CMD_PASTE)
            {
                result = ExecuteClipboardCommand(command, ref buffer);
            }

            // Обновляем буфер и состояние памяти
            buffer = _number.Value.ToString();
            memoryState = _memory.GetStateString();

            if (!string.IsNullOrEmpty(_processor.Error))
            {
                _state = TCtrlState.cError;
                result = _processor.Error;
            }
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Выполнить команду редактора
    public string ExecuteEditorCommand(int command)
    {
        string result = string.Empty;

        try
        {
            if (command >= CMD_DIGIT_0 && command <= CMD_DIGIT_9)
            {
                char digit = (char)('0' + command);
                _editor.InputDigit(digit);
            }
            else if (command == CMD_DECIMAL_POINT)
            {
                _editor.InputDecimalPoint();
            }
            else if (command == CMD_CHANGE_SIGN)
            {
                _editor.ChangeSign();
            }
            else if (command == CMD_BACKSPACE)
            {
                _editor.Backspace();
            }

            _number = _editor.GetNumber();
            result = _editor.GetDisplayString();
            _state = TCtrlState.cEditing;
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Выполнить операцию
    public string ExecuteOperation(int command)
    {
        string result = string.Empty;

        try
        {
            // Если есть предыдущая операция и состояние не cOpChange, выполняем её
            if (_processor.Operation != TOperation.None && _state != TCtrlState.cOpChange)
            {
                _processor.Ropd = _number.Copy();
                _processor.OprtnRun();
                
                if (!string.IsNullOrEmpty(_processor.Error))
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }
            }
            else
            {
                // Сохраняем первый операнд
                _processor.Lopd_Res = _number.Copy();
            }

            // Устанавливаем новую операцию
            switch (command)
            {
                case CMD_ADD:
                    _processor.Operation = TOperation.Add;
                    break;
                case CMD_SUBTRACT:
                    _processor.Operation = TOperation.Subtract;
                    break;
                case CMD_MULTIPLY:
                    _processor.Operation = TOperation.Multiply;
                    break;
                case CMD_DIVIDE:
                    _processor.Operation = TOperation.Divide;
                    break;
            }

            result = _processor.Lopd_Res.Value.ToString();
            _state = TCtrlState.cOpChange;
            _editor.Clear();
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Выполнить функцию
    public string ExecuteFunction(int command)
    {
        string result = string.Empty;

        try
        {
            TFunction func = command switch
            {
                CMD_FUNC_SIN => TFunction.Sin,
                CMD_FUNC_COS => TFunction.Cos,
                CMD_FUNC_TAN => TFunction.Tan,
                CMD_FUNC_SQRT => TFunction.Sqrt,
                CMD_FUNC_LOG => TFunction.Log,
                CMD_FUNC_LN => TFunction.Ln,
                CMD_FUNC_ABS => TFunction.Abs,
                CMD_FUNC_EXP => TFunction.Exp,
                _ => TFunction.None
            };

            _processor.Lopd_Res = _number.Copy();
            _processor.FuncRun(func);

            if (!string.IsNullOrEmpty(_processor.Error))
            {
                _state = TCtrlState.cError;
                return _processor.Error;
            }

            _number = _processor.Lopd_Res.Copy();
            result = _number.Value.ToString();
            _state = TCtrlState.FunDone;
            _editor.SetNumber(_number);
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Вычислить выражение (нажатие =)
    public string CalculateExpression(int command)
    {
        string result = string.Empty;

        try
        {
            if (_processor.Operation != TOperation.None)
            {
                _processor.Ropd = _number.Copy();
                _processor.OprtnRun();

                if (!string.IsNullOrEmpty(_processor.Error))
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                _number = _processor.Lopd_Res.Copy();
                result = _number.Value.ToString();
                _state = TCtrlState.cExpDone;
                _processor.Operation = TOperation.None;
                _editor.SetNumber(_number);
            }
            else
            {
                result = _number.Value.ToString();
                _state = TCtrlState.cValDone;
            }
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Установить начальное состояние калькулятора
    public string SetInitialCalculatorState(int command)
    {
        try
        {
            _editor.ReSet();
            _processor.ReSet();
            _memory.ReSet();
            _clipBoard.ReSet();
            _number.Clear();
            _state = TCtrlState.cStart;

            return "0";
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            return ex.Message;
        }
    }

    // Выполнить команду памяти
    public string ExecuteMemoryCommand(int command, ref string memoryState)
    {
        string result = string.Empty;

        try
        {
            switch (command)
            {
                case CMD_MEMORY_STORE:
                    _memory.Store(_number);
                    break;
                case CMD_MEMORY_RECALL:
                    _number = _memory.Recall();
                    _editor.SetNumber(_number);
                    break;
                case CMD_MEMORY_CLEAR:
                    _memory.Clear();
                    break;
                case CMD_MEMORY_ADD:
                    _memory.Add(_number);
                    break;
                case CMD_MEMORY_SUBTRACT:
                    _memory.Subtract(_number);
                    break;
            }

            memoryState = _memory.GetStateString();
            result = _number.Value.ToString();

            if (!string.IsNullOrEmpty(_memory.Error))
            {
                result = _memory.Error;
            }
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }

    // Выполнить команду буфера обмена
    public string ExecuteClipboardCommand(int command, ref string buffer)
    {
        string result = string.Empty;

        try
        {
            switch (command)
            {
                case CMD_COPY:
                    _clipBoard.Copy(_number.Value.ToString());
                    buffer = _clipBoard.Content;
                    break;
                case CMD_PASTE:
                    string pasted = _clipBoard.Paste();
                    if (!string.IsNullOrEmpty(pasted))
                    {
                        if (double.TryParse(pasted, out double value))
                        {
                            _number = new TANumber(value);
                            _editor.SetNumber(_number);
                            result = _number.Value.ToString();
                        }
                    }
                    buffer = _clipBoard.Content;
                    break;
            }
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }

    ~TCtrl()
    {
        _editor = null;
        _processor = null;
        _memory = null;
        _clipBoard = null;
        _number = null;
    }
}
#nullable enable
