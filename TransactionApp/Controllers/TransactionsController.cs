using System.Web.Mvc;

namespace TransactionApp.Controllers
{
    public class TransactionsController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}