using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Entidade.Fluxos;

namespace Site.View.ViewModel
{
    public class LoteFluxoViewModel
    {
        public LoteFluxoViewModel()
        {
            Pedidos = new List<int>();
            Etapas = new List<int>();
            Acoes = new List<int>();
            PedidoProdutos = new List<PedidoProdutoLote>();
            FluxoHistoricoExistentes = new List<FluxoHistorico>();
            DadosEnvio = new DadosEnvio();
            LancamentosAtualizar = new List<int>();
        }

        public IList<int> Pedidos { get; set; }
        public IList<int> Etapas { get; set; }
        public IList<int> Acoes { get; set; }
        public IList<PedidoProdutoLote> PedidoProdutos { get; set; }
        public IList<FluxoHistorico> FluxoHistoricoExistentes { get; set; }
        public DadosEnvio DadosEnvio { get; set; }
        public IList<int> LancamentosAtualizar { get; set; } 
    }

    public class PedidoProdutoLote
    {
        public int Produto { get; set; }
        public int Quantidade { get; set; }
        public int Pedido { get; set; }
    }

    public class DadosEnvio
    {
        public DateTime Data { get; set; }
        public int FaccaoId { get; set; }
        public int Intervalo { get; set; }
        public int Status { get; set; }
    }
}