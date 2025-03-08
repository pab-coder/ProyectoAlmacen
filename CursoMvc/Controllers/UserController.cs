using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using CursoMvc.Models;
using CursoMvc.Models.TableViewModels;
using CursoMvc.Models.ViewModels;

namespace CursoMvc.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<UserTableViewModel> lst = null;
            using (cursomvcEntities db = new cursomvcEntities())
            {
                lst = (from d in db.user
                       where d.idState == 1
                       orderby d.email
                       select new UserTableViewModel
                       {
                           Email = d.email,
                           Id = d.id,
                           Edad = d.edad,
                           Name = d.name,
                       }).ToList();
            }
            return View(lst);
        }

        [HttpGet]
        public ActionResult Add() {

            return View();
        }

        [HttpPost]
        public ActionResult Add(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            List<UserTableViewModel> email = null;
            using (var db = new cursomvcEntities())
            {

                email = (from d in db.user
                         where d.email == model.Email
                         select new UserTableViewModel
                         {
                             Email = d.email,
                         }).ToList();

                if (email.Count() > 0)
                {
                    return Content("El usuario ya existe :(");
                }
                else
                {
                    user oUser = new user();
                    oUser.idState = 1;
                    oUser.email = model.Email;
                    oUser.name = model.Name;
                    oUser.edad = model.Edad;
                    oUser.password = model.Password;
                    
                    
                    db.user.Add(oUser);

                    db.SaveChanges();

                }
                return Content("1");

            }
        }

        public ActionResult Edit(int Id)
        {
            EditUserViewModel model = new EditUserViewModel();

            using (var db = new cursomvcEntities())
            {
                var oUser = db.user.Find(Id);
                model.Email = oUser.email;
                model.Edad = (int)oUser.edad;
                model.Id = oUser.id;

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new cursomvcEntities())
            {
                var oUser = db.user.Find(model.Id);
                oUser.email = model.Email;
                oUser.edad = (int)model.Edad;
                 

                if(model.Password != null && model.Password.Trim() != "")
                {
                    oUser.password = model.Password;
                }

                db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }
            return Content("1");

        }

        public ActionResult Delete(int Id)
        {
      
            using (var db = new cursomvcEntities())
            {
                var oUser = db.user.Find(Id);
                oUser.idState = 3; //Eliminaremos

                db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }
            return Content("1");
        }
    }
}