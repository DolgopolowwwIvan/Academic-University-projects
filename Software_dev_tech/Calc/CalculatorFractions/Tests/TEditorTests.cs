// Тесты для класса TEditor (редактор дробей)

using CalculatorFractions;

namespace CalculatorFractions.Tests;

public class TEditorTests
{
    #region Начальное состояние

    [Fact]
    public void Constructor_DefaultState_IsZeroFraction()
    {
        var editor = new TEditor();
        Assert.Equal("0/1", editor.GetString());
    }

    #endregion

    #region Ввод цифр

    [Fact]
    public void Edit_Digit_AddsDigitToString()
    {
        var editor = new TEditor();
        editor.Clear();
        var result = editor.Edit(TEditCommand.ecDigit5);
        Assert.Equal("5", result);
    }

    [Fact]
    public void Edit_MultipleDigits_AppendsDigits()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit1);
        var result = editor.Edit(TEditCommand.ecDigit2);
        Assert.Equal("12", result);
    }

    [Fact]
    public void Edit_DigitAfterSeparator_AddsToDenominator()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit1);
        editor.Edit(TEditCommand.ecSeparator);
        var result = editor.Edit(TEditCommand.ecDigit2);
        Assert.Equal("1/2", result);
    }

    #endregion

    #region Ввод разделителя

    [Fact]
    public void Edit_Separator_AddsSeparator()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit1);
        var result = editor.Edit(TEditCommand.ecSeparator);
        Assert.Equal("1/", result);
    }

    [Fact]
    public void Edit_TwoSeparators_IgnoresSecond()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit1);
        editor.Edit(TEditCommand.ecSeparator);
        editor.Edit(TEditCommand.ecDigit2);
        var result = editor.Edit(TEditCommand.ecSeparator);
        Assert.Equal("1/2", result);
    }

    #endregion

    #region Ввод знака

    [Fact]
    public void Edit_Sign_AddsMinus()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit5);
        var result = editor.Edit(TEditCommand.ecSign);
        Assert.Equal("-5", result);
    }

    [Fact]
    public void Edit_SignTwice_RemovesMinus()
    {
        var editor = new TEditor();
        editor.Clear();
        editor.Edit(TEditCommand.ecDigit5);
        editor.Edit(TEditCommand.ecSign);
        var result = editor.Edit(TEditCommand.ecSign);
        Assert.Equal("5", result);
    }

    #endregion

    #region Backspace

    [Fact]
    public void Backspace_RemovesLastChar()
    {
        var editor = new TEditor();
        editor.SetString("123");
        var result = editor.Backspace();
        Assert.Equal("12", result);
    }

    [Fact]
    public void Backspace_EmptyString_ReturnsZero()
    {
        var editor = new TEditor();
        editor.SetString("5");
        editor.Backspace();
        var result = editor.Backspace();
        Assert.Equal("0/1", result);
    }

    [Fact]
    public void Backspace_AfterSeparator_RemovesSeparator()
    {
        var editor = new TEditor();
        editor.SetString("1/");
        var result = editor.Backspace();
        Assert.Equal("1", result);
    }

    #endregion

    #region Clear

    [Fact]
    public void Clear_ResetsToZero()
    {
        var editor = new TEditor();
        editor.SetString("123/456");
        var result = editor.Clear();
        Assert.Equal("0/1", result);
    }

    #endregion

    #region ToFraction

    [Fact]
    public void ToFraction_SimpleNumber_CreatesFraction()
    {
        var editor = new TEditor();
        editor.SetString("5");
        var frac = editor.ToFraction();
        Assert.Equal(5, frac.Num);
        Assert.Equal(1, frac.Denom);
    }

    [Fact]
    public void ToFraction_FractionString_CreatesFraction()
    {
        var editor = new TEditor();
        editor.SetString("3/4");
        var frac = editor.ToFraction();
        Assert.Equal(3, frac.Num);
        Assert.Equal(4, frac.Denom);
    }

    [Fact]
    public void ToFraction_NegativeFraction_CreatesFraction()
    {
        var editor = new TEditor();
        editor.SetString("-3/4");
        var frac = editor.ToFraction();
        Assert.Equal(-3, frac.Num);
        Assert.Equal(4, frac.Denom);
    }

    [Fact]
    public void ToFraction_EmptyDenominator_UsesOne()
    {
        var editor = new TEditor();
        editor.SetString("5/");
        var frac = editor.ToFraction();
        Assert.Equal(5, frac.Num);
        Assert.Equal(1, frac.Denom);
    }

    #endregion

    #region SetString

    [Fact]
    public void SetString_ValidFraction_StoresCorrectly()
    {
        var editor = new TEditor();
        editor.SetString("3/4");
        Assert.Equal("3/4", editor.GetString());
    }

    [Fact]
    public void SetString_Integer_AddsDenominator()
    {
        var editor = new TEditor();
        editor.SetString("5");
        Assert.Equal("5/1", editor.GetString());
    }

    [Fact]
    public void SetString_EmptyString_ResetsToZero()
    {
        var editor = new TEditor();
        editor.SetString("");
        Assert.Equal("0/1", editor.GetString());
    }

    [Fact]
    public void SetString_Null_ResetsToZero()
    {
        var editor = new TEditor();
        editor.SetString(null!);
        Assert.Equal("0/1", editor.GetString());
    }

    #endregion
}
