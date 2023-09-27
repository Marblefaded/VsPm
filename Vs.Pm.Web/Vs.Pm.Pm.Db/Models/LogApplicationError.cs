using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vs.Pm.Pm.Db.Models
{
    [Table("LogApplicationError")]
    public class LogApplicationError
    {
        [Key]
        public int LogApplicationErrorId { get; set; }
        public DateTime InsertDate { get; set; }
        public string ErrorContext { get; set; }
        public string ErrorMessage { get; set; }
        public string? ErrorInnerException { get; set; }
    }

}

