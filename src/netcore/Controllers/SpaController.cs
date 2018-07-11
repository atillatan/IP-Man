using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace netcore.Controllers
{
    [Route("[controller]/[action]")]
    public class SpaController : Controller
    {
        // GET spa/getusers
        [HttpGet]
        public IEnumerable<dynamic> GetUsers()
        {
            return new List<dynamic> {
                new { Name = "Bob", FamilyName = "Smith", Age = 32, email = "test1" },
                new { Name = "Alice", FamilyName = "Smith", Age = 33, email = "test2" },
                new { Name = "Amy", FamilyName = "Smith", Age = 32, email = "test3" },
                new { Name = "Adam", FamilyName = "Smith", Age = 32, email = "test4" }
             };
        }
    }
}