using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.Enums;
using ToDoList.Entity.Models;

namespace ToDoList.Entity.DTOs
{
    public class ToDoModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }

        public int CreatedByUser { get; set; }
    }
}
