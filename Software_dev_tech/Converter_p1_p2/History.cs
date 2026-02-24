using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Converter_p1_p2
{
    /// <summary>
    /// Структура для хранения одной записи истории
    /// </summary>
    public struct Record
    {
        int p1;              // Основание исходной системы счисления
        int p2;              // Основание результирующей системы счисления
        string number1;      // Исходное число
        string number2;      // Результат преобразования

        /// <summary>
        /// Конструктор записи
        /// </summary>
        /// <param name="p1">Основание исходной системы счисления</param>
        /// <param name="p2">Основание результирующей системы счисления</param>
        /// <param name="n1">Исходное число</param>
        /// <param name="n2">Результат преобразования</param>
        public Record(int p1, int p2, string n1, string n2)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.number1 = n1;
            this.number2 = n2;
        }

        /// <summary>
        /// Преобразование записи в строковый формат
        /// </summary>
        public override string ToString()
        {
            return $"Число {number1} (с.сч. {p1}) -> {number2} (с.сч. {p2})";
        }
    }

    /// <summary>
    /// Класс для управления историей преобразований
    /// </summary>
    public class History
    {
        private List<Record> L;  // Список записей истории

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public History()
        {
            L = new List<Record>();
        }

        /// <summary>
        /// Индексатор для доступа к записи по номеру
        /// </summary>
        /// <param name="i">Индекс записи (начиная с 0)</param>
        /// <returns>Запись истории в строковом представлении</returns>
        public string this[int i]
        {
            get
            {
                if (i < 0 || i >= L.Count)
                {
                    throw new IndexOutOfRangeException("Неверный номер записи");
                }
                return L[i].ToString();
            }
        }

        /// <summary>
        /// Добавление новой записи в историю
        /// </summary>
        /// <param name="p1">Основание исходной системы счисления</param>
        /// <param name="p2">Основание результирующей системы счисления</param>
        /// <param name="n1">Исходное число</param>
        /// <param name="n2">Результат преобразования</param>
        public void AddRecord(int p1, int p2, string n1, string n2)
        {
            Record newRecord = new Record(p1, p2, n1, n2);
            L.Add(newRecord);
        }

        /// <summary>
        /// Очистка всей истории
        /// </summary>
        public void Clear()
        {
            L.Clear();
        }

        /// <summary>
        /// Получение количества записей в истории
        /// </summary>
        /// <returns>Количество записей</returns>
        public int Count()
        {
            return L.Count;
        }

        /// <summary>
        /// Получение записи по индексу в виде объекта Record
        /// </summary>
        /// <param name="index">Индекс записи</param>
        /// <returns>Объект Record</returns>
        public Record GetRecord(int index)
        {
            if (index < 0 || index >= L.Count)
            {
                throw new IndexOutOfRangeException("Неверный номер записи");
            }
            return L[index];
        }

        /// <summary>
        /// Вывод всей истории в консоль
        /// </summary>
        public void PrintAll()
        {
            if (L.Count == 0)
            {
                Console.WriteLine("История пуста");
                return;
            }

            Console.WriteLine("\n=== ИСТОРИЯ ПРЕОБРАЗОВАНИЙ ===");
            for (int i = 0; i < L.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {L[i].ToString()}");
            }
            Console.WriteLine("================================\n");
        }
    }
}

