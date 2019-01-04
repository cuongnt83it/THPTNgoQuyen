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
namespace Website.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class BusinessController : BaseController
    {
        DBContext db = null;
        // GET: Business
        public ActionResult Index()
        {
             db = new DBContext();
            return View(db.Businesses.ToList());

        }
        // GET: Admin/Business
        public ActionResult UpdateBusiness()
        {
            db = new DBContext();
            ReflectionController reflection = new ReflectionController();
            List<Type> listController = reflection.GetControllers("Website.Areas.Admin");
            List<string> listControllerOld = db.Businesses.Select(x => x.BusinessID).ToList();
            List<string> listPermistionOld = db.Permissions.Select(x => x.Name).ToList();
            foreach (var c in listController)
            {
                if (!listControllerOld.Contains(c.Name))
                {
                    Business b = new Business();
                    b.BusinessID = c.Name;
                    b.BusinessName = "Chưa có mô tả";
                    db.Businesses.Add(b);
                }
                List<string> listPermistion = reflection.GetAction(c);
                foreach (var p in listPermistion)
                {
                    if (!listPermistionOld.Contains(c.Name + "-" + p))
                    {
                        Permission permistion = new Permission();
                        permistion.Name = c.Name + "-" + p;
                        permistion.Description = "Chưa có mô tả";
                        permistion.BusinessID = c.Name;
                        db.Permissions.Add(permistion);
                    }
                }

            }
            if (db.SaveChanges() > 0)
            {
                SetAlert("Cập nhật thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Không thành công", "danger");
            }


            return RedirectToAction("Index");
        }
        // GET: Admin/Business
        public ActionResult Edit(string id)
        {
             db = new DBContext();
            var b = db.Businesses.SingleOrDefault(x => x.BusinessID == id);
            return View(b);
        }
        // GET: Admin/Business
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Business model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                     db = new DBContext();
                    Business b = db.Businesses.SingleOrDefault(x => x.BusinessID == model.BusinessID);
                    b.BusinessName = model.BusinessName;


                    if (db.SaveChanges() > 0)
                    {
                        SetAlert("Thêm thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Không thêm được", "danger");
                    }
                }
                return View();
            }
            catch
            {

                SetAlert("Không thêm được", "danger");
                return View();
            }


        }

        // POST: Group/Delete/5
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here

                 db = new DBContext();

                db.Businesses.Remove(db.Businesses.Find(id));
                db.SaveChanges();

                var perm = db.Permissions.Where(p => p.BusinessID == id).ToList<Permission>();
                foreach (Permission pr in perm)
                {
                    db.Permissions.Remove(pr);
                }
                db.SaveChanges();
                // SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                // SetAlert("Không xóa được", "danger");
                return View();
            }
        }
    }
}