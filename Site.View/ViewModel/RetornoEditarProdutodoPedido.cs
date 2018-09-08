using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Entidade.Pedidos;

namespace Site.View.ViewModel
{
    public class RetornoEditarProdutodoPedido
    {
        public RetornoEditarProdutodoPedido()
        {
            PedidoProduto = new PedidoProduto();
        }

        public string PedidoJson { get; set; }
        public PedidoProduto PedidoProduto { get; set; }
    }
}