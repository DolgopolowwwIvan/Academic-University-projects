// Тесты для класса TCtrl (контроллер калькулятора)
using CalculatorFractions;

namespace Tests;

public class TCtrlTests
{
    [Fact]
    public void Constructor_DefaultState_DisplayIsZero()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        var display = ctrl.ExecuteCommand((int)TCalcCommand.cmdNone, ref mState, ref clipboard);
        Assert.True(display == "0" || display == "0/1");
    }

    [Fact]
    public void ExecuteCommand_DigitInput_ShowsDigit()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        Assert.Equal("5", result);
    }

    [Fact]
    public void ExecuteCommand_MultipleDigits_AppendsDigits()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref mState, ref clipboard);
        Assert.Equal("12", result);
    }

    [Fact]
    public void ExecuteCommand_Addition_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdAdd, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit3, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("8", result);
    }

    [Fact]
    public void ExecuteCommand_Subtraction_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit7, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdSub, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit4, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("3", result);
    }

    [Fact]
    public void ExecuteCommand_Multiplication_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit6, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMul, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit7, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("42", result);
    }

    [Fact]
    public void ExecuteCommand_Division_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit8, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("4", result);
    }

    [Fact]
    public void ExecuteCommand_DivisionByZero_ShowsError()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit0, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("Error", result);
    }

    [Fact]
    public void ExecuteCommand_Error_CanBeRecovered()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit0, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit3, ref mState, ref clipboard);
        Assert.Equal("3", result);
    }

    [Fact]
    public void ExecuteCommand_Sqr_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdSqr, ref mState, ref clipboard);
        Assert.Equal("25", result);
    }

    [Fact]
    public void ExecuteCommand_Rev_CalculatesCorrectly()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdRev, ref mState, ref clipboard);
        Assert.Equal("1/2", result);
    }

    [Fact]
    public void ExecuteCommand_Clear_ResetsDisplay()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        Assert.True(result == "0" || result == "0/1");
    }

    [Fact]
    public void ExecuteCommand_Backspace_RemovesLastDigit()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit2, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdBackspace, ref mState, ref clipboard);
        Assert.Equal("1", result);
    }

    [Fact]
    public void ExecuteCommand_MS_StoresValue()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMS, ref mState, ref clipboard);
        Assert.Equal("M", mState);
    }

    [Fact]
    public void ExecuteCommand_MC_ClearsMemory()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMS, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdMC, ref mState, ref clipboard);
        Assert.Equal("OFF", mState);
    }

    [Fact]
    public void ExecuteCommand_Copy_CopiesToClipboard()
    {
        var ctrl = new TCtrl();
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdCopy, ref clipboard, ref mState);
        Assert.Equal("5", clipboard);
    }

    [Fact]
    public void ShowAsFraction_True_ShowsFractionFormat()
    {
        var ctrl = new TCtrl();
        ctrl.ShowAsFraction = true;
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("5/1", result);
    }

    [Fact]
    public void ShowAsFraction_False_HidesDenominator1()
    {
        var ctrl = new TCtrl();
        ctrl.ShowAsFraction = false;
        string mState = "";
        string clipboard = "";
        ctrl.ExecuteCommand((int)TCalcCommand.cmdClear, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit5, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDiv, ref mState, ref clipboard);
        ctrl.ExecuteCommand((int)TCalcCommand.cmdDigit1, ref mState, ref clipboard);
        var result = ctrl.ExecuteCommand((int)TCalcCommand.cmdEqual, ref mState, ref clipboard);
        Assert.Equal("5", result);
    }
}
