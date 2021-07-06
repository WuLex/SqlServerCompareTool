using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServerWebToolProject.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CompareDB()
        {
            return View();
        }

        public IActionResult CopyProcedure()
        {
            return View();
        }
        public IActionResult Explorer()
        {
            return View();
        }

    }
}
