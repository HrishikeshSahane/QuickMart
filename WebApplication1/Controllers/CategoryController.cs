using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickMartDataAccessLayer;
using QuickMartDataAccessLayer.Models;
using AutoMapper;

namespace QuickMartCoreMVCApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly QuickMartDBContext _context;
        QuickMartRepository repObj;
        private readonly IMapper _mapper;


        public CategoryController(QuickMartDBContext context, IMapper mapper)
        {
            _context = context;
            repObj = new QuickMartRepository(_context);
            _mapper = mapper;
        }


        // GET: /<controller>/
        public IActionResult ViewCategories()
        {
            var lstEntityCategories = repObj.GetCategories();
            List<Models.Categories> lstModelCategories = new List<Models.Categories>();
            foreach (var category in lstEntityCategories)
            {
                lstModelCategories.Add(_mapper.Map<Models.Categories>(category));
            }
            return View(lstModelCategories);
        }




        public IActionResult AddCategory()
        {
            return View();
        }


        public IActionResult SaveAddedCategory(Models.Categories category)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    status = repObj.AddCategory(_mapper.Map<Categories>(category));
                    if (status)
                        return RedirectToAction("ViewCategories");
                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View("AddCategory", category);
        }



        public IActionResult EditCategory(Models.Categories catObj)
        {
            return View(catObj);
        }


        [HttpPost]
        public IActionResult SaveUpdatedCategory(Models.Categories category)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    status = repObj.UpdateCategory(_mapper.Map<Categories>(category));
                    if (status)
                        return RedirectToAction("ViewCategories");
                    else
                        return View("Error");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error", ex);

                }
            }
            return View("EditCategory", category);
        }




        public IActionResult DeleteCategory(Models.Categories category)
        {
            return View(category);
        }


        [HttpPost]
        public IActionResult SaveDeletion(int categoryId)
        {
            bool status = false;
            try
            {
                status = repObj.DeleteCategory(categoryId);
                if (status)
                    return View("SaveDeletion");
                else
                    return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }




    }
}
