using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Customer
{
    public class CustomerViewModel
    {
        public CustomerViewModel() { }

        public CustomerViewModel(CustomerModel cm)
        {
            CustomerId = cm.CustomerId;
            FirstName = cm.FirstName;
            LastName = cm.LastName;
            Email = cm.Email;
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
    }
}
