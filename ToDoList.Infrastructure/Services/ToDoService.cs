using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ToDoService : IToDoService
    {
        private readonly IGenericRepository _genericRepo;
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(IGenericRepository genericRepo, ILogger<ToDoService> logger)
        {
            _genericRepo = genericRepo;
            _logger = logger;
        }

        public async Task<ServiceResponseDTO<List<ToDoModel>>> Get()
        {
            _logger.LogInformation("Fetchin all active jobs");
            var result = await _genericRepo.Get<ToDoTask>();
            var data = result.Select(job => ToDoModel(job)).ToList();
            var response = new ServiceResponseDTO<List<ToDoModel>>
            {
                Data = data,
                IsSuccess = true,
                Status = ServiceStatusEnum.Success
            };
            return response;
        }

        private ToDoModel ToDoModel(ToDoTask entity)
        {
            var model = new ToDoModel
            {
               Id = entity.Id,
               Title = entity.Title,
               Description = entity.Description,
               Priority = Enum.GetName(typeof(TaskPriority), entity.Priority),
               Status = Enum.GetName(typeof(TaskStatus),entity.Status),
               CreatedByUser = entity.CreatedBy,
              
            };
            return model;
        }
    }
}
