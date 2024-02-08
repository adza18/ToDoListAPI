using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Helper;
using ToDoList.Application.Interfaces;
using ToDoList.Entity.DTOs;
using ToDoList.Entity.DTOs.Request;
using ToDoList.Entity.Models;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserDTO model)
        {
            _logger.LogInformation("Creating User initiated with information:{0}", model.EmailAddress);
            var result = await _userService.RegisterUserAsync(model);
            var response = MapResponse.MapToApiResponse<UserModel>(result);
            return MapResponse.ActionResponse<UserModel>(response);
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO model)
        {
            _logger.LogInformation("Loggin in User initiated with information:{0}", model.Email);
            var result = await _userService.AuthenticateAsync(model);
            var response = MapResponse.MapToApiResponse<UserModel>(result);
            return MapResponse.ActionResponse<UserModel>(response);
        }

    }
}
