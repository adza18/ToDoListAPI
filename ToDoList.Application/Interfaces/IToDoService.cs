using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.DTOs;
using ToDoList.Entity.DTOs.Request;
using ToDoList.Entity.DTOs.Response;
using ToDoList.Entity.Models;

namespace ToDoList.Application.Interfaces
{
    public interface IToDoService
    {
        Task<ServiceResponseDTO<List<ToDoModel>>> Get();

    }
}
