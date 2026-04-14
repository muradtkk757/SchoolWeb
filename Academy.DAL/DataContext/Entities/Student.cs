using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext.Entities
{
    public class Student : Entity
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
