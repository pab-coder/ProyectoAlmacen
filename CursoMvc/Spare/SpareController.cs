using CursoMvc.Models;
using CursoMvc.Models.TableViewModels;
using CursoMvc.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CursoMvc.Spare
{
    public class SpareController : Controller
    {
        // GET: Spare
        public ActionResult Index()
        {
            List<SpareTableViewModel> tbl = null;

            using (cursomvcEntities db = new cursomvcEntities())
            {
                tbl = (from d in db.items
                       join m in db.machines on d.idmachine equals m.id
                       join l in db.lpn on d.partnumber equals l.partumber.DefaultIfEmpty()                     where d.idstatus == 1
                       select new SpareTableViewModel
                       {
                           Id = d.id,
                           Partnumber = d.partnumber,
                           Description = d.description,
                           Machine = m.machineName,
                           Quantity = d.quantity,
                           Lpn = l.lpn1,
                           Ilustration = d.ilustration
                       }).ToList();
            }


            return View(tbl);
        }

        public ActionResult Register()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            List<FileViewModel> lstL = null;
            List<FileViewModel> lstM = null;
            using (cursomvcEntities db = new cursomvcEntities())
            {
                lstL = (from l in db.location
                        where l.idstatus == 1
                        select new FileViewModel
                        {
                            IdLocation = l.id,
                            Location = l.location1,
                        }).ToList();

                lstM = (from m in db.machines
                        where m.idStatus == 1
                        select new FileViewModel
                        {
                            IdMachines = m.id,
                            MachineName = m.machineName
                        }).ToList();
            }

            List<SelectListItem> location = lstL.ConvertAll(l =>
            {
                return new SelectListItem()
                {
                    Text = l.Location.ToString(),
                    Value = l.IdLocation.ToString(),
                    Selected = false
                };
            });
            List<SelectListItem> machines = lstM.ConvertAll(m =>
            {
                return new SelectListItem()
                {
                    Text = m.MachineName.ToString(),
                    Value = m.IdMachines.ToString(),
                    Selected = false
                };
            });



            ViewBag.Location = location;
            ViewBag.Machines = machines;

            return View();
        }

        [HttpPost]
        public ActionResult SaveRegister(FileViewModel model)
        {

            
            using (cursomvcEntities db = new cursomvcEntities())
            {
                 var lpn = (from d in db.lpn
                       where d.quantity > 0
                       orderby d.lpn1 descending
                       select new
                       {
                           lpn = d.lpn1
                       }).ToList().FirstOrDefault();
                
                MemoryStream target = new MemoryStream();
                model.SpareImage.InputStream.CopyTo(target);// para convertir el htttpostedfilebase a byte[]

                items oItems = new items(); // ingresamos datos en tabla items
                oItems.partnumber = model.Partnumber;
                oItems.description = model.Description;
                oItems.ilustration = target.ToArray();
                oItems.idmachine = model.IdMachines;
                oItems.quantity = model.Quantity;
                oItems.idstatus = 1;

                sparepartLocation oSpare = new sparepartLocation(); // ingresamos datos en tabla sparePartLocation
                oSpare.idlocation = model.IdLocation;
                oSpare.partnumber = model.Partnumber;
                oSpare.username = (string)Session["username"];
                oSpare.dateUpdate = DateTime.Now;
                oSpare.quantity = model.Quantity;

                lpn oLpn = new lpn(); // ingresamos datos y generamos nuevo lpn
                if (lpn.ToString() == null || lpn.ToString() == "")
                    oLpn.lpn1 = "TG1";
                else
                    oLpn.lpn1 = lpn + "1";  
                oLpn.partumber = model.Partnumber;
                oLpn.idocation = model.IdLocation;
                oLpn.quantity = model.Quantity;
                oLpn.idstatus = 1;

                db.items.Add(oItems);
                db.sparepartLocation.Add(oSpare);
                db.lpn.Add(oLpn);
                db.SaveChanges();
                            
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