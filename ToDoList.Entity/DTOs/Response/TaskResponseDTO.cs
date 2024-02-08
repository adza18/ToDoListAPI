﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.Enums;
using ToDoList.Entity.Models;

namespace ToDoList.Entity.DTOs.Response
{
    public class TaskResponseDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime DueDate { get; set; }

        public  int CreatedByUser { get; set; }
    }
}
