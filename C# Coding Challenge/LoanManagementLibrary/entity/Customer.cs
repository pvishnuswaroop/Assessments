using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementLibrary.entity
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }

        
        public Customer() { }

        
        public Customer(int customerId, string name, string email, string phone, string address, int creditScore)
        {
            CustomerID = customerId;
            Name = name;
            EmailAddress = email;
            PhoneNumber = phone;
            Address = address;
            CreditScore = creditScore;
        }
    }
}

