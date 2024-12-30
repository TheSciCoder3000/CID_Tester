using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CID_Tester.Model
{
    public class TEST_USER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserCode { get; private set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; private set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; private set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; private set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ProfileImage { get; private set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Username { get; private set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; private set; } = null!;

        public ICollection<TEST_PLAN> TEST_PLANS { get; set; } = [];


        public TEST_USER(int userCode, string firstName, string lastName, string email, string profileImage, string username, string password)
        {
            UserCode = userCode;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ProfileImage = profileImage;
            Username = username;
            Password = password;
        }

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
