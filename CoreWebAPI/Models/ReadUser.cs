using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class ReadUser
    {
        [JsonIgnore]
        [Key]
        public int RUId { get; set; }

        public string Value { get; set; }
    }
}
