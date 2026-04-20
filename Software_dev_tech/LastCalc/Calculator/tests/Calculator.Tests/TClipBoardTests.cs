using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TClipBoard
public class TClipBoardTests
{
    [Fact]
    public void Constructor_Default_InitializesEmpty()
    {
        // Инициализация
        var clipBoard = new TClipBoard();

        // Проверка
        Assert.Empty(clipBoard.Content);
        Assert.False(clipBoard.HasContent);
    }

    [Fact]
    public void Content_Setter_UpdatesContent()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        string testValue = "Test value";

        // Действие
        clipBoard.Content = testValue;

        // Проверка
        Assert.Equal(testValue, clipBoard.Content);
        Assert.True(clipBoard.HasContent);
    }

    [Fact]
    public void Content_SetEmptyString_HasContentIsFalse()
    {
        // Инициализация
        var clipBoard = new TClipBoard();

        // Действие
        clipBoard.Content = string.Empty;

        // Проверка
        Assert.False(clipBoard.HasContent);
    }

    [Fact]
    public void Copy_StoresValue()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        string testValue = "123.45";

        // Действие
        clipBoard.Copy(testValue);

        // Проверка
        Assert.Equal(testValue, clipBoard.Content);
        Assert.True(clipBoard.HasContent);
    }

    [Fact]
    public void Copy_NullValue_StoresEmptyString()
    {
        // Инициализация
        var clipBoard = new TClipBoard();

        // Действие
        clipBoard.Copy(null!);

        // Проверка
        Assert.Empty(clipBoard.Content);
        Assert.False(clipBoard.HasContent);
    }

    [Fact]
    public void Paste_WithContent_ReturnsContent()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        clipBoard.Copy("Test data");

        // Действие
        var result = clipBoard.Content;

        // Проверка
        Assert.Equal("Test data", result);
    }

    [Fact]
    public void Paste_Empty_ReturnsEmptyString()
    {
        // Инициализация
        var clipBoard = new TClipBoard();

        // Действие
        var result = clipBoard.Content;

        // Проверка
        Assert.Empty(result);
    }

    [Fact]
    public void PasteValue_ValidNumber_ReturnsNumber()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        clipBoard.Copy("123.45");

        // Действие
        var result = clipBoard.PasteValue();

        // Проверка
        Assert.Equal(123.45, result, 2);
    }

    [Fact]
    public void PasteValue_InvalidContent_ReturnsZero()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        clipBoard.Copy("not a number");

        // Действие
        var result = clipBoard.PasteValue();

        // Проверка
        Assert.Equal(0, result);
    }

    [Fact]
    public void Clear_EmptiesBuffer()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        clipBoard.Copy("Some data");

        // Действие
        clipBoard.Clear();

        // Проверка
        Assert.Empty(clipBoard.Content);
        Assert.False(clipBoard.HasContent);
    }

    [Fact]
    public void ReSet_ClearsBuffer()
    {
        // Инициализация
        var clipBoard = new TClipBoard();
        clipBoard.Copy("Some data");

        // Действие
        clipBoard.ReSet();

        // Проверка
        Assert.Empty(clipBoard.Content);
        Assert.False(clipBoard.HasContent);
    }
}
