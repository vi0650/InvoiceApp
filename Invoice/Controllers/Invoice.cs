using Invoice.Data;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Invoice : ControllerBase
    {
        private readonly IInvoiceService service;

        public Invoice(IInvoiceService service)
        {
            this.service = service;
        }

        //[HttpGet]
        //[Authorize(Roles = "SuperAdmin,User,Employee")]
        //public async Task<ActionResult<Response<IEnumerable<InvoiceResDTO>>>> GetInvoices()
        //{
        //    var invoice = await service.GetInvoice();
        //    var response = StatusCode(invoice.StatusCode, invoice);
        //    return Ok(response);
        //}

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<IEnumerable<InvoiceResDTO>>>> GetInvoices()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (!Guid.TryParse(userId, out Guid guid))
            //    return Unauthorized();
            Guid userId = Guid.Parse(userIdClaim);
            var invoice = await service.GetInvoiceByUser(userId);
            var response = StatusCode(invoice.StatusCode, invoice);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<InvoiceResDTO>>> GetInvoiceById(Guid id)
        {
            try
            {
                var invoice = await service.GetInvoiceById(id);
                return invoice.StatusCode switch
                {
                    200 => Ok(invoice),
                    404 => NotFound(invoice)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while retrieving data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<InvoiceResDTO>>> CreateInvoice(CreateInvoiceDTO createDTO)
        {
            try
            {
                var createInvoice = await service.CreateInvoice(createDTO);
                return createInvoice.StatusCode switch
                {
                    200 => Ok(createInvoice),
                    409 => Conflict(createInvoice),
                    400 => BadRequest(createInvoice),
                    _ => StatusCode(500, createInvoice)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while Creating The data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<InvoiceResDTO>>> UpdateInvoice(Guid id, UpdateInvoiceDTO updateDTO)
        {
            try
            {
                var updateInvoice = await service.UpdateInvoice(id, updateDTO);
                return updateInvoice.StatusCode switch
                {
                    200 => Ok(updateInvoice),
                    409 => Conflict(updateInvoice),
                    400 => BadRequest(updateInvoice),
                    404 => NotFound(updateInvoice),
                    _ => StatusCode(500, updateInvoice)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("UPDATE INVOICE ERROR:");
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("INNER EXCEPTION:");
                    Console.WriteLine(ex.InnerException.Message);
                }
                var errorResponse = Response<object>.Error(500, "An error occurred while updating the data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<object>>> DeleteInvoice(Guid id)
        {
            try
            {
                var deleteInvoice = await service.DeleteInvoice(id);
                return deleteInvoice.StatusCode switch
                {
                    200 => Ok(deleteInvoice),
                    400 => BadRequest(deleteInvoice),
                    404 => NotFound(deleteInvoice),
                    409 => Conflict(deleteInvoice),
                    _ => StatusCode(500, deleteInvoice)
                };
            }

            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while deleting the data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
