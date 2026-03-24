using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Customer : ControllerBase
    {
        private readonly ICustomerService service;

        public Customer(ICustomerService service)
        {
            this.service = service;
        }

        //[HttpGet]
        //[Authorize(Roles = "SuperAdmin,User,Employee")]
        //public async Task<ActionResult<Response<IEnumerable<CustomerResDTO>>>> GetCustomers()
        //{
        //    var customer = await service.GetCustomers();
        //    var response = StatusCode(customer.StatusCode, customer);
        //    return Ok(response);
        //}

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<CustomerResDTO>>> GetCustomers()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (!Guid.TryParse(userId, out Guid guid))
                //    return Unauthorized();
                Guid userId = Guid.Parse(userIdClaim);
                var customer = await service.GetCustomerByUserId(userId);
                var response = StatusCode(customer.StatusCode, customer);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while retrieving data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<CustomerResDTO>>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await service.GetCustomerById(id);
                return customer.StatusCode switch
                {
                    200 => Ok(customer),
                    404 => NotFound(customer)
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
        public async Task<ActionResult<Response<CustomerResDTO>>> CreateCustomer(CreateCustomerDTO createDTO)
        {
            try
            {
                var createCustomer = await service.CreateCustomer(createDTO);
                return createCustomer.StatusCode switch
                {
                    200 => Ok(createCustomer),
                    409 => Conflict(createCustomer),
                    400 => BadRequest(createCustomer),
                    _ => StatusCode(500, createCustomer)
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
        public async Task<ActionResult<Response<CustomerResDTO>>> UpdateCustomer(Guid id, UpdateCustomerDTO updateDTO)
        {
            try
            {
                var updateCustomer = await service.UpdateCustomer(id, updateDTO);
                return updateCustomer.StatusCode switch
                {
                    200 => Ok(updateCustomer),
                    409 => Conflict(updateCustomer),
                    400 => BadRequest(updateCustomer),
                    404 => NotFound(updateCustomer),
                    _ => StatusCode(500, updateCustomer)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while updating the data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<object>>> DeleteCustomer(Guid id)
        {
            try
            {
                var deleteCustomer = await service.DeleteCustomer(id);
                return deleteCustomer.StatusCode switch
                {
                    200 => Ok(deleteCustomer),
                    400 => BadRequest(deleteCustomer),
                    404 => NotFound(deleteCustomer),
                    409 => Conflict(deleteCustomer),
                    _ => StatusCode(500, deleteCustomer)
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
