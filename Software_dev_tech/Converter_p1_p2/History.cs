using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Converter_p1_p2
{

    public struct Record
    {
        int p1;              // Основание исходной системы счисления
        int p2;              // Основание результирующей системы счисления
        string number1;      // Исходное число
        string number2;      // Результат преобразования

        public Record(int p1, int p2, string n1, string n2)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.number1 = n1;
            this.number2 = n2;
        }

        public override string ToString()
        {
            return $"Число {number1} (с.сч. {p1}) -> {number2} (с.сч. {p2})";
        }
    }

    public class History
    {
        private List<Record> L;  // Список записей истории
        public History()
        {
            L = new List<Record>();
        }

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

        public void AddRecord(int p1, int p2, string n1, string n2)
        {
            Record newRecord = new Record(p1, p2, n1, n2);
            L.Add(newRecord);
        }

        public void Clear()
        {
            L.Clear();
        }

        /// Получение количества записей в истории
        public int Count()
        {
            return L.Count;
        }

        // Получение записи по индексу в виде объекта Record
        public Record GetRecord(int index)
        {
            if (index < 0 || index >= L.Count)
            {
                throw new IndexOutOfRangeException("Неверный номер записи");
            }
            return L[index];
        }

        // Вывод всей истории в консоль
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

