using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using QuickMartDataAccessLayer;
using QuickMartDataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickMartCoreMVCApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly QuickMartDBContext _context;
        QuickMartRepository repObj;
        public string SaveUserID { get; set; }
        public int finalPrice { get; set; }
        private readonly IMapper _mapper;
        public PurchaseController(QuickMartDBContext context, IMapper mapper)
        {
            _context = context;
            repObj = new QuickMartRepository(_context);
            _mapper = mapper;
        }




        public IActionResult GetProductForCategory(byte? categoryId)
        {
            ViewBag.CategoryList = repObj.GetCategories();
            var productList = repObj.GetProducts();
            var products = new List<Models.Products>();
            foreach (var product in productList)
            {
                products.Add(_mapper.Map<Models.Products>(product));
            }
            var filteredProducts = products.Where(model => model.CategoryId == categoryId);
            return View(filteredProducts);
        }


        // GET: /<controller>/
        public IActionResult PurchaseProduct(Models.Products productObj)
        {
            Models.PurchaseDetails purchaseObj = new Models.PurchaseDetails();
            purchaseObj.EmailId = HttpContext.Session.GetString("Customer_userId").ToString();
            purchaseObj.ProductId = productObj.ProductId;
            purchaseObj.DateOfPurchase = DateTime.Now;
            TempData["ProductName"] = productObj.ProductName;
            
            return View(purchaseObj);
        }


        public IActionResult SavePurchase(Models.PurchaseDetails purchaseObj)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["QuantityPurchased"] = purchaseObj.QuantityPurchased;



                    status = repObj.PurchaseProduct(_mapper.Map<PurchaseDetails>(purchaseObj));
                    status = true;
                    decimal totalPrice=repObj.AddToCart(_mapper.Map<PurchaseDetails>(purchaseObj));



                    if (status)
                        return View("Purchase_success");
                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View("PurchaseProduct", purchaseObj);

        }

        public IActionResult GetCartDetails(string UserId)
        {
            string jsonstring = String.Empty;
            decimal FinalCost = 0;
            if (UserId == null)
            {
                UserId = HttpContext.Session.GetString("Customer_userId").ToString();
            }
            List<Items> itemList = repObj.GetCartDetails(UserId,out jsonstring);
            ViewBag.UserID = UserId;
            ViewBag.TotalCost = 0;
            foreach (Items item in itemList)
            {
                ViewBag.TotalCost += item.TotalPrice;
                FinalCost += item.TotalPrice;
            }
            TempData["FinalCost"] = FinalCost.ToString();
            TempData["Order"] = jsonstring;
            return View(itemList);

        }

        public IActionResult RemoveItemFromCart(string productName, string emailId, string qtyPurchased)
        {
            string jsonstring = String.Empty;
            decimal FinalCost = 0;
            if (emailId == null)
            {
                emailId= HttpContext.Session.GetString("Customer_userId").ToString();
            }
            List<Items> itemList = repObj.RemoveItemFromCart(productName, emailId,out jsonstring);
            bool isRemoved = repObj.RemoveFromPurchaseDetails(productName, emailId, qtyPurchased);
            ViewBag.TotalCost = 0;
            foreach (Items item in itemList)
            {
                ViewBag.TotalCost += item.TotalPrice;
                FinalCost += item.TotalPrice;

            }
            TempData["FinalCost"] = FinalCost.ToString();
            TempData["Order"] = jsonstring;
            return View("GetCartDetails", itemList);
        }

        public IActionResult GetCheckoutDetails(string id)
        {

            string stritems = TempData["Order"].ToString();
            string UserId = HttpContext.Session.GetString("Customer_userId").ToString();
            decimal TotalPrice = Convert.ToDecimal(TempData["FinalCost"]);
            bool isorderSaved = repObj.SaveOrder(stritems, TotalPrice, UserId);
            //ViewData["CheckoutFinalPrice"] = "0";
            if (isorderSaved)
            {
                return View("Checkout_success");
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult ShowOrderedItems(Order order)
        {
            List<Items> itemList = repObj.DeserializaOrdersString(order.FinalItems);
            TempData["IsPaymentReceived"] = order.IsPaymentReceived.ToString();
            TempData["OrderId"] = order.OrderId.ToString(); ;
            return View("ShowOrderedItems", itemList);
        }

        public IActionResult ShowAllOrders()
        {
            List<Order> orderList = new List<Order>();
            
            orderList = repObj.FetchAllOrders();
            return View("ShowAllOrders",orderList);
        }

        public IActionResult PaymentConfirmation()
        {
            List<Order> orderList = new List<Order>();
            orderList = repObj.UpdateSelectedOrder(TempData["OrderId"].ToString());
            return View("ShowAllOrders", orderList);
        }

    }
}
