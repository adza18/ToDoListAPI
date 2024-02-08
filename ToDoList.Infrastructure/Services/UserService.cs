using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Entity.DTOs;
using ToDoList.Entity.DTOs.Request;
using ToDoList.Entity.DTOs.Response;
using ToDoList.Entity.Enums;
using ToDoList.Entity.Models;

namespace ToDoList.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        public UserService(IGenericRepository genericRepository = null, ILogger<UserService> logger = null, IConfiguration configuration = null)
        {
            _genericRepository = genericRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public string GetJWTSecretKey()
        {
            return _configuration["Authentication:JWTSecretKey"];
        }

        public async Task<ServiceResponseDTO<UserModel>> AuthenticateAsync(LoginDTO model)
        {
            try
            {
                var response = new ServiceResponseDTO<UserModel>();
                var emailExists = await _genericRepository.GetFirstOrDefault<User>(x => x.Email == model.Email);
                if(emailExists == null)
                {
                    response.Status = ServiceStatusEnum.NotFound;
                    response.IsSuccess = false;
                    response.ErrorMessage = "Email or password doesnt match";
                    return response;
                }
                if (!BCrypt.Net.BCrypt.Verify(model.Password, emailExists.HashedPassword))
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Invalid Credentials Provided.";
                    response.Status = ServiceStatusEnum.Unauthorized;
                    return response;
                }

                User user = new User()
                {
                    Id = emailExists.Id,
                    HashedPassword = emailExists.HashedPassword,
                    UserName = emailExists.UserName,
                    Email = emailExists.Email,
                };

                response.Data = UserModel(user);
                response.IsSuccess = true;
                response.Status = ServiceStatusEnum.Success;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in authentication");
                throw ex;
            }
        }

        public Task<bool> DeleteByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePermanentlyByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseDTO<UserModel>> GetByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponseDTO<UserModel>> RegisterUserAsync(RegisterUserDTO signUpModel)
        {
            var response = new ServiceResponseDTO<UserModel>();

            try
            {
                var emailExists = await _genericRepository.GetFirstOrDefault<User>(x => x.Email == signUpModel.EmailAddress);

                if (emailExists != null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Duplicate Email";
                    response.Status = ServiceStatusEnum.Error;
                    _logger.LogWarning("The Email you are registering already exists in the system for email: {0}", signUpModel.EmailAddress);
                    return response;
                }

                var userModel = new User()
                {
                    Email = signUpModel.EmailAddress,
                    UserName = signUpModel.UserName,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password),
                    FullName = signUpModel.FullName,
                };

                var userId = await _genericRepository.Insert(userModel);

                if(userId == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Error Saving User in database";
                    response.Status = ServiceStatusEnum.Unknown;
                    _logger.LogWarning("Something went wrong while saving the user");
                    return response;
                }
                var newUser = await _genericRepository.GetById<User>(userId);


                response.IsSuccess = true;
                response.Status = ServiceStatusEnum.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration.");
                response.IsSuccess = false;
                response.ErrorMessage = "An error occurred during user registration.";
                response.Status = ServiceStatusEnum.Error;
            }

            return response;
        }

        private string GetToken(int userID,string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyApp = GetJWTSecretKey();
            var key = Encoding.ASCII.GetBytes(keyApp);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new Claim[]
                //{
                //    new Claim("id", userID.ToString()),
                //    new Claim("username", userName.ToString())

                //}),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userID.ToString()),
                    new Claim(ClaimTypes.Name, userName.ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private UserModel UserModel(User user)
        {
            if (user == null) return null;
            return new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = GetToken(user.Id, user.UserName)
            };
        }

    }
}
