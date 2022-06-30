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
    public class EntidadesController : Controller
    {


        DataListing objListing;
        LoginController valida;
        public EntidadesController()
        {
            objListing = new DataListing();
            valida = new LoginController();
        }


        // GET: Entidades
        [HttpGet]
        public ActionResult Index(Entidades Model, FormCollection collection, string TextoBuscar, string tpe_tipo_persona_fk, string id_lista, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;

            try
            {


                Model.EntidadesList = objListing.AllEntidadesListing("", tpe_tipo_persona_fk, "");

                Model.ListasList = objListing.AllListasListing("", "", "", "", "");

                Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();


                string[] codigos = new string[8];
                foreach (Entidades item in Model.EntidadesList)
                {
                    if (!String.IsNullOrEmpty(item.tpe_tipo_persona_fk))
                    {
                        if (!String.IsNullOrEmpty(codigos[int.Parse(item.tpe_tipo_persona_fk)]))                    
                            codigos[int.Parse(item.tpe_tipo_persona_fk)] += ",";
                        codigos[int.Parse(item.tpe_tipo_persona_fk)] += item.tpe_id_persona_fk;
                    }
                }



                ////////////// CLIENTES

                if (!String.IsNullOrEmpty(codigos[1]))
                {
                    Model.ClientesList = objListing.AllClientesListing(codigos[1], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_clientes cliente in Model.ClientesList)
                        {
                            if (item.tpe_id_persona_fk == cliente.id_cliente)
                            {
                                item.tpe_persona_nombre = cliente.nombre_cliente;

                                if (cliente.es_coloader == "1") item.tpe_persona_nombre += " (C)";

                                if (cliente.es_consigneer == "1") item.tpe_persona_nombre += " (c)";

                                if (cliente.es_shipper == "1") item.tpe_persona_nombre += " (S)";

                                break;
                            }
                        }
                    }
                }


                ////////////// PROVEEDORES

                if (!String.IsNullOrEmpty(codigos[2]))
                {
                    Model.ProveedoresList = objListing.AllProveedoresListing(codigos[2], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_proveedores proveedor in Model.ProveedoresList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.numero)
                            {
                                item.tpe_persona_nombre = proveedor.nombre;
                                break;
                            }
                        }
                    }
                }


                ////////////// AGENTES

                if (!String.IsNullOrEmpty(codigos[3]))
                {
                    Model.AgentesList = objListing.AllAgentesListing(codigos[3], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_agentes proveedor in Model.AgentesList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.agente_id)
                            {
                                item.tpe_persona_nombre = proveedor.agente;
                                break;
                            }
                        }
                    }
                }



                ////////////// CARRIERS

                if (!String.IsNullOrEmpty(codigos[4]))
                {
                    Model.CarriersList = objListing.AllCarrierListing(codigos[4], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_carriers carrier in Model.CarriersList)
                        {
                            if (item.tpe_id_persona_fk == carrier.carrier_id)
                            {
                                item.tpe_persona_nombre = carrier.name;
                                break;
                            }
                        }
                    }
                }





                ////////////// NAVIERAS

                if (!String.IsNullOrEmpty(codigos[5]))
                {
                    Model.NavierasList = objListing.AllNavierasListing(codigos[5], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_navieras proveedor in Model.NavierasList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.id_naviera)
                            {
                                item.tpe_persona_nombre = proveedor.nombre;
                                break;
                            }
                        }
                    }
                }

                return View(Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;



            
        }


        // GET: Entidades
        [HttpGet]
        public ActionResult IndexVenta(Entidades Model, FormCollection collection, string TextoBuscar, string tpe_tipo_persona_fk, string id_lista, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;
            ViewBag.Tipo = "VENTA";

            try
            {


                Model.EntidadesList = objListing.AllEntidadesListing("", tpe_tipo_persona_fk, "1");

                Model.ListasList = objListing.AllListasListing("", "VENTA", "", "", "");

                //Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();
                //Model.TipoEntidadOptions.RemoveAt(0);

                Model.TipoEntidadOptions = new List<Opciones>();

                Model.TipoEntidadOptions.Add(new Opciones { id = "1", nombre = "Clientes"});

                string[] codigos = new string[8];
                foreach (Entidades item in Model.EntidadesList)
                {
                    if (!String.IsNullOrEmpty(item.tpe_tipo_persona_fk))
                    {
                        if (!String.IsNullOrEmpty(codigos[int.Parse(item.tpe_tipo_persona_fk)]))
                            codigos[int.Parse(item.tpe_tipo_persona_fk)] += ",";

                        codigos[int.Parse(item.tpe_tipo_persona_fk)] += item.tpe_id_persona_fk;
                    }
                }



                ////////////// CLIENTES

                if (!String.IsNullOrEmpty(codigos[1]))
                {
                    Model.ClientesList = objListing.AllClientesListing(codigos[1], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_clientes cliente in Model.ClientesList)
                        {
                            if (item.tpe_id_persona_fk == cliente.id_cliente)
                            {
                                item.tpe_persona_nombre = cliente.nombre_cliente;

                                if (cliente.es_coloader == "True") item.tpe_persona_nombre += "(Co)";

                                if (cliente.es_consigneer == "True") item.tpe_persona_nombre += "(C)";

                                if (cliente.es_shipper == "True") item.tpe_persona_nombre += "(Sh)";

                                break;
                            }
                        }
                    }
                }


                
                return View("Index",Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;

        }


        // GET: Entidades
        [HttpGet]
        public ActionResult IndexCosto(Entidades Model, FormCollection collection, string TextoBuscar, string tpe_tipo_persona_fk, string id_lista, string m)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.m = m;
            ViewBag.Tipo = "COSTO";

            try
            {


                Model.EntidadesList = objListing.AllEntidadesListing("", tpe_tipo_persona_fk, "2,3,4,5");

                Model.ListasList = objListing.AllListasListing("", "", "", "", "");

                Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();
                Model.TipoEntidadOptions.RemoveAt(1);

                string[] codigos = new string[8];
                foreach (Entidades item in Model.EntidadesList)
                {
                    if (!String.IsNullOrEmpty(item.tpe_tipo_persona_fk)) { 
                        if (!String.IsNullOrEmpty(codigos[int.Parse(item.tpe_tipo_persona_fk)]))
                            codigos[int.Parse(item.tpe_tipo_persona_fk)] += ",";

                        codigos[int.Parse(item.tpe_tipo_persona_fk)] += item.tpe_id_persona_fk;
                    }
                }





                ////////////// PROVEEDORES

                if (!String.IsNullOrEmpty(codigos[2]))
                {
                    Model.ProveedoresList = objListing.AllProveedoresListing(codigos[2], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_proveedores proveedor in Model.ProveedoresList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.numero)
                            {
                                item.tpe_persona_nombre = proveedor.nombre;
                                break;
                            }
                        }
                    }
                }


                ////////////// AGENTES

                if (!String.IsNullOrEmpty(codigos[3]))
                {
                    Model.AgentesList = objListing.AllAgentesListing(codigos[3], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_agentes proveedor in Model.AgentesList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.agente_id)
                            {
                                item.tpe_persona_nombre = proveedor.agente;
                                break;
                            }
                        }
                    }
                }



                ////////////// CARRIERS

                if (!String.IsNullOrEmpty(codigos[4]))
                {
                    Model.CarriersList = objListing.AllCarrierListing(codigos[4], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_carriers carrier in Model.CarriersList)
                        {
                            if (item.tpe_id_persona_fk == carrier.carrier_id)
                            {
                                item.tpe_persona_nombre = carrier.name;
                                break;
                            }
                        }
                    }
                }





                ////////////// NAVIERAS

                if (!String.IsNullOrEmpty(codigos[5]))
                {
                    Model.NavierasList = objListing.AllNavierasListing(codigos[5], "", "");

                    foreach (Entidades item in Model.EntidadesList)
                    {
                        foreach (cs_navieras proveedor in Model.NavierasList)
                        {
                            if (item.tpe_id_persona_fk == proveedor.id_naviera)
                            {
                                item.tpe_persona_nombre = proveedor.nombre;
                                break;
                            }
                        }
                    }
                }

                return View("Index",Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;

        }


        // POST: Entidades/Create
        [HttpGet]
        public ActionResult Create(Entidades Model, string tpe_tipo_persona_fk, string pais_iso, string TextoBuscar, string Tipo, string BtnBuscar)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Create";
            ViewBag.Tipo = Tipo;
            ViewBag.DisplayPK = "display:none";
            ViewBag.DisplayBtn = "display:inline";

            try
            {

                if (String.IsNullOrEmpty(BtnBuscar)) //con enter en el dato, viene null
                {

                    Model.tpe_tps_fk = "1";
                    Model.tpe_insert_user = Session["UserID"].ToString();

                    if (ModelState.IsValid)
                    {
                        //int c = objListing.GetListasComplete(Model.tpe_tpl_fk);

                        //if (c > 0 ) { 

                            objListing.InsertEntidades(Model);

                            ViewBag.m = "Entidades Created Successfully";

                            return RedirectToAction(Tipo == "COSTO" ? "IndexCosto" : "IndexVenta", new { m = ViewBag.m});

                        //} else
                        //{
                        //    ViewBag.m = "No es posible Vincular la lista, no esta completa";
                        //}

                    }
                    else
                    {
                        ModelState.AddModelError("Error", "something its wrong");

                        //pais_iso = String.IsNullOrEmpty(TextoBuscar) ? pais_iso : (TextoBuscar.Length < 4 ? "999" : pais_iso);
                    }

                }


                Model.ListasList = objListing.AllListasListing("", Tipo, "", "", "");

                //Model.ListasList = Model.ListasList.Prepend(new Listas { tpl_pk = "", tpl_codigo = "Select" });

                IEnumerable<Listas> second = new[] { new Listas { tpl_pk = "", tpl_codigo = "Select" } };
                Model.ListasList = second.Concat(Model.ListasList);

                Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();

                if (Tipo == "COSTO")
                {
                    Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();
                    Model.TipoEntidadOptions.RemoveAt(1);
                }

                if (Tipo == "VENTA")
                {
                    Model.TipoEntidadOptions = new List<Opciones>();
                    Model.TipoEntidadOptions.Add(new Opciones { id = "", nombre = "Select" });
                    Model.TipoEntidadOptions.Add(new Opciones { id = "1", nombre = "Clientes" });
                }




                TextoBuscar = String.IsNullOrEmpty(TextoBuscar) ? "999999999999999999" : TextoBuscar;
                pais_iso = String.IsNullOrEmpty(pais_iso) ? "" : "'" + pais_iso + "'";

                /*
                if (String.IsNullOrEmpty(tpe_tipo_persona_fk)) {
                    Model.ClientesList = objListing.AllClientesListing("0", "", TextoBuscar);
                }
                */

                Model.EstadosOpciones = objListing.GetEstadosOptions();
                Model.EstadosOpciones.RemoveAt(0);

                Model.PaisesOpciones = objListing.GetPaisesOptions("", "", "");

                ViewBag.tpe_tipo_persona_fk = tpe_tipo_persona_fk;

                ViewBag.entidad_nombre = "";
                ViewBag.entidad_color = "";

                switch (tpe_tipo_persona_fk)
                {
                    case "1":
                        ViewBag.entidad_nombre = "Clientes";
                        ViewBag.entidad_color = "bg-default";
                        Model.ClientesList = objListing.AllClientesListing("", pais_iso, TextoBuscar);
                        break;

                    case "2":
                        ViewBag.entidad_nombre = "Proveedores";
                        ViewBag.entidad_color = "bg-info";
                        Model.ProveedoresList = objListing.AllProveedoresListing("", pais_iso, TextoBuscar);
                        break;

                    case "3":
                        ViewBag.entidad_nombre = "Agentes";
                        ViewBag.entidad_color = "bg-warning";
                        Model.AgentesList = objListing.AllAgentesListing("", pais_iso, TextoBuscar);
                        break;

                    case "4":
                        ViewBag.entidad_nombre = "Carriers";
                        ViewBag.entidad_color = "bg-primary";
                        Model.CarriersList = objListing.AllCarrierListing("", pais_iso, TextoBuscar);
                        break;

                    case "5":
                        ViewBag.entidad_nombre = "Navieras";
                        ViewBag.entidad_color = "bg-danger";
                        Model.NavierasList = objListing.AllNavierasListing("", pais_iso, TextoBuscar);
                        break;

                    default:
                        break;
                }

                return View("Form", Model);
            }
            catch (Exception e)
            {
                Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
            }

            return null;
        }

        // GET: Entidades/Edit/5
        [HttpGet]
        public ActionResult Edit(Entidades Model, string id, string Tipo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.actionType = "Update";

            ViewBag.Tipo = Tipo;
            ViewBag.id_entidad = id;

            ViewBag.DisplayPK = "display:inline";
            ViewBag.DisplayBtn = "display:none";


            try
            {



                Model.tpe_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateEntidades(Model);

                    ViewBag.m = "Entidades Updated Successfully";


                    return RedirectToAction(Tipo == "COSTO" ? "IndexCosto" : "IndexVenta", new { m = ViewBag.m });

                }
                else
                {
                    ModelState.AddModelError("Error", "something its wrong");

                }

                Model = objListing.GetEntidadesByID(id);  //Convert.ToString(


                Model.ListasList = objListing.AllListasListing("", Tipo, "", "", "");

                //Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();

                if (Tipo == "COSTO")
                {
                    Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();
                    Model.TipoEntidadOptions.RemoveAt(1);
                }

                if (Tipo == "VENTA")
                {
                    Model.TipoEntidadOptions = new List<Opciones>();
                    Model.TipoEntidadOptions.Add(new Opciones { id = "1", nombre = "Clientes" });
                }


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

        /*

        // POST: Entidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entidades Model, FormCollection collection, string  Tipo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            ViewBag.Tipo = Tipo;

            if (!t) return null;

            try
            {

                Model.tpe_update_user = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    objListing.UpdateEntidades(Model);

                    ViewBag.m = "Entidades Updated Successfully";


                    return RedirectToAction(Tipo == "COSTO" ? "IndexCosto" : "IndexVenta", new { m = ViewBag.m });

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

            Model.ListasList = objListing.AllListasListing("", Tipo, "", "", "");

            //Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();

            if (Tipo == "COSTO")
            {
                Model.TipoEntidadOptions = objListing.GetTipoEntidadOptions();
                Model.TipoEntidadOptions.RemoveAt(1);
            }

            if (Tipo == "VENTA")
            {
                Model.TipoEntidadOptions = new List<Opciones>();
                Model.TipoEntidadOptions.Add(new Opciones { id = "1", nombre = "Clientes" });
            }


            Model.EstadosOpciones = objListing.GetEstadosOptions();
            Model.EstadosOpciones.RemoveAt(0);

            return View("Form", Model);
        }
        */



        // GET: Entidades/Delete/5
        public ActionResult Delete(string id, string Tipo)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            ViewBag.s = "panel-info";
            ViewBag.c = "Entidades";
            ViewBag.a = "Delete";
            ViewBag.m = "";

            ViewBag.Tipo = Tipo;

            ViewBag.Tipo = String.IsNullOrEmpty(Tipo) ? "" : "?Tipo=" + Tipo;

            try
            {
                var Model = objListing.GetEntidadesByID(id);

                if (Model != null)
                {
                    objListing.DeleteEntidades(Model);

                    ViewBag.m = "Entidades Deleted Successfully";
                }

            }
            catch (Exception e)
            {
                //Response.Redirect("~/Login/Error?c=" + HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() + "&a=" + HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString() + "&m=" + e.Message);  //MessageBox.Show(e.Message, HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString());
                ViewBag.s = "panel-danger";
                ViewBag.m = e.Message;
            }


            //return RedirectToAction("Index", new { m = ViewBag.m});
            return PartialView("~/Views/Login/Error.cshtml");

        }


        public ActionResult Select(string id, string tipo_entidad, string id_lista, string entidad_nombre)
        {
            var t = valida.ValidaSession(Convert.ToString(Session["Name"]), Response);

            if (!t) return null;

            //var Model = objListing.GetArticulosByID(id);

            ViewBag.s = "panel-info";
            ViewBag.c = "Entidades";
            ViewBag.a = "Select";
            ViewBag.m = "";

            //ViewBag.id = String.IsNullOrEmpty(id) ? "" : "?id=" + id;
            //ViewBag.tipo_entidad = String.IsNullOrEmpty(tipo_entidad) ? "" : "&tipo_entidad=" + tipo_entidad;

            ViewBag.id = String.IsNullOrEmpty(id) ? "" : id;
            ViewBag.tipo_entidad = String.IsNullOrEmpty(tipo_entidad) ? "" : tipo_entidad;

            try
            {

                Entidades row = objListing.ValidaEntidadesList(id, tipo_entidad, id_lista, entidad_nombre);

                if (row.tpe_pk == null || row.tpe_pk < 1)
                {

                    ViewBag.m = "Entidad " + row.tpe_id_persona_fk + "  " + row.tpe_persona_nombre + " no tiene restriccion.";

                }
                else {

                    ViewBag.s = "panel-danger";
                    ViewBag.m = "Entidad " + row.tpe_id_persona_fk + "  " + row.tpe_persona_nombre + " ya tiene asignada la Lista " + row.tpe_tpl_nombre + " con las mismas caracterizticas. ";

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

    }
}
