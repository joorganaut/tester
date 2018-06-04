using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ZenithBankXpath.Core;
using ZenithBankXpath.Web.Models;

namespace ZenithBankXpath.Web.Controllers
{
    public class XTransactionController : Controller
    {
        // GET: XTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new TransactionModel());
        }

        [HttpPost]
        public ActionResult Create(TransactionModel model)
        {
            //Session["TransModel"] = model;

            var enmodel = new EncryptModel();
            enmodel.accountNumber = model.AccountNo;
            enmodel.channelId = "10";
            int mid = 0;
            int.TryParse(model.MerchantId, out mid);
            enmodel.merchantId = mid;
            //SecurityHelper sechelper = new SecurityHelper();
            //var param = sechelper.EncryptRendererParam(enmodel);
            var rendurl = "http://172.29.30.43:3000/?param=";// + param;
            return Json(rendurl, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult AccountDetails(string accountno)
        {
            //var A = accountno.Split('|');
            //var account = AccountSingleton.Instance.Accounts.FirstOrDefault(m => m.AccountNo.Trim().Equals(accountno.Trim()));
            //AccountServices asvc = new AccountServices();
            //var acctdetails = asvc.GetAccountDetails(accountno);
            TransactionModel model = new TransactionModel();
            model.AccountNo = accountno;
            //model.AvailableBalance = acctdetails.AvailableBalance;
            //model.Currency = acctdetails.Currency;
            //if(account != null)
            //model.MerchantId = account.MerchantId;
            Session["transmodel"] = model;

            return PartialView("_AccountDetails", model);
        }

        public ActionResult TransactionDetails()
        {
            TransactionModel model = (TransactionModel)Session["transmodel"];

            return PartialView("_TransactionDetails", model);
        }

        public ActionResult AdditionalFields()
        {
            return View(new TransactionModel());
        }

        public ActionResult PostTransaction()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult PostTransaction()
        //{
        //    return View();
        //}

        public JsonResult GetAccounts(string text)
        {
            //var accounts = ;// = AccountSingleton.Instance.Accounts;

            //if (!string.IsNullOrEmpty(text))
            //{
            //    accounts = accounts.Where(p => p.AccountName.ToLower().Contains(text.ToLower()) 
            //    || p.AccountNo.ToLower().Contains(text.ToLower()) 
            //    || p.MerchantId.ToLower().Contains(text.ToLower())).ToList();
            //}
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

    }
}