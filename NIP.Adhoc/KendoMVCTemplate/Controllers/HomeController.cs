using NIP.Adhoc.Core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ZenithBankXpath.Core;

namespace ZenithBankXpath.Web.Controllers
{
    public class HomeController : Controller
    {
        Random rand = new Random();
        Random dirRand = new Random();
        public ActionResult RefreshDoughnut(string direction)
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 6);
                t_model.Direction = direction == "Inward" ? TransactionDirection.Inward : TransactionDirection.Outward;//(TransactionDirection)dirRand.Next(0, 2);
                if (direction == "Inward")
                {
                    model.IncomingTransactions.Add(t_model);
                }
                else
                {
                    model.OutgoingTransactions.Add(t_model);
                }
            }
            return PartialView($"~/Views/Partials/{direction}PieChart.cshtml", model);
        }
        public ActionResult RefreshBar(string direction)
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 6);
                t_model.Direction = direction == "InwardBarChart" ? TransactionDirection.Inward : TransactionDirection.Outward;//(TransactionDirection)dirRand.Next(0, 2);
                if (direction == "InwardBarChart")
                {
                    model.IncomingTransactions.Add(t_model);
                }
                else
                {
                    model.OutgoingTransactions.Add(t_model);
                }
            }
            return PartialView($"~/Views/Partials/{direction}.cshtml", model);
        }
        public ActionResult RefreshSummary(string direction)
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 6);
                t_model.Direction = direction == "Inward" ? TransactionDirection.Inward : TransactionDirection.Outward;//(TransactionDirection)dirRand.Next(0, 2);
                if (direction == "Inward")
                {
                    model.IncomingTransactions.Add(t_model);
                }
                else
                {
                    model.OutgoingTransactions.Add(t_model);
                }
            }
            //return PartialView($"~/Views/Partials/{direction}.cshtml", model);
            return PartialView($"~/Views/Partials/{direction}TransactionSummary.cshtml", model);
        }
        public ActionResult Index()
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    TransactionModel t_model = new TransactionModel();
                    t_model.Status = (TransactionStatus)rand.Next(0, 6);
                    t_model.Direction = (TransactionDirection)j;
                    if (t_model.Direction == TransactionDirection.Inward)
                        model.IncomingTransactions.Add(t_model);
                    if (t_model.Direction == TransactionDirection.Outward)
                        model.OutgoingTransactions.Add(t_model);
                }
            }
            return View(model);
        }
        public ActionResult GetSummary()
        {
            ChartModel model = new ChartModel();
            //Random rand = new Random(new System.Guid().GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 3);
                t_model.Direction = (TransactionDirection)dirRand.Next(0, 2);
                if (t_model.Direction == TransactionDirection.Inward)
                    model.IncomingTransactions.Add(t_model);
                if (t_model.Direction == TransactionDirection.Outward)
                    model.OutgoingTransactions.Add(t_model);
            }

            return View("~/Views/Home/IncomingStatistics.cshtml", model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult IncomingStatistics()
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 6);
                t_model.Direction = TransactionDirection.Inward;
                model.IncomingTransactions.Add(t_model);

            }
            return View(model);
        }
        public ActionResult OutgoingStatistics()
        {
            ChartModel model = new ChartModel();
            rand = new Random(new System.Guid(DateTime.Now.ToString("ddMMyyyy-hhmm-mmhh-ssff-ddMMyyhhmmss")).GetHashCode());
            for (int i = 0; i < 1000; i++)
            {
                TransactionModel t_model = new TransactionModel();
                t_model.Status = (TransactionStatus)rand.Next(0, 6);
                t_model.Direction = TransactionDirection.Outward;
                model.OutgoingTransactions.Add(t_model);

            }
            return View(model);
        }
        public ActionResult TransactionSearch(string sessionID)
        {
            ChartModel model = new ChartModel();
            if (!string.IsNullOrWhiteSpace(sessionID))
            {
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string sessionID)
        {
            return RedirectToAction("TransactionSearch", "Home", new {sessionID = sessionID });
        }
    }
}