using Microsoft.AspNetCore.Mvc;
using QuickMartDataAccessLayer.Models;
using QuickMartDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace QuickMartCoreMVCApp.Controllers
{
    public class AdminController : Controller
    {

            private readonly QuickMartDBContext _context;
            QuickMartRepository repObj;
            private readonly IMapper _mapper;
            public AdminController(QuickMartDBContext context, IMapper mapper)
            {
                _context = context;
                repObj = new QuickMartRepository(_context);
                _mapper = mapper;
            }


            // GET: /<controller>/
            public IActionResult AdminHome()
            {
                //ViewBag.CategoryList = repObj.GetCategories();
                //var productList = repObj.GetProducts();
                //var products = new List<Models.Products>();
                //foreach (var product in productList)
                //{
                //    products.Add(_mapper.Map<Models.Products>(product));
                //}
                //var filteredProducts = products.Where(model => model.CategoryId == categoryId);
                //return View(filteredProducts);

                var recentpurchases = repObj.RecentPurchases();
                List<MostPurchases> mostpurchases = repObj.MostPurchases();
                //var purchases = new List<Models.PurchaseDetails>();
                List<string> purchases = new List<string>();
                List<string> mostpurchaselist = new List<string>();
                if (recentpurchases != null)
                {
                    foreach (var purchase in recentpurchases)
                    {
                        purchases.Add((purchase));
                    }
                    ViewBag.TopProducts = purchases;
                }
                else
                {
                    ViewBag.TopProducts = null;

                }

            if (mostpurchases.Count > 0)
            {
                foreach(MostPurchases mp in mostpurchases)
                {
                    mostpurchaselist.Add(mp.ProductName + ":" + mp.TotalQuantity);
                }
                ViewBag.MostPurchases = mostpurchaselist;
            }


                return View();
            }

    }
}
