using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using TransactionApp.Common.Exceptions;

namespace TransactionApp.Filters
{
    public class HandleExceptionsAttribute:ExceptionFilterAttribute
    { public override void OnException(HttpActionExecutedContext context)
        {
            if (TryCreateResponse(context.Exception, out var response))
            {
                context.Response = response;
            }
            else
            {
                base.OnException(context);
            }
        }

        public override async Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            if (TryCreateResponse(context.Exception, out var response))
            {
                context.Response = response;
            }
            else
            {
                await base.OnExceptionAsync(context, cancellationToken);
            }
        }
         private bool TryCreateResponse(Exception e, out HttpResponseMessage response)
        {
            response = null;

            switch (e.GetType().Name)
            {
                case nameof(BadRequestResponse):
                    var invalidModelException = (BadRequestResponse) e;
                    var error = new BadRequestResponse(invalidModelException.Errors);
                    var json = JsonConvert.SerializeObject(error);
                    response = CreateErrorResponse(json, contentType: "application/json");

                    break;

                // case nameof(DeleteDependenciesException):
                //     var deleteDependenciesException = (DeleteDependenciesException) e;
                //     var dependencyJson = JsonConvert.SerializeObject(deleteDependenciesException.DependenciesValidationResult);
                //     response = CreateErrorResponse(dependencyJson, HttpStatusCode.BadRequest, contentType: "application/json");
                //
                //     break;
            }

            return response != null;
        }
         private static HttpResponseMessage CreateErrorResponse(string message,
             HttpStatusCode code = HttpStatusCode.BadRequest,
             string contentType = "text/plain")
         {
             var result = new HttpResponseMessage(code)
             {
                 Content = new StringContent(message, Encoding.UTF8, contentType)
             };

             result.Headers.CacheControl = new CacheControlHeaderValue
             {
                 NoCache = true,
                 NoStore = true,
             };

             return result;
         }
        
    }
}