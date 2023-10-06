using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vs.Pm.Pm.Db.Models
{
    [Table("User")]
    public class User : IChangeLog
    {
        [Key]
        public int UserId { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string? ChangeLogJson { get; set; }
        [Required]
        [Timestamp]
        public byte[]? Timestamp { get; set; }

        public string Login {get;set;}
    }
}
