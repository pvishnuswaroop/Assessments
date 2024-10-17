using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementLibrary.entity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }

        public HomeLoan(int loanId, Customer customer, double principal, double interestRate, int loanTerm, string loanType, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principal, interestRate, loanTerm, loanType, loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }
    }
}
