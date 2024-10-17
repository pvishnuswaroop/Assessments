using System;
using System.Collections.Generic;
using LoanManagementLibrary.entity;
using LoanManagementLibrary.dao;
using LoanManagementLibrary.exception;

namespace LoanManagementSystem
{
    class Program
    {
        private static ILoanRepository loanRepo = new LoanRepositoryImpl();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- Loan Management System ---");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan Details");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ApplyLoan();
                        break;
                    case "2":
                        GetAllLoans();
                        break;
                    case "3":
                        GetLoanDetails();
                        break;
                    case "4":
                        LoanRepayment();
                        break;
                    case "5":
                        Console.WriteLine("Exiting the system");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 1 and 5.");
                        break;
                }
            }
        }

        static void ApplyLoan()
        {
            Console.WriteLine("\n--- Apply for a Loan ---");

            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Credit Score (CIBIL Score): ");
            int creditScore = int.Parse(Console.ReadLine());

            
            Customer existingCustomer;
            try
            {
                existingCustomer = loanRepo.GetCustomerById(customerId);
                Console.WriteLine("Customer found. Proceeding with loan application.");
            }
            catch (InvalidCustomerException)
            {
                Console.WriteLine("Customer not found. Adding new customer.");
                Customer customer = new Customer(customerId, name, email, phone, address, creditScore);
                loanRepo.AddCustomer(customer);
                existingCustomer = customer; 
            }

            
            Console.Write("Enter Principal Amount (in INR): ");
            double principalAmount = double.Parse(Console.ReadLine());

            Console.Write("Enter Interest Rate (in %): ");
            double interestRate = double.Parse(Console.ReadLine());

            Console.Write("Enter Loan Term (in months): ");
            int loanTerm = int.Parse(Console.ReadLine());

            Console.Write("Enter Loan Type (HomeLoan/CarLoan): ");
            string loanType = Console.ReadLine();

            Loan loan;

            if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Property Address: ");
                string propertyAddress = Console.ReadLine();
                Console.Write("Enter Property Value (in INR): ");
                int propertyValue = int.Parse(Console.ReadLine());
                loan = new HomeLoan(0, existingCustomer, principalAmount, interestRate, loanTerm, loanType, "Pending", propertyAddress, propertyValue);
            }
            else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Car Model: ");
                string carModel = Console.ReadLine();
                Console.Write("Enter Car Value (in INR): ");
                int carValue = int.Parse(Console.ReadLine());
                loan = new CarLoan(0, existingCustomer, principalAmount, interestRate, loanTerm, loanType, "Pending", carModel, carValue);
            }
            else
            {
                Console.WriteLine("Invalid loan type. Loan not applied.");
                return;
            }


            try
            {
                loanRepo.ApplyLoan(loan);
                Console.WriteLine("Loan application submitted successfully.");

                
                string status = loanRepo.LoanStatus(loan.LoanID);
                Console.WriteLine($"The loan status for Loan ID {loan.LoanID} is: {status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error applying for loan: " + ex.Message);
            }
        }

        static void GetAllLoans()
        {
            Console.WriteLine("\n--- Get All Loans ---");
            List<Loan> loans = loanRepo.GetAllLoans();
            if (loans.Count == 0)
            {
                Console.WriteLine("No loans available.");
                return;
            }

            foreach (var loan in loans)
            {
                Console.WriteLine($"Loan ID: {loan.LoanID}, Customer ID: {loan.Customer.CustomerID}, Principal: {loan.PrincipalAmount}, Status: {loan.LoanStatus}");
            }
        }

        static void GetLoanDetails()
        {
            Console.WriteLine("\n--- Get Loan Details ---");
            Console.Write("Enter Loan ID: ");
            if (!int.TryParse(Console.ReadLine(), out int loanId))
            {
                Console.WriteLine("Invalid Loan ID. Please enter a valid number.");
                return;
            }
            try
            {
                Loan loan = loanRepo.GetLoanById(loanId);
                Console.WriteLine($"Loan ID: {loan.LoanID}, Customer ID: {loan.Customer.CustomerID}, Principal: {loan.PrincipalAmount}, Status: {loan.LoanStatus}, Loan Type: {loan.LoanType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void LoanRepayment()
        {
            Console.WriteLine("\n--- Loan Repayment ---");
            Console.Write("Enter Loan ID: ");
            if (!int.TryParse(Console.ReadLine(), out int loanId))
            {
                Console.WriteLine("Invalid Loan ID. Please enter a valid number.");
                return;
            }

            Console.Write("Enter Amount to Pay: ");
            if (!double.TryParse(Console.ReadLine(), out double amount))
            {
                Console.WriteLine("Invalid Amount. Please enter a valid number.");
                return;
            }

            try
            {
                loanRepo.LoanRepayment(loanId, amount);
                Console.WriteLine("Loan repayment processed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
