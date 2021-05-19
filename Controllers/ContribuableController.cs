using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Taxes.Models;
using System.IO;
using System.Web.UI.WebControls;

namespace Taxes.Controllers
{
    public class ContribuableController : Controller
    {
        marieEntities2 db = new marieEntities2();
        string phot;
        // GET: Contribuable
        public ActionResult listecontribuable()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.listecontribuable = db.contibuables.ToList();
            return View();
        }
        public ActionResult ajoutcontribuable()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult editcontribuable(int id)
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index");
            }
            contibuable cont = db.contibuables.Find(id);
            if(cont != null)
            {
                phot = cont.photo_contibuable;
                return View(cont);
            }
            else
              return RedirectToAction("Listecontribuable");
        }
        [HttpPost]
        public ActionResult ajoutcontribuable(contibuable cont)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/fichier/"), filename);
                        file.SaveAs(path);
                        cont.photo_contibuable= filename;
                    }
                }
                db.contibuables.Add(cont);
                db.SaveChanges();
            }
                
            return RedirectToAction("Listecontribuable");
        }
        [HttpPost]
        public ActionResult editcontribuable(contibuable cons)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/fichier/"), filename);
                        file.SaveAs(path);
                        cons.photo_contibuable = filename;
                    }
                }
                else
                {
                    cons.photo_contibuable = phot;
                }
                db.Entry(cons).State = EntityState.Modified; 
                db.SaveChanges();
            }
            return RedirectToAction("Listecontribuable");
        }
        [HttpGet]
        public ActionResult delcontribuable(int id)
        {
            contibuable cond = db.contibuables.Find(id);
            if(cond != null)
            {
                db.contibuables.Remove(cond);
                db.SaveChanges();
            }
            return RedirectToAction("Listecontribuable");
        }
    }
}