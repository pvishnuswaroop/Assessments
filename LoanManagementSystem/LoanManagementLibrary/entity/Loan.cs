using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementLibrary.entity
{
    public class Loan
    {
        public int LoanID { get; set; }
        public Customer Customer { get; set; } 
        public double PrincipalAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }

        public Loan(int loanId, Customer customer, double principal, double interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanID = loanId;
            Customer = customer;
            PrincipalAmount = principal;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }
    }
}
