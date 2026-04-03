using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PTFBook
{
    public class TControl
    {
        private TAbonentList FL;
        private string currentFileName = "phonebook.txt";

        public TControl()
        {
            FL = new TAbonentList();
        }

        public int RecordsCount()
        {
            return FL.Count();
        }

        public string ReadRecord(int i)
        {
            var abonent = FL.ReadRecord(i);
            if (abonent != null)
            {
                var rec = abonent.Read();
                return $"{rec.LastName} {rec.FirstName}|{rec.Phone}|{rec.Address}";
            }
            return "";
        }

        public string ReadFirstName(int i)
        {
            var ab = FL.ReadRecord(i);
            return ab?.Read().FirstName ?? "";
        }

        public string ReadPhone(int i)
        {
            var ab = FL.ReadRecord(i);
            return ab?.Read().Phone ?? "";
        }

        public void AddRecord(string name, string number)
        {
            TRec rec = new TRec
            {
                LastName = "",
                FirstName = name,
                Phone = number,
                Address = ""
            };
            TAbonent abonent = new TAbonent();
            abonent.Write(rec);
            FL.AddRecord(abonent);
        }

        public void DeleteSelectedRecord(int i)
        {
            FL.DeleteAt(i);
        }

        public void ClearBook()
        {
            FL.ClearList();
        }

        public int FindRecord(string name, string number)
        {
            return FL.FindRecord(new TAbonent(name, number));
        }

        public int FindRecordUniversal(string name, string number)
        {
            for (int i = 0; i < FL.Count(); i++)
            {
                var rec = FL.ReadRecord(i).Read();

                bool nameMatch = !string.IsNullOrWhiteSpace(name) &&
                                 rec.FirstName != null &&
                                 rec.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;

                bool numberMatch = !string.IsNullOrWhiteSpace(number) &&
                                   rec.Phone != null &&
                                   rec.Phone.IndexOf(number, StringComparison.OrdinalIgnoreCase) >= 0;

                if (nameMatch || numberMatch)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SaveToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(currentFileName))
                {
                    for (int i = 0; i < FL.Count(); i++)
                    {
                        var rec = FL.ReadRecord(i).Read();
                        sw.WriteLine($"{rec.FirstName}|{rec.Phone}");
                    }
                }
                MessageBox.Show("Телефонная книга сохранена", "Сохранение", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists(currentFileName)) return;

            try
            {
                FL.ClearList();
                using (StreamReader sr = new StreamReader(currentFileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            FL.AddRecord(parts[0], parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message, "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Загрузка из указанного файла
        public void LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName)) 
            {
                throw new FileNotFoundException("Файл не найден: " + fileName);
            }

            try
            {
                FL.ClearList();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            FL.AddRecord(parts[0], parts[1]);
                        }
                    }
                }
                currentFileName = fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке: {ex.Message}");
            }
        }

        public void UpdateRecord(int i, string newName, string newNumber)
        {
            if (i >= 0 && i < FL.Count())
            {
                TRec newData;
                newData.LastName = "";
                newData.FirstName = newName;
                newData.Phone = newNumber;
                newData.Address = "";

                FL.ReadRecord(i).Write(newData);
            }
        }
    }
}