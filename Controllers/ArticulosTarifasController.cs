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
    public class ArticulosTarifasController : Controller
    {

        DataListing objListing;
        LoginController valida;
        public ArticulosTarifasController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }

        public ActionResult TarifasRangos(ArticulosTarifas Model, string id_articulo, List<string> Ide, List<string> Valor)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t)
                return null;

            ViewBag.id_articulo = id_articulo;


            Model.ArticulosRangosTarifasList = objListing.AllArticulosRangosTarifasListing(id_articulo);

            try
            {

                if (Valor != null)
                {

                    int c = 0;
                    int d = 0;
                    decimal Dinput, Ddata;
                    foreach (var item in Model.ArticulosRangosTarifasList)
                    {
                        var a = Valor[c];
                        var b = Ide[c];


                        if (!String.IsNullOrEmpty(a))
                        {
                            Dinput = Convert.ToDecimal(a);
                            Ddata = Convert.ToDecimal(item.tpat_tarifa);

                            if (Dinput != Ddata)
                            {
                                d++;
                            }
                        }

                        c++;
                    }

                    ViewBag.m = "ArticulosTarifas No Changes Registered";

                    if (d > 0)
                    {

                        objListing.InsertArticulosTarifasRangos(Session["UserID"].ToString(), id_articulo, Ide, Valor);

                        ViewBag.m = "ArticulosTarifas Created Successfully";
                    }
                    return RedirectToAction("Index", new { id_articulo = id_articulo, m = ViewBag.m });

                }
                
            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                
                ViewBag.c = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
                ViewBag.a = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();
                ViewBag.m = e.Message;
                ViewBag.s = "panel-danger";
                return PartialView("~/Views/Login/Error.cshtml");
            }

            Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();

            ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk;

            Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
            ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

            Model.RangosList = objListing.AllRangosListing("");

            return PartialView(Model);
        }

        public ActionResult Articulos(string id_ruta)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/Articulos/Index?id_ruta=" + id_ruta);
            return null;
        }



        public ActionResult Rangos(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            Response.Redirect("~/ArticulosRangos/Index?id_articulo=" + id_articulo);
            return null;
        }


        // GET: ArticulosTarifas
        [HttpGet]
        public ActionResult Index(ArticulosTarifas Model, FormCollection collection, string TextoBuscar, string tpa_servicio_fk, string id_articulo, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            try
            {
                Model.Articulos = objListing.AllArticulosListing("" , "", "", id_articulo).First();

                ViewBag.id_articulo = id_articulo;
                ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk;

                Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

                //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

                Model.ArticulosTarifasList = objListing.AllArticulosTarifasListing(id_articulo, "");

                //Model.RangosList = objListing.AllRangosListing("");

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;

        }



        // GET: ArticulosTarifas/Create
        public ActionResult Create(string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";

            ViewBag.id_articulo = id_articulo;

            ArticulosTarifas Model = new ArticulosTarifas();

            try
            {

                if (String.IsNullOrEmpty(id_articulo)) id_articulo = "0";

                Model.tpat_tpa_fk = int.Parse(id_articulo);

                Model.RangosList = objListing.AllRangosArticulosTarifasListing(id_articulo,"");
                //Model.RangosList = Model.RangosList.Prepend(new Rangos { tpg_pk = "", tpg_valor_str = "Select" });

                IEnumerable<Rangos> second = new[] { new Rangos { tpg_pk = "", tpg_valor_str = "Select" } };
                Model.RangosList = second.Concat(Model.RangosList);

                Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();


                Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
                ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

                ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk.ToString();

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

        // POST: ArticulosTarifas/Create
        [HttpPost]
        public ActionResult Create(ArticulosTarifas Model, FormCollection collection, string id_articulo, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            try
            {
                Model.tpat_tps_fk = "1";
                Model.tpat_insert_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.InsertArticulosTarifas(Model);

                    ViewBag.m = "ArticulosTarifas Created Successfully";

                    return RedirectToAction("Index", new { id_articulo = Model.tpat_tpa_fk, m = ViewBag.m });
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

            Model.RangosList = objListing.AllRangosArticulosTarifasListing(id_articulo,"");
            //Model.RangosList = Model.RangosList.Prepend(new Rangos { tpg_pk = "", tpg_valor_str = "Select" });

            IEnumerable<Rangos> second = new[] { new Rangos { tpg_pk = "", tpg_valor_str = "Select" } };
            Model.RangosList = second.Concat(Model.RangosList);

            Model.Articulos = objListing.AllArticulosListing("", "", "", id_articulo).First();


            //Model.Rutas = objListing.GetRutasByID(Model.Articulos.tpa_tpr_fk.ToString());
            //ViewBag.id_lista = Model.Rutas.tpr_tpl_fk;

            ViewBag.id_lista = id_lista;

            ViewBag.id_ruta = Model.Articulos.tpa_tpr_fk.ToString();

            //Model.ListasList = objListing.AllListasListing("","","",Model.Articulos.tpa_tpr_fk.ToString());

            //Model.ServiciosOpciones = objListing.GetServiciosOptions(Model.Articulos.tpa_servicio_fk.ToString());

            //Model.RubrosOpciones = objListing.GetRubrosOptions("", Model.Articulos.tpa_rubro_fk.ToString());

            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }

        // GET: ArticulosTarifas/Edit/5
        public ActionResult Edit(string id_tarifa, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";
            ViewBag.id_tarifa = id_tarifa;

            var Model = objListing.GetArticulosTarifasByID(id_tarifa);

            try
            {
                ViewBag.id_articulo = Model.tpat_tpa_fk;
                ViewBag.id_ruta = id_ruta;

                Model.RangosList = objListing.AllRangosArticulosTarifasListing(Model.tpat_tpa_fk.ToString(), Model.tpat_tpg_fk.ToString());

                Model.Articulos = objListing.AllArticulosListing("", "", "", Model.tpat_tpa_fk.ToString()).First();


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

        // POST: ArticulosTarifas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticulosTarifas Model, FormCollection collection, string actionType, string id_ruta, string id_lista)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";

            try
            {
                Model.tpat_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateArticulosTarifas(Model);

                    ViewBag.m = "ArticulosTarifas Updated Successfully";

                    return RedirectToAction("Index", new { id_articulo = Model.tpat_tpa_fk, m = ViewBag.m });
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

            ViewBag.id_tarifa = Model.tpat_pk;


            ViewBag.id_articulo = Model.tpat_tpa_fk;

            ViewBag.id_ruta = id_ruta;

            Model.RangosList = objListing.AllRangosArticulosTarifasListing(Model.tpat_tpa_fk.ToString(), Model.tpat_tpg_fk.ToString());

            Model.Articulos = objListing.AllArticulosListing("", "", "", Model.tpat_tpa_fk.ToString()).First();


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



        public ActionResult Delete(int id, string id_articulo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            var Model = objListing.GetArticulosTarifasByID(Convert.ToString(id));

            ViewBag.s = "panel-info";
            ViewBag.c = "ArticulosTarifas";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            ViewBag.id_articulo = String.IsNullOrEmpty(id_articulo) ? "" : "?id_articulo=" + id_articulo;

            try
            {

                if (Model != null)
                {

                    objListing.DeleteArticulosTarifas(Model);

                    ViewBag.m = "ArticulosTarifas Deleted Successfully";

                }
            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }

            //return RedirectToAction("Index", new { id_articulo = Model.tpat_tpa_fk });
            return PartialView("~/Views/Login/Error.cshtml");
        }
    }
}
