using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using FluentValidation;
using Newtonsoft.Json;
using NLog;
using TransactionApp.Common.Exceptions;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;

namespace TransactionApp.Filters
{
    public class HandleExceptionsAttribute : IExceptionFilter
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            if (TryCreateResponse(filterContext.Exception, out var response))
            {
                filterContext.Result = response.Item2;
                filterContext.HttpContext.Response.StatusCode = (int) response.Item1;
                filterContext.ExceptionHandled = true;
                Logger.Error(response.Item2);
            }
        }


        private static bool TryCreateResponse(Exception e, out Tuple<HttpStatusCode, ContentResult> response)
        {
            response = null;

            switch (e.GetType().Name)
            {
                case nameof(InvalidFileException):
                    var invalidModelException = (InvalidFileException) e;
                    var invalidModel = new InvalidFileException(invalidModelException.Errors);
                    var invalidModelJson = JsonConvert.SerializeObject(invalidModel.Errors, Formatting.Indented);
                    response = CreateErrorResponse(invalidModelJson, contentType: "application/json");

                    break;
                case nameof(ValidationException):
                    var validationException = (ValidationException) e;
                    var validationJson = JsonConvert.SerializeObject(
                        validationException.Errors.Select(x => new
                        {
                            Item = x.PropertyName.Split('.')[0], PropertyName = x.PropertyName.Split('.')[1],
                            x.ErrorMessage
                        }), Formatting.Indented);
                    response = CreateErrorResponse(validationJson, contentType: "application/json");

                    break;
            }

            return response != null;
        }

        private static Tuple<HttpStatusCode, ContentResult> CreateErrorResponse(string message,
            HttpStatusCode code = HttpStatusCode.BadRequest,
            string contentType = "text/plain")
        {
            var result = new ContentResult
            {
                Content = message, ContentEncoding = Encoding.UTF8, ContentType = contentType
            };
            return new Tuple<HttpStatusCode, ContentResult>(code, result);
        }
    }
}