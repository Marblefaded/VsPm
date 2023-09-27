using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vs.Pm.Pm.Db.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string Title { get; set; }
        public string? ChangeLogJson { get; set; }
        [Required]
        [Timestamp]
        public byte[]? Timestamp { get; set; }
        public int OrderId { get; set; }
    }
}
