using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using Newtonsoft.Json;
using Site.DTO;
using Site.DTO.Relatorio;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico.Pedidos;
using Site.IServico.Relatorios;
using Site.Servico;
using Site.View.ViewModel;

namespace Site.View.Controllers.Pedidos
{
    public class PedidoController : BaseController
    {
        private readonly IPedidoServico _pedidoServico = ServiceFactory.CreateInstance<IPedidoServico>();
        private readonly IPedidoProdutoServico _pedidoProdutoServico = ServiceFactory.CreateInstance<IPedidoProdutoServico>();
        private readonly IParteAcrescimoServico _parteAcrescimoServico = ServiceFactory.CreateInstance<IParteAcrescimoServico>();
        private readonly IParteAcrescimoAviamentoServico _parteAcrescimoAviamentoServico = ServiceFactory.CreateInstance<IParteAcrescimoAviamentoServico>();
        private readonly IAviamentoServico _aviamentoServico = ServiceFactory.CreateInstance<IAviamentoServico>();
        private readonly IProdutoParteAcrescimoServico _produtoParteAcrescimoServico = ServiceFactory.CreateInstance<IProdutoParteAcrescimoServico>();
        private readonly ITecidoServico _tecidoServico = ServiceFactory.CreateInstance<ITecidoServico>();
        private readonly IRelatorioServico _relatorioServido = ServiceFactory.CreateInstance<IRelatorioServico>();
        
        [HttpPost]
        public string AlterarObs(int id, string texto)
        {
            var pedido = _pedidoServico.PedidoPorId(id);
            pedido.ObsDia = texto;
            _pedidoServico.Salvar(pedido);
            return texto;
        }

        [HttpPost]
        public ActionResult CreatePesquisa(string pedidoAtualPesquisa)
        {
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtualPesquisa) ?? new Pedido();
            pedido.PedidoJson = JsonConvert.SerializeObject(pedido);
            pedido.AbaAndamento = 1;
            return View("Create", pedido);
        }

        public ActionResult Create()
        {
            var pedido = new Pedido
            {
                DataPrevisaoEntrega = DateTime.Now.AddDays(30),
                AbaAndamento = 1
            };

            return View(pedido);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Create(Pedido pedido)
        {
            var pedidoSession = JsonConvert.DeserializeObject<Pedido>(pedido.PedidoJson) ?? new Pedido();
            pedido.PedidoProduto = pedidoSession.PedidoProduto;

            try
            {
                _pedidoServico.Salvar(pedido);
                return RedirectToAction("Create");
            }
            catch(Exception exception)
            {
                Response.Write(exception);
                Response.Write(exception.Message);
                Response.End();
                return View("Create");
            }
        }

        public ActionResult Edit(int id)
        {
            var pedido = _pedidoServico.PedidoPorId(id);
            pedido.PedidoJson = JsonConvert.SerializeObject(pedido);
            return View("Create", pedido);
        }

        public ActionResult ConsumoMP(int id)
        {
            var pedidos = new List<int> {id};

            var filtro = new DTOFiltroRelatorioMP
            {
              IdPedidos = pedidos,
              Ordem = OrdemRelatorio.Tecido,
              ExibirAviamentos = true,
              ExibirSubTotal = true
            };

            return ConsumoMateriaPrima(filtro);
        }

        public ActionResult Copy(int id)
        {
            var pedido = _pedidoServico.PedidoPorId(id);
            pedido.Id = 0;
            pedido.StatusPedido = StatusPedido.Aberto;
            pedido.DataPrevisaoEntrega = DateTime.Now.AddDays(30);
            pedido.DataCriacao = DateTime.Now;
            pedido.AbaAndamento = 1;

            foreach (var item in pedido.PedidoProduto)
            {
                item.Quantidade = 0;
                item.ProdutoTamanhos.Clear();
                ZerarIdPedidoProduto(item);
            }

            pedido.PedidoJson = JsonConvert.SerializeObject(pedido);
            return View("Create", pedido);
        }


        [HttpPost]
        public ActionResult BuscarProdutoPedido(Pedido pedido)
        {
            var newPedido = string.IsNullOrEmpty(pedido.PedidoJson) ? new Pedido() : JsonConvert.DeserializeObject<Pedido>(pedido.PedidoJson);
            pedido.PedidoProduto = newPedido.PedidoProduto;
            var pedidoPesquisa = new PesquisaPedidoViewModel();
            pedidoPesquisa.PedidoSession = JsonConvert.SerializeObject(pedido);
            return View("CopiarPedido", pedidoPesquisa);
        }

        public ActionResult CopiarPedido()
        {
            var pedido = new PesquisaPedidoViewModel();
            return View(pedido);
        }

        [HttpPost]
        public ActionResult CopiarPedido(PesquisaPedidoViewModel pesquisaPedidoViewModel)
        {
            var filtro = new DTOFiltroPedido
            {
                ClienteId = pesquisaPedidoViewModel.Cliente.Id,
                ProdutoId = pesquisaPedidoViewModel.Produto.Id,
                StatusPedido = pesquisaPedidoViewModel.Status,
                TecidoId = pesquisaPedidoViewModel.Tecido.Id,
                VendedorId = pesquisaPedidoViewModel.Vendedor.Id,
                Limite = pesquisaPedidoViewModel.Limite
            };

            var pedidos = _pedidoServico.ListarPedidos(filtro).OrderBy(c => c.Cliente.Nome).ToList();
            pesquisaPedidoViewModel.Pedidos = pedidos;

            return View(pesquisaPedidoViewModel);
        }

        public ActionResult ConsultarPedido()
        {
            var consulta = new ConsultaPedidoViewModel
            {
                DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                DataFinal = DateTime.Now
            };

            return View(consulta);
        }

        public ActionResult CancelarPedido(int id)
        {
            try
            {
                var pedido = _pedidoServico.PedidoPorId(id);
                pedido.StatusPedido = StatusPedido.Cancelado;
                _pedidoServico.Salvar(pedido);
                Mensagem("Pedido Cancelado!", TipoMensagem.Sucesso);
            }
            catch (Exception exception)
            {
                Mensagem(exception.Message, TipoMensagem.Erro);
            }

            return RedirectToAction("ConsultarPedido");
        }

        [HttpPost]
        public ActionResult ConsultarPedido(ConsultaPedidoViewModel pedidoViewModel)
        {
            try
            {
                pedidoViewModel.Pedidos =
                _pedidoServico.ListarPedidos(new DTOFiltroPedido
                {
                    DataInicial = pedidoViewModel.DataInicial,
                    DataFinal = pedidoViewModel.DataFinal
                });
            }
            catch (Exception exception)
            {
                
                Mensagem(exception.Message, TipoMensagem.Erro);
            }

            return View(pedidoViewModel);
        }

        public ActionResult ConsumoMateriaPrima()
        {
            var filtro = new DTOFiltroRelatorioMP();

            filtro.DataFinal = DateTime.Now;
            filtro.DataInicial = DateTime.Now.AddDays(-30);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult ConsumoMateriaPrima(DTOFiltroRelatorioMP filtro)
        {
            TempData["Filtro"] = filtro;

            try
            {
                var pedidoList = new List<Pedido>();
                foreach (var id in filtro.IdPedidos)
                {
                    var newPedido = _pedidoServico.PedidoPorId(id);
                    pedidoList.Add(newPedido);
                }

                var relatorioMP = new List<RelatorioMateriaPrima>();
                var relatorios = new List<RelatorioMateriaPrimaViewModel>();
                var relatorioAviamento = _relatorioServido.ConsumoAviamentos(pedidoList.Select(x => x.Id).ToList());
                TempData["RelatorioAviamento"] = relatorioAviamento;

                foreach (var pedido in pedidoList)
                {
                    foreach (var pedidoProduto in pedido.PedidoProduto)
                    {
                        var relatorioProduto = new RelatorioMateriaPrima
                        {
                            Pedido =  pedido.Id + " " + pedido.Cliente.Nome,
                            QtdeProduto = pedidoProduto.Quantidade,
                            Produto = pedidoProduto.Produto.Nome,
                            TecidoProduto = pedidoProduto.Tecido.Nome,
                            ConsumoTecido = pedidoProduto.Produto.RetiraTecido,
                            ProporcaoMalha = pedidoProduto.Tecido.Proporcao,
                            ParteAcrescimo = "CORPO",
                            EhProduto = true
                        };

                        relatorioMP.Add(relatorioProduto);

                        foreach (var produtoParteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
                        {
                            var produtoPA =
                                _produtoParteAcrescimoServico.GetById(produtoParteAcrescimo.Id);

                            var tecidoPA = string.Empty;

                            if (produtoPA.Tecido.Id > 0)
                            {
                                tecidoPA = _tecidoServico.GetById(produtoPA.Tecido.Id).Nome;
                            }

                            var partAcresc =
                                _parteAcrescimoServico.GetAllPartes().FirstOrDefault(x => x.Id == produtoParteAcrescimo.ParteAcrescimo.Id) ??
                                _parteAcrescimoServico.GetAllAcrescimos().FirstOrDefault(x => x.Id == produtoParteAcrescimo.ParteAcrescimo.Id);

                            var relatorioParteAcrescimo = new RelatorioMateriaPrima
                            {
                                Pedido = pedido.Id + " " + pedido.Cliente.Nome,
                                QtdeProduto = pedidoProduto.Quantidade,
                                Produto = pedidoProduto.Produto.Nome,
                                QtdeParteAcrescimo = produtoParteAcrescimo.Quantidade,
                                ParteAcrescimo = partAcresc.Nome,
                                ConsumoTecidoParteAcrescimo = partAcresc.ConsumoTecido,
                                ProporcaoMalha = pedidoProduto.Tecido.Proporcao,
                                TecidoParteAcrescimo = tecidoPA,
                                TecidoProduto = pedidoProduto.Tecido.Nome
                            };

                            foreach (var parteAcrescimoAviamento in produtoParteAcrescimo.ParteAcrescimoAviamento)
                            {
                                var aviamento =
                                    _parteAcrescimoAviamentoServico.ParteAcrescimoAviamentosPorParteAcrescimo(
                                        parteAcrescimoAviamento.ProdutoParteAcrescimo.Id).Where(x => x.Id == parteAcrescimoAviamento.Id);

                                foreach (var a in aviamento)
                                {
                                    var av = _aviamentoServico.GetAviamentoByFilter(new DTOFiltroAviamento { Id = a.Aviamento.Id }).FirstOrDefault();
                                    relatorioParteAcrescimo.Aviamentos += av.TipoAviamento.Descricao + " ";
                                }
                            }

                            relatorioMP.Add(relatorioParteAcrescimo);

                        }
                    }

                    foreach (var item in relatorioMP)
                    {
                        var relatorio = new RelatorioMateriaPrimaViewModel
                        {
                            Aviamentos = item.Aviamentos,
                            Pedido = item.Pedido,
                            Qtde = item.QtdeProduto,
                            Produto = item.Produto,
                            QtdeParteAcrescimo = item.QtdeParteAcrescimo,
                            ParteAcrescimo = item.ParteAcrescimo,
                            Tecido = item.EhProduto ? item.TecidoProduto : (item.TecidoParteAcrescimo.Trim() != string.Empty ? item.TecidoParteAcrescimo : item.TecidoProduto),
                            Consumo = item.ConsumoTecido > 0 ? (item.ProporcaoMalha > 0 ? item.ConsumoTecido / item.ProporcaoMalha : item.ConsumoTecido) : (item.ProporcaoMalha > 0 ? item.ConsumoTecidoParteAcrescimo / item.ProporcaoMalha : item.ConsumoTecidoParteAcrescimo),
                            EhProduto = item.EhProduto,
                            ProporcaoMalha = item.ProporcaoMalha,
                            UnidadeMedida = item.UnidadeMedida
                        };

                        if (item.QtdeParteAcrescimo > 0)
                        {
                            relatorio.SubTotalConsumo = relatorio.Qtde * relatorio.QtdeParteAcrescimo * relatorio.Consumo;
                        }
                        else
                        {
                            relatorio.SubTotalConsumo = relatorio.Qtde * relatorio.Consumo;
                        }

                        relatorios.Add(relatorio);
                    }

                    relatorioMP.Clear();
                }

                var relatorioOrdenado = new List<RelatorioMateriaPrimaViewModel>();
                switch (filtro.Ordem)
                {
                    case OrdemRelatorio.Tecido:
                        relatorioOrdenado = relatorios.OrderBy(x => x.Tecido).ToList();
                        break;
                    case OrdemRelatorio.Produto:
                        relatorioOrdenado = relatorios.OrderBy(x => x.Produto).ToList();
                        break;
                    case OrdemRelatorio.Pedido:
                        relatorioOrdenado = relatorios.OrderBy(x => x.Pedido).ToList();
                        break;
                }

                return View("ConsumoMP", relatorioOrdenado);
            }
            catch (Exception exception)
            {
                Mensagem(exception.Message, TipoMensagem.Erro);
                return View("ConsumoMP", new List<RelatorioMateriaPrimaViewModel>());
            }
        }

        public string ProdutosPorPedidos(int id)
        {
            var pedido = _pedidoServico.PedidoPorId(id);
            string table = "<table class='table'><tr><th>Qtde / Produto / Tecido</th><th></th></tr>";
            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                table += "<tr><td>" + pedidoProduto.Quantidade + " " + pedidoProduto.Produto.Nome + " " + pedidoProduto.Tecido.Nome + "  </td><td><a hidden onclick='mostrarItens()'>Itens</a><button type='button' class='btn btn-mini btn_incluir' data-id='" + pedidoProduto .Id + "' data-toggle='button'>Incluir</button></td></tr>";
            }
            table += "</table>";
            return table;
        }

        public string IncluirProdutoPesquisa(int id, string pedidoAtual)
        {
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            var pedidoProduto = _pedidoProdutoServico.PedidoProdutoPorId(id);
            ZerarIdPedidoProduto(pedidoProduto);
            pedido.PedidoProduto.Add(pedidoProduto);
            var pedidoString = JsonConvert.SerializeObject(pedido);
            return pedidoString;
        }

        public string PedidosRelatorio(string dataInicial, string dataFinal)
        {
            var filtro = new DTOFiltroPedido
            {
                DataInicial = Convert.ToDateTime(dataInicial),
                DataFinal = Convert.ToDateTime(dataFinal)
            };

            var pedidos = _pedidoServico.ListarPedidos(filtro);
            var table = @"<table class='table table-condensed'>
                        <tbody>
                            <tr>
                                <th>
                                </th>
                                <th>
                                    Pedido/Cliente
                                </th>
                                <th>
                                    Valor
                                </th>
                                <th>
                                    Data
                                </th>
                                <th>
                                    Status
                                </th>
                            </tr>";

            foreach (var pedido in pedidos)
            {
                table += @"<tr>
                                <td>
                                    <input class='checkPedidos' type='checkbox' name='idPedidos[" + pedido.Id + @"]' value='" + pedido.Id + @"'>
                                </td>
                                <td>
                                    " + pedido.Id + @" " + pedido.Cliente.Nome + @"
                                </td>
                                <td>
                                    " + pedido.PedidoProduto.Sum(x => x.Valor * x.Quantidade).ToString("N2") + @"
                                </td>
                                <td>
                                    " + pedido.DataCriacao.ToString("dd/MM/yyyy") + @"
                                </td>
                                <td>
                                    " + pedido.StatusPedido + @"
                                </td>
                            </tr>";
            }
            table += @"</tbody>
                    </table>";
            
            return table;
        }

        private void ZerarIdPedidoProduto(PedidoProduto pedidoProduto)
        {
            pedidoProduto.Id = 0;
            foreach (var parteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
            {
                    parteAcrescimo.Id = 0;
                    foreach (var aviamento in parteAcrescimo.ParteAcrescimoAviamento)
                    {
                        aviamento.Id = 0;
                    }
            }
        }
    }
}