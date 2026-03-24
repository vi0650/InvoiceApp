using Invoice.DTOs.AuthDTO;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IAuthService
    {
        public Task<Response<UserResDTO>> RegisterAsync(RegReqDTO regReqDTO);
        public Task<Response<object>> LoginAsync(LogReqDTO logReqDTO);
        public Task<bool> IsEmailExistAsync(string Email);
    }
}
