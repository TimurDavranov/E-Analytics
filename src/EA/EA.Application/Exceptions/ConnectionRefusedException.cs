using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Application.Exceptions
{
    public class ConnectionRefusedException : Exception
    {
        public ConnectionRefusedException(string message) : base(message)
        {
        }
    }
}