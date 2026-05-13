namespace Calculator.Core.Editors;

/// <summary>
/// Редактор для простых дробей (внутренний класс)
/// </summary>
internal class FEditor : AEditor
{
    private string _number;
    private bool _isNegative;
    private bool _fractionSeparatorExists;
    private bool _inFractionPart;

    public FEditor() : base("0", "/")
    {
        _number = string.Empty;
        _isNegative = false;
        _fractionSeparatorExists = false;
        _inFractionPart = false;
    }

    public FEditor(string zeroString, string separator) : base(zeroString, separator)
    {
        _number = string.Empty;
        _isNegative = false;
        _fractionSeparatorExists = false;
        _inFractionPart = false;
    }

    protected override void SetNum(string n)
    {
        _number = n ?? string.Empty;
    }

    protected override string GetNum()
    {
        return _number;
    }

    public override bool IsZero()
    {
        if (_isNegative) return false;
        if (string.IsNullOrEmpty(_number)) return true;
        if (_fractionSeparatorExists)
        {
            string[] parts = _number.Split(_separator[0]);
            return parts.Length > 0 && parts[0].All(c => c == '0');
        }
        return _number.All(c => c == '0');
    }

    protected virtual void AddDigitLS(int a)
    {
        if (_inFractionPart || _number.Length >= MAX_LENGTH) return;
        _number += (char)('0' + a);
    }

    protected virtual void AddDigitRS(int a)
    {
        if (!_fractionSeparatorExists || !_inFractionPart || _number.Length >= MAX_LENGTH) return;
        _number += (char)('0' + a);
    }

    public override string AddSeparator(uint a = 0)
    {
        if (_fractionSeparatorExists) return GetResultString();
        if (!string.IsNullOrEmpty(_number) || _isNegative)
        {
            _number += _separator;
            _fractionSeparatorExists = true;
            _inFractionPart = true;
        }
        return GetResultString();
    }

    public override string AddDigit(uint a)
    {
        if (a > 9) return GetResultString();
        if (!_fractionSeparatorExists)
            AddDigitLS((int)a);
        else
            AddDigitRS((int)a);
        return GetResultString();
    }

    public override string AddSigne(uint a = 0)
    {
        _isNegative = !_isNegative;
        return GetResultString();
    }

    public override string BackSpace()
    {
        if (string.IsNullOrEmpty(_number)) return GetResultString();
        if (_number.EndsWith(_separator))
        {
            _fractionSeparatorExists = false;
            _inFractionPart = false;
        }
        _number = _number[..^1];
        if (_fractionSeparatorExists)
        {
            int sepIndex = _number.IndexOf(_separator);
            if (sepIndex >= 0 && sepIndex == _number.Length - 1)
            {
                _fractionSeparatorExists = false;
                _inFractionPart = false;
            }
        }
        return GetResultString();
    }

    public override string Clear()
    {
        _number = string.Empty;
        _isNegative = false;
        _fractionSeparatorExists = false;
        _inFractionPart = false;
        return GetResultString();
    }

    private string GetResultString()
    {
        if (string.IsNullOrEmpty(_number) && !_isNegative) return _zeroString;
        string result = _isNegative ? "-" + _number : _number;
        return string.IsNullOrEmpty(result) ? _zeroString : result;
    }

    public override string ReadStringAsString() => GetResultString();
    public override void WriteStringAsString(string value)
    {
        if (string.IsNullOrEmpty(value)) { Clear(); return; }
        _isNegative = value.StartsWith("-");
        string numPart = _isNegative ? value.Substring(1) : value;
        _fractionSeparatorExists = numPart.Contains(_separator);
        _inFractionPart = _fractionSeparatorExists;
        _number = numPart;
    }

    public override void ReSet() => Clear();

    private const int MAX_LENGTH = 20;
}
