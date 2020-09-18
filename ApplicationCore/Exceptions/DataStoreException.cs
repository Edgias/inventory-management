using System;

namespace Edgias.Inventory.Management.ApplicationCore.Exceptions
{
    public class DataStoreException : Exception
    {
        public DataStoreException(string message)
            : base(message)
        {

        }

        public DataStoreException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
