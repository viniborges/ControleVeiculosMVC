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
            //var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var model = (db.Clientes.ToList()
                        .Select(x => new
                        {
                            clienteId = x.ClienteId,
                            nome = x.Nome,
                            endereco = x.Endereco,
                            telefone = x.Telefone,
                            email = x.Email
                        })).ToList();

            //Parametros adicionais no retorno do json, usados no JQuery Datable para listagem dos registros
            return Json(new
            {
                //draw = draw,
                recordsFiltered = model.Count,
                recordsTotal = model.Count,
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        //Action para salvar o cliente e seus respectivos veículos
        public ActionResult salvarCliente(ClienteViewModel cli)
        {
            //Se localizar o cliente, é uma edição
            var findCli = db.Clientes.FirstOrDefault(x => x.ClienteId == cli.ClienteId);
            if (findCli != null)
            {
                findCli.Nome = cli.Nome;
                findCli.Endereco = cli.Endereco;
                findCli.Email = cli.Email;
                findCli.Telefone = cli.Telefone;
                
            }
            //Caso contrário, cria um novo registro
            else
            {
                var clienteId = Guid.NewGuid();
                var cliente = new Clientes()
                {
                    ClienteId = clienteId,
                    Nome = cli.Nome,
                    Endereco = cli.Endereco,
                    Telefone = cli.Telefone,
                    Email = cli.Email,
                };
                db.Clientes.Add(cliente);

                //Processa os veículos do cliente
                if (cli.Veiculos.Any())
                {
                    foreach (var item in cli.Veiculos)
                    {
                        var veiculoId = Guid.NewGuid();
                        var veiculo = new Veiculos()
                        {
                            VeiculoId = veiculoId,
                            ClienteId = clienteId,
                            Marca = item.Marca,
                            Modelo = item.Modelo,
                            Cor = item.Cor,
                            Ano = Convert.ToInt32(item.Ano),
                            Placa = item.Placa
                        };

                        db.Veiculos.Add(veiculo);
                    }
                }
            }

            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { error = false, message = "Cliente salvo com sucesso!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message });
            }

            return Json(new { error = true, message = "Erro ao salvar." });
        }

        public ActionResult getSingleCliente(Guid clienteId)
        {
            var model = (from cli in db.Clientes
                         where cli.ClienteId == clienteId
                         select new ClienteViewModel()
                         {
                             ClienteId = cli.ClienteId,
                             Nome = cli.Nome,
                             Endereco = cli.Endereco,
                             Telefone = cli.Telefone,
                             Email = cli.Email
                         }).SingleOrDefault();

            if (model != null)
            {
                model.Veiculos = (from veic in db.Veiculos
                                  where veic.ClienteId == model.ClienteId
                                  select new VeiculosViewModel()
                                  {
                                      VeiculoId = veic.VeiculoId,
                                      Marca = veic.Marca,
                                      Modelo = veic.Modelo,
                                      Cor = veic.Cor,
                                      Ano = veic.Ano,
                                      Placa = veic.Placa

                                  }).ToList();
            }

            return Json(new { result = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteVeiculo(Guid id)
        {
            var veiculo = db.Veiculos.Find(id);
            if (null != veiculo)
            {
                db.Veiculos.Remove(veiculo);
                db.SaveChanges();
                return Json(new { error = false });
            }
            return Json(new { error = true });
        }

        public ActionResult getSingleVeiculo(Guid id)
        {
            var veiculo = (from veic in db.Veiculos
                               where veic.VeiculoId == id
                               select new VeiculosViewModel()
                               {
                                   VeiculoId = veic.VeiculoId,
                                   Marca = veic.Marca,
                                   Modelo = veic.Modelo,
                                   Cor = veic.Cor,
                                   Ano = veic.Ano,
                                   Placa = veic.Placa
                               }).SingleOrDefault();

            return Json(new { result = veiculo }, JsonRequestBehavior.AllowGet);
        }

    }
}