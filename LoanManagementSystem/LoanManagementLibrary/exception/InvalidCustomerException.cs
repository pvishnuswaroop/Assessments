using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace LoanManagementLibrary.exception
{
    public class InvalidCustomerException : Exception
    {
        public InvalidCustomerException()
        {
        }

        public InvalidCustomerException(string message) : base(message)
        {
        }

        public InvalidCustomerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
