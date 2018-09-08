using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Entidade.Pedidos;

namespace Site.View.ViewModel
{
    public class ConsultaPedidoViewModel
    {
        public ConsultaPedidoViewModel()
        {
            Pedidos = new List<Pedido>();
        }

        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public IList<Pedido> Pedidos { get; set; }
    }
}