using Invoice.Data;
using Invoice.DTOs.AuthDTO;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly InvoiceDbContext db;
        private readonly IAuthService authService;

        public Auth(InvoiceDbContext db,IAuthService authService)
        {
            this.authService = authService;
            this.db = db;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Response<UserResDTO>>> Register([FromBody]RegReqDTO regReqDTO)
        {
            try
            {
                var registerRes = await authService.RegisterAsync(regReqDTO);
                if (!registerRes.Success)
                {
                    return BadRequest(registerRes);
                }
                return CreatedAtAction(nameof(Register), registerRes);
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "An error occurred during Registration", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Response<LogResDTO>>> Login([FromBody]LogReqDTO logReqDTO)
        {
            try
            {
                var loginRes = await authService.LoginAsync(logReqDTO);

                if(!loginRes.Success)
                {
                    return BadRequest(loginRes);
                }
                return Ok(loginRes);
            }
            catch (Exception ex)
            {
                var errorResponse = Response<object>.Error(500, "Something unexpected during login ", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
