using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class ErrorResponse
    {
        public string CorrelationId { get; set; }
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
