using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace PTFBook
{
    public class TControl
    {
        private TAbonentList FL;      // Список абонентов
        private FileStream FF;         // Файловый поток
        private string currentFileName;

        // Конструктор
        public TControl()
        {
            FL = new TAbonentList();
            FF = null;
            currentFileName = "";
        }

        // Количество записей в книге
        public int RecordsCount()
        {
            return FL.RecordCount;
        }

        // Чтение записи по индексу (возвращает строку)
        public string ReadRecord(int i)
        {
            TAbonent ab = FL.ReadRecord(i);
            if (ab != null)
            {
                TRec rec = ab.Read();
                return $"{rec.LastName} {rec.FirstName}|{rec.Phone}|{rec.Address}";
            }
            return "";
        }

        // Чтение поля Имя по индексу
        public string ReadFirstName(int i)
        {
            TAbonent ab = FL.ReadRecord(i);
            if (ab != null)
            {
                TRec rec = ab.Read();
                return rec.FirstName;
            }
            return "";
        }

        // Чтение поля Номер по индексу
        public string ReadPhone(int i)
        {
            TAbonent ab = FL.ReadRecord(i);
            if (ab != null)
            {
                TRec rec = ab.Read();
                return rec.Phone;
            }
            return "";
        }

        // Добавление записи
        public void AddRecord(string name, string number)
        {
            TRec rec = new TRec
            {
                FirstName = name,
                Phone = number,
                LastName = "",
                Address = ""
            };
            TAbonent abonent = new TAbonent();
            abonent.Write(rec);
            FL.AddRecord(abonent);
        }

        // Добавление записи с полными данными
        public void AddRecordFull(string lastName, string firstName, string phone, string address)
        {
            TRec rec = new TRec
            {
                LastName = lastName,
                FirstName = firstName,
                Phone = phone,
                Address = address
            };
            TAbonent abonent = new TAbonent();
            abonent.Write(rec);
            FL.AddRecord(abonent);
        }

        // Удаление записи по имени и номеру
        public void DeleteRecord(string name, string number)
        {
            for (int i = 0; i < FL.RecordCount; i++)
            {
                TAbonent ab = FL.ReadRecord(i);
                TRec rec = ab.Read();
                if (rec.FirstName == name && rec.Phone == number)
                {
                    FL.DeleteSelectedRecord(i);
                    break;
                }
            }
        }

        // Удаление выделенной записи по индексу
        public void DeleteSelectedRecord(int i)
        {
            FL.DeleteSelectedRecord(i);
        }

        // Очистка книги
        public void ClearBook()
        {
            FL.ClearList();
        }

        // Поиск записи по имени и номеру (возвращает индекс)
        public int FindRecord(string name, string number)
        {
            for (int i = 0; i < FL.RecordCount; i++)
            {
                TAbonent ab = FL.ReadRecord(i);
                TRec rec = ab.Read();
                if (rec.FirstName == name && rec.Phone == number)
                {
                    return i;
                }
            }
            return -1;
        }

        // Создание файла с заданным именем
        public void CreateFile(string fileName)
        {
            try
            {
                if (FF != null)
                {
                    FF.Close();
                }
                FF = new FileStream(fileName, FileMode.Create);
                currentFileName = fileName;
                FF.Close();
                FF = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка создания файла: {ex.Message}");
            }
        }

        // Сохранение книги в файл (JSON)
        public void SaveBookToFile()
        {
            if (string.IsNullOrEmpty(currentFileName))
            {
                throw new Exception("Файл не создан. Вызовите CreateFile() сначала.");
            }

            try
            {
                // Собираем все записи в список
                var records = new List<TRec>();
                for (int i = 0; i < FL.RecordCount; i++)
                {
                    TAbonent ab = FL.ReadRecord(i);
                    records.Add(ab.Read());
                }

                // Сериализуем в JSON
                string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(currentFileName, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Копирование книги из файла (загрузка) (JSON)
        public void CopyBookFromFile()
        {
            if (string.IsNullOrEmpty(currentFileName))
            {
                throw new Exception("Файл не указан.");
            }

            try
            {
                string json = File.ReadAllText(currentFileName);
                var records = JsonSerializer.Deserialize<List<TRec>>(json);

                FL.ClearList();
                if (records != null)
                {
                    foreach (var rec in records)
                    {
                        TAbonent ab = new TAbonent();
                        ab.Write(rec);
                        FL.AddRecord(ab);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки: {ex.Message}");
            }
        }

        // Загрузка из указанного файла
        public void LoadFromFile(string fileName)
        {
            currentFileName = fileName;
            CopyBookFromFile();
        }

        // Деструктор
        ~TControl()
        {
            if (FF != null)
            {
                FF.Close();
                FF = null;
            }
        }
    }
}