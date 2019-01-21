﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Common;
using Resources;
using DBModel.DAO;
using DBModel.ET;
using Website.Areas.Models;
using Website.Areas.Controllers;
using Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
       
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult MenuPartial()

        {
            CategoryDao dbCat = new CategoryDao();
            List<Category> menusource = dbCat.ToListActiveHome(); // get your menus here
            ViewBag.Menus = CreateVM(null, menusource);  // transform it into the ViewModel
            return PartialView(ViewBag.Menus);
        }
        public List<CategoryViewModel> CreateVM(long? CategoryID, List<Category> source) {

            return (from cat in source
                   where cat.ParentID == CategoryID
                    select new CategoryViewModel()
                   {
                       CategoryID = cat.CategoryID,
                       Name = cat.Name,
                       MetaTite = cat.MetaTite,
                       ParentID = cat.ParentID,
                       Image = cat.Image,
                       Description = cat.Description,
                       DisplayOrder = cat.DisplayOrder,
                       SeoTite = cat.SeoTite,
                       MetakeyWords = cat.MetakeyWords,
                       MetaDescription = cat.MetaDescription,
                       Status = cat.Status,
                       ShowOnHome = cat.ShowOnHome,
                       Position = cat.Position,
                       LanguageID = cat.LanguageID,
                       Children = CreateVM(cat.CategoryID, source)
                   }).ToList();
        }
    }
}