﻿using EmployeePortal.Data;
using EmployeePortal.Data.Models;
using EmployeePortal.ViewModels;

namespace EmployeePortal
{
    public class DBOperations
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public DBOperations(AppDbContext context, ILogger<DBOperations> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public bool AddEmployee(SignUpViewModel obj)
        {
            try
            {
                Employee employee = new Employee()
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    MobileNumber = obj.MobileNumber,
                    EmailAddress = obj.EmailAddress,
                    Address = obj.Address,
                    Department = obj.Department,
                    User = new User()
                    {
                        Username = obj.EmailAddress,
                        Password = obj.Password
                    }
                };
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during adding details of a new employee into database.");
                return false;
            }
        }
    }
}