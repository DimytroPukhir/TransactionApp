using System.Web.Http;
using TransactionApp.Filters;

namespace TransactionApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new HandleExceptionsAttribute());
        }
    }
}