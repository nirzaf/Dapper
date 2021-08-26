using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonRepo;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonRepo)
        {
            _logger = logger;
            _bonRepo = bonRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companies = _bonRepo.GetAllCompanyWithEmployees();
            return View(companies);
        }

        public IActionResult AddTestRecords()
        {

            Company company = new Company()
            {
                Name = Faker.Company.Name() ,
                Address = Faker.Address.StreetAddress(),
                City = Faker.Address.City(),
                PostalCode = Faker.Address.UkPostCode(),
                State = Faker.Address.UsState(),
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Phone = Faker.Phone.Number(),
                Title = Faker.Company.Name()
            });

            company.Employees.Add(new Employee()
            {
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Phone = Faker.Phone.Number(),
                Title = Faker.Company.Name()
            });
            _bonRepo.AddTestCompanyWithEmployeesWithTransaction(company);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveTestRecords()
        {
            int[] companyIdToRemove = _bonRepo.FilterCompanyByName("Test").Select(i => i.CompanyId).ToArray();
            _bonRepo.RemoveRange(companyIdToRemove);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
