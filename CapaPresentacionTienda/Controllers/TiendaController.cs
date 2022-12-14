using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using CapaEntidad.Paypal;
using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idProducto = 0)
        {

            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idProducto).FirstOrDefault();
            if (oProducto != null) 
            {
                oProducto.Base64 = CN_Recursos.CovertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extensiion = Path.GetExtension(oProducto.NombreImagen);
            }
            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListarCategorias()
        
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idCategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaPorCategoria(idCategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idCategoria, int idMarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca2 = p.oMarca2,
                oCategoria2 = p.oCategoria2,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.CovertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extensiion = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                p.oCategoria2.IdCategoria == (idCategoria == 0 ? p.oCategoria2.IdCategoria : idCategoria) &&
                p.oMarca2.IdMarca == (idMarca == 0 ? p.oMarca2.IdMarca : idMarca) &&
                p.Stock > 0 && p.Activo == true

            ).ToList();
            
            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;            
            bool existe = new CN_Carrito().ExisteCarrito(idCliente, idProducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = " el Producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idCliente, idProducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadCarrito() 
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idCliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarProductoCarrito()
        { 
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            
            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idCliente).Select(oc => new Carrito()
            {
                oProducto2 = new Producto()
                {
                    IdProducto = oc.oProducto2.IdProducto,
                    Nombre = oc.oProducto2.Nombre,
                    oMarca2 = oc.oProducto2.oMarca2,
                    Precio = oc.oProducto2.Precio,
                    RutaImagen = oc.oProducto2.RutaImagen,
                    Base64 = CN_Recursos.CovertirBase64(Path.Combine(oc.oProducto2.RutaImagen, oc.oProducto2.NombreImagen), out conversion),
                    Extensiion = Path.GetExtension(oc.oProducto2.NombreImagen)
                },
                Cantidad = oc.Cantidad

            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idProducto, bool sumar)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;
            
            respuesta = new CN_Carrito().OperacionCarrito(idCliente, idProducto, sumar, out mensaje);
           
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idCliente, idProducto);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            oLista = new CN_Ubicacion().ObtenerDepartamento();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ObtenerProvincias(idDepartamento);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = new CN_Ubicacion().ObtenerDistrito(idDepartamento, idProvincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }
        
        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost] 
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-CH");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();


            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto2.Precio;
                total += subtotal;

                oListaItem.Add(new Item
                {
                    name = oCarrito.oProducto2.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount() 
                    {
                        currency_code = "USD",
                        value = oCarrito.oProducto2.Precio.ToString("G", new CultureInfo("es-CH"))
                    }
                });
                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto2.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-CH")),
                    breakdown = new Breakdown()
                    { 
                        item_total = new ItemTotal() 
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-CH")),
                        },
                    }
                },
                description = "compra de articulos en mi tienda",
                items = oListaItem
            };

            Checkout_Order ocheckoutOrder = new Checkout_Order() 
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext() 
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44335/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44335/Tienda/Carrito"
                }
                
            };
            
            oVenta.MontoTotal = total;
            oVenta.IdCliente3 = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            CN_Paypal opaypayl = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            response_paypal = await opaypayl.CrearSolicitud(ocheckoutOrder);
                

            return Json(response_paypal, JsonRequestBehavior.AllowGet);

        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            response_paypal = await opaypal.AprobarPago(token);

            ViewData["Status"] = response_paypal.Status;

            if (response_paypal.Status)
            {
                Venta oVenta = (Venta) TempData["Venta"];
                DataTable detalle_venta = (DataTable) TempData["DetalleVenta"];

                oVenta.IdTrasaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTrasaccion;
            }
            return View();
        }

        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<DetalleVenta> oLista = new List<DetalleVenta>();

            bool conversion;

            oLista = new CN_Venta().ListarCompras(idCliente).Select(oc => new DetalleVenta()
            {
                oProducto3 = new Producto()
                {
                    Nombre = oc.oProducto3.Nombre,
                    oMarca2 = oc.oProducto3.oMarca2,
                    Precio = oc.oProducto3.Precio,
                    Base64 = CN_Recursos.CovertirBase64(Path.Combine(oc.oProducto3.RutaImagen, oc.oProducto3.NombreImagen), out conversion),
                    Extensiion = Path.GetExtension(oc.oProducto3.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTrasaccion = oc.IdTrasaccion

            }).ToList();

            return View(oLista);

        }

    }
}