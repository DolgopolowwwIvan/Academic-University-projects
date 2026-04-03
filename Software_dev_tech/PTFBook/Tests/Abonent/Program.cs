using System;

namespace PTFBook
{
    // Структура записи абонента
    public struct TRec
    {
        public string LastName;   // Фамилия
        public string FirstName;  // Имя
        public string Phone;      // Телефон
        public string Address;    // Адрес
    }

    // Класс Абонент
    public class TAbonent
    {
        private TRec FRec;

        // Конструктор
        public TAbonent()
        {
            FRec = new TRec();
        }

        // Запись
        public void Write(TRec r)
        {
            FRec = r;
        }

        // Чтение
        public TRec Read()
        {
            return FRec;
        }

        // Сравнение по фамилии
        public int Less(TAbonent other)
        {
            return string.Compare(this.FRec.LastName, other.FRec.LastName);
        }

        public bool Equal(TAbonent other)
        {
            return this.FRec.LastName == other.FRec.LastName &&
                   this.FRec.FirstName == other.FRec.FirstName &&
                   this.FRec.Phone == other.FRec.Phone &&
                   this.FRec.Address == other.FRec.Address;
        }

        // Деструктор
        ~TAbonent()
        {
            
        }
    }
}

namespace TestAbonent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ КЛАССА TAbonent ===\n");

            // ТЕСТ 1: Создание пустого конструктора
            TestConstructor();

            // ТЕСТ 2: Запись и чтение записи
            TestWriteAndRead();

            // ТЕСТ 3: Сравнение записей (Equal)
            TestEqual();

            Console.WriteLine("\n=== ТЕСТЫ ЗАВЕРШЕНЫ ===");
            Console.ReadKey();
        }

        static void TestConstructor()
        {
            Console.WriteLine("ТЕСТ 1: Создание пустого конструктора");
            Console.WriteLine("--------------------------------------");

            PTFBook.TAbonent abonent = new PTFBook.TAbonent();
            PTFBook.TRec rec = abonent.Read();

            if (rec.LastName == null && rec.FirstName == null &&
                rec.Phone == null && rec.Address == null)
            {
                Console.WriteLine("Результат: Конструктор создает пустой объект");
                Console.WriteLine("Тест пройден");
            }
            else
            {
                Console.WriteLine("Результат: Конструктор не инициализирует поля");
                Console.WriteLine("Тест не пройден");
            }

            Console.WriteLine();
        }

        static void TestWriteAndRead()
        {
            Console.WriteLine("ТЕСТ 2: Запись и чтение записи");
            Console.WriteLine("------------------------------");

            PTFBook.TAbonent abonent = new PTFBook.TAbonent();
            PTFBook.TRec originalRec = new PTFBook.TRec
            {
                LastName = "Иванов",
                FirstName = "Иван",
                Phone = "+7(999)123-45-67",
                Address = ""
            };

            abonent.Write(originalRec);
            PTFBook.TRec readRec = abonent.Read();

            Console.WriteLine($"  ФИО: {readRec.LastName} {readRec.FirstName}");
            Console.WriteLine($"  номер: {readRec.Phone}");

            if (readRec.LastName == originalRec.LastName &&
                readRec.FirstName == originalRec.FirstName &&
                readRec.Phone == originalRec.Phone)
            {
                Console.WriteLine("Тест пройден");
            }
            else
            {
                Console.WriteLine("Тест не пройден");
            }

            Console.WriteLine();
        }

        static void TestEqual()
        {
            Console.WriteLine("ТЕСТ 3: Сравнение записей (Equal)");
            Console.WriteLine("----------------------------------");

            PTFBook.TAbonent abonent1 = CreateAbonent("Петров", "Петр", "333-33-33", "");
            PTFBook.TAbonent abonent2 = CreateAbonent("Петров", "Петр", "333-33-33", "");
            PTFBook.TAbonent abonent3 = CreateAbonent("Петров", "Петр", "444-44-44", "");

            bool result1 = abonent1.Equal(abonent2);
            Console.WriteLine($"Сравнение одинаковых записей: {result1}");
            Console.WriteLine(result1 ? "Тест пройден" : "Тест не пройден");

            bool result2 = abonent1.Equal(abonent3);
            Console.WriteLine($"Сравнение записей с разными номерами: {result2}");
            Console.WriteLine(!result2 ? "Тест пройден" : "Тест не пройден");

            bool result3 = abonent1.Equal(abonent1);
            Console.WriteLine($"Сравнение записи с самой собой: {result3}");
            Console.WriteLine(result3 ? "Тест пройден" : "Тест не пройден");

            Console.WriteLine();
        }

        static PTFBook.TAbonent CreateAbonent(string lastName, string firstName, string phone, string address)
        {
            PTFBook.TAbonent abonent = new PTFBook.TAbonent();
            PTFBook.TRec rec = new PTFBook.TRec
            {
                LastName = lastName,
                FirstName = firstName,
                Phone = phone,
                Address = address
            };
            abonent.Write(rec);
            return abonent;
        }
    }
}
