namespace Calculator.Core;

// Редактор - отвечает за ввод и редактирование чисел
#nullable disable
public class TEditor
{
    private string _inputBuffer;
    private bool _isNegative;
    private bool _hasDecimalPoint;
    private string _error;

    public TEditor()
    {
        _inputBuffer = string.Empty;
        _isNegative = false;
        _hasDecimalPoint = false;
        _error = string.Empty;
    }

    // Буфер ввода
    public string InputBuffer
    {
        get => _inputBuffer;
        set => _inputBuffer = value;
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

        if (_inputBuffer.Length >= 20)
        {
            _error = "Превышена максимальная длина числа";
            return;
        }

        _inputBuffer += digit;
        _error = string.Empty;
    }

    // Ввести десятичную точку
    public void InputDecimalPoint()
    {
        if (_hasDecimalPoint)
        {
            _error = "Десятичная точка уже введена";
            return;
        }

        if (string.IsNullOrEmpty(_inputBuffer))
        {
            _inputBuffer = "0.";
        }
        else
        {
            _inputBuffer += ".";
        }

        _hasDecimalPoint = true;
        _error = string.Empty;
    }

    // Изменить знак числа
    public void ChangeSign()
    {
        _isNegative = !_isNegative;
        _error = string.Empty;
    }

    // Удалить последний символ
    public void Backspace()
    {
        if (string.IsNullOrEmpty(_inputBuffer))
        {
            return;
        }

        if (_inputBuffer.EndsWith("."))
        {
            _hasDecimalPoint = false;
        }

        _inputBuffer = _inputBuffer[..^1];
        _error = string.Empty;
    }

    // Очистить ввод
    public void Clear()
    {
        _inputBuffer = string.Empty;
        _isNegative = false;
        _hasDecimalPoint = false;
        _error = string.Empty;
    }

    // Получить число из буфера ввода
    public TANumber GetNumber()
    {
        if (string.IsNullOrEmpty(_inputBuffer))
        {
            return new TANumber(0);
        }

        string numberStr = _inputBuffer;
        if (_isNegative && !numberStr.StartsWith("-"))
        {
            numberStr = "-" + numberStr;
        }

        if (double.TryParse(numberStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value))
        {
            return new TANumber(value);
        }

        _error = "Ошибка преобразования числа";
        return new TANumber(0);
    }

    // Установить число в буфер ввода
    public void SetNumber(TANumber number)
    {
        _inputBuffer = number.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        _isNegative = number.Value < 0;
        _hasDecimalPoint = _inputBuffer.Contains(".");
        _error = string.Empty;
    }

    // Получить отображаемую строку
    public string GetDisplayString()
    {
        string result = _inputBuffer;
        if (_isNegative && !string.IsNullOrEmpty(result) && !result.StartsWith("-"))
        {
            result = "-" + result;
        }
        return string.IsNullOrEmpty(result) ? "0" : result;
    }

    // Получить число как строку для отображения
    public string GetNumberString()
    {
        var number = GetNumber();
        return number.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    // Сбросить состояние
    public void ReSet()
    {
        Clear();
    }

    ~TEditor()
    {
        _inputBuffer = null;
    }
}
#nullable enable
