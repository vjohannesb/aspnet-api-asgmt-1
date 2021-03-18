using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Admin
{
    public class AdminViewModel
    {
        // För JSON-serializing
        public AdminViewModel() { }

        public AdminViewModel(AdminModel am)
        {
            AdminId = am.AdminId;
            DisplayName = $"{am.FirstName} {am.LastName}";
            Email = am.Email;
        }

        public int AdminId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
