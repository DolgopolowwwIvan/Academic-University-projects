namespace Calculator.Core.Editors;

using Calculator.Core.Numbers;

/// <summary>
/// Редактор для p-ичных чисел (внутренний класс)
/// </summary>
internal class REEditor : AEditor
{
    private int _baseSystem;
    private bool _isNegative;

    public const int MIN_BASE = 2;
    public const int MAX_BASE = 16;

    public REEditor() : base("0", ".")
    {
        _baseSystem = 10;
        _isNegative = false;
    }

    public REEditor(int baseSystem) : base("0", ".")
    {
        _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, baseSystem));
        _isNegative = false;
    }

    public int BaseSystem
    {
        get => _baseSystem;
        set => _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, value));
    }

    protected override void SetNum(string n)
    {
        _stringValue = n ?? string.Empty;
        _isNegative = _stringValue.StartsWith("-");
    }

    protected override string GetNum() => _stringValue;

    public override bool IsZero()
    {
        string val = _isNegative ? _stringValue.Substring(1) : _stringValue;
        return string.IsNullOrEmpty(val) || val.All(c => c == '0' || c == '.');
    }

    protected virtual void AddDigitLS(int a)
    {
        if (_stringValue.Length >= MAX_LENGTH) return;
        char digit = GetDigitChar(a);
        if (!IsValidDigit(digit)) return;

        // Блокировка ведущих незначащих нулей
        if (a == 0)
        {
            // Не добавлять ноль, если строка пустая или уже "0"
            if (string.IsNullOrEmpty(_stringValue) || _stringValue == "0")
                return;
        }
        else
        {
            // Если текущее значение "0", заменить его на новую цифру
            if (_stringValue == "0")
            {
                _stringValue = digit.ToString();
                return;
            }
        }

        _stringValue += digit;
    }

    protected virtual void AddDigitRS(int a)
    {
        if (!_separatorExists)
        {
            _stringValue += _separator;
            _separatorExists = true;
        }
        if (_stringValue.Length >= MAX_LENGTH) return;
        char digit = GetDigitChar(a);
        if (IsValidDigit(digit))
            _stringValue += digit;
    }

    private bool IsValidDigit(char digit)
    {
        int digitValue = GetDigitValue(digit);
        return digitValue >= 0 && digitValue < _baseSystem;
    }

    public override string AddDigit(uint a)
    {
        if (a >= _baseSystem) return GetResultString();
        if (!_separatorExists)
            AddDigitLS((int)a);
        else
            AddDigitRS((int)a);
        return GetResultString();
    }

    private static char GetDigitChar(int value)
    {
        const string digits = "0123456789ABCDEF";
        return digits[value];
    }

    public override string AddSeparator(uint a = 0)
    {
        if (_separatorExists) return GetResultString();
        if (string.IsNullOrEmpty(_stringValue) && !_isNegative)
        {
            _stringValue = "0";
        }
        _stringValue += _separator;
        _separatorExists = true;
        return GetResultString();
    }

    public override string AddSigne(uint a = 0)
    {
        _isNegative = !_isNegative;
        return GetResultString();
    }

    public override string BackSpace()
    {
        if (string.IsNullOrEmpty(_stringValue)) return GetResultString();
        if (_stringValue.EndsWith(_separator))
            _separatorExists = false;
        _stringValue = _stringValue[..^1];
        return GetResultString();
    }

    public override string Clear()
    {
        _stringValue = string.Empty;
        _isNegative = false;
        _separatorExists = false;
        return GetResultString();
    }

    private string GetResultString()
    {
        if (string.IsNullOrEmpty(_stringValue) && !_isNegative) return _zeroString;
        string result = _isNegative ? "-" + _stringValue : _stringValue;
        return string.IsNullOrEmpty(result) ? _zeroString : result;
    }

    public override string ReadStringAsString() => GetResultString();
    public override void WriteStringAsString(string value)
    {
        if (string.IsNullOrEmpty(value)) { Clear(); return; }
        _isNegative = value.StartsWith("-");
        _stringValue = _isNegative ? value.Substring(1) : value;
        _separatorExists = _stringValue.Contains(_separator);
    }

    public override void ReSet() => Clear();

    public double ParseValue() => TPNumber.ParseBaseStringInternal(_stringValue, _baseSystem);

    private const int MAX_LENGTH = 30;

    private static int GetDigitValue(char c)
    {
        const string digits = "0123456789ABCDEF";
        return digits.IndexOf(char.ToUpper(c));
    }
}
