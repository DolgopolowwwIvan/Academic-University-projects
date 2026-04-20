// TEditor.cs - Класс "Ввод и редактирование простых дробей"
// Original: РедакторПростыхДробей

using System;

namespace CalculatorFractions
{
    /// <summary>
    /// Класс TEditor - Ввод и редактирование простых дробей
    /// Обязанность: ввод, хранение и редактирование строкового представления простых дробей
    /// </summary>
    public class TEditor
    {
        private string FString;  // Строковое представление редактируемой простой дроби

        // Константы формата
        private const string FSeparator = "/";  // Разделитель числителя и знаменателя
        private const string FZeroString = "0/1"; // Строковое представление нуля

        /// <summary>
        /// Конструктор
        /// </summary>
        public TEditor()
        {
            FString = FZeroString;
        }

        /// <summary>
        /// Проверка: можно ли добавить цифру
        /// </summary>
        private bool CanAddDigit()
        {
            if (string.IsNullOrEmpty(FString)) return true;

            int sepPos = FString.IndexOf(FSeparator);
            if (sepPos == -1)
            {
                // Ещё нет разделителя - добавляем в числитель
                return FString.Length < 10;
            }
            else
            {
                // Есть разделитель - добавляем в знаменатель
                return (FString.Length - sepPos - 1) < 10;
            }
        }

        /// <summary>
        /// Проверка: можно ли добавить разделитель
        /// </summary>
        private bool CanAddSeparator()
        {
            // Разделитель можно добавить только если есть числитель и ещё нет разделителя
            if (string.IsNullOrEmpty(FString) || FString == "-" || FString == "-0") return false;

            // Проверяем, нет ли уже разделителя
            return FString.IndexOf(FSeparator) == -1;
        }

        /// <summary>
        /// Дробь есть ноль - возвращает true, если строка содержит 0/1
        /// </summary>
        public bool IsFractionZero()
        {
            try
            {
                TFrac frac = ToFraction();
                return frac.IsZero();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Добавить знак - добавляет или удаляет знак "-" из строки
        /// </summary>
        public string AddSign()
        {
            if (string.IsNullOrEmpty(FString))
            {
                FString = "-";
            }
            else if (FString[0] == '-')
            {
                FString = FString.Substring(1);
            }
            else if (FString != "0" && !string.IsNullOrEmpty(FString) && FString.IndexOf(FSeparator) == -1)
            {
                FString = "-" + FString;
            }
            return FString;
        }

        /// <summary>
        /// Добавить цифру - добавляет цифру к строке, если формат позволяет
        /// </summary>
        public string AddDigit(int digit)
        {
            if (digit < 0 || digit > 9) return FString;

            char digitChar = (char)('0' + digit);

            // Если текущее значение "0/1" или "-0/1" - начинаем новый ввод
            if (FString == "0/1" || FString == "-0/1" || FString == "0" || FString == "-0")
            {
                if (FString.Length > 0 && FString[0] == '-')
                {
                    FString = "-" + digitChar;
                }
                else
                {
                    FString = digitChar.ToString();
                }
                return FString;
            }

            if (!CanAddDigit()) return FString;

            if (FString == "0" || FString == "-0")
            {
                if (FString.Length > 0 && FString[0] == '-')
                {
                    FString = "-" + digitChar;
                }
                else
                {
                    FString = digitChar.ToString();
                }
            }
            else
            {
                FString += digitChar;
            }

            return FString;
        }

        /// <summary>
        /// Добавить ноль - добавляет ноль к строке, если формат позволяет
        /// </summary>
        public string AddZero()
        {
            return AddDigit(0);
        }

        /// <summary>
        /// Добавить разделитель - добавляет '/' к строке
        /// </summary>
        public string AddSeparator()
        {
            if (!CanAddSeparator()) return FString;

            // Если строка пустая или только "-", добавляем 0 перед разделителем
            if (string.IsNullOrEmpty(FString) || FString == "-")
            {
                FString += "0";
            }

            FString += FSeparator;
            return FString;
        }

        /// <summary>
        /// Забой символа - удаляет крайний правый символ (Backspace)
        /// </summary>
        public string Backspace()
        {
            if (string.IsNullOrEmpty(FString)) return FString;

            // Удаляем последний символ
            FString = FString.Substring(0, FString.Length - 1);

            // Если строка стала пустой или "-", устанавливаем 0/1
            if (string.IsNullOrEmpty(FString) || FString == "-")
            {
                FString = FZeroString;
            }

            return FString;
        }

        /// <summary>
        /// Очистить - устанавливает строку в 0/1 (Clear)
        /// </summary>
        public string Clear()
        {
            FString = FZeroString;
            return FString;
        }

        /// <summary>
        /// Редактировать - выполняет команду редактирования по номеру
        /// </summary>
        public string Edit(TEditCommand command)
        {
            switch (command)
            {
                case TEditCommand.ecDigit0: return AddZero();
                case TEditCommand.ecDigit1: return AddDigit(1);
                case TEditCommand.ecDigit2: return AddDigit(2);
                case TEditCommand.ecDigit3: return AddDigit(3);
                case TEditCommand.ecDigit4: return AddDigit(4);
                case TEditCommand.ecDigit5: return AddDigit(5);
                case TEditCommand.ecDigit6: return AddDigit(6);
                case TEditCommand.ecDigit7: return AddDigit(7);
                case TEditCommand.ecDigit8: return AddDigit(8);
                case TEditCommand.ecDigit9: return AddDigit(9);
                case TEditCommand.ecSign: return AddSign();
                case TEditCommand.ecSeparator: return AddSeparator();
                case TEditCommand.ecBackspace: return Backspace();
                case TEditCommand.ecClear: return Clear();
                default: return FString;
            }
        }

        /// <summary>
        /// Свойство: читать строку в формате строки
        /// </summary>
        public string GetString() => FString;

        /// <summary>
        /// Свойство: писать строку в формате строки
        /// </summary>
        public void SetString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                FString = FZeroString;
                return;
            }

            int sepPos = value.IndexOf(FSeparator);
            if (sepPos == -1)
            {
                FString = value + FSeparator + "1";
            }
            else
            {
                FString = value;
            }

            try
            {
                ToFraction();
            }
            catch
            {
                FString = FZeroString;
            }
        }

        /// <summary>
        /// Получить дробь как TFrac
        /// </summary>
        public TFrac ToFraction()
        {
            int sepPos = FString.IndexOf(FSeparator);
            if (sepPos == -1)
            {
                return new TFrac(int.Parse(FString), 1);
            }

            string numStr = FString.Substring(0, sepPos);
            string denomStr = FString.Substring(sepPos + 1);

            if (string.IsNullOrEmpty(numStr) || numStr == "-") numStr = "0";
            if (string.IsNullOrEmpty(denomStr)) denomStr = "1";

            int num = int.Parse(numStr);
            int denom = int.Parse(denomStr);

            if (denom == 0) denom = 1;

            return new TFrac(num, denom);
        }
    }

    /// <summary>
    /// Команды редактирования
    /// </summary>
    public enum TEditCommand
    {
        ecDigit0 = 0,
        ecDigit1 = 1,
        ecDigit2 = 2,
        ecDigit3 = 3,
        ecDigit4 = 4,
        ecDigit5 = 5,
        ecDigit6 = 6,
        ecDigit7 = 7,
        ecDigit8 = 8,
        ecDigit9 = 9,
        ecSign = 10,
        ecSeparator = 11,
        ecBackspace = 12,
        ecClear = 13
    }
}
