using System;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using test.Models;

namespace test.Filters
{
    public class PerformanceFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private Stopwatch stopWatch = new Stopwatch();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            filterContext.HttpContext.Items.Add("Stopwatch", stopwatch);

            using (DatabaseContext database = new DatabaseContext())
            {
                Log log = new Log();
                log.LogMessage = "OnActionExecuting";
                log.LogTime = DateTime.Now;

                database.Logs.Add(log);
                database.SaveChanges();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                Log log = new Log();
                log.LogMessage = "OnActionExecuted";
                log.LogTime = DateTime.Now;

                database.Logs.Add(log);
                database.SaveChanges();
            }


            if (filterContext.HttpContext.Items["Stopwatch"] != null)
            {
                Stopwatch stopwatch = (Stopwatch)filterContext.HttpContext.Items["Stopwatch"];
                stopwatch.Stop();

                string message = string.Format("Finished executing controller {0}, action {1} - time spent {2}",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName,
                    stopwatch.ElapsedMilliseconds);

                using (DatabaseContext database = new DatabaseContext())
                {
                    Log log = new Log();
                    log.LogMessage = message;
                    log.LogTime = DateTime.Now;

                    database.Logs.Add(log);
                    database.SaveChanges();
                }

                filterContext.HttpContext.Items.Remove("Stopwatch");
            }
        }
    }
}