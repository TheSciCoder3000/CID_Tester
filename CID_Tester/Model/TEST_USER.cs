using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CID_Tester.Model
{
    public class TEST_USER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ProfileImage { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Username { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; } = null!;

        public ICollection<TEST_PLAN> TEST_PLANS { get; set; } = [];

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public bool VerifyUser(string username, string password)
        {
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, Password);
            return passwordMatch && username == Username;
        }
    }
}
