using CoreWebAPI.Data;
using CoreWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = CoreWebAPI.Models.Attribute;

namespace CoreWebAPI.Repository
{
    public class BatchRepository : IBatchRepository
    {
        private readonly BatchContext _context;

        public BatchRepository(BatchContext context)
        {
            _context = context;
        }
        public BatchViewModel AddBatchDetails(BatchViewModel batchViewModel)
        {
            _context.BatchDetails.Add(batchViewModel);
            _context.SaveChanges();
            return batchViewModel;
        }

        public List<Attribute> GetAttributeList(Guid id)
        {
            var result = new List<Attribute>();
            if (BatchViewModelExists(id))
                result = _context.Attribute.Where(s => s.BatchId == id).ToList();

            return result;
        }

        private bool BatchViewModelExists(Guid id)
        {
            return _context.BatchDetails.Any(e => e.Id == id);
        }

        public BatchViewModel GetBatchDetails(Guid id)
        {
            var attributeList = GetAttributeList(id);

            List<BatchViewModel> batchViewModelList = (from bd in _context.BatchDetails
                                      join acl in _context.Acl on bd.AclId equals acl.AclId
                                      //join c in _context.Attribute on ot.Id equals c.BatchId
                                      where bd.Id == id
                                      select new BatchViewModel
                                      {
                                          Id = bd.Id,
                                          Acl = acl,
                                          BusinessUnit = bd.BusinessUnit,
                                          ExpiryDate = bd.ExpiryDate,
                                          Attributes = attributeList,
                                          AclId = bd.AclId
                                      }).ToList();

            var batchViewModel = batchViewModelList.FirstOrDefault();
            return batchViewModel;
        }
    }
}
