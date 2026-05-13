namespace Calculator.Core.Editors;

using Calculator.Core.Numbers;

/// <summary>
/// Редактор для комплексных чисел (внутренний класс)
/// </summary>
internal class CEditor : AEditor
{
    private REEditor _realEditor;
    private REEditor _imagEditor;
    private bool _hasImaginaryPart;
    private bool _isNegative;

    public const string IMAGINARY_SEPARATOR = "i*";

    public CEditor() : base("0", ".")
    {
        _realEditor = new REEditor();
        _imagEditor = new REEditor();
        _hasImaginaryPart = false;
        _isNegative = false;
    }

    public CEditor(int baseSystem) : base("0", ".")
    {
        _realEditor = new REEditor(baseSystem);
        _imagEditor = new REEditor(baseSystem);
        _hasImaginaryPart = false;
        _isNegative = false;
    }

    public int BaseSystem
    {
        get => _realEditor.BaseSystem;
        set
        {
            _realEditor.BaseSystem = value;
            _imagEditor.BaseSystem = value;
        }
    }

    protected override void SetNum(string n) => ParseFromString(n ?? string.Empty);
    protected override string GetNum() => GetResultString();

    public override bool IsZero() => _realEditor.IsZero() && (_imagEditor.IsZero() || !_hasImaginaryPart);

    public override string AddDigit(uint a)
    {
        if (!_hasImaginaryPart)
        {
            _realEditor.AddDigit(a);
        }
        else
        {
            _imagEditor.AddDigit(a);
        }
        return GetResultString();
    }

    public override string AddSeparator(uint a = 0)
    {
        if (_hasImaginaryPart) return GetResultString();
        _hasImaginaryPart = true;
        return GetResultString();
    }

    public override string AddSigne(uint a = 0)
    {
        _isNegative = !_isNegative;
        return GetResultString();
    }

    public override string BackSpace()
    {
        if (_hasImaginaryPart)
        {
            string imagStr = _imagEditor.ReadStringAsString();
            if (!string.IsNullOrEmpty(imagStr))
                _imagEditor.BackSpace();
            else
                _hasImaginaryPart = false;
        }
        else
        {
            _realEditor.BackSpace();
        }
        return GetResultString();
    }

    public override string Clear()
    {
        _realEditor.Clear();
        _imagEditor.Clear();
        _hasImaginaryPart = false;
        _isNegative = false;
        return GetResultString();
    }

    private string GetResultString()
    {
        string realStr = _realEditor.ReadStringAsString();
        
        if (!_hasImaginaryPart)
        {
            if (_isNegative && !realStr.StartsWith("-"))
                realStr = "-" + realStr;
            return realStr;
        }

        string imagStr = _imagEditor.ReadStringAsString();
        string sign = imagStr.StartsWith("-") ? " - " : " + ";
        if (imagStr.StartsWith("-"))
            imagStr = imagStr.Substring(1);
        
        if (_isNegative && !realStr.StartsWith("-"))
            realStr = "-" + realStr;

        return $"{realStr}{sign}{imagStr}{IMAGINARY_SEPARATOR}";
    }

    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value)) { Clear(); return; }

        _isNegative = value.StartsWith("-");
        string str = _isNegative ? value.Substring(1) : value;

        int imagSepIndex = str.IndexOf(IMAGINARY_SEPARATOR);
        
        if (imagSepIndex < 0)
        {
            _realEditor.WriteStringAsString(str);
            _hasImaginaryPart = false;
        }
        else
        {
            string realPartStr = str.Substring(0, imagSepIndex).Trim();
            string imagPartStr = str.Substring(imagSepIndex + IMAGINARY_SEPARATOR.Length).Trim();
            
            _realEditor.WriteStringAsString(realPartStr);
            
            bool imagNegative = imagPartStr.StartsWith("-");
            if (imagNegative) imagPartStr = imagPartStr.Substring(1);
            imagPartStr = imagPartStr.TrimStart('+', ' ');

            if (!string.IsNullOrEmpty(imagPartStr))
            {
                _imagEditor.WriteStringAsString(imagPartStr);
                _hasImaginaryPart = true;
            }
            else
            {
                _hasImaginaryPart = false;
            }
        }
    }

    public override string ReadStringAsString() => GetResultString();
    public override void WriteStringAsString(string value) => ParseFromString(value);
    public override void ReSet() => Clear();

    public string GetRealPart() => _realEditor.ReadStringAsString();
    public string GetImaginaryPart() => _imagEditor.ReadStringAsString();
    public void SetRealPart(string value) => _realEditor.WriteStringAsString(value);
    public void SetImaginaryPart(string value) => _imagEditor.WriteStringAsString(value);
}
