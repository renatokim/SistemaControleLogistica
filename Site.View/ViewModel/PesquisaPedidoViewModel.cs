using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Entidade.Pedidos;

namespace Site.View.ViewModel
{
    public class PesquisaPedidoViewModel
    {
        public PesquisaPedidoViewModel()
        {
            Cliente = new Cliente();
            Produto = new Produto();
            Tecido = new Tecido();
            Pedidos = new List<Pedido>();
            Vendedor = new Vendedor();
        }

        public Cliente Cliente { get; set; }
        public Produto Produto { get; set; }
        public Tecido Tecido { get; set; }
        public int Limite { get; set; }
        public int Status { get; set; }
        public Vendedor Vendedor { get; set; }
        public IList<Pedido> Pedidos { get; set; }
        public string PedidoSession { get; set; }
    }
}