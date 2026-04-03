using System;
using System.Collections.Generic;

namespace PTFBook.Tests
{
    // Структура для хранения записи об абоненте
    public struct TRec
    {
        public string fio;
        public string phone;
    }

    // Класс Абонент
    public class TAbonent
    {
        private TRec record;

        // Конструктор (инициализирует пустую запись)
        public TAbonent()
        {
            record.fio = "";
            record.phone = "";
        }

        // Занесение записи в объект
        public void Write(TRec r)
        {
            record.fio = r.fio.Length > 99 ? r.fio.Substring(0, 99) : r.fio;
            record.phone = r.phone.Length > 19 ? r.phone.Substring(0, 19) : r.phone;
        }

        // Чтение записи из объекта
        public TRec Read()
        {
            return record;
        }

        // Сравнение записей по отношению "меньше"
        public int Less(TRec a)
        {
            int cmp = string.Compare(record.phone, a.phone);
            if (cmp < 0) return -1;
            if (cmp == 0) return 0;
            return 1;
        }

        // Сравнение записей по отношению "равно"
        public bool Equal(TRec a)
        {
            return record.phone == a.phone && record.fio == a.fio;
        }

        // Перегрузка для сравнения объектов TAbonent
        public bool Equal(TAbonent other)
        {
            TRec otherRec = other.Read();
            return record.phone == otherRec.phone && record.fio == otherRec.fio;
        }
    }

    // Класс TAbonentList
    public class TAbonentList
    {
        private List<TAbonent> FList;

        // Конструктор
        public TAbonentList()
        {
            FList = new List<TAbonent>();
        }

        // Чтение записи по индексу
        public TAbonent ReadRecord(int i)
        {
            if (i >= 0 && i < FList.Count)
                return FList[i];
            return null;
        }

        // Количество записей
        public int RecordCount
        {
            get { return FList.Count; }
        }

        // Добавление записи
        public void AddRecord(TAbonent r)
        {
            FList.Add(r);
        }

        // Поиск записи (возвращает индекс)
        public int FindRecord(TAbonent r)
        {
            for (int i = 0; i < FList.Count; i++)
            {
                if (FList[i].Equal(r))
                    return i;
            }
            return -1;
        }

        // Удаление записи по значению
        public void DeleteRecord(TAbonent r)
        {
            int index = FindRecord(r);
            if (index != -1)
                FList.RemoveAt(index);
        }

        // Удаление записи по индексу
        public void DeleteSelectedRecord(int i)
        {
            if (i >= 0 && i < FList.Count)
                FList.RemoveAt(i);
        }

        // Очистка списка
        public void ClearList()
        {
            FList.Clear();
        }

        // Деструктор
        ~TAbonentList()
        {
            // Финализатор
        }
    }

    // Функция для вывода записи на экран
    public static class Program
    {
        private static void PrintRecord(TRec r)
        {
            Console.WriteLine("  ФИО: " + r.fio);
            Console.WriteLine("  номер: " + r.phone);
        }

        private static void PrintAbonent(TAbonent ab)
        {
            TRec rec = ab.Read();
            PrintRecord(rec);
        }

        public static void Main()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 1: Создание пустого конструктора TAbonentList");
            Console.WriteLine("========================================");
            TAbonentList list1 = new TAbonentList();
            Console.WriteLine("Список создан. Количество записей: " + list1.RecordCount);
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 2: Добавление записей в список");
            Console.WriteLine("========================================");
            TAbonentList list2 = new TAbonentList();

            TRec rec1 = new TRec();
            rec1.fio = "Иванов Иван";
            rec1.phone = "+7(999)123-45-67";
            TAbonent ab1 = new TAbonent();
            ab1.Write(rec1);
            list2.AddRecord(ab1);

            TRec rec2 = new TRec();
            rec2.fio = "Петров Петр";
            rec2.phone = "+7(999)987-65-43";
            TAbonent ab2 = new TAbonent();
            ab2.Write(rec2);
            list2.AddRecord(ab2);

            TRec rec3 = new TRec();
            rec3.fio = "Сидоров Сидор";
            rec3.phone = "+7(999)555-55-55";
            TAbonent ab3 = new TAbonent();
            ab3.Write(rec3);
            list2.AddRecord(ab3);

            Console.WriteLine("Добавлено 3 записи. Количество записей: " + list2.RecordCount);
            Console.WriteLine("Содержимое списка:");
            for (int i = 0; i < list2.RecordCount; i++)
            {
                Console.WriteLine("Запись " + (i + 1) + ":");
                TAbonent ab = list2.ReadRecord(i);
                if (ab != null)
                {
                    PrintAbonent(ab);
                }
            }
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 3: Чтение записи по индексу (ReadRecord)");
            Console.WriteLine("========================================");
            TAbonent readAb = list2.ReadRecord(1);
            if (readAb != null)
            {
                Console.WriteLine("Запись с индексом 1:");
                PrintAbonent(readAb);
            }
            else
            {
                Console.WriteLine("Запись не найдена");
            }

            TAbonent invalidAb = list2.ReadRecord(10);
            if (invalidAb == null)
            {
                Console.WriteLine("Попытка чтения с неверным индексом (10): вернуло null");
            }
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 4: Поиск записи (FindRecord)");
            Console.WriteLine("========================================");

            TRec searchRec = new TRec();
            searchRec.fio = "Петров Петр";
            searchRec.phone = "+7(999)987-65-43";
            TAbonent searchAb = new TAbonent();
            searchAb.Write(searchRec);

            int foundIndex = list2.FindRecord(searchAb);
            Console.WriteLine("Поиск существующей записи (Петров Петр): индекс = " + foundIndex);

            TRec notFoundRec = new TRec();
            notFoundRec.fio = "Неизвестный";
            notFoundRec.phone = "+7(999)000-00-00";
            TAbonent notFoundAb = new TAbonent();
            notFoundAb.Write(notFoundRec);

            int notFoundIndex = list2.FindRecord(notFoundAb);
            Console.WriteLine("Поиск несуществующей записи: индекс = " + notFoundIndex + " (-1 означает не найдено)");
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 5: Удаление записи по значению (DeleteRecord)");
            Console.WriteLine("========================================");
            TAbonentList list3 = new TAbonentList();
            TRec testRec1 = new TRec();
            testRec1.fio = "Абонент 1";
            testRec1.phone = "111";
            TRec testRec3 = new TRec();
            testRec3.fio = "Абонент 3";
            testRec3.phone = "333";

            TAbonent t1 = new TAbonent();
            t1.Write(testRec1);
            TAbonent t2 = new TAbonent();
            t2.Write(rec2);
            TAbonent t3 = new TAbonent();
            t3.Write(testRec3);

            list3.AddRecord(t1);
            list3.AddRecord(t2);
            list3.AddRecord(t3);

            Console.WriteLine("До удаления. Количество записей: " + list3.RecordCount);

            TRec deleteRec = new TRec();
            deleteRec.fio = "Петров Петр";
            deleteRec.phone = "+7(999)987-65-43";
            TAbonent deleteAb = new TAbonent();
            deleteAb.Write(deleteRec);

            list3.DeleteRecord(deleteAb);
            Console.WriteLine("После удаления записи Петров Петр. Количество записей: " + list3.RecordCount);
            Console.WriteLine("Оставшиеся записи:");
            for (int i = 0; i < list3.RecordCount; i++)
            {
                TAbonent ab = list3.ReadRecord(i);
                if (ab != null)
                {
                    TRec rec = ab.Read();
                    Console.WriteLine("  Запись " + i + ": " + rec.fio + " - " + rec.phone);
                }
            }
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 6: Удаление записи по индексу (DeleteSelectedRecord)");
            Console.WriteLine("========================================");
            TAbonentList list4 = new TAbonentList();
            TRec delIdxRec1 = new TRec();
            delIdxRec1.fio = "Первый";
            delIdxRec1.phone = "111";
            TRec delIdxRec2 = new TRec();
            delIdxRec2.fio = "Второй";
            delIdxRec2.phone = "222";
            TRec delIdxRec3 = new TRec();
            delIdxRec3.fio = "Третий";
            delIdxRec3.phone = "333";

            TAbonent idxAb1 = new TAbonent();
            idxAb1.Write(delIdxRec1);
            TAbonent idxAb2 = new TAbonent();
            idxAb2.Write(delIdxRec2);
            TAbonent idxAb3 = new TAbonent();
            idxAb3.Write(delIdxRec3);

            list4.AddRecord(idxAb1);
            list4.AddRecord(idxAb2);
            list4.AddRecord(idxAb3);

            Console.WriteLine("До удаления. Количество записей: " + list4.RecordCount);
            list4.DeleteSelectedRecord(1);
            Console.WriteLine("После удаления индекса 1. Количество записей: " + list4.RecordCount);
            Console.WriteLine("Оставшиеся записи:");
            for (int i = 0; i < list4.RecordCount; i++)
            {
                TAbonent ab = list4.ReadRecord(i);
                if (ab != null)
                {
                    TRec rec = ab.Read();
                    Console.WriteLine("  " + rec.fio + " - " + rec.phone);
                }
            }
            Console.WriteLine();

            // -------------------------------------------------------------
            Console.WriteLine("========================================");
            Console.WriteLine("ТЕСТ 7: Очистка списка (ClearList)");
            Console.WriteLine("========================================");
            TAbonentList list5 = new TAbonentList();
            TRec clearRec = new TRec();
            clearRec.fio = "Тестовый";
            clearRec.phone = "000";
            TAbonent clearAb = new TAbonent();
            clearAb.Write(clearRec);
            list5.AddRecord(clearAb);
            list5.AddRecord(clearAb);

            Console.WriteLine("До очистки. Количество записей: " + list5.RecordCount);
            list5.ClearList();
            Console.WriteLine("После очистки. Количество записей: " + list5.RecordCount);
            Console.WriteLine();

            Console.WriteLine("========================================");
            Console.WriteLine("ВСЕ ТЕСТЫ ЗАВЕРШЕНЫ УСПЕШНО");
            Console.WriteLine("========================================");
        }
    }
}