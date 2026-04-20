// Program.cs - Точка входа приложения
// Калькулятор простых дробей на C# Windows Forms

using System;
using System.Windows.Forms;

namespace CalculatorFractions
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
