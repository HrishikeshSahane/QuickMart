using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickMartDataAccessLayer;
using QuickMartDataAccessLayer.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickMartCoreMVCApp.Controllers
{
    public class CustomerController : Controller
    {
        // GET: /<controller>/
        private readonly QuickMartDBContext _context;
        QuickMartRepository repObj;
        private readonly IMapper _mapper;
        public CustomerController(QuickMartDBContext context, IMapper mapper)
        {
            _context = context;
            repObj = new QuickMartRepository(_context);
            _mapper = mapper;
        }
        

        public IActionResult CustomerHome()
        {
            List<string> recentpurchases = new List<string>();
            recentpurchases = repObj.RecentPurchases();
            //var purchases = new List<Models.PurchaseDetails>();
            List<string> purchases = new List<string>();
            if (recentpurchases != null)
            {
                foreach (var purchase in recentpurchases)
                {
                    purchases.Add(purchase);
                }
                ViewBag.TopProducts = purchases;
            }
            return View();
        }


        public IActionResult GiveFeedback()
        {
            ViewData["FeedbackEmail"] = HttpContext.Session.GetString("Customer_userId").ToString();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Home");
        }

        [HttpPost]
        public IActionResult SaveAddedFeedback(Models.Feedback feedback)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(feedback.EmailId)){
                        feedback.EmailId = HttpContext.Session.GetString("Customer_userId").ToString();
                    }
                    status = repObj.AddFeedback(_mapper.Map<Feedback>(feedback));
                    if (status)
                    {
                        ViewData["FeedbackEmail"] = HttpContext.Session.GetString("Customer_userId").ToString();
                        ViewData["FeedbackSuccess"] = "Thankyou for the feedback!";
                        return View("GiveFeedback");
                    }

                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View("GiveFeedback");
        }

    }
}
