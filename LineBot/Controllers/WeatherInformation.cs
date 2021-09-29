using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineBot.Controllers
{
    public class WeatherInformation : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
