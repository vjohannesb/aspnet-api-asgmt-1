namespace SharedLibrary.Models.Admin
{
    public class AdminViewModel
    {
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
