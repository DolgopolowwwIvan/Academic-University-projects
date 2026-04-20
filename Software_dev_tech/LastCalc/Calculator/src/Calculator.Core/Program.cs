namespace Calculator.Core;

// Точка входа в приложение
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new CalculatorForm());
    }
}
