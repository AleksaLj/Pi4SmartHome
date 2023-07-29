
namespace AdminManagementDSL.Core.Entities
{
    public class Users
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Addr { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string Pswrd { get; set; } = null!;

        public char GDPRFlag { get; set; }

        public string SignInKey { get; set; } = null!;

        public char EmailVerify { get; set; }
    }
}
