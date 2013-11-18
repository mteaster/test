using System;
using System.Diagnostics;
using System.IO;
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

                string path = filterContext.HttpContext.Server.MapPath("~/App_Data/Log.txt");

                if (!File.Exists(path)) 
                {
                    File.Create(path);
                }

                using (StreamWriter sw = File.AppendText(path)) 
                {
                    sw.WriteLine(message);
                }	

                filterContext.HttpContext.Items.Remove("Stopwatch");
            }
        }
    }
}