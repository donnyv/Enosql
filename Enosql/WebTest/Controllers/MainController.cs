using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using enosql;

namespace WebTest.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test6-30-13.jdb");
            db.GetCollection("Persons").Insert("{ Lost: 23 }");
            return View();
        }

       
    }
}
