using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vs.Pm.Pm.Db.Models
{
    [Table("Task")]
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string? ChangeLogJson { get; set; }
        [Required]
        [Timestamp]
        public byte[]? Timestamp { get; set; }

        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public int TaskTypeId { get; set; }
        public string Description { get; set; }

    }
}
