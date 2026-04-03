using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PTFBook
{
    public class TControl
    {
        private TAbonentList FL;

        public TControl()
        {
            FL = new TAbonentList();
        }

        public int RecordsCount() => FL.Count();

        public string ReadRecord(int i)
        {
            var abonent = FL.ReadRecord(i);
            if (abonent != null)
            {
                var rec = abonent.Read();
                return $"{rec.Name} \t тел: {rec.Number}";
            }
            return "";
        }

        public string ReadFieldName(int i) => FL.ReadRecord(i).Read().Name;
        public string ReadFieldNumber(int i) => FL.ReadRecord(i).Read().Number;

        public void AddRecord(string name, string number)
        {
            FL.AddRecord(name, number);
        }

        public void DeleteAt(int i) => FL.DeleteAt(i);

        public void ClearBook() => FL.ClearList();

        public int FindRecord(string name, string number)
        {
            return FL.FindRecord(new TAbonent(name, number));
        }

        public void SaveToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("phonebook.txt"))
                {
                    for (int i = 0; i < FL.Count(); i++)
                    {
                        var rec = FL.ReadRecord(i).Read();
                        sw.WriteLine($"{rec.Name}|{rec.Number}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists("phonebook.txt")) return;

            try
            {
                FL.ClearList();
                using (StreamReader sr = new StreamReader("phonebook.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 2)
                        {
                            FL.AddRecord(parts[0], parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        public int FindRecordUniversal(string name, string number)
        {
            for (int i = 0; i < FL.Count(); i++)
            {
                var rec = FL.ReadRecord(i).Read();

                bool nameMatch = !string.IsNullOrWhiteSpace(name) &&
                                 rec.Name.Equals(name, StringComparison.OrdinalIgnoreCase);

                bool numberMatch = !string.IsNullOrWhiteSpace(number) &&
                                   rec.Number.Equals(number, StringComparison.OrdinalIgnoreCase);

                if (nameMatch || numberMatch)
                {
                    return i;
                }
            }
            return -1;
        }

        public void UpdateRecord(int i, string newName, string newNumber)
        {
            if (i >= 0 && i < FL.Count())
            {
                TRec newData;
                newData.Name = newName;
                newData.Number = newNumber;

                FL.ReadRecord(i).Write(newData);
            }
        }
    }
}