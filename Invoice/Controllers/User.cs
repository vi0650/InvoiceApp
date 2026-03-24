using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly IUserService service;

        public User(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Response<IEnumerable<UserResDTO>>>> GetUsers()
        {
            var user = await service.GetUsers();
            var response = StatusCode(user.StatusCode, user);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Response<UserResDTO>>> GetUserById(Guid id)
        {
            try
            {
                var user = await service.GetUserById(id);
                return user.StatusCode switch
                {
                    200 => Ok(user),
                    404 => NotFound(user)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while retrieving data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Response<UserResDTO>>> CreateUser(CreateUserDTO createDTO)
        {
            try
            {
                var createUser = await service.CreateUser(createDTO);
                return createUser.StatusCode switch
                {
                    200 => Ok(createUser),
                    409 => Conflict(createUser),
                    400 => BadRequest(createUser),
                    _ => StatusCode(500, createUser)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while Creating The data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Response<UserResDTO>>> UpdateUser(Guid id, UpdateUserDTO updateDTO)
        {
            try
            {
                var updateUser = await service.UpdateUser(id, updateDTO);
                return updateUser.StatusCode switch
                {
                    200 => Ok(updateUser),
                    409 => Conflict(updateUser),
                    400 => BadRequest(updateUser),
                    404 => NotFound(updateUser),
                    _ => StatusCode(500, updateUser)
                };
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred while updating the data", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        //[HttpPatch("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult<Response<UserResDTO>>> PatchUser(Guid id, [FromBody] UpdateUserDTO dto)
        //{

        //}

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Response<object>>> DeleteUser(Guid id)
        {
            try
            {
                var deleteUser = await service.DeleteUser(id);
                return deleteUser.StatusCode switch
                {
                    200 => Ok(deleteUser),
                    400 => BadRequest(deleteUser),
                    404 => NotFound(deleteUser),
                    _ => StatusCode(500, deleteUser)
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
