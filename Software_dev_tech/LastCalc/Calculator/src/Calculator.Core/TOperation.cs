namespace Calculator.Core;

// Перечисление операций калькулятора
public enum TOperation
{
    None = 0,
    Add = 1,
    Subtract = 2,
    Multiply = 3,
    Divide = 4,
    Power = 5,
    Modulo = 6
}

// Перечисление функций калькулятора
public enum TFunction
{
    None = 0,
    Sin = 1,
    Cos = 2,
    Tan = 3,
    Sqrt = 4,
    Log = 5,
    Ln = 6,
    Abs = 7,
    Exp = 8
}
