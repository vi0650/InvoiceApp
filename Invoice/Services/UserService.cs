using AutoMapper;
using Invoice.DTOs.CreateDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;

namespace Invoice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo repo;
        private readonly IMapper mapper;

        public UserService(IUserRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<UserResDTO>>> GetUsers()
        {
            var user = await repo.GetAllAsync();
            if (!user.Any())
            {
                return Response<IEnumerable<UserResDTO>>.NotFound();
            }
            var res = mapper.Map<IEnumerable<UserResDTO>>(user);
            return Response<IEnumerable<UserResDTO>>.Ok(res, "User Data retrieved successfully");
        }

        public async Task<Response<UserResDTO>> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<UserResDTO>.BadRequest("Invalid user id");
            }
            var user = await repo.GetByIdAsync(id);

            if (user is null)
            {
                return Response<UserResDTO>.NotFound($"User of Id {id} does not exist");
            }

            var dto = mapper.Map<UserResDTO>(user);

            return Response<UserResDTO>.Ok(dto, "Record retrieved successfully");
        }

        public async Task<Response<UserResDTO>> CreateUser(CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return Response<UserResDTO>.BadRequest("User data is required");
            }
            if (await repo.IsEmailExistAsync(userDTO.Email))
            {
                return Response<UserResDTO>.Conflict("Email is already exists");
            }
            var model = mapper.Map<UserModel>(userDTO);
            model.userid = Guid.NewGuid();
            model.createdat = DateTime.UtcNow;
            await repo.CreateAsync(model);
            var createUser = mapper.Map<UserResDTO>(model);
            return Response<UserResDTO>.Ok(createUser, "User Created Successfully");
        }

        public async Task<Response<UserResDTO>> UpdateUser(Guid id, UpdateUserDTO updateDTO)
        {
            if (id == Guid.Empty || id != updateDTO.UserId)
            {
                return Response<UserResDTO>.BadRequest("User id does not match to entered records with request body");
            }
            var existingUser = await repo.GetByIdAsync(updateDTO.UserId);
            if (existingUser is null)
            {
                return Response<UserResDTO>.NotFound($"User with id {id} does not exist in Records");
            }
            if (await repo.IsDataExistAsync(updateDTO.Email, id))
            {
                return Response<UserResDTO>.Conflict($"{updateDTO.Email} is already Taken");
            }
            mapper.Map(updateDTO, existingUser);
            existingUser.updatedat = DateTime.UtcNow;
            await repo.UpdateAsync(existingUser);
            var updateUser = mapper.Map<UserResDTO>(existingUser);
            return Response<UserResDTO>.Ok(updateUser, "User Updated Successfully");
        }

        public async Task<Response<UserResDTO>> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response<UserResDTO>.NotFound("Invalid user id");
            }
            var existingUser = await repo.GetByIdAsync(id);
            if (existingUser is null)
            {
                return Response<UserResDTO>.NotFound($"User with id {id} does not exist in Records");
            }

            await repo.RemoveAsync(existingUser);
            var res = mapper.Map<UserResDTO>(existingUser);
            return Response<UserResDTO>.Ok(null, "User Deleted Successfully...");
        }
    }
}
