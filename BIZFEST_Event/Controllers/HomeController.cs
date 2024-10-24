﻿using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BIZFEST_Event.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return RedirectToAction("UserView", "User");
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult Cancellation()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}