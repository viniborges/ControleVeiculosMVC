using ControleVeiculoTCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleVeiculoTCS.Controllers
{
    public class HomeController : Controller
    {
        private DBveiculosTCSEntities db = new DBveiculosTCSEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getClientes()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var model = (db.Clientes.ToList()
                        .Select(x => new
                        {
                            clienteId = x.ClienteId,
                            nome = x.Nome,
                            endereco = x.Endereco,
                            email = x.Email
                        })).ToList();

            //Parametros adicionais no retorno do json, usados no JQuery Datable para listagem dos registros
            return Json(new
            {
                draw = draw,
                recordsFiltered = model.Count,
                recordsTotal = model.Count,
                data = model
            }, JsonRequestBehavior.AllowGet);
        }


    }
}