using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Entity.Models
{
   
        public class BaseEntity
        {
            public int CreatedBy { get; set; }
            public int UpdatedBy { get; set; }
            public DateTime CreatedDate { get; set; } = DateTime.Now;
            public DateTime UpdatedDate { get; set; }
            public int IsDeleted { get; set; }

    }
}
