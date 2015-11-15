using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace eurovision.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Vote(string username, int place1, int place2, int place3)
		{
			bool voteState = false;
			var cacheState = HttpRuntime.Cache.Get("votestate");
			if (cacheState != null)
			{
				voteState = (bool)cacheState;
			}

			string returnMessage;
			if (voteState)
			{
				HttpRuntime.Cache.Insert(
					username,
					new Vote() {username = username, Place1 = place1, Place2 = place2, Place3 = place3},
					null,
					DateTime.Now.AddDays(1),
					Cache.NoSlidingExpiration);
				returnMessage = "Din röst är registrerad!";
			}
			else
			{
				returnMessage = "Röstning ej aktiverat!";
			}


			return Json(new { msg = returnMessage }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetVotes(string username)
		{
			var data = HttpRuntime.Cache.Get(username);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult StateVoting(bool state)
		{
			HttpRuntime.Cache.Insert(
				"votestate",
				state,
				null,
				DateTime.Now.AddDays(1),
				Cache.NoSlidingExpiration);

			return Json(new {msg = state ? "Röstning påslagen" : "Röstning avslagen"}, JsonRequestBehavior.AllowGet);
		}
	}

	public class Vote
	{
		public string username { get; set; }
		public int Place1 { get; set; }
		public int Place2 { get; set; }
		public int Place3 { get; set; }
	}
}