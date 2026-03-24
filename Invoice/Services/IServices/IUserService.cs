using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;

namespace Invoice.Services.IServices
{
    public interface IUserService
    {
        public Task<Response<IEnumerable<UserResDTO>>> GetUsers();
        public Task<Response<UserResDTO>> GetUserById(Guid id);
        public Task<Response<UserResDTO>> CreateUser(CreateUserDTO userDTO);
        public Task<Response<UserResDTO>> UpdateUser(Guid id, UpdateUserDTO updateDTO);
        public Task<Response<UserResDTO>> DeleteUser(Guid id);
    }
}
