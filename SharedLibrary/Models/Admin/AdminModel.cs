using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Admin
{
    public class AdminModel : UserModel
    {
        public AdminModel() { }

        public AdminModel(RegisterModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
        }

        [Key]
        [Required]
        public int AdminId { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Token { get; set; }

        [Required]
        [JsonIgnore]
        [Column(TypeName = "varbinary(max)")]
        public byte[] AdminSalt { get; set; }

        [Required]
        [JsonIgnore]
        [Column(TypeName = "varbinary(max)")]
        public byte[] AdminHash { get; set; }

        public void CreatePasswordWithHash(string password)
        {
            using var hmac = new HMACSHA512();
            AdminSalt = hmac.Key;
            AdminHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
        }

        public bool ValidatePasswordHash(string password)
        {
            var saltedHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
            if (saltedHash.Length != AdminHash.Length)
                return false;

            for (var i = 0; i < saltedHash.Length; i++)
                if (saltedHash[i] != AdminHash[i])
                    return false; 

            return true;
        }

        public void CreateTokenWithHash(string token)
            => Token = GenerateSaltedHash(Encoding.UTF8.GetBytes(token));

        public bool ValidateTokenHash(string token)
        {
            var saltedToken = GenerateSaltedHash(Encoding.UTF8.GetBytes(token));
            if (saltedToken.Length != Token.Length)
                return false;

            for (var i = 0; i < saltedToken.Length; i++)
                if (saltedToken[i] != Token[i])
                    return false;

            return true;
        }

        private byte[] GenerateSaltedHash(byte[] password)
        {
            using var hmac = new HMACSHA512(AdminSalt);
            byte[] saltedPass = password.Concat(AdminSalt).ToArray();
            return hmac.ComputeHash(saltedPass);
        }
    }
}
