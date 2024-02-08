using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.Enums;

namespace ToDoList.Entity.DTOs.Response
{
    public class ApiResponseMessage
    {
        public MessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }
    }
}
