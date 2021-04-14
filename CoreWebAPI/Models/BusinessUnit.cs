using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class BusinessUnit
    {
        [Key]
        public int BusinessUnitId { get; set; }

        [Required]
        public string BusinessUnitName { get; set; }
    }
}
