using CursoMvc.Models;
using CursoMvc.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace CursoMvc.Files
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            
            return View();
        }




            
        

        [HttpPost]
        public ActionResult Save(FileViewModel model)
        {
            string fileName = null;
            string RuteSite = Server.MapPath("~/");

            if (model.File != null) 
            {
                fileName = model.File.FileName;
                string PathFile = Path.Combine(RuteSite + "Files\\Files\\" + fileName);
                model.File.SaveAs(PathFile);   
            }
            else
            {
                List<FileViewModel> partnumber = new List<FileViewModel>();
                    using(cursomvcEntities db = new cursomvcEntities())
                    {
                        //partnumber = (from d in db.items
                        //              where d.partnumber == model.Partnumber && d.ilustration != null
                        //              select new FileViewModel
                        //              {
                        //                  Partnumber = model.Partnumber,
                        //              }).ToList();

                        
                        //if (partnumber.Count() > 0 )
                        //{
                        //    items oItems = new items();
                        //    oItems.quantity = model.Quantity;

                        //    sparepartLocation oSpare = new sparepartLocation();
                        //    oSpare.location = model.Location;
                        //    oSpare.partnumber = model.Partnumber;
                        //    oSpare.username = "pablo_rivera";
                        //    oSpare.quantity = model.Quantity;

                        //    //return Content("Numero de parte ya existe :(");

                        //}
                        //else
                        //{
                            MemoryStream target = new MemoryStream();
                            model.SpareImage.InputStream.CopyTo(target);// para convertir el htttpostedfilebase a byte[]

                            items oItems = new items();
                            oItems.partnumber = model.Partnumber;
                            oItems.description = model.Description;
                            oItems.ilustration = target.ToArray();
                            oItems.idmachine = model.IdMachines;
                            oItems.quantity = model.Quantity;
                            oItems.idstatus = 1;

                            sparepartLocation oSpare = new sparepartLocation();
                            oSpare.idlocation = model.IdLocation;
                            oSpare.partnumber = model.Partnumber;
                            oSpare.username = "pablo_rivera";
                            oSpare.dateUpdate = DateTime.Now;
                            oSpare.quantity = model.Quantity;

                            db.items.Add(oItems);
                            db.sparepartLocation.Add(oSpare);
                            db.SaveChanges();
                       // }
                       

                    }
            }
  
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            @TempData["Message"] = "Se cargo el archivo";
            return RedirectToAction("Index");
        }

        

        
    }
}  