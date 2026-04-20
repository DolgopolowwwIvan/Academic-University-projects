using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TEditor
public class TEditorTests
{
    [Fact]
    public void Constructor_Default_InitializesEmpty()
    {
        // Инициализация
        var editor = new TEditor();

        // Проверка
        Assert.Empty(editor.InputBuffer);
        Assert.Empty(editor.Error);
    }

    [Fact]
    public void InputDigit_ValidDigit_AppendsDigit()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        editor.InputDigit('5');

        // Проверка
        Assert.Equal("5", editor.InputBuffer);
        Assert.Empty(editor.Error);
    }

    [Fact]
    public void InputDigit_InvalidCharacter_SetsError()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        editor.InputDigit('a');

        // Проверка
        Assert.NotEmpty(editor.Error);
    }

    [Fact]
    public void InputDecimalPoint_EmptyBuffer_AddsZeroAndPoint()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        editor.InputDecimalPoint();

        // Проверка
        Assert.Equal("0.", editor.InputBuffer);
    }

    [Fact]
    public void InputDecimalPoint_ExistingBuffer_AppendsPoint()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('5');

        // Действие
        editor.InputDecimalPoint();

        // Проверка
        Assert.Equal("5.", editor.InputBuffer);
    }

    [Fact]
    public void InputDecimalPoint_AlreadyHasPoint_SetsError()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDecimalPoint();

        // Действие
        editor.InputDecimalPoint();

        // Проверка
        Assert.NotEmpty(editor.Error);
    }

    [Fact]
    public void ChangeSign_TogglesSign()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        editor.ChangeSign();
        Assert.True(editor.GetType().GetField("_isNegative", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(editor) is bool negative && negative);
        
        editor.ChangeSign();

        // Проверка
        Assert.False(editor.GetType().GetField("_isNegative", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(editor) is bool negative2 && negative2);
    }

    [Fact]
    public void Backspace_RemovesLastCharacter()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('1');
        editor.InputDigit('2');
        editor.InputDigit('3');

        // Действие
        editor.Backspace();

        // Проверка
        Assert.Equal("12", editor.InputBuffer);
    }

    [Fact]
    public void Backspace_EndsWithPoint_RemovesPointFlag()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('5');
        editor.InputDecimalPoint();

        // Действие
        editor.Backspace();

        // Проверка
        Assert.Equal("5", editor.InputBuffer);
        Assert.False(editor.GetType().GetField("_hasDecimalPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(editor) is bool hasPoint && hasPoint);
    }

    [Fact]
    public void Clear_ResetsAllState()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('5');
        editor.InputDecimalPoint();

        // Действие
        editor.Clear();

        // Проверка
        Assert.Empty(editor.InputBuffer);
        Assert.Empty(editor.Error);
    }

    [Fact]
    public void GetNumber_EmptyBuffer_ReturnsZero()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        var number = editor.GetNumber();

        // Проверка
        Assert.Equal(0, number.Value);
    }

    [Fact]
    public void GetNumber_WithInput_ReturnsCorrectNumber()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('4');
        editor.InputDigit('2');

        // Действие
        var number = editor.GetNumber();

        // Проверка
        Assert.Equal(42, number.Value);
    }

    [Fact]
    public void GetNumber_WithDecimal_ReturnsCorrectNumber()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('3');
        editor.InputDecimalPoint();
        editor.InputDigit('1');
        editor.InputDigit('4');

        // Действие
        var number = editor.GetNumber();

        // Проверка
        Assert.Equal(3.14, number.Value, 2);
    }

    [Fact]
    public void SetNumber_UpdatesBuffer()
    {
        // Инициализация
        var editor = new TEditor();
        var number = new TANumber(123.45);

        // Действие
        editor.SetNumber(number);

        // Проверка
        Assert.Equal("123.45", editor.InputBuffer);
    }

    [Fact]
    public void GetDisplayString_Empty_ReturnsZero()
    {
        // Инициализация
        var editor = new TEditor();

        // Действие
        var result = editor.GetDisplayString();

        // Проверка
        Assert.Equal("0", result);
    }

    [Fact]
    public void GetDisplayString_WithInput_ReturnsInput()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('7');
        editor.InputDigit('8');
        editor.InputDigit('9');

        // Действие
        var result = editor.GetDisplayString();

        // Проверка
        Assert.Equal("789", result);
    }

    [Fact]
    public void ReSet_ClearsEditor()
    {
        // Инициализация
        var editor = new TEditor();
        editor.InputDigit('5');

        // Действие
        editor.ReSet();

        // Проверка
        Assert.Empty(editor.InputBuffer);
    }
}
