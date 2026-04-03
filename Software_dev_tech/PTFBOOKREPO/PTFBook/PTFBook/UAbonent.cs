using System;

namespace PTFBook
{
    public struct TRec
    {
        public string LastName;
        public string FirstName;
        public string Phone;
        public string Address;
    }

    public class TAbonent
    {
        protected TRec FRec;

        public TAbonent()
        {
            FRec = new TRec { LastName = "", FirstName = "", Phone = "", Address = "" };
        }

        public TAbonent(string name, string number)
        {
            FRec = new TRec { LastName = "", FirstName = name, Phone = number, Address = "" };
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
            return string.Compare(this.FRec.FirstName, r.FirstName, StringComparison.OrdinalIgnoreCase) < 0;
        }

        public bool IsEqual(TRec r)
        {
            return string.Equals(this.FRec.FirstName, r.FirstName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(this.FRec.Phone, r.Phone, StringComparison.OrdinalIgnoreCase);
        }
    }
}
