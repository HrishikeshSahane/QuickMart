using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuickMartDataAccessLayer.Models;

namespace QuickMartCoreMVCWeb.Repository
{
    public class QuickMartMapper : Profile
    {
        public QuickMartMapper()
        {
            CreateMap<Products, Models.Products>();
            CreateMap<Models.Products, Products>();

            CreateMap<Categories, Models.Categories>();
            CreateMap<Models.Categories, Categories>();

            CreateMap<Models.PurchaseDetails, PurchaseDetails>();
            CreateMap<PurchaseDetails, Models.PurchaseDetails>();


            CreateMap<Models.Feedback, Feedback>();
            CreateMap<Feedback, Models.Feedback>();



        }
    }
}
