using System;

namespace PTFBook
{
    public struct TRec
    {
        public string Name;
        public string Number;
    }

    public class TAbonent
    {
        protected TRec FRec;

        public TAbonent()
        {
            FRec = new TRec { Name = "", Number = "" };
        }

        public TAbonent(string name, string number)
        {
            FRec = new TRec { Name = name, Number = number };
        }

        public TRec Read()
        {
            return FRec;
        }

        public void Write(TRec r)
        {
            FRec = r;
        }

        public bool IsLess(TRec r)
        {
            return string.Compare(this.FRec.Name, r.Name, StringComparison.OrdinalIgnoreCase) < 0;
        }

        public bool IsEqual(TRec r)
        {
            return string.Equals(this.FRec.Name, r.Name, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(this.FRec.Number, r.Number, StringComparison.OrdinalIgnoreCase);
        }
    }
}