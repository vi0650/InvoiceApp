using AutoMapper;
using Invoice.Controllers;
using Invoice.DTOs.AuthDTO;
using Invoice.DTOs.BaseDTOs;
using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;
using Invoice.Repository.IRepository;
using Invoice.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Invoice.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;
        private readonly string secretKey;

        public AuthService(IUserRepo userRepo, IMapper mapper, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.secretKey = configuration.GetSection("JwtSettings")["Secret"];
        }

        public async Task<bool> IsEmailExistAsync(string Email)
        {
            return await userRepo.IsEmailExistAsync(Email);
        }

        public async Task<Response<object>> LoginAsync(LogReqDTO logReqDTO)
        {
            if (logReqDTO == null)
            {
                return Response<object>.BadRequest("All details are required");
            }
            var creds = await userRepo.GetByEmailAsync(logReqDTO.Email);
            if (creds == null)
            {
                return Response<object>.BadRequest("Not  Registered");
            }
            if (creds == null || creds.password != logReqDTO.Password)
            {
                return Response<object>.Unauthorized("Email and Password is Incorrect");
            }
            if (!creds.isactive)
            {
                return Response<object>.Unauthorized("Your account is inactive. Contact admin.");
            }

            var token = JwtToken(creds);
            var result = new LogResDTO
            {
                Token = token,
                //AdminDTO = mapper.Map<UserResDTO>(creds)
            };
            return Response<object>.Ok(result, "Login Successfully");
        }

        public async Task<Response<UserResDTO>> RegisterAsync(RegReqDTO regReqDTO)
        {
            if (regReqDTO == null)
            {
                return Response<UserResDTO>.BadRequest("Registration data is Required");
            }
            if (await IsEmailExistAsync(regReqDTO.Email))
            {
                return Response<UserResDTO>.Conflict("Email is already exist");
            }
            UserModel admin = new()
            {
                userid = Guid.NewGuid(),
                shopname = regReqDTO.ShopName,
                username = regReqDTO.UserName,
                password = regReqDTO.Password,
                email = regReqDTO.Email,
                mobileno = regReqDTO.MobileNo,
                address = regReqDTO.Address,
                role = regReqDTO.Role,
                isactive = regReqDTO.IsActive,
                createdat = DateTime.Now
            };

            await userRepo.CreateAsync(admin);
            var registerUser = mapper.Map<UserResDTO>(admin);
            return Response<UserResDTO>.CreatedAt(registerUser, "Admin Registered Successfully");
        }

        private string JwtToken(UserModel admin)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,admin.userid.ToString()),
                    new Claim(ClaimTypes.Email,admin.email),
                    new Claim(ClaimTypes.Name, admin.username),
                    new Claim(ClaimTypes.Role, admin.role),
                    new Claim("ShopName", admin.shopname),
                    new Claim("IsActive",admin.isactive.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var Token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(Token);
        }
    }
}