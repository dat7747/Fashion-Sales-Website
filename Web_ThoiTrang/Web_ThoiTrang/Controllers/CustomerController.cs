﻿using Microsoft.AspNetCore.Mvc;

namespace Web_ThoiTrang.Controllers
{
	public class CustomerController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
