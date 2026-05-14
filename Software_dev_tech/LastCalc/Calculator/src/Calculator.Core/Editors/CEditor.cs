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

    public const string IMAGINARY_SEPARATOR = "i*";

    public CEditor() : base("0", ".")
    {
        _realEditor = new REEditor();
        _imagEditor = new REEditor();
        _hasImaginaryPart = false;
    }

    public CEditor(int baseSystem) : base("0", ".")
    {
        _realEditor = new REEditor(baseSystem);
        _imagEditor = new REEditor(baseSystem);
        _hasImaginaryPart = false;
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
            _realEditor.AddDigit(a);
        else
            _imagEditor.AddDigit(a);
        return GetResultString();
    }

    public override string AddSeparator(uint a = 0)
    {
        if (_hasImaginaryPart) return GetResultString();
        _hasImaginaryPart = true;
        return GetResultString();
    }

    /// <summary>
    /// Смена знака делегируется текущему редактору (действительной или мнимой части)
    /// </summary>
    public override string AddSigne(uint a = 0)
    {
        if (!_hasImaginaryPart)
            _realEditor.AddSigne(a);
        else
            _imagEditor.AddSigne(a);
        return GetResultString();
    }

    public override string BackSpace()
    {
        if (_hasImaginaryPart)
        {
            string imagStr = _imagEditor.ReadStringAsString();
            if (!string.IsNullOrEmpty(imagStr) && imagStr != "0")
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
        return GetResultString();
    }

    /// <summary>
    /// Формирует строку вида "a + bi*" или "a - bi*"
    /// Знак мнимой части берётся из _imagEditor
    /// </summary>
    private string GetResultString()
    {
        string realStr = _realEditor.ReadStringAsString();
        if (string.IsNullOrEmpty(realStr)) realStr = "0";

        if (!_hasImaginaryPart)
            return realStr;

        string imagStr = _imagEditor.ReadStringAsString();
        if (string.IsNullOrEmpty(imagStr)) imagStr = "0";

        // Определяем знак мнимой части
        bool imagNegative = imagStr.StartsWith("-");
        if (imagNegative)
            imagStr = imagStr.Substring(1);

        // Если мнимая часть 0 — показываем только действительную
        if (imagStr == "0" || string.IsNullOrEmpty(imagStr))
            return realStr;

        string sign = imagNegative ? " - " : " + ";
        return $"{realStr}{sign}{imagStr}{IMAGINARY_SEPARATOR}";
    }

    /// <summary>
    /// Парсит строки вида:
    ///   "5", "-5", "5 + 3i*", "5 - 3i*", "-5 + 3i*", "-5 - 3i*"
    ///   "5+3i*", "5-3i*" (без пробелов — fallback)
    /// </summary>
    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value)) { Clear(); return; }

        int imagSepIndex = value.IndexOf(IMAGINARY_SEPARATOR);

        if (imagSepIndex < 0)
        {
            // Только действительная часть
            _realEditor.WriteStringAsString(value);
            _hasImaginaryPart = false;
            return;
        }

        string beforeImag = value.Substring(0, imagSepIndex).TrimEnd();

        // Ищем последний " + " или " - " — разделитель действительной и мнимой части
        int lastPlus = beforeImag.LastIndexOf(" + ");
        int lastMinus = beforeImag.LastIndexOf(" - ");

        int signPos = Math.Max(lastPlus, lastMinus);

        if (signPos > 0)
        {
            // Формат с пробелами: "a + bi*" / "a - bi*"
            string realStr = beforeImag.Substring(0, signPos).Trim();
            string imagStr = beforeImag.Substring(signPos + 3).Trim();

            if (lastMinus > lastPlus)
                imagStr = "-" + imagStr;

            _realEditor.WriteStringAsString(realStr);
            _imagEditor.WriteStringAsString(imagStr);
            _hasImaginaryPart = true;
            return;
        }

        // Fallback: формат без пробелов "a-bi*" или "a+bi*"
        int lastMinusCompact = beforeImag.LastIndexOf('-');
        int lastPlusCompact = beforeImag.LastIndexOf('+');

        int compactSignPos = Math.Max(lastMinusCompact, lastPlusCompact);

        if (compactSignPos > 0)
        {
            string realStr = beforeImag.Substring(0, compactSignPos).Trim();
            string imagStr = beforeImag.Substring(compactSignPos + 1).Trim();

            if (lastMinusCompact > lastPlusCompact)
                imagStr = "-" + imagStr;

            _realEditor.WriteStringAsString(realStr);
            _imagEditor.WriteStringAsString(imagStr);
            _hasImaginaryPart = true;
            return;
        }

        // Нет разделителя — всё в действительной части
        _realEditor.WriteStringAsString(beforeImag);
        _hasImaginaryPart = false;
    }

    public override string ReadStringAsString() => GetResultString();
    public override void WriteStringAsString(string value) => ParseFromString(value);
    public override void ReSet() => Clear();

    public string GetRealPart() => _realEditor.ReadStringAsString();
    public string GetImaginaryPart() => _imagEditor.ReadStringAsString();
    public void SetRealPart(string value) => _realEditor.WriteStringAsString(value);
    public void SetImaginaryPart(string value) => _imagEditor.WriteStringAsString(value);
}
