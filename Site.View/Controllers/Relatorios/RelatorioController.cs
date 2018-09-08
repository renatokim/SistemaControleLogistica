using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.DTO;
using Site.DTO.Relatorio;
using Site.Entidade.Fluxos;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico;
using Site.IServico.Pedidos;
using Site.IServico.Relatorios;
using Site.Servico;
using Site.Servico.Pedidos;
using Site.Servico.Relatorios;

namespace Site.View.Controllers.Relatorios
{
    public class RelatorioController : Controller
    {
        private readonly IFaccaoServico _faccaoServico = new FaccaoServico();
        private readonly IPedidoServico _pedidoServico = new PedidoServico();
        private readonly IRelatorioServico _relatorioServico = new RelatorioServico();

        public ActionResult RelatorioPorFaccao()
        {
            TempData["Faccoes"] = _faccaoServico.GetAll().Where(s => s.Status == Status.Ativo).Where(t => t.Tipo == 1).OrderBy(d => d.Descricao) .ToList();

            var filtro = new DTOFiltroRelatorioPorFaccao
            {
                DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                DataFinal = DateTime.Now,
                Acao = Acao.Tudo,
                StatusFluxo = StatusFluxo.Todos
            };

            return View(filtro);
        }

        [HttpPost]
        public PartialViewResult RelatorioPorFaccao(DTOFiltroRelatorioPorFaccao filtro)
        {
            IList<Pedido> pedidos = _pedidoServico.PedidosRelatorioFaccao(filtro);
            return PartialView("_RelatorioPorFaccaoResultado", pedidos);
        }

        [HttpPost]
        public ActionResult RelatorioPorFaccaoResultado(DTOFiltroRelatorioFaccao filtro)
        {
            var retorno = _relatorioServico.RelatorioPorFaccaoResultado(filtro);
            return View(retorno);
        }
    }
}
