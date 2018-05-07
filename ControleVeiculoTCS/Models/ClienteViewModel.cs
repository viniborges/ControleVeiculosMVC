using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleVeiculoTCS.Models
{
    public class ClienteViewModel
    {
        public ClienteViewModel()
        {
            this.Veiculos = new List<VeiculosViewModel>();
        }

        public System.Guid ClienteId { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public virtual ICollection<VeiculosViewModel> Veiculos { get; set; }
    }

    public class VeiculosViewModel
    {
        public System.Guid VeiculoId { get; set; }
        public System.Guid ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
    }
}