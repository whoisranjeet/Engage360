﻿using EmployeePortal.Core.DTOs;

namespace EmployeePortal.Core.Interfaces
{
    public interface IDashboardService
    {
        bool CreatePost(PostDto postDto);
    }
}
