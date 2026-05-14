namespace Calculator.Core;

using Calculator.Core.Numbers;

// Управление калькулятором - распределяет команды между объектами
public class TCtrl
{
    private TEditor _editor;
    private TProc _processor;
    private TMemory _memory;
    private TClipBoard _clipBoard;
    private TANumber _number;
    private TCtrlState _state;
    private THelp _help;
    private const string _zeroString = "0";

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
    public const int CMD_DIGIT_A = 35;
    public const int CMD_DIGIT_B = 36;
    public const int CMD_DIGIT_C = 37;
    public const int CMD_DIGIT_D = 38;
    public const int CMD_DIGIT_E = 39;
    public const int CMD_DIGIT_F = 40;
    public const int CMD_SEPARATOR_FRAC = 41;
    public const int CMD_SEPARATOR_COMPLEX = 42;
    public const int CMD_FUNC_SQR = 43;
    public const int CMD_FUNC_REV = 44;

    public TCtrl()
    {
        _editor = new TEditor(NumberType.Real, 10);
        _processor = new TProc();
        _memory = new TMemory();
        _clipBoard = new TClipBoard();
        _number = new TPNumber(0, 10);
        _state = TCtrlState.cStart;
        _help = new THelp();
    }

    public TCtrlState State
    {
        get => _state;
        set => _state = value;
    }

    public TEditor Editor
    {
        get => _editor;
        set => _editor = value;
    }
    
    public TProc Processor => _processor;
    public TMemory Memory => _memory;
    
    public TANumber Number
    {
        get => _number;
        set => _number = value;
    }
    
    public THelp Help => _help;

    // Выполнить команду калькулятора
    public string ExecuteCalculatorCommand(int command, ref string buffer, ref string memoryState)
    {
        string result = string.Empty;

        try
        {
            if ((command >= CMD_DIGIT_0 && command <= CMD_DIGIT_9) ||
                (command >= CMD_DIGIT_A && command <= CMD_DIGIT_F))
            {
                result = ExecuteEditorCommand(command);
                _state = TCtrlState.cEditing;
            }
            else if (command == CMD_DECIMAL_POINT || command == CMD_SEPARATOR_FRAC || command == CMD_SEPARATOR_COMPLEX)
            {
                result = ExecuteEditorCommand(command);
                _state = TCtrlState.cEditing;
            }
            else if (command == CMD_CHANGE_SIGN)
            {
                result = ExecuteEditorCommand(command);
            }
            else if (command == CMD_BACKSPACE)
            {
                result = ExecuteEditorCommand(command);
            }
            else if (command == CMD_CLEAR)
            {
                _editor.Clear();
                _number = new TPNumber(0);
                result = "0";
                _state = TCtrlState.cStart;
            }
            else if (command == CMD_CLEAR_ALL)
            {
                SetInitialCalculatorState();
                result = "0";
            }
            else if (command >= CMD_ADD && command <= CMD_DIVIDE)
            {
                result = ExecuteOperation(command);
            }
            else if (command == CMD_EQUALS)
            {
                result = CalculateExpression();
            }
            else if ((command >= CMD_FUNC_SIN && command <= CMD_FUNC_EXP) ||
                     command == CMD_FUNC_SQR || command == CMD_FUNC_REV)
            {
                result = ExecuteFunction(command);
            }
            else if (command >= CMD_MEMORY_STORE && command <= CMD_MEMORY_SUBTRACT)
            {
                result = ExecuteMemoryCommand(command, ref memoryState);
            }
            else if (command == CMD_COPY || command == CMD_PASTE)
            {
                result = ExecuteClipboardCommand(command, ref buffer);
            }

            buffer = _number.ReadNumberAsString();
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
                _editor.InputDigit((uint)command);
            }
            else if (command >= CMD_DIGIT_A && command <= CMD_DIGIT_F)
            {
                _editor.InputDigit((uint)(command - CMD_DIGIT_A + 10));
            }
            else if (command == CMD_DECIMAL_POINT)
            {
                _editor.InputDecimalPoint();
            }
            else if (command == CMD_SEPARATOR_FRAC)
            {
                _editor.InputSeparator();
            }
            else if (command == CMD_SEPARATOR_COMPLEX)
            {
                _editor.InputSeparator();
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
            if (string.IsNullOrEmpty(result) || result == _zeroString)
                result = _number.ReadNumberAsString();
            
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
            if (_processor.Operation != TOperation.None && _state != TCtrlState.cOpChange)
            {
                _processor.Ropd = _number.Copy();
                _processor.OprtnRun();

                if (!string.IsNullOrEmpty(_processor.Error))
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                _number = _processor.Lopd_Res.Copy();
                _editor.SetNumberWithoutReset(_number);
                result = _editor.GetDisplayString();
                if (string.IsNullOrEmpty(result))
                    result = _number.ReadNumberAsString();
            }
            else
            {
                _processor.Lopd_Res = _number.Copy();
                _number = _editor.GetNumber();
            }

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

            result = _number.ReadNumberAsString();
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
                CMD_FUNC_SQR => TFunction.Sqr,
                CMD_FUNC_REV => TFunction.Reverse,
                _ => TFunction.None
            };

            // Сохраняем текущее значение как десятичное
            double currentValue = _editor.GetNumberAsDouble();
            _number = _editor.GetNumber();
            
            // Применяем функцию к числу
            _processor.Lopd_Res = _number.Copy();
            _processor.FuncRun(func);

            if (!string.IsNullOrEmpty(_processor.Error))
            {
                _state = TCtrlState.cError;
                return _processor.Error;
            }

            // Сохраняем результат и обновляем редактор
            _number = _processor.Lopd_Res.Copy();
            _editor.SetNumberWithoutReset(_number);
            result = _editor.GetDisplayString();
            if (string.IsNullOrEmpty(result))
                result = _number.ReadNumberAsString();
            _state = TCtrlState.FunDone;
        }
        catch (Exception ex)
        {
            _state = TCtrlState.cError;
            result = ex.Message;
        }

        return result;
    }

    // Вычислить выражение (нажатие =)
    public string CalculateExpression()
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
                _editor.SetNumberWithoutReset(_number);
                result = _editor.GetDisplayString();
                if (string.IsNullOrEmpty(result))
                    result = _number.ReadNumberAsString();
                _state = TCtrlState.cExpDone;
                _processor.Operation = TOperation.None;
            }
            else
            {
                result = _number.ReadNumberAsString();
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
    public string SetInitialCalculatorState()
    {
        try
        {
            _editor.ReSet();
            _processor.ReSet();
            _memory.ReSet();
            _clipBoard.ReSet();
            
            _number = _editor.NumberType switch
            {
                NumberType.Fraction => new Frac(0, 1),
                NumberType.Complex => new TComplex(0, 0),
                _ => new TPNumber(0, _editor.BaseSystem)
            };
            
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
                    result = _number.ReadNumberAsString();
                    _editor.Clear();
                    _number = new TPNumber(0, _editor.BaseSystem);
                    break;
                case CMD_MEMORY_RECALL:
                    _number = _memory.Recall();
                    _editor.SetNumberWithoutReset(_number);
                    result = _editor.GetDisplayString();
                    if (string.IsNullOrEmpty(result))
                        result = _number.ReadNumberAsString();
                    break;
                case CMD_MEMORY_CLEAR:
                    _memory.Clear();
                    result = _number.ReadNumberAsString();
                    break;
                case CMD_MEMORY_ADD:
                    _memory.Add(_number);
                    result = _memory.Recall().ReadNumberAsString();
                    _editor.Clear();
                    _number = new TPNumber(0, _editor.BaseSystem);
                    break;
                case CMD_MEMORY_SUBTRACT:
                    _memory.Subtract(_number);
                    result = _memory.Recall().ReadNumberAsString();
                    _editor.Clear();
                    _number = new TPNumber(0, _editor.BaseSystem);
                    break;
            }

            memoryState = _memory.GetStateString();

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
                    _clipBoard.Copy(_number.ReadNumberAsString());
                    buffer = _clipBoard.Content;
                    break;
                case CMD_PASTE:
                    double pastedValue = _clipBoard.PasteValue();
                    _number = new TPNumber(pastedValue, _editor.BaseSystem);
                    _editor.SetNumberWithoutReset(_number);
                    result = _editor.GetDisplayString();
                    if (string.IsNullOrEmpty(result))
                        result = _number.ReadNumberAsString();
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
}

