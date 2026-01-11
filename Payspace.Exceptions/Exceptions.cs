using System;

namespace Payspace.Exceptions
{

    //I'll just put all these classes into the same file.
    public class InvalidInputAmount : Exception
    {
        public InvalidInputAmount(string message) : base(message)
        {

        }
    }
    public class InvalidTaxTable : Exception
    {
        public InvalidTaxTable(string message) : base(message)
        {

        }
    }
    public class UnsupportedTaxTaxRegime : Exception
    {
        public UnsupportedTaxTaxRegime(string message) : base(message)
        {

        }
    }
}
