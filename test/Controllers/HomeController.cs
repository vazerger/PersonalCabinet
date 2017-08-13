using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    //test@test.ru
    // Пароль: Test-1

    public class HomeController : Controller
    {
        private Repository repository = new Repository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TransactionList()
        {
            return View();
        }

        public ActionResult ViewDetail()
        {
            return View();
        }

        public ActionResult GroupResultList()
        {
            var model = repository.getGroupResult(User.Identity.Name);
            return View(model);
        }
    }
}