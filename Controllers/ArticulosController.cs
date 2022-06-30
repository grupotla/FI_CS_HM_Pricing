using ti_pricing.Models;
using ti_pricing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace ti_pricing.Controllers
{
    public class ArticulosController : Controller
    {


        DataListing objListing;
        LoginController valida;

        public ArticulosController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }


        public ActionResult Rutas(string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/Rutas/Index?id_lista=" + id_lista);
            return null;
        }

        public ActionResult Rangos(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosRangos/Index?id_articulo=" + id_articulo);
            return null;
        }

        public ActionResult TarifasRangos(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosTarifas/TarifasRangos?id_articulo=" + id_articulo);
            return null;
        }


        public ActionResult Tarifas(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosTarifas/Index?id_articulo=" + id_articulo);
            return null;
        }





        // GET: Articulos
        [HttpGet]
        public ActionResult Index(Articulos Model, FormCollection collection, string TextoBuscar, string tpa_servicio_fk, string id_ruta, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            string controller = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();

            ViewBag.controller = controller;

            try
            {

                ViewBag.id_ruta = id_ruta;

                Model.Rutas = objListing.AllRutasListing("", "", id_ruta).First();

                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                Model.Listas = objListing.GetListasByID(Model.Rutas.tpr_tpl_fk.ToString());
                /*
                Model.Rutas.tpr_transporte_fk = Model.Listas.tpl_transporte_fk.ToString();

                Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk);


                Model.OrigenOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_origen_fk.ToString(), "SI", "");
                Model.DestinoOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_destino_fk.ToString(), "SI", "");
                */
                Model.ArticulosList = objListing.AllArticulosListing(TextoBuscar, tpa_servicio_fk, id_ruta, "");


                string servicios = "";


                /*
                ////// USADO POR EL FILTRO SERVICIO
                Model.ArticulosList2 = objListing.AllArticulosListing(TextoBuscar, "", id_ruta, "");
                servicios = "";
                foreach (Articulos item in Model.ArticulosList2)
                {
                    if (servicios != "") servicios += ",";
                    servicios += item.tpa_servicio_fk;
                }*/
                Model.ServiciosOpciones = objListing.GetServiciosOptions(servicios);

 

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + controller + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
            
        }


        // GET: Articulos/Details/5
        public ActionResult Details(int id)
        {
  

            return View();
        }

        // GET: Articulos/Create
        public ActionResult Create(string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            ViewBag.id_ruta = id_ruta;

            Articulos Model = new Articulos();

            try
            {
                Model.tpa_tpr_fk = int.Parse(id_ruta);

                Model.ServiciosOpciones = objListing.GetServiciosOptions("");

                Model.RubrosOpciones = objListing.GetRubrosOptions("", "");

                Model.Rutas = objListing.AllRutasListing("", "", id_ruta).First();

                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;
                /*
                Model.Listas = objListing.GetListasByID(Model.Rutas.tpr_tpl_fk.ToString());

                Model.Rutas.tpr_transporte_fk = Model.Listas.tpl_transporte_fk.ToString();

                Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk);

                Model.OrigenOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_origen_fk.ToString(), "SI", "");
                Model.DestinoOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_destino_fk.ToString(), "SI", "");
*/
                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                Model.MedidaOpciones = objListing.GetMedidasOptions();

                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                string test = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }


            return View("Form", Model);
        }

        // POST: Articulos/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articulos Model)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            try
            {
                Model.tpa_tps_fk = "1";
                Model.tpa_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertArticulos(Model);

                    ViewBag.m = "Articulos Created Successfully";

                    return RedirectToAction("Index", new { id_ruta = Model.tpa_tpr_fk, m = ViewBag.m });
                }
                else
                {
                    ModelState.AddModelError("Error", "something its wrong");

                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }

            ViewBag.id_ruta = Model.tpa_tpr_fk.ToString();

            Model.ServiciosOpciones = objListing.GetServiciosOptions("");

            Model.RubrosOpciones = objListing.GetRubrosOptions("", "");

            Model.Rutas = objListing.AllRutasListing("", "", Model.tpa_tpr_fk.ToString()).First();

            Model.Listas = objListing.GetListasByID(Model.Rutas.tpr_tpl_fk.ToString());
/*
            Model.Rutas.tpr_transporte_fk = Model.Listas.tpl_transporte_fk.ToString();

            Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk);

            //Model.PaisesOpciones = objListing.GetPaisesOptions("", "", Model.Rutas.tpr_pais_fk.ToString());
            //Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk.ToString());
            //Model.MedidaOpciones = objListing.GetMedidasOptions();
            //Model.MonedaOpciones = objListing.GetMonedasOptions();

            Model.OrigenOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_origen_fk.ToString(), "SI", "");
            Model.DestinoOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_destino_fk.ToString(), "SI", "");
*/
            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            Model.MedidaOpciones = objListing.GetMedidasOptions();

            ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;


            return View("Form", Model);
        }

        // GET: Articulos/Edit/5
        public ActionResult Edit(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";
            ViewBag.id_articulo = id_articulo;

            var Model = objListing.GetArticulosByID(id_articulo);  //Convert.ToString(

      
            try
            {
                ViewBag.id_ruta = Model.tpa_tpr_fk;


                Model.ArticulosTarifasList = objListing.AllArticulosTarifasListing(id_articulo, "");

                Model.ArticulosRangosList = objListing.AllArticulosRangosListing(id_articulo,"");


                ViewBag.Dependencias = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.ArticulosTarifasList.Count() : 0;

                if (ViewBag.Dependencias == 0)
                {
                    ViewBag.Dependencias = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.ArticulosRangosList.Count() : 0;
                }

                Model.ServiciosOpciones = objListing.GetServiciosOptions("");

                Model.RubrosOpciones = objListing.GetRubrosOptions(Model.tpa_servicio_fk.ToString(), "");

         
                Model.Rutas = objListing.AllRutasListing("", "", Model.tpa_tpr_fk.ToString()).First();
                /*
                Model.Listas = objListing.GetListasByID(Model.Rutas.tpr_tpl_fk.ToString());

                Model.Rutas.tpr_transporte_fk = Model.Listas.tpl_transporte_fk.ToString();

                Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk);

                Model.OrigenOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_origen_fk.ToString(), "SI", "");
                Model.DestinoOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_destino_fk.ToString(), "SI", "");
                */
                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                Model.MedidaOpciones = objListing.GetMedidasOptions();

                
                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;


                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        // POST: Articulos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Articulos Model, FormCollection collection)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;
            
            ViewBag.actionType = "Update";

            try
            {

                Model.tpa_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateArticulos(Model);

                    ViewBag.m = "Articulos Updated Successfully";

                    return RedirectToAction("Index", new { id_ruta = Model.tpa_tpr_fk, m = ViewBag.m });

                }
                else
                {
                    ModelState.AddModelError("Error", "something its wrong");

                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                return null;
            }

            ViewBag.id_ruta = Model.tpa_tpr_fk;

            ViewBag.id_articulo = Model.tpa_pk;

            Model.ServiciosOpciones = objListing.GetServiciosOptions("");

            Model.RubrosOpciones = objListing.GetRubrosOptions(Model.tpa_servicio_fk.ToString(), "");


            Model.Rutas = objListing.AllRutasListing("", "", Model.tpa_tpr_fk.ToString()).First();
/*
            Model.Rutas = objListing.GetRutasByID(Model.tpa_tpr_fk.ToString());

            Model.Listas = objListing.GetListasByID(Model.Rutas.tpr_tpl_fk.ToString());

            Model.Rutas.tpr_transporte_fk = Model.Listas.tpl_transporte_fk.ToString();

            Model.TransporteOpciones = objListing.GetTransportesOptions(Model.Rutas.tpr_transporte_fk);

            Model.OrigenOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_origen_fk.ToString(), "SI", "");
            Model.DestinoOpciones = objListing.AllPuertosListing("", "", Model.Rutas.tpr_tpp_destino_fk.ToString(), "SI", "");
*/
            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            Model.MedidaOpciones = objListing.GetMedidasOptions();

            ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;


            return View("Form", Model);
        }

        // GET: Articulos/Delete/5
        public ActionResult Delete(string id, string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            var Model = objListing.GetArticulosByID(id);

            ViewBag.s = "panel-info";
            ViewBag.c = "Articulos";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            ViewBag.id_ruta = String.IsNullOrEmpty(id_ruta) ? "" : "?id_ruta=" + id_ruta;

            try
            {

                if (Model != null)
                {
                    objListing.DeleteArticulos(Model);

                    ViewBag.m = "Articulos Deleted Successfully";
                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }


            //return RedirectToAction("Index", new { id_ruta = Model.tpa_tpr_fk });
            return PartialView("~/Views/Login/Error.cshtml");


        }


        public JsonResult GetRubros(string id_servicio)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            var rubros = objListing.GetRubrosOptions(id_servicio, "");

            return Json(rubros, JsonRequestBehavior.AllowGet);
        }



    }
}
