using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using Newtonsoft.Json;
using Site.DTO;
using Site.DTO.Fluxo;
using Site.Entidade.Fluxos;
using Site.Enums;
using Site.IServico;
using Site.IServico.Fluxo;
using Site.IServico.Pedidos;
using Site.Servico;
using Site.Servico.Pedidos;
using Site.Transforms;
using Site.View.ViewModel;

namespace Site.View.Controllers.Fluxo
{
    public class FluxoController : BaseController
    {
        private readonly IFluxoServico _fluxoServicoServico = ServiceFactory.CreateInstance<IFluxoServico>();
        private readonly IPedidoServico _pedidoServico = ServiceFactory.CreateInstance<IPedidoServico>();
        private readonly IClienteServico _clienteServico = new ClienteServico();
        private readonly IFaccaoServico _faccaoServico = new FaccaoServico();

        public ActionResult Finalizar(int id)
        {
            var pedido = _pedidoServico.PedidoPorId(id);
            pedido.StatusPedido = StatusPedido.Concluido;
            _pedidoServico.Salvar(pedido);
            return RedirectToAction("Index");
        }

        public ActionResult Paginar(int pagina, string filtro)
        {
            var filtroPaginar = JsonConvert.DeserializeObject<DTOFluxoPaginacao>(filtro);
            filtroPaginar.PaginaAtual = pagina;
            var fluxoGeral = MontarFluxoGeral(filtroPaginar);
            return View("Index", fluxoGeral);
        }

        public ActionResult Ordenar(string coluna, string filtro)
        {
            var filtroPaginar = JsonConvert.DeserializeObject<DTOFluxoPaginacao>(filtro);
            filtroPaginar.Ordem = coluna;
            var fluxoGeral = MontarFluxoGeral(filtroPaginar);
            return View("Index", fluxoGeral);
        }

        [HttpPost]
        public ActionResult Index(DTOFluxoPaginacao filtro)
        {
            filtro.PaginaAtual = 0;
            var fluxoGeral = MontarFluxoGeral(filtro);
            return View(fluxoGeral);
        }

        public ActionResult Index()
        {
            var filtro = new DTOFluxoPaginacao
            {
                QtdePorPagina = 10,
                PaginaAtual = 0,
                Ordem = "id",
                SubTotal = "Sim"
            };

            var fluxoGeral = MontarFluxoGeral(filtro);
            return View(fluxoGeral);
        }

        public ActionResult LancarLote()
        {
            var loteFluxo = new LoteFluxoViewModel();
            return View(loteFluxo);
        }

        [HttpPost]
        public ActionResult LancarLote(LoteFluxoViewModel loteFluxo)
        {
            if (!loteFluxo.Pedidos.Any() || !loteFluxo.Etapas.Any() || !loteFluxo.Acoes.Any())
            {
                Mensagem("Campos Obrigatórios não preenchidos", TipoMensagem.Erro);
                return View(loteFluxo);
            }

            try
            {
                var dtoLoteFluxo = TransformDtoLoteFluxo(loteFluxo);

                var fluxoHistoricos = _fluxoServicoServico.MontarHistoricoFluxo(dtoLoteFluxo);
                loteFluxo.FluxoHistoricoExistentes = BuscarHistoricoFluxo(fluxoHistoricos);

                var idsFluxoExcluir = loteFluxo.LancamentosAtualizar.ToList();
                var idsFluxoHistorico = loteFluxo.FluxoHistoricoExistentes.Select(x => x.Id).ToList();
                var idsFluxoIgnorar = idsFluxoHistorico.Except(idsFluxoExcluir).ToList();

                IList<FluxoHistorico> fluxoIgnorar = new List<FluxoHistorico>();

                foreach (var fluxoHistorico in loteFluxo.FluxoHistoricoExistentes)
                {
                    foreach (var idFluxoIgnorar in idsFluxoIgnorar)
                    {
                        if (fluxoHistorico.Id == idFluxoIgnorar)
                        {
                            fluxoIgnorar.Add(fluxoHistorico);
                        }
                    }
                }

                if (idsFluxoExcluir.Count > 0)
                {
                    _fluxoServicoServico.Excluir(idsFluxoExcluir);
                }

                foreach (var fluxoHistorico in fluxoHistoricos)
                {
                    bool salvar = true;
                    foreach (var fluxo in fluxoIgnorar)
                    {
                        if (fluxo.Andamento == fluxoHistorico.Andamento
                            && fluxo.EnvioRetorno == fluxoHistorico.EnvioRetorno
                            && fluxo.Pedido.Id == fluxoHistorico.Pedido.Id
                            && fluxo.PedidoProduto.Id == fluxoHistorico.PedidoProduto.Id
                            )
                        {
                            salvar = false;
                        }
                    }

                    if (salvar)
                    {
                        _fluxoServicoServico.Salvar(fluxoHistorico);
                    }
                }

                Mensagem("Registros Lançados com Sucesso!", TipoMensagem.Sucesso);
                return RedirectToAction("LancarLote");
            }
            catch(Exception e)
            {
                Mensagem(e.Message, TipoMensagem.Erro);
                return View(loteFluxo);
            }
        }

        private List<FluxoHistorico> BuscarHistoricoFluxo(IList<FluxoHistorico> fluxoHistoricos)
        {
            var historicosJaLancados = new List<FluxoHistorico>();

            foreach (var fluxo in fluxoHistoricos)
            {
                var lancados = _fluxoServicoServico.GetAll()
                    .Where(ped => ped.Pedido.Id == fluxo.Pedido.Id)
                    .Where(prod => prod.PedidoProduto.Id == fluxo.PedidoProduto.Id)
                    .Where(er => er.EnvioRetorno == fluxo.EnvioRetorno)
                    .Where(a => a.Andamento == fluxo.Andamento)
                    .ToList();

                historicosJaLancados.AddRange(lancados);
            }

            return historicosJaLancados;
        }

        private DTOLoteFluxo TransformDtoLoteFluxo(LoteFluxoViewModel loteFluxo)
        {
            var dtoLoteFluxo = Transform.TransformObject<DTOLoteFluxo>(loteFluxo);
            dtoLoteFluxo.DadosEnvio = Transform.TransformObject<DTO.DadosEnvio>(loteFluxo.DadosEnvio);
            dtoLoteFluxo.PedidoProdutos = new List<DTO.PedidoProdutoLote>();

            if (loteFluxo.PedidoProdutos != null)
            {
                foreach (var pedidoProduto in loteFluxo.PedidoProdutos)
                {
                    dtoLoteFluxo.PedidoProdutos.Add(Transform.TransformObject<DTO.PedidoProdutoLote>(pedidoProduto));
                }
            }

            return dtoLoteFluxo;
        }

        private IList<string> FaccoesPorFluxo(DTOFluxoGeral dtoFluxoGeral)
        {
            IList<string> fluxos = new List<string>();

            int idPedido = dtoFluxoGeral.PedidoId;
            int andamento;
            int quantidade = 0;

            if (dtoFluxoGeral.QtdeArte > 0)
            {
                andamento = 0;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeModelagem > 0)
            {
                andamento = 1;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeMateriaPrima > 0)
            {
                andamento = 2;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeCorte > 0)
            {
                andamento = 3;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeSilk > 0)
            {
                andamento = 4;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeBordadoParte > 0)
            {
                andamento = 5;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeCostura > 0)
            {
                andamento = 6;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeBotoes > 0)
            {
                andamento = 7;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeConferencia > 0)
            {
                andamento = 8;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeEtapaExtra1 > 0)
            {
                andamento = 9;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeSilkPeca > 0)
            {
                andamento = 10;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeBordadePeca > 0)
            {
                andamento = 11;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeBotaoPosterior > 0)
            {
                andamento = 12;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeEtapaExtra2 > 0)
            {
                andamento = 13;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeEmbalagem > 0)
            {
                andamento = 14;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            if (dtoFluxoGeral.QtdeEntrega > 0)
            {
                andamento = 15;
                quantidade = -999;
                fluxos.AddRange(_fluxoServicoServico.FaccoesPorFluxo(idPedido, andamento, quantidade));
            }

            return fluxos;
        }

        private IList<DTOFluxoGeral> MontarFluxoGeral(DTOFluxoPaginacao filtro)
        {
            IList<DTOFluxoGeral> lista = new List<DTOFluxoGeral>();
            bool temFaccao = false;

            var fluxoGeral = _fluxoServicoServico.GetFluxoGeralPorFiltro(filtro);
            foreach (var dtoFluxoGeral in fluxoGeral)
            {
                dtoFluxoGeral.Faccoes = FaccoesPorFluxo(dtoFluxoGeral);

                foreach (var faccoes in dtoFluxoGeral.Faccoes)
                {
                    int intFaccao = Convert.ToInt32(faccoes.Split('_')[0]);
                    if (filtro.Faccoes.Contains(intFaccao))
                    {
                        temFaccao = true;
                    }
                }

                if (temFaccao)
                {
                    lista.Add(dtoFluxoGeral);
                }

                temFaccao = false;
            }

            if (filtro.Faccoes.Count > 0)
            {
                fluxoGeral.Clear();
                fluxoGeral = lista;
            }

            int qtdeRegistros = _fluxoServicoServico.QtdeRegistros(filtro);

            if (filtro.Faccoes.Count > 0)
            {
                qtdeRegistros = fluxoGeral.Count;
            }

            filtro.QtdTotalRegistros = qtdeRegistros;
            filtro.FluxoPaginacao = JsonConvert.SerializeObject(filtro);

            var qtdeBotoes = filtro.QtdTotalRegistros/filtro.QtdePorPagina +
                                     (filtro.QtdTotalRegistros%filtro.QtdePorPagina > 0 ? 1 : 0);

            TempData["QtdeBotoes"] = qtdeBotoes;
            TempData["Filtro"] = filtro;
            TempData["Clientes"] = _clienteServico.ClientesFluxo();
            TempData["Faccoes"] = _faccaoServico.FaccoesFluxo();

            return fluxoGeral;
        }
    }
}
