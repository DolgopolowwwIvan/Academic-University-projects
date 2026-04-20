namespace Calculator.Core;

// Буфер обмена калькулятора
#nullable disable
public class TClipBoard
{
    private string _content;
    private bool _hasContent;

    public TClipBoard()
    {
        _content = string.Empty;
        _hasContent = false;
    }

    // Содержимое буфера
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            _hasContent = !string.IsNullOrEmpty(value);
        }
    }

    // Есть содержимое
    public bool HasContent => _hasContent;

    // Копировать значение в буфер
    public void Copy(string value)
    {
        _content = value ?? string.Empty;
        _hasContent = !string.IsNullOrEmpty(_content);
    }

    // Вставить значение из буфера и конвертировать в число
    public double PasteValue()
    {
        if (!_hasContent)
        {
            return 0;
        }

        if (double.TryParse(_content, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value))
        {
            return value;
        }

        return 0;
    }

    // Очистить буфер
    public void Clear()
    {
        _content = string.Empty;
        _hasContent = false;
    }

    // Сбросить состояние
    public void ReSet()
    {
        Clear();
    }
}
#nullable enable
