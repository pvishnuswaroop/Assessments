using LoanManagementLibrary.entity;
using System.Collections.Generic;

namespace LoanManagementLibrary.dao
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        void AddCustomer(Customer customer);
        double CalculateInterest(int loanId);
        string LoanStatus(int loanId);
        double CalculateEMI(int loanId);
        void AddHomeLoan(HomeLoan homeLoan);
        void AddCarLoan(CarLoan carLoan);
        void LoanRepayment(int loanId, double amount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
        Customer GetCustomerById(int customerId);
    }
}
