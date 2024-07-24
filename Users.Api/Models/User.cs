using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Api.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MinLength(10)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("email")]
        [MinLength(10)]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
