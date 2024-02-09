using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PractiseTest1.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [Column("Username", TypeName = "nvarchar")]
        [StringLength(50)]     
        public string Username { get; set; }
        [Required]
        [StringLength(50)]

        [Column("Email", TypeName = "nvarchar")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Password", TypeName = "nvarchar")]
        public string Password { get; set; }

        [Column("Role", TypeName = "nvarchar")]
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
