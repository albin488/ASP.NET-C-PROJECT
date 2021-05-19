using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Taxes.Models;
using System.IO;
using System.Collections;
using System.Data.SqlClient;


namespace Taxes.Controllers
{
    public class HomeController : Controller
    {
        marieEntities2 db = new marieEntities2();
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
      

        public ActionResult Index()
        {
            return View();
           
        }

        public ActionResult About()
        {
            if (Session["nom"] == null) {
              return  RedirectToAction("Index");
            }
                return View();

        }


        public ActionResult Contact()
        {
            if (Session["nom"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(agent agt)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if(file !=null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/fichier/"), filename);
                        file.SaveAs(path);
                        agt.photo_agent=filename;
                    }
                }
                agt.role = "user";
                db.agents.Add(agt);
                db.SaveChanges();
               
            }

            return RedirectToAction("Contact");
        }
        void connectionString()
        {
            con.ConnectionString = "data source=PHILIPE;database=taxes;integrated security=SSPI;";
        }
        [HttpPost]
        public ActionResult Index(compt cont)
        {
            if (ModelState.IsValid)
            {
                connectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from agent where login='" + cont.login + "' and pass='" + cont.pass + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    useragent usa = new useragent();
                    Session["matricule"] = dr["matri"].ToString();
                    Session["nom"] = dr["nom_agent"].ToString();
                    Session["prenom"] = dr["prenom_agent"].ToString();
                    Session["photo"] = dr["photo_agent"].ToString();
                    usa.adress = dr["adress"].ToString();
                    usa.login = dr["login"].ToString();
                    usa.pass = dr["pass"].ToString();
                  string rol = dr["role"].ToString();
                    string etat= dr["matricule"].ToString();
                    if (rol == "1") {
                        Session["role"] = "admin";
                    }
                    else { Session["role"] = "user1"; }
                 
                   Session["user"] = "agent";
                    Session["mesage"] = "ok";
                    if (etat == "1111111111") { return RedirectToAction("About"); }
                    else { Session["mesage"] = "block"; }
                   

                }
                else
                {
                    con.Close();
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "select * from contibuable where login='" + cont.login + "' and pass='" + cont.pass + "'";
                    SqlDataReader drc = com.ExecuteReader();
                    if (drc.Read())
                    {
                        

                        Session["id"] = drc["id_contibuable"].ToString();
                        Session["nom"] = drc["nom_contibuable"].ToString();
                        Session["prenom"] = drc["prenom_contibuable"].ToString();
                        Session["cni"] = drc["cni_contibuable"].ToString();
                        Session["tel"] = drc["telephon_contibuable"].ToString();
                        Session["activite"] = drc["activite"].ToString();
                        Session["role"] = "cotribuable";
                        Session["user"] = "cotribuable";
                        Session["photo"] = drc["photo_contibuable"].ToString();
                        string etata= drc["etat"].ToString();
                        Session["mesage"] = "ok";
                        if (etata=="1") { return RedirectToAction("About"); }
                        else { Session["mesage"] = "block"; }


                    }
                    else { Session["mesage"] = "erreur"; }
                    con.Close();
                }




            }
            
            return View();

        }

    }
}