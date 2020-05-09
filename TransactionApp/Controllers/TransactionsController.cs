using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TransactionApp.Models;
using TransactionApp.Services.Abstractions;

namespace TransactionApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;
        private readonly List<string> _extensions =new List<string>{".csv",".xml"};

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedFile)
        {
            if (postedFile == null) return View();
            try
            {
                string fileExtension = Path.GetExtension(postedFile.FileName);

                //Validate uploaded file extension and return error.
                if (!_extensions.Contains(fileExtension))
                {
                    ViewBag.Message = "“Unknown format";
                       
                }

                _transactionsService.ProcessImportAsync(postedFile.InputStream);

            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            IEnumerable<TransactionViewModel> a = new[] {new TransactionViewModel(), new TransactionViewModel(),};
            return View(a);
        }
    }
}
