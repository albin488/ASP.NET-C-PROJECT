using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Taxes.Models;
using System.IO;

namespace Taxes.Controllers
{
    public class AgentController : Controller
    {
        marieEntities2 db = new marieEntities2();
        // GET: Agent

        public ActionResult Listeagent()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.listesagent = db.agents.ToList();

            return View();
        }
        public ActionResult editagent(int id)
        {

            if (Session["nom"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // ViewBag.listesagent = db.agents.ToList();
            agent agd = db.agents.Find(id);
            if (agd != null)
            {
                // ViewBag.matricule = agd.matricule;
                // ViewBag.nom = agd.nom_agent;
                //ViewBag.prenom = agd.prenom_agent;
                //ViewBag.edit = agd.ToString();
                return View(agd);
            }
            else
                return RedirectToAction("Listeagent");
        }

        [HttpPost]
        public ActionResult editagent(agent age)
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
                        age.photo_agent = "~/fichier/" + filename;
                    }
                }
           
                db.Entry(age).State = EntityState.Modified;
                db.SaveChanges();
            }
           
            return RedirectToAction("Listeagent");
        }

        [HttpGet]
        public ActionResult delagent(int id)
        {
            try
            {
                agent ags = db.agents.Find(id);
                if (ags != null)
                {
                    db.agents.Remove(ags);
                    db.SaveChanges();
                }
                return RedirectToAction("Listeagent");
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}