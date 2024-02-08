using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.Enums;

namespace ToDoList.Entity.DTOs.Response
{
    public class ApiResponseDTO<T>
    {
        public ApiResponseDTO()
        {
            Messages = new List<ApiResponseMessage> { };
        }

        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public List<ApiResponseMessage> Messages { get; set; }

        public void AddMessage(MessageTypeEnum messageType, string message)
        {
            Messages.Add(new ApiResponseMessage { MessageType = messageType, Message = message });
        }

    }
}
