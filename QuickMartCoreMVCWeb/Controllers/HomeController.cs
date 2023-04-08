using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QuickMartCoreMVCWeb.Models;
using QuickMartDataAccessLayer.Models;
using QuickMartDataAccessLayer;
using Microsoft.AspNetCore.Http;

namespace QuickMartCoreMVCWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuickMartDBContext _context;
        QuickMartRepository repObj;
        public HomeController(QuickMartDBContext context)
        {
            _context = context;
            repObj = new QuickMartRepository(_context);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Register()
        {

            return View();
        }


        public IActionResult FAQ()
        {

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {

            ViewBag.EmailId = "admin@quickKart.com";
            ViewData["ContactNumber"] = 9876543210;
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        public IActionResult CheckRole(IFormCollection frm)
        {
            string userId = frm["name"];
            string password = frm["pwd"];
            string checkbox = frm["RememberMe"];

            if (checkbox == "on")
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("UserId", userId, option);
                Response.Cookies.Append("Password", password, option);
            }
            string username = userId.Split('@')[0];
            byte? roleId = repObj.ValidateCredentials(userId, password);
            if (roleId == 1)
            {
                //HttpContext.Session.SetString("username",username);
                HttpContext.Session.SetString("Admin_userId", userId);
                return Redirect("/Admin/AdminHome?username=" + username);
            }
            else if (roleId == 2)
            {
                HttpContext.Session.SetString("Customer_userId", userId);
                ViewBag.UserID = userId;
                return Redirect("/Customer/CustomerHome?username=" + username);
            }

            else
            {
                ViewData["Message"] = "Invalid Credentials";
            }
            return View("Login");
        }




        public IActionResult UserRegistration()
        {
            return View();

        }


        [HttpPost]
        public IActionResult UserRegistration(Users usermodel)

        {
            bool status;
            status = repObj.RegisterUser(usermodel.EmailId, usermodel.UserPassword, usermodel.Gender, usermodel.DateOfBirth, usermodel.Address);

            if (status)
            {
                ViewData["SuccessMessage"] = "Successfully Registered";
                

            }
            else
            {
                ViewData["SuccessMessage"] = null;
                ViewData["FailureMessage"] = "Incorrect details entered OR user with same e-mailId already registred. Please try again.";
            }

            //return RedirectToAction("Index", "Home");
            return View("Register");

        }

        public IActionResult EditProfile()
        {
            string emailId= HttpContext.Session.GetString("Customer_userId").ToString();
            Users userObj = repObj.GetUserDetails(emailId);
            return View(userObj);
        }

        [HttpPost]
        public IActionResult SaveEditedProfile(Users usermodel)
        {
            bool status = false;
            if (String.IsNullOrEmpty(usermodel.EmailId)){
                usermodel.EmailId = HttpContext.Session.GetString("Customer_userId").ToString();
            }
            Users userObj = repObj.UpdateProfile(usermodel,out status);
            if (status)
            {
                ViewData["SuccessMessage"] = "Profile Updated Successfully";
                return View("EditProfileSuccess");
            }
            else
            {
                
                return View("Error");
            }
        }

        public JsonResult GetCoupons()
        {
            Random random = new Random();
            Dictionary<string, string> data = new Dictionary<string, string>();
            string[] value = new String[5];
            string[] key = { "Arts", "Electronics", "Fashion", "Home", "Toys" };
            for (int i = 0; i < 5; i++)
            {
                string number = "RUSH" + random.Next(1, 10).ToString() + random.Next(1, 10).ToString() + random.Next(1, 10).ToString();
                value[i] = number;
            }
            for (int i = 0; i < 5; i++)
            {
                data.Add(key[i], value[i]);
            }
            return Json(data);
        }




    }
}
