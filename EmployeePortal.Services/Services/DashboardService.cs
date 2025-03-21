﻿using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EmployeePortal.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IDashboardRepository dashboardRepository, ILogger<DashboardService> logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
        }

        public bool CreatePost(PostDto postDto)
        {
            try
            {
                var post = new Post
                {
                    Title = postDto.Title,
                    Description = postDto.Description,
                    ImageData = postDto.ImageData,
                    Author = postDto.Author,
                    DateOfPublishing = postDto.DateOfPublishing
                };

                _dashboardRepository.CreatePost(post);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while creating a social post.");
                return false;
            }
        }
    }
}
