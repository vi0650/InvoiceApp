using AutoMapper;
using Invoice.Data;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        private readonly IProductService service;

        public Product(IProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<IEnumerable<ProductResDTO>>>> GetProducts()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (!Guid.TryParse(userId, out Guid guid))
                //    return Unauthorized();
                Guid userId = Guid.Parse(userIdClaim);
                var product = await service.GetProductByUserId(userId);
                var response = StatusCode(product.StatusCode, product);
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
        public async Task<ActionResult<Response<ProductDTO>>> GetProductById(Guid id)
        {
            try
            {
                var product = await service.GetProductById(id);
                return product.StatusCode switch
                {
                    200 => Ok(product),
                    404 => NotFound(product)
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
        public async Task<ActionResult<Response<ProductResDTO>>> CreateProduct(CreateProductDTO createDTO)
        {
            try
            {
                var createProduct = await service.CreateProduct(createDTO);
                return createProduct.StatusCode switch
                {
                    200 => Ok(createProduct),
                    409 => Conflict(createProduct),
                    400 => BadRequest(createProduct),
                    _ => StatusCode(500, createProduct)
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
        public async Task<ActionResult<Response<ProductResDTO>>> UpdateProduct(Guid id, UpdateProductDTO updateDTO)
        {
            try
            {
                var updateProduct = await service.UpdateProduct(id, updateDTO);
                return updateProduct.StatusCode switch
                {
                    200 => Ok(updateProduct),
                    409 => Conflict(updateProduct),
                    400 => BadRequest(updateProduct),
                    404 => NotFound(updateProduct),
                    _ => StatusCode(500, updateProduct)
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
        public async Task<ActionResult<Response<object>>> DeleteProduct(Guid id)
        {
            try
            {
                var deleteProduct = await service.DeleteProduct(id);
                return deleteProduct.StatusCode switch
                {
                    200 => Ok(deleteProduct),
                    400 => BadRequest(deleteProduct),
                    404 => NotFound(deleteProduct),
                    409 => Conflict(deleteProduct),
                    _ => StatusCode(500, deleteProduct)
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
