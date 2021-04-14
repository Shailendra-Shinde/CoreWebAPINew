using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class Acl
    {
        [JsonIgnore]
        [Key]
        public int AclId { get; set; }

        [JsonPropertyName("readUsers")]
        public string[] ReadUsers { get; set; }
        //public List<ReadUser> ReadUsers { get; set; }

        [JsonPropertyName("readGroups")]
        public string[] ReadGroups { get; set; }
        //public List<ReadGroup> ReadGroups { get; set; }
    }
}
