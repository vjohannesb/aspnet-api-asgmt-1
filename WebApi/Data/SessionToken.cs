using SharedLibrary.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data
{
    public class SessionToken
    {
        [Key]
        [Required]
        public string AdminId { get; set; }
        public virtual AdminModel Admin { get; set; }

        [Required]
        [Column(TypeName = "varbinary(max)")]
        public string AccessToken { get; set; }
    }
}
