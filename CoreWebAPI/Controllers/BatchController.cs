﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreWebAPI.Models;
using Microsoft.Extensions.Logging;
using CoreWebAPI.Repository;
using CoreWebAPI.Filters;

namespace CoreWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilter))]
    public class BatchController : ControllerBase
    {
        //private readonly BatchContext _context;
        private readonly IBatchRepository _batchRepository;
        private readonly ILogger<BatchController> _logger;

        public BatchController(IBatchRepository batchRepository, ILogger<BatchController> logger)
        {
            _batchRepository = batchRepository;
            _logger = logger;
        }

        // GET: api/Batch
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BatchViewModel>>> Getbatches()
        //{
        //    return await _context.BatchDetails.ToListAsync();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">A Batch ID</param>
        /// <returns></returns>
        // GET: api/Batch/5
        [HttpGet("{id}")]
        public ActionResult<BatchViewModel> GetBatchViewModel(Guid id)
        {
            try
            {
                var batchViewModel = _batchRepository.GetBatchDetails(id);

                if (batchViewModel == null)
                {
                    _logger.LogInformation("Batch ID - {0} doesn't exits.", id);
                    ModelState.AddModelError(id.ToString(), "Could be that tha batch ID doesn't exits.");
                    return NotFound(ModelState);
                }
                else if (batchViewModel.ExpiryDate < DateTime.Now)
                {
                    _logger.LogInformation("Batch ID - {0} has been expired on date {1}.", batchViewModel.Id, batchViewModel.ExpiryDate.ToLongDateString());
                    return StatusCode(StatusCodes.Status410Gone, "The Batch has been expired and is no longer available.");
                }


                return batchViewModel;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "");
                return Content(ex.ToString());
            }
        }

        // POST: api/Batch
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult PostBatchViewModel(BatchViewModel batchViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BatchViewModel newbatchViewModel = _batchRepository.AddBatchDetails(batchViewModel);

                    //return CreatedAtAction("GetBatchGuid", new { id = batchViewModel.Id }, batchViewModel);
                    return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + newbatchViewModel.Id, new { batchId = newbatchViewModel.Id });
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return Content(ex.ToString());
            }
        }

        // PUT: api/Batch/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBatchViewModel(Guid id, BatchViewModel batchViewModel)
        //{
        //    if (id != batchViewModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(batchViewModel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BatchViewModelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        // DELETE: api/Batch/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<BatchViewModel>> DeleteBatchViewModel(Guid id)
        //{
        //    var batchViewModel = await _context.BatchDetails.FindAsync(id);
        //    if (batchViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BatchDetails.Remove(batchViewModel);
        //    await _context.SaveChangesAsync();

        //    return batchViewModel;
        //}

        //private bool BatchViewModelExists(Guid id)
        //{
        //    return _context.BatchDetails.Any(e => e.Id == id);
        //}

        //public async Task<ActionResult<System.Guid>> GetBatchGuid(Guid id)
        //{
        //    var batchViewModel = await _context.BatchDetails.FindAsync(id);

        //    if (batchViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(new { batchId = id });
        //}
    }
}
