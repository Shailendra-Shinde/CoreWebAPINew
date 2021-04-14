using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CoreWebAPI.Models
{
    public class BatchViewModel
    {
        [JsonIgnore]
        [JsonPropertyName("batchId")]
        [DisplayName("A Batch ID")]
        [Key]
        public Guid Id { get; set; }

        public string BusinessUnit { get; set; }

        //public DateTime ExpiryDate { get; set; }

        [JsonPropertyName("acl")]
        public Acl Acl { get; set; }

        [JsonPropertyName("attributes")]
        [ForeignKey("BatchId")]
        public List<Attribute> Attributes { get; set; }

        [JsonPropertyName("expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonIgnore]
        [ForeignKey("Acl")]
        public int AclId { get; set; }
    }
}
