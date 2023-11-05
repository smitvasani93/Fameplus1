using System;
using System.Net;
using System.Web.Mvc;

namespace Transactiondetails.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult RaiseError(string error = null)
        {
            string msg = error ?? "An error has been thrown (intentionally).";
            throw new Exception(msg);
        }

        public ActionResult Error404()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        [AllowAnonymous]
        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }


        public ActionResult Error500()
        {
            Response.TrySkipIisCustomErrors = true;

            var model = new Error500()
            {
                ServerException = Server.GetLastError(),
                HTTPStatusCode = Response.StatusCode
            };

            return View(model);
        }

    }

    public class Error500
    {
        public Exception ServerException { get; set; }
        public int HTTPStatusCode { get; set; }

    }
}