using System;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using test.Models;
using test.Models.Test;

namespace test.Filters
{
    public class PerformanceFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private Stopwatch stopWatch = new Stopwatch();


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            filterContext.HttpContext.Items.Add("Stopwatch", stopwatch);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (filterContext.HttpContext.Items["Stopwatch"] != null)
            {
                Stopwatch stopwatch = (Stopwatch)filterContext.HttpContext.Items["Stopwatch"];
                stopwatch.Stop();

                string message = string.Format("Controller: {0}, Action: {1} - Time elapsed: {2} ms",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName,
                    stopwatch.ElapsedMilliseconds);

                using (DatabaseContext database = new DatabaseContext())
                {
                    Log log = new Log();
                    log.LogMessage = message;
                    log.LogTime = DateTime.UtcNow;

                    database.Logs.Add(log);
                    database.SaveChanges();
                }

                filterContext.HttpContext.Items.Remove("Stopwatch");
            }
        }
    }
}