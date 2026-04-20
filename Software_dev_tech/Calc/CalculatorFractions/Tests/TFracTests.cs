// Тесты для класса TFrac (простая дробь)
using CalculatorFractions;

namespace Tests;

public class TFracTests
{
    #region Конструкторы

    [Fact]
    public void Constructor_Default_CreatesZeroFraction()
    {
        var frac = new TFrac();
        Assert.Equal(0, frac.Numerator);
        Assert.Equal(1, frac.Denominator);
    }

    [Fact]
    public void Constructor_WithValues_CreatesCorrectFraction()
    {
        var frac = new TFrac(3, 4);
        Assert.Equal(3, frac.Numerator);
        Assert.Equal(4, frac.Denominator);
    }

    [Fact]
    public void Constructor_ZeroNumerator_CreatesZeroFraction()
    {
        var frac = new TFrac(0, 5);
        Assert.Equal(0, frac.Numerator);
        Assert.Equal(1, frac.Denominator);
    }

    [Fact]
    public void Constructor_NegativeDenominator_MovesSignToNumerator()
    {
        var frac = new TFrac(3, -4);
        Assert.Equal(-3, frac.Numerator);
        Assert.Equal(4, frac.Denominator);
    }

    [Fact]
    public void Constructor_BothNegative_MakesPositive()
    {
        var frac = new TFrac(-3, -4);
        Assert.Equal(3, frac.Numerator);
        Assert.Equal(4, frac.Denominator);
    }

    [Fact]
    public void Constructor_ReducesFraction()
    {
        var frac = new TFrac(6, 8);
        Assert.Equal(3, frac.Numerator);
        Assert.Equal(4, frac.Denominator);
    }

    #endregion

    #region Арифметические операции

    [Fact]
    public void Add_TwoFractions_ReturnsCorrectSum()
    {
        var a = new TFrac(1, 2);
        var b = new TFrac(1, 3);
        var result = a.Add(b);
        Assert.Equal(5, result.Numerator);
        Assert.Equal(6, result.Denominator);
    }

    [Fact]
    public void Sub_TwoFractions_ReturnsCorrectDifference()
    {
        var a = new TFrac(3, 4);
        var b = new TFrac(1, 4);
        var result = a.Sub(b);
        Assert.Equal(1, result.Numerator);
        Assert.Equal(2, result.Denominator);
    }

    [Fact]
    public void Mul_TwoFractions_ReturnsCorrectProduct()
    {
        var a = new TFrac(1, 2);
        var b = new TFrac(2, 3);
        var result = a.Mul(b);
        Assert.Equal(1, result.Numerator);
        Assert.Equal(3, result.Denominator);
    }

    [Fact]
    public void Div_TwoFractions_ReturnsCorrectQuotient()
    {
        var a = new TFrac(3, 4);
        var b = new TFrac(1, 2);
        var result = a.Div(b);
        Assert.Equal(3, result.Numerator);
        Assert.Equal(2, result.Denominator);
    }

    [Fact]
    public void Div_ByZero_ThrowsException()
    {
        var a = new TFrac(3, 4);
        var b = new TFrac(0, 1);
        Assert.Throws<DivideByZeroException>(() => a.Div(b));
    }

    #endregion

    #region Функции

    [Fact]
    public void Sqr_Fraction_ReturnsCorrectSquare()
    {
        var frac = new TFrac(2, 3);
        var result = frac.Sqr();
        Assert.Equal(4, result.Numerator);
        Assert.Equal(9, result.Denominator);
    }

    [Fact]
    public void Rev_Fraction_ReturnsCorrectReciprocal()
    {
        var frac = new TFrac(3, 4);
        var result = frac.Rev();
        Assert.Equal(4, result.Numerator);
        Assert.Equal(3, result.Denominator);
    }

    [Fact]
    public void Rev_Zero_ThrowsException()
    {
        var frac = new TFrac(0, 1);
        Assert.Throws<DivideByZeroException>(() => frac.Rev());
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_ShowAsFractionTrue_ShowsFractionFormat()
    {
        var frac = new TFrac(5, 1) { ShowAsFraction = true };
        Assert.Equal("5/1", frac.ToString());
    }

    [Fact]
    public void ToString_ShowAsFractionFalse_HidesDenominator1()
    {
        var frac = new TFrac(5, 1) { ShowAsFraction = false };
        Assert.Equal("5", frac.ToString());
    }

    [Fact]
    public void ToString_Zero_ShowAsFractionTrue()
    {
        var frac = new TFrac(0, 1) { ShowAsFraction = true };
        Assert.Equal("0/1", frac.ToString());
    }

    [Fact]
    public void ToString_Zero_ShowAsFractionFalse()
    {
        var frac = new TFrac(0, 1) { ShowAsFraction = false };
        Assert.Equal("0", frac.ToString());
    }

    #endregion
}
