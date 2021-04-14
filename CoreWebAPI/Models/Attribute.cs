using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoreWebAPI.Models
{
    public class Attribute
    {
        [JsonIgnore]
        [Key]
        public int AttributeId { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonIgnore]
        public Guid BatchId { get; set; }
    }
}
