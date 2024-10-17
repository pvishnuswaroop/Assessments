using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using LoanManagementLibrary.entity;
using LoanManagementLibrary.exception;
using LoanManagementLibrary.util;

namespace LoanManagementLibrary.dao
{

    public class LoanRepositoryImpl : ILoanRepository
    {

        public void AddCustomer(Customer customer) 
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                
                string query = "INSERT INTO Customer (Name, EmailAddress, PhoneNumber, Address, CreditScore) VALUES (@Name, @EmailAddress, @PhoneNumber, @Address, @CreditScore)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@EmailAddress", customer.EmailAddress);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@CreditScore", customer.CreditScore);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery(); 
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Customer added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add customer.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error adding customer: " + ex.Message);
                    }
                }
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            return new Customer
                            {
                                CustomerID = (int)reader["CustomerID"],
                                Name = reader["Name"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                CreditScore = (int)reader["CreditScore"]
                            };
                        }
                    }
                }
            }

            
            throw new InvalidCustomerException("Customer not found.");
        }



        public void ApplyLoan(Loan loan)
        {
            loan.LoanStatus = "Pending"; 

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                
                string query = "INSERT INTO Loan (CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) OUTPUT INSERTED.LoanID VALUES (@CustomerID, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", loan.Customer.CustomerID);
                    command.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                    command.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                    command.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                    command.Parameters.AddWithValue("@LoanType", loan.LoanType);
                    command.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);

                    
                    int insertedLoanId = (int)command.ExecuteScalar();
                    loan.LoanID = insertedLoanId; 
                    Console.WriteLine("Loan application submitted successfully.");
                }
            }

            
            string status = LoanStatus(loan.LoanID);
            Console.WriteLine($"The loan status for Loan ID {loan.LoanID} is: {status}");
        }

        public double CalculateInterest(int loanId)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Loan WHERE LoanID = @LoanID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", loanId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principalAmount = Convert.ToDouble(reader["PrincipalAmount"]);
                            double interestRate = Convert.ToDouble(reader["InterestRate"]);
                            int loanTerm = (int)reader["LoanTerm"];
                            return (principalAmount * interestRate * loanTerm) / 12; 
                        }
                    }
                }
            }
            throw new InvalidLoanException("Loan not found.");
        }

        public double CalculateEMI(int loanId)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Loan WHERE LoanID = @LoanID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", loanId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double principalAmount = Convert.ToDouble(reader["PrincipalAmount"]);
                            double interestRate = Convert.ToDouble(reader["InterestRate"]) / 12 / 100; 
                            int loanTerm = (int)reader["LoanTerm"];
                            return (principalAmount * interestRate * Math.Pow(1 + interestRate, loanTerm)) / (Math.Pow(1 + interestRate, loanTerm) - 1);
                        }
                    }
                }
            }
            throw new InvalidLoanException("Loan not found.");
        }


  

 
        public void LoanRepayment(int loanId, double amount)
        {
            Loan loan = GetLoanById(loanId); 

            double emi = CalculateEMI(loanId); 
            if (amount >= emi)
            {
                Console.WriteLine("EMI paid successfully.");
                
            }
            else
            {
                Console.WriteLine("Insufficient amount to pay EMI.");
            }
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"
            SELECT l.LoanID, l.CustomerID, l.PrincipalAmount, l.InterestRate, 
                   l.LoanTerm, l.LoanType, l.LoanStatus, 
                   c.Name, c.EmailAddress, c.PhoneNumber, c.Address, c.CreditScore 
            FROM Loan l 
            JOIN Customer c ON l.CustomerID = c.CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Loan loan = new Loan
                            {
                                LoanID = (int)reader["LoanID"],
                                Customer = new Customer
                                {
                                    CustomerID = (int)reader["CustomerID"],
                                    Name = reader["Name"].ToString(),
                                    EmailAddress = reader["EmailAddress"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    CreditScore = (int)reader["CreditScore"]
                                },
                                PrincipalAmount = Convert.ToDouble(reader["PrincipalAmount"]),
                                InterestRate = Convert.ToDouble(reader["InterestRate"]), 
                                LoanTerm = (int)reader["LoanTerm"],
                                LoanType = reader["LoanType"].ToString(),
                                LoanStatus = reader["LoanStatus"].ToString()
                            };

                            loans.Add(loan);
                        }
                    }
                }
            }

            return loans;
        }



        public void AddHomeLoan(HomeLoan homeLoan)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "INSERT INTO HomeLoan (LoanID, PropertyAddress, PropertyValue) VALUES (@LoanID, @PropertyAddress, @PropertyValue)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", homeLoan.LoanID); 
                    command.Parameters.AddWithValue("@PropertyAddress", homeLoan.PropertyAddress);
                    command.Parameters.AddWithValue("@PropertyValue", homeLoan.PropertyValue);

                    command.ExecuteNonQuery(); 
                }
            }
        }

        public void AddCarLoan(CarLoan carLoan)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "INSERT INTO CarLoan (LoanID, CarModel, CarValue) VALUES (@LoanID, @CarModel, @CarValue)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", carLoan.LoanID);
                    command.Parameters.AddWithValue("@CarModel", carLoan.CarModel);
                    command.Parameters.AddWithValue("@CarValue", carLoan.CarValue);

                    command.ExecuteNonQuery(); 
                }
            }
        }

        public string LoanStatus(int loanId)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"
            SELECT c.CreditScore 
            FROM Loan l 
            JOIN Customer c ON l.CustomerID = c.CustomerID 
            WHERE l.LoanID = @LoanID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", loanId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int creditScore = (int)reader["CreditScore"];
                            string status = creditScore > 650 ? "Approved" : "Rejected";
                            UpdateLoanStatus(loanId, status); 
                            return status; 
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
            }
        }


        private void UpdateLoanStatus(int loanId, string status)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "UPDATE Loan SET LoanStatus = @LoanStatus WHERE LoanID = @LoanID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanStatus", status);
                    command.Parameters.AddWithValue("@LoanID", loanId);
                    command.ExecuteNonQuery(); 
                }
            }
        }


        public Loan GetLoanById(int loanId)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Loan WHERE LoanID = @LoanID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanID", loanId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Loan loan = new Loan
                            {
                                LoanID = (int)reader["LoanID"],
                                PrincipalAmount = Convert.ToDouble(reader["PrincipalAmount"]), 
                                InterestRate = Convert.ToDouble(reader["InterestRate"]), 
                                LoanTerm = (int)reader["LoanTerm"],
                                LoanType = reader["LoanType"].ToString(),
                                LoanStatus = reader["LoanStatus"].ToString(),
                                Customer = new Customer
                                {
                                    CustomerID = (int)reader["CustomerID"] 
                                }
                            };
                            return loan;
                        }
                    }
                }
            }
            throw new InvalidLoanException("Loan not found.");
        }

    }
}
