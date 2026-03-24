using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.DTOs.UpdateDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Gst : ControllerBase
    {
        private readonly IGstService Service;

        public Gst(IGstService service)
        {
            Service = service;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User,Employee")]
        public async Task<ActionResult<Response<IEnumerable<GstResDTO>>>> GetGst()
        {
            try
            {
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ////if (!Guid.TryParse(userId, out Guid guid))
                ////    return Unauthorized();
                //Guid userId = Guid.Parse(userIdClaim);
                //var gst = await Service.GetGstByUserId(userId);
                var gst = await Service.GetGst();
                var response = StatusCode(gst.StatusCode, gst);
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
        public async Task<ActionResult<Response<GstResDTO>>> GetGstById(Guid id)
        {
            try
            {
                var gst = await Service.GetGstById(id);
                return gst.StatusCode switch
                {
                    200 => Ok(gst),
                    404 => NotFound(gst)
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
        public async Task<ActionResult<Response<GstResDTO>>> CreateGst(CreateGstDTO createDTO)
        {
            try
            {
                var createGst = await Service.CreateGst(createDTO);
                return createGst.StatusCode switch
                {
                    200 => Ok(createGst),
                    409 => Conflict(createGst),
                    400 => BadRequest(createGst),
                    _ => StatusCode(500, createGst)
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
        public async Task<ActionResult<Response<GstResDTO>>> UpdateGst(Guid id, UpdateGstDTO updateDTO)
        {
            try
            {
                var updateGst = await Service.UpdateGst(id, updateDTO);
                return updateGst.StatusCode switch
                {
                    200 => Ok(updateGst),
                    409 => Conflict(updateGst),
                    400 => BadRequest(updateGst),
                    404 => NotFound(updateGst),
                    _ => StatusCode(500, updateGst)
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
        public async Task<ActionResult<Response<object>>> DeleteGst(Guid id)
        {
            try
            {
                var deleteGst = await Service.DeleteGst(id);
                return deleteGst.StatusCode switch
                {
                    200 => Ok(deleteGst),
                    400 => BadRequest(deleteGst),
                    404 => NotFound(deleteGst),
                    409 => Conflict(deleteGst),
                    _ => StatusCode(500, deleteGst)
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
