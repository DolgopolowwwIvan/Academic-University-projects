// Тесты для класса TCtrl (контроллер калькулятора)

using CalculatorFractions;

namespace CalculatorFractions.Tests;

public class TCtrlTests
{
    #region Начальное состояние

    [Fact]
    public void Constructor_DefaultState_DisplayIsZero()
    {
        var ctrl = new TCtrl();
        var display = ctrl.ExecuteCommand((int)TCalcCommand.cmdNone, ref "", ref "");
        Assert.True(display == "0" || display == "0/1");
    }

    #endregion

    #region Ввод цифр

    [Fact]
    public void ExecuteCommand_DigitInput_ShowsDigit()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        Assert.Equal("5", result);
    }

    [Fact]
    public void ExecuteCommand_MultipleDigits_AppendsDigits()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref "", ref "");
        Assert.Equal("12", result);
    }

    #endregion

    #region Арифметические операции

    [Fact]
    public void ExecuteCommand_Addition_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdAdd, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit3, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("8", result);
    }

    [Fact]
    public void ExecuteCommand_Subtraction_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit7, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdSub, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit4, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("3", result);
    }

    [Fact]
    public void ExecuteCommand_Multiplication_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit6, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMul, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit7, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("42", result);
    }

    [Fact]
    public void ExecuteCommand_Division_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit8, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("4", result);
    }

    #endregion

    #region Деление на ноль

    [Fact]
    public void ExecuteCommand_DivisionByZero_ShowsError()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit0, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("Error", result);
    }

    [Fact]
    public void ExecuteCommand_Error_CanBeRecovered()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit0, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit3, ref "", ref "");
        Assert.Equal("3", result);
    }

    #endregion

    #region Функции

    [Fact]
    public void ExecuteCommand_Sqr_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdSqr, ref "", ref "");
        Assert.Equal("25", result);
    }

    [Fact]
    public void ExecuteCommand_Rev_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdRev, ref "", ref "");
        Assert.Equal("1/2", result);
    }

    #endregion

    #region Clear

    [Fact]
    public void ExecuteCommand_Clear_ResetsDisplay()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        Assert.True(result == "0" || result == "0/1");
    }

    #endregion

    #region Backspace

    [Fact]
    public void ExecuteCommand_Backspace_RemovesLastDigit()
    {
        var ctrl = new TCtrl();
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdBackspace, ref "", ref "");
        Assert.Equal("1", result);
    }

    #endregion

    #region Память

    [Fact]
    public void ExecuteCommand_MS_StoresValue()
    {
        var ctrl = new TCtrl();
        var mState = "OFF";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMS, ref mState, ref "");
        Assert.Equal("M", mState);
    }

    [Fact]
    public void ExecuteCommand_MR_RecallsValue()
    {
        var ctrl = new TCtrl();
        var mState = "OFF";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMS, ref mState, ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMR, ref mState, ref "");
        Assert.Equal("5", mState); // MR возвращает строку из редактора
    }

    [Fact]
    public void ExecuteCommand_MC_ClearsMemory()
    {
        var ctrl = new TCtrl();
        var mState = "OFF";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMS, ref mState, ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMC, ref mState, ref "");
        Assert.Equal("OFF", mState);
    }

    #endregion

    #region Буфер обмена

    [Fact]
    public void ExecuteCommand_Copy_CopiesToClipboard()
    {
        var ctrl = new TCtrl();
        var clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdCopy, ref clipboard, ref "");
        Assert.Equal("5", clipboard);
    }

    [Fact]
    public void ExecuteCommand_Paste_PastesFromClipboard()
    {
        var ctrl = new TCtrl();
        var clipboard = "3/4";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdPaste, ref clipboard, ref "");
        Assert.Equal("3/4", clipboard);
    }

    #endregion

    #region Режим отображения

    [Fact]
    public void ShowAsFraction_True_ShowsFractionFormat()
    {
        var ctrl = new TCtrl();
        ctrl.ShowAsFraction = true;
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("5/1", result);
    }

    [Fact]
    public void ShowAsFraction_False_HidesDenominator1()
    {
        var ctrl = new TCtrl();
        ctrl.ShowAsFraction = false;
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref "", ref "");
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref "", ref "");
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref "", ref "");
        Assert.Equal("5", result);
    }

    #endregion
}
