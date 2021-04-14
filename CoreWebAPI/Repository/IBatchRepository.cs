using CoreWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = CoreWebAPI.Models.Attribute;

namespace CoreWebAPI.Repository
{
    public interface IBatchRepository
    {
        BatchViewModel GetBatchDetails(Guid Id);

        List<Attribute> GetAttributeList(Guid Id);

        BatchViewModel AddBatchDetails(BatchViewModel batchViewModel);
    }
}
