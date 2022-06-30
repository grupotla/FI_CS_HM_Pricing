using ti_pricing.Models;
using ti_pricing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Net;

namespace ti_pricing.Controllers
{
    public class LoginController : Controller
    {


        public LoginController()
        {

        }

        // GET: Login
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Name"])))
            {
                ViewData["actionLinkClass"] = "btn btn-danger btn-lg modal-link glyphicon glyphicon-log-in BtnShow ";

                return View("Login2");

            }
            else
            {
                ViewData["actionLinkClass"] = "btn btn-danger btn-lg modal-link glyphicon glyphicon-log-in BtnHide ";
            }

            return View();
        }



        // GET: Login/Create
        public ActionResult Login()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["Name"])))
            {
                Login login = new Login();

                login.username = "soporte7";
                login.password = "andreasofia";

                return PartialView(login);
            }
            else
            {
                RedirectToAction("Login", "Index");

                return null;
            }
        }

        public ActionResult Login2(Login Model)
        {
            string msg = "";
            try
            {
                

                if (ModelState.IsValid)
                {
                    DataListing objListing = new DataListing();

                    Login login = objListing.GetLogin(Model.username, Model.password);

                    try { 
                        
                        if (Convert.ToInt32(login.id_usuario) < 1)
                            msg = "Por favor digite Usuario y Contraseña validos";

                        if (int.Parse(login.days) > 0)
                            msg = "Su Constraseña ha vencido";

                        if (int.Parse(login.pw_activo) != 1)
                            msg = "Usuario deshabilitado";
                    }
                    catch (Exception e)
                    {
                        msg = "Por favor digite Usuario y Contraseña validos";
                    }

                    if (msg == "")
                    {

                        Session["Pais"] = login.pais;
                        Session["Name"] = login.pw_gecos;
                        Session["UserID"] = login.id_usuario;
                        Session["User"] = Model.username;

                        var row = objListing.GetPerfil(login.id_usuario);

                        if (row == null || row.id == null)
                        {
                            row.id = "5";
                            row.nombre = "'" + login.pais + "'";
                        }

                        Session["LoginType"] = row.id;

                        switch (row.id) {
                            case "1":
                                Session["LoginTypeStr"] = "Root";
                                break;
                            case "2":
                                Session["LoginTypeStr"] = "Admin";
                                break;
                            case "3":
                                Session["LoginTypeStr"] = "Operador";
                                break;
                            case "4":
                                Session["LoginTypeStr"] = "Consulta";
                                break;
                        }


                        Session["Countries"] = row.nombre;
                        Session["Ip"] = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].MapToIPv4();

                        objListing.SetLogs((string)Session["UserID"], (string)Session["User"], (string)Session["LoginType"], (string)Session["Pais"], 1);

                        return RedirectToAction("Index");
                    }



                }
                else
                {
                    //ModelState.AddModelError("Error", "something its wrong");

                    ViewBag.errors = ModelState.Values;

                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            if (msg != "")
            {
                Response.Write("<script>alert('" + msg + "');</script>");

                //MessageBox.Show(msg, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());

            }

            return View("Login2");
        }

            // POST: Login/Create
            [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult Login(Login model, string returnUrl)
        {
            string[] resultset = new String[3];
            resultset[0] = "";
            resultset[1] = "";
            resultset[2] = "";

            if (!ModelState.IsValid)            
            { 
                resultset[1] = "5";
                resultset[2] = "Por favor digite Usuario y Contraseña validos";
                return Json(resultset);
            }

            DataListing objListing = new DataListing();

            Login login = new Login();

            login = objListing.GetLogin(model.username, model.password);

            if (login.id_usuario == null)
            {
                resultset[1] = "4";
                resultset[2] = "Por favor digite Usuario y Contraseña validos";
                return Json(resultset);
            }

            if (int.Parse(login.days) > 0)
            {
                resultset[1] = "3";
                resultset[2] = "Su Constraseña ha vencido";
                return Json(resultset);
            }

            if (int.Parse(login.pw_activo) != 1)
            {
                resultset[1] = "5";
                resultset[2] = "Usuario deshabilitado";
                return Json(resultset);
            }


            Session["Name"] = login.pw_gecos;
            Session["UserID"] = login.id_usuario;


            var row = objListing.GetPerfil(login.id_usuario);

            if (row == null || row.id == null)
            {
                row.id = "5";
                row.nombre = "'" + login.pais + "'";
            }

            Session["LoginType"] = row.id;
            Session["Countries"] = row.nombre; 

            resultset[1] = "0";
            resultset[2] = "Usuario correcto";

            return Json(resultset);
        }




        public ActionResult Logout()
        {
            DataListing objListing = new DataListing();

            objListing.SetLogs((string)Session["UserID"], (string)Session["User"], (string)Session["LoginType"], (string)Session["Pais"], 0);

            Session.Remove("Name");
            Session.Remove("UserID");
            Session.Remove("User");
            Session.Remove("LoginType");
            Session.Remove("Index");
            Session.Remove("Pais");
            Session.Remove("Countries");
            Session.Remove("Ip");

            Response.Redirect("Index");
            return View();
        }


        public Boolean ValidaSession(string ses, HttpResponseBase res )
        {


            //if (Session == null || string.IsNullOrEmpty(Convert.ToString(Session["Name"])))
            if (String.IsNullOrEmpty(ses))
            {
                res.Redirect("~/Login/Index");

                return false;
            }
            else
            {
                return true;
            }
                
        }


        public ActionResult Confirm(int id, string id_lista, string id_ruta, string id_articulo, string tipo_entidad, string Tipo, string control, string accion)
        {
            ViewBag.id = id;
            ViewBag.id_lista = String.IsNullOrEmpty(id_lista) ? "" : "?id_lista=" + id_lista;
            ViewBag.id_articulo = String.IsNullOrEmpty(id_articulo) ? "" : "?id_articulo=" + id_articulo;
            ViewBag.id_ruta = String.IsNullOrEmpty(id_ruta) ? "" : "?id_ruta=" + id_ruta;
            ViewBag.tipo_entidad = String.IsNullOrEmpty(tipo_entidad) ? "" : "?tipo_entidad=" + tipo_entidad;
            ViewBag.Tipo = String.IsNullOrEmpty(Tipo) ? "" : "?Tipo=" + Tipo;
            ViewBag.control = control;
            ViewBag.accion = accion;

            switch (accion) {
                case "Delete":
                    ViewBag.titulo = "Esta seguro de borrar el registro " + id + " ?";
                    break;

                case "Select":
                    ViewBag.titulo = "Desea seleccionar el codigo " + id + " ?";
                    break;

                default:
                    ViewBag.titulo = "Desea Continuar ?";
                    break;
            }
            return PartialView();
        }


        public ActionResult Error(string c, string a, string m)
        {
            ViewBag.c = c;
            ViewBag.a = a;
            ViewBag.m = m;
            ViewBag.s = "panel-danger";
            ViewBag.b = "back";
            return View();
        }



    }
}
