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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TransactionList()
        {
            TransactionsModel model = new TransactionsModel();
            model.list = repository.GetTransactions();
            model.resultDesc = repository.GetResultDesc();
            model.resultDescKey = 0;
            return View(model);
        }

        public JsonResult GetTable(string search, int result)
        {
            var list = repository.GetTransactions().OrderBy(c => c.ORDERID).ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                bool IsDigit = search.Length == search.Where(c => char.IsDigit(c)).Count();
                if (IsDigit)
                {
                    list = list.Where(c => c.ORDERID == Convert.ToInt32(search)).ToList();
                }
                else
                {
                    list.Clear();
                }
            }

            list = list.Where(c => c.RESULT == result).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDatails(int id)
        {
            var list = repository.GetTransactions().Where(c => c.ORDERID == id).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
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