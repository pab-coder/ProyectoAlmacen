using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CursoMvc.Models;
using CursoMvc.Models.TableViewModels;
using System.Data.Entity.Infrastructure;
using System.Collections;
using System.Xml.Linq;

namespace CursoMvc.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Enter(string user, string password)
        {
            try
            {
                using (cursomvcEntities db = new cursomvcEntities())
                {
                    var name = "";
                    var lst = (from d in db.user
                              where d.email == user && d.password == password && d.idState == 1
                              select d);

                    // ciclo for para obtener el nombre de usuario del correo al inicio de sesi
                    foreach (var item in lst)
                    {
                        if (item.email == user)
                        {
                            name = item.name;
                        }
                    }

                    if (lst.Count() > 0)
                    {
                        user oUser = lst.First();
                        Session["user"] = oUser;
                        ViewBag.User = "Hola," + name;
                        return Content("1");
                    }

                    else
                    {
                        return Content("Usuario invalido :(");
                    }
                }

                
            }
            catch(Exception ex) 
            {
                return Content("ocurrio un error :("+ex.Message  ); 
            }

        }

        public string Name() {


            return Name();

        }
    }
}