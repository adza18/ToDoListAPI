using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.DTOs;
using ToDoList.Entity.DTOs.Request;
using ToDoList.Entity.DTOs.Response;

namespace ToDoList.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponseDTO<UserModel>> AuthenticateAsync(LoginDTO model);
        Task<ServiceResponseDTO<UserModel>> RegisterUserAsync(RegisterUserDTO signUpModel);
        Task<ServiceResponseDTO<UserModel>> GetByIdAsync(int userId);
        Task<bool> DeleteByIdAsync(int userId);
        Task<bool> DeletePermanentlyByIdAsync(int userId);
    }
}
