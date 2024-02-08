using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.Enums;

namespace ToDoList.Entity.DTOs.Response
{
    public class ServiceResponseDTO<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public ServiceStatusEnum Status { get; set; }
    }
}
