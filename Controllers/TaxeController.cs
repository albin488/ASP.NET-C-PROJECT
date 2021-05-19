using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Taxes.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.Mvc.Ajax;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Taxes.Controllers
{ 
    public class TaxeController : Controller
    {


        // GET: Taxe
        marieEntities2 db = new marieEntities2();
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        
      void connectionString()
      {
          con.ConnectionString = "data source=PHILIPE;database=taxes;integrated security=SSPI;";
      }
      /* public List<quitance> GetTaxeBygroup(string from, string to)
        {
            connectionString();
            con.Open();
            List<quitance> quitances = new List<quitance>;
            string req = "select * from quitance where date '" + from + "' and '" + to + "'";
            SqlCommand sqlcmd = new SqlCommand(req, con);
            SqlDataReader myreader = sqlcmd.ExecuteReader();
            if (myreader.HasRows)
            {
                while (myreader.Read())
                {
                    quitance quinta = new quitance();
                    quinta.id_taxe =Convert.ToString(myreader["id_taxe"]);
                }
            }


            return quitances;
        }*/
        public ActionResult ajaxqui(quitance qui)
        {
          
            
          
            return RedirectToAction("listetaxe");
           
        }
       
        public ActionResult listetaxe()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index","Home");
            }

            ViewBag.listetaxe = db.quitances.ToList();
            ViewBag.listecontri = db.contibuables.ToList();
            ViewBag.id = 1;

            return View();
            

        }
        public ActionResult rapport()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.listetaxe = db.quitances.ToList();
            ViewBag.listecontri = db.contibuables.ToList();
            ViewBag.listagent = db.agents.ToList();
            return View();
        }
        [HttpGet] ActionResult _rapport(gdate dat)
        {

           
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from quitance where date between  '2020-12-10' and '2020-12-12' ";
            SqlDataAdapter sda = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            List<quitance> qtc = new List<quitance>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                qtc.Add(new quitance
                {
                    id_taxe = Convert.ToInt32(dr["id_taxe"]),
                    id_contri= Convert.ToInt32(dr["id_contri"]),
                    matricule=Convert.ToInt32(dr["matricule"]),
                    anne= Convert.ToString(dr["anne"]),
                    taux= Convert.ToInt32(dr["taux"]),
                    bordereau = Convert.ToString(dr["bordereau"]),
                    date = Convert.ToString(dr["date"]),
                    etat = Convert.ToString(dr["etat"]),

                }) ;
            }

            // ViewBag.listetaxe = db.getfuctiondate(start, end).ToString();
            ViewBag.listetaxe = qtc;
            ViewBag.listecontri = db.contibuables.ToList();
            ViewBag.listagent = db.agents.ToList();
          
            return PartialView();
        }
        [HttpGet]
        public ActionResult _listetaxe(int id_taxe)
        {

            if (Session["nom"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.listetaxe = db.quitances.ToList();
            ViewBag.listecontri = db.contibuables.ToList();
            quitance quitance = db.quitances.Find(id_taxe);
       
            return PartialView(quitance);

        }

        [HttpPost]
        public ActionResult _listetaxe(int id_taxe,string matricule)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "update quitance set matricule='"+matricule+"',etat=1 where id_taxe='" + id_taxe+"' ";
            com.ExecuteNonQuery();
            db.SaveChanges();
            con.Close();
            return RedirectToAction("listetaxe");
            
        }
      
        [HttpGet]
        public ActionResult quitance(int id)
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            contibuable cont = db.contibuables.Find(id);
            string act = cont.activite;
           if (act == "A") {
                Session["activi"] = 10000;
            }
            if (act == "B")
            {
                Session["activi"] = 30000;
            }
            if (act == "C")
            {
                Session["activi"] = 60000;
            }
            return View(cont);
        }
        [HttpPost]
        public ActionResult quitance(quitance qui)
        {

            if (Session["nom"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/fichier/"),filename);
                        file.SaveAs(path);
                        qui.bordereau= filename;
                    }
                }
                qui.matricule=0;
                qui.date = DateTime.Now.ToString("dd/MM/yyy");
                qui.etat = "0";
                 db.quitances.Add(qui);
                 db.SaveChanges();
                /*  string taux = qui.id_contri.ToString();
                  connectionString();
                  con.Open();
                  com.Connection = con;
                  com.CommandText = "insert into quitance(id_taxe,id_contri,matricule,taux,anne,date,etat,bordereau) values(@num,@idc,@matr,@taux,@anne,@date,@etat,@bord)";
                  com.Parameters.AddWithValue("@num",null);
                  com.Parameters.AddWithValue("@idc", int.Parse(qui.id_contri.ToString()));
                  com.Parameters.AddWithValue("@matr", qui.matricule.ToString());
                  com.Parameters.AddWithValue("@taux", int.Parse(qui.taux.ToString()));
                  com.Parameters.AddWithValue("@anne", qui.anne.ToString());
                  com.Parameters.AddWithValue("@date", qui.date.ToString());
                  com.Parameters.AddWithValue("@etat", qui.etat);
                  com.Parameters.AddWithValue("@bord", qui.bordereau);
                  com.ExecuteNonQuery();
                  com.Parameters.Clear();
                con.Close();*/
             
            }

            return View();

        }
        public ActionResult Operation(quitance qui)
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                connectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from quitance where id_contri='" + Session["id"] + "'";
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                List<quitance> qtc = new List<quitance>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    qtc.Add(new quitance
                    {
                        id_taxe = Convert.ToInt32(dr["id_taxe"]),
                        id_contri = Convert.ToInt32(dr["id_contri"]),
                        matricule = Convert.ToInt32(dr["matricule"]),
                        anne = Convert.ToString(dr["anne"]),
                        taux = Convert.ToInt32(dr["taux"]),
                        bordereau = Convert.ToString(dr["bordereau"]),
                        date = Convert.ToString(dr["date"]),
                        etat = Convert.ToString(dr["etat"]),

                    });
                }
                con.Close();
                ViewBag.listetaxe = qtc;
            }
            return View();
        }
     }
}