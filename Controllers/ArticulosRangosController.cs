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
    public class ArticulosRangosController : Controller
    {

        DataListing objListing;
        LoginController valida;
        public ArticulosRangosController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }


        public ActionResult Articulos(string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/Articulos/Index?id_ruta=" + id_ruta);
            return null;
        }


        public ActionResult Tarifas(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosTarifas/Index?id_articulo=" + id_articulo);
            return null;
        }


        public ActionResult TarifasRangos(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosTarifas/TarifasRangos?id_articulo=" + id_articulo);
            return null;
        }

        // GET: ArticulosRangos
        [HttpGet]
        public ActionResult Index(ArticulosRangos Model, FormCollection collection, string TextoBuscar, string tpa_servicio_fk, string id_articulo, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            try
            {
       

                Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();


                ViewBag.id_articulo = id_articulo;
                ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk;

                Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                //Model.ListasList = objListing.AllListasListing("", "", "", Model.Articulos.tpa_tpr_fk.ToString());

                //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

                //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

                Model.ArticulosRangosList = objListing.AllArticulosRangosListing(id_articulo,"");

                //Model.RangosList = objListing.AllRangosListing("");

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
            
        }



        // GET: ArticulosRangos/Create
        public ActionResult Create(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            ViewBag.id_articulo = id_articulo;

            ArticulosRangos Model = new ArticulosRangos();

            try
            {

                if (String.IsNullOrEmpty(id_articulo)) id_articulo = "0";

                Model.tpag_tpa_fk = int.Parse(id_articulo);

                //Model.RangosList = objListing.AllRangosListing("");
                Model.RangosList = objListing.AllRangosArticulosListing(id_articulo, "");
                //Model.RangosList = Model.RangosList.Prepend(new Rangos { tpg_pk = "", tpg_valor_str = "Select" });

                IEnumerable<Rangos> second = new[] { new Rangos { tpg_pk = "", tpg_valor_str = "Select" } };
                Model.RangosList = second.Concat(Model.RangosList);

                Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();

                ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk.ToString();

                Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

                //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

                //Model.ListasList = objListing.AllListasListing("", "", "", Model.Articulos.tpa_tpr_fk.ToString());

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);
                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }


            return null;

        }

        // POST: ArticulosRangos/Create
        [HttpPost]
        public ActionResult Create(ArticulosRangos Model, FormCollection collection, string id_articulo, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            try
            {
                Model.tpag_tps_fk = "1";
                Model.tpag_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertArticulosRangos(Model);

                    ViewBag.m = "ArticulosRangos Created Successfully";

                    return RedirectToAction("Index", new { id_articulo = Model.tpag_tpa_fk, m = ViewBag.m });
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

            ViewBag.id_articulo = id_articulo;

            //Model.RangosList = objListing.AllRangosListing("");
            Model.RangosList = objListing.AllRangosArticulosListing(id_articulo, "");
            //Model.RangosList = Model.RangosList.Prepend(new Rangos { tpg_pk = "", tpg_valor_str = "Select" });

            IEnumerable<Rangos> second = new[] { new Rangos { tpg_pk = "", tpg_valor_str = "Select" } };
            Model.RangosList = second.Concat(Model.RangosList);

            Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();

            ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk.ToString();

            //Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
            //ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;
            ViewBag.id_lista = id_lista;

            //Model.ListasList = objListing.AllListasListing("","","",Model.Articulos.tpa_tpr_fk.ToString());

//            Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

  //          Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }

        // GET: ArticulosRangos/Edit/5
        public ActionResult Edit(string id_rango, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";
            ViewBag.id_rango = id_rango;
            var Model = objListing.GetArticulosRangosByID(id_rango);

            try
            {
                ViewBag.id_articulo = Model.tpag_tpa_fk;
                ViewBag.id_ruta = id_ruta;

                //Model.RangosList = objListing.AllRangosListing("");
                Model.RangosList = objListing.AllRangosArticulosListing(Model.tpag_tpa_fk.ToString(), Model.tpag_tpg_fk.ToString());


                Model.Articulos = objListing.AllArticulosListing("", "", "", Model.tpag_tpa_fk.ToString()).First();


                Model.ArticulosTarifasList = objListing.AllArticulosTarifasListing(Model.tpag_tpa_fk.ToString(), Model.tpag_tpg_fk.ToString());

                ViewBag.Tarifas = Convert.ToInt32(Convert.ToString(Session["LoginType"])) > 1 ? Model.ArticulosTarifasList.Count() : 0;


                //Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
                //ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                ViewBag.id_lista = id_lista;

                //Model.ListasList = objListing.AllListasListing("", "", "", Model.Articulos.tpa_tpr_fk.ToString());

                //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

                //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }
            return null;
        }

        // POST: ArticulosRangos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticulosRangos Model, FormCollection collection, string actionType, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";


            try
            {
                Model.tpag_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateArticulosRangos(Model);

                    ViewBag.m = "ArticulosRangos Updated Successfully";

                    return RedirectToAction("Index", new { id_articulo = Model.tpag_tpa_fk, m = ViewBag.m });
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

            ViewBag.id_rango = Model.tpag_pk;
            ViewBag.id_articulo = Model.tpag_tpa_fk;
            ViewBag.id_ruta = id_ruta;

            //Model.RangosList = objListing.AllRangosListing("");
            Model.RangosList = objListing.AllRangosArticulosListing(Model.tpag_tpa_fk.ToString(), Model.tpag_tpg_fk.ToString());

            Model.Articulos = objListing.AllArticulosListing("", "", "", Model.tpag_tpa_fk.ToString()).First();


            //Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
            //ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;
            ViewBag.id_lista = id_lista;

            //Model.ListasList = objListing.AllListasListing("","","",Model.Articulos.tpa_tpr_fk.ToString());

            //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

            //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }



        public ActionResult Delete(int id, string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            var Model = objListing.GetArticulosRangosByID(Convert.ToString(id));

            ViewBag.s = "panel-info";
            ViewBag.c = "ArticulosRangos";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            ViewBag.id_articulo = String.IsNullOrEmpty(id_articulo) ? "" : "?id_articulo=" + id_articulo;

            try
            {

                if (Model != null)
                {

                    objListing.DeleteArticulosRangos(Model);

                    ViewBag.m = "ArticulosRangos Deleted Successfully";

                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }

            //return RedirectToAction("Index", new { id_articulo = Model.tpag_tpa_fk });
            return PartialView("~/Views/Login/Error.cshtml");
        }
    }
}
