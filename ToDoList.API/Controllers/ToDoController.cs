using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Helper;
using ToDoList.Application.Interfaces;
using ToDoList.Entity.DTOs;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _todoService;
        private readonly ILogger<ToDoController> _logger;
        public ToDoController(IToDoService todoService, ILogger<ToDoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var result = await _todoService.Get();
            var response = MapResponse.MapToApiResponse<List<ToDoModel>>(result);
            return MapResponse.ActionResponse<List<ToDoModel>>(response);
        }
    }
}
