create database LoanManagementSystem
go

USE LoanManagementSystem; 


CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    EmailAddress NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
    Address NVARCHAR(255),
    CreditScore INT NOT NULL
);


CREATE TABLE Loan (
    LoanID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    PrincipalAmount DECIMAL(18, 2) NOT NULL,
    InterestRate DECIMAL(5, 2) NOT NULL,
    LoanTerm INT NOT NULL,
    LoanType NVARCHAR(50) NOT NULL,
    LoanStatus NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);


CREATE TABLE HomeLoan (
    HomeLoanID INT PRIMARY KEY IDENTITY(1,1),
    LoanID INT NOT NULL,
    PropertyAddress NVARCHAR(255) NOT NULL,
    PropertyValue INT NOT NULL,
    FOREIGN KEY (LoanID) REFERENCES Loan(LoanID)
);


CREATE TABLE CarLoan (
    CarLoanID INT PRIMARY KEY IDENTITY(1,1),
    LoanID INT NOT NULL,
    CarModel NVARCHAR(100) NOT NULL,
    CarValue INT NOT NULL,
    FOREIGN KEY (LoanID) REFERENCES Loan(LoanID)
);

SELECT * FROM Customer;
SELECT * FROM Loan;
SELECT * FROM HomeLoan;
SELECT * FROM CarLoan;
