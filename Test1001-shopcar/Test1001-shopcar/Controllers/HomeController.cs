using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test1001_shopcar.Models;


namespace Test1001_shopcar.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                var result = (from s in db.Products select s).ToList();
                return View(result);
            }
            
        }
    }
}