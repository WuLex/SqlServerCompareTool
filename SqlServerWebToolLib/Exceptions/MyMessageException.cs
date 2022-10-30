using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlServerWebToolLib.Exceptions
{
    [Serializable]
    public class MyMessageException : Exception
    {
        // Methods
        public MyMessageException(string message)
            : base(message)
        {
        }
    }
}