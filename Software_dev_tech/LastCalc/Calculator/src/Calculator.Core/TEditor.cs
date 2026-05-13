namespace Calculator.Core;

using Calculator.Core.Numbers;
using Calculator.Core.Editors;

// Редактор - отвечает за ввод и редактирование чисел (обёртка для универсальности)
public class TEditor
{
    private AEditor _editor;
    private TANumber _currentNumber;
    private NumberType _numberType;
    private int _baseSystem;
    private string _error;

    public TEditor()
    {
        _editor = new FEditor();
        _currentNumber = new TPNumber(0);
        _numberType = NumberType.Real;
        _baseSystem = 10;
        _error = string.Empty;
    }

    public TEditor(NumberType numberType)
    {
        _numberType = numberType;
        _baseSystem = 10;
        CreateEditor();
        _currentNumber = CreateDefaultNumber();
        _error = string.Empty;
    }

    public TEditor(NumberType numberType, int baseSystem)
    {
        _numberType = numberType;
        _baseSystem = Math.Max(2, Math.Min(16, baseSystem));
        CreateEditor();
        _currentNumber = CreateDefaultNumber();
        _error = string.Empty;
    }

    private void CreateEditor()
    {
        switch (_numberType)
        {
            case NumberType.Real:
                _editor = new REEditor(_baseSystem);
                break;
            case NumberType.Fraction:
                _editor = new FEditor();
                break;
            case NumberType.Complex:
                _editor = new CEditor(_baseSystem);
                break;
        }
    }

    private TANumber CreateDefaultNumber()
    {
        return _numberType switch
        {
            NumberType.Real => new TPNumber(0),
            NumberType.Fraction => new Frac(0, 1),
            NumberType.Complex => new TComplex(0, 0),
            _ => new TPNumber(0)
        };
    }

    // Текущий редактор
    public AEditor Editor => _editor;

    // Тип числа
    public NumberType NumberType
    {
        get => _numberType;
        set
        {
            _numberType = value;
            CreateEditor();
            _currentNumber = CreateDefaultNumber();
        }
    }

    // Основание системы счисления
    public int BaseSystem
    {
        get => _baseSystem;
        set
        {
            _baseSystem = Math.Max(2, Math.Min(16, value));
            if (_numberType == NumberType.Real || _numberType == NumberType.Complex)
                CreateEditor();
        }
    }

    // Ошибка
    public string Error
    {
        get => _error;
        set => _error = value;
    }

    // Очистить ошибку
    public void ClearError()
    {
        _error = string.Empty;
    }

    // Ввести цифру
    public void InputDigit(char digit)
    {
        if (!char.IsDigit(digit))
        {
            _error = "Некорректный символ цифры";
            return;
        }

        int digitValue = digit - '0';
        _editor.AddDigit((uint)digitValue);
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Ввести цифру (по коду, 0-15 для систем 2-16)
    public void InputDigit(uint digit)
    {
        if (digit >= (uint)_baseSystem)
        {
            _error = "Некорректная цифра для данной системы счисления";
            return;
        }

        _editor.AddDigit(digit);
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Ввести десятичную точку
    public void InputDecimalPoint()
    {
        _editor.AddSeparator(AEditor.cSeparatorFR);
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Ввести разделитель (дроби или комплексной части)
    public void InputSeparator()
    {
        if (_numberType == NumberType.Fraction)
            _editor.AddSeparator(AEditor.cSeparatorFrac);
        else if (_numberType == NumberType.Complex)
            _editor.AddSeparator(AEditor.cSeparatorComplex);
        else
            _editor.AddSeparator(AEditor.cSeparatorFR);
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Изменить знак числа
    public void ChangeSign()
    {
        _editor.AddSigne();
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Удалить последний символ
    public void Backspace()
    {
        _editor.BackSpace();
        _currentNumber = GetNumber();
        _error = string.Empty;
    }

    // Очистить ввод
    public void Clear()
    {
        _editor.Clear();
        _currentNumber = CreateDefaultNumber();
        _error = string.Empty;
    }

    // Получить число из буфера ввода
    public TANumber GetNumber()
    {
        string str = _editor.ReadStringAsString();
        
        switch (_numberType)
        {
            case NumberType.Real:
                double realValue = TPNumber.ParseBaseStringInternal(str, _baseSystem);
                return new TPNumber(realValue, _baseSystem);
            
            case NumberType.Fraction:
                return new Frac(str);
            
            case NumberType.Complex:
                return new TComplex(str);
            
            default:
                return new TPNumber(0);
        }
    }

    // Установить число в буфер ввода
    public void SetNumber(TANumber number)
    {
        if (number == null)
        {
            Clear();
            return;
        }

        _numberType = number.GetType();
        CreateEditor();
        
        _editor.WriteStringAsString(number.ReadNumberAsString());
        _currentNumber = number.Copy();
        _error = string.Empty;
    }

    // Получить отображаемую строку
    public string GetDisplayString()
    {
        return _editor.ReadStringAsString();
    }

    // Получить число как строку для отображения
    public string GetNumberString()
    {
        return _currentNumber.ReadNumberAsString();
    }

    // Сбросить состояние
    public void ReSet()
    {
        Clear();
    }

    ~TEditor()
    {
        _editor = null;
        _currentNumber = null;
    }
}
#nullable enable
