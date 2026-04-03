using System;
using System.Windows.Forms;

namespace PTFBook
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Глобальный обработчик исключений
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show($"Произошла ошибка: {e.Exception.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            Application.Run(new TPanel());
        }
    }
}