using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }

        public IEnumerable<Student> Students { get; set; } = [];
    }
}
