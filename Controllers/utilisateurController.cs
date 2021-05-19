using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taxes.Models;
using System.Data.SqlClient;

namespace Taxes.Controllers
{
    public class utilisateurController : Controller
    {
        // GET: utilisateur
        marieEntities2 db = new marieEntities2();
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
       
        public ActionResult GestionUser()
        {
            ViewBag.listagent = db.agents.ToList();
            ViewBag.listcontr = db.contibuables.ToList();
            return View();
        }
        void ConnectionString()
        {
            con.ConnectionString = "data source=PHILIPE;database=taxes;integrated security=SSPI;";
        }
        [HttpGet]
        public ActionResult Blockagent(int id)
        {
            agent ags = db.agents.Find(id);
            if (ags != null)
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "update agent set matricule='0000000000' where  matri='" + id + "'";
                com.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("GestionUser");
        }
        [HttpGet]
        public ActionResult Autagent(int id)
        {
            agent ags = db.agents.Find(id);
            if (ags != null)
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "update agent set matricule='1111111111' where  matri='" + id + "'";
                com.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("GestionUser");
        }
        [HttpGet]
        public ActionResult Autcont(int id)
        {
            contibuable cont = db.contibuables.Find(id);
            if (cont != null)
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "update contibuable set etat=1 where  id_contibuable='" + id + "'";
                com.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("GestionUser");
        }
        [HttpGet]
        public ActionResult Blockcont(int id)
        {
            contibuable cont = db.contibuables.Find(id);
            if (cont != null)
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "update contibuable set etat=0 where  id_contibuable='" + id + "'";
                com.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("GestionUser");
        }
    }
}