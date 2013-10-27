﻿using System.Web;
using System.Web.Mvc;
using test.Stuff;

namespace test
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute
            {
                ExceptionType = typeof(BandNotFoundException),
                View = "Exception",
            });

            filters.Add(new HandleErrorAttribute());
        }
    }
}