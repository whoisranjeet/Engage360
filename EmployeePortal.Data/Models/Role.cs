﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
