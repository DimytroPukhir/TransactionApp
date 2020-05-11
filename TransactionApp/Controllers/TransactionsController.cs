﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using TransactionApp.Common.Exceptions;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DomainModel.Models;
using TransactionApp.Models;
using TransactionApp.Services.Abstractions;

namespace TransactionApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsProvider _transactionsProvider;
        private readonly IMapper _mapper;
        private readonly List<string> _extensions = new List<string> {".csv", ".xml"};
        private readonly int _appropriateFileSize = 1024;

        public TransactionsController(ITransactionsService transactionsService,
            ITransactionsProvider transactionsProvider, IMapper mapper)
        {
            _transactionsService = transactionsService;
            _transactionsProvider = transactionsProvider;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase postedFile)
        {
            if (postedFile == null) return View();
            try
            {
                var fileExtension = Path.GetExtension(postedFile.FileName);
                if (!_extensions.Contains(fileExtension))
                {
                    var errors = new List<string> {"UnknownFormat"};
                    if (postedFile.ContentLength > _appropriateFileSize)
                    {
                        errors.Add("File size more than 1MB");
                    }
                    throw new InvalidFileException(errors);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            await _transactionsService.AddTransactionsAsync(postedFile.InputStream);
            var items = await _transactionsProvider.GetAllAsync();
            var transactionViewModels = _mapper.Map<Transaction, TransactionViewModel>(items);
            return View(transactionViewModels);
        }

        public async Task<ActionResult> GetAllAsync()
        {
            var items = await _transactionsProvider.GetAllAsync();
            var transactionViewModels = _mapper.Map<Transaction, TransactionViewModel>(items);
            if (transactionViewModels == null)
            {
                ViewBag.Message = "List are empty";
                return View();
            }

            return View(transactionViewModels);
        }

        public async Task<ActionResult> GetFilteredIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetFilteredAsync(TransactionFilterModel filters)
        {
            var items = await _transactionsProvider.GetFiltered(filters.CurrencyCode,
                filters.StartDate, filters.EndDate , filters.Status);
            var transactionViewModels = _mapper.Map<Transaction, TransactionViewModel>(items);
            if (transactionViewModels == null)
            {
                ViewBag.Message = "List are empty";
                return View();
            }

            return View(transactionViewModels);
        }
    }
}