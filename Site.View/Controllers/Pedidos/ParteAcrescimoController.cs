using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using Newtonsoft.Json;
using Site.DTO;
using Site.DTO.Chamado;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico.Pedidos;
using Site.Servico;
using Site.View.ViewModel;

namespace Site.View.Controllers.Pedidos
{
    public class ParteAcrescimoController : Controller
    {
        private readonly IParteAcrescimoServico _parteAcrescimoServico = ServiceFactory.CreateInstance<IParteAcrescimoServico>();
        private readonly IAviamentoServico _aviamentoServico = ServiceFactory.CreateInstance<IAviamentoServico>();
        private readonly ITecidoServico _tecidoServico = ServiceFactory.CreateInstance<ITecidoServico>();
        private readonly IProdutoServico _produtoServico = ServiceFactory.CreateInstance<IProdutoServico>();
        private readonly IFornecedorServico _fornecedorServico = ServiceFactory.CreateInstance<IFornecedorServico>();
        private readonly ITamanhoServico _tamanhoServico = ServiceFactory.CreateInstance<ITamanhoServico>();

        public ActionResult AdicionarProdutoDoPedido(PedidoProdutoViewModel pedidoProdutoViewModel, string fornecedores, string tamanhos, string pedidoProdutoAtual, string pedidoAtual)
        {
            pedidoProdutoViewModel.Fornecedores = JsonConvert.DeserializeObject<int[]>(fornecedores);
            pedidoProdutoViewModel.Tamanhos = JsonConvert.DeserializeObject<TamanhoViewModel[]>(tamanhos);
            var pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();

            pedidoProduto.Fornecedores.Clear();
            foreach (var item in pedidoProdutoViewModel.Fornecedores)
            {
                pedidoProduto.Fornecedores.Add(new Fornecedor {Id = item});
            }

            pedidoProduto.ProdutoTamanhos.Clear();
            foreach (var item in pedidoProdutoViewModel.Tamanhos)
            {
                pedidoProduto.ProdutoTamanhos.Add(new ProdutoTamanho { Quantidade = item.Qtde , Tamanho = new Tamanho { Id = item.Id, Descricao = item.Tamanho }, Obs = item.Obs });
            }

            pedidoProduto.RowNum = DateTime.Now.ToLongTimeString().Replace(":", string.Empty);
            pedidoProduto.Produto = CarregarProduto(pedidoProdutoViewModel.Peca, pedidoProdutoViewModel.Modelo);
            pedidoProduto.Tecido = CarregarTecido(pedidoProdutoViewModel.Tecido, pedidoProdutoViewModel.Cor, pedidoProdutoViewModel.Codigo);
            pedidoProduto.Quantidade = pedidoProdutoViewModel.Quantidade;
            pedidoProduto.MasculinoFeminino = (MasculinoFeminino) pedidoProdutoViewModel.MasculinoFeminino;
            pedidoProduto.Obs = pedidoProdutoViewModel.Obs;
            pedidoProduto.Valor = !string.IsNullOrEmpty(pedidoProdutoViewModel.Valor)
                ? Convert.ToDouble(pedidoProdutoViewModel.Valor.Replace('.', ','))
                : 0;

            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            pedido.PedidoProduto.Add(pedidoProduto);

            pedido.PedidoJson = JsonConvert.SerializeObject(pedido);

            return View("_AdicionarProdutoDoPedido", pedido);
        }

        public JsonResult CarregarSelecTipoAviamento(string rowNum, string pedidoProdutoAtual)
        {
            var pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();

            var produtoParteAcrescimo = new ProdutoParteAcrescimo();

            foreach (var parteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
            {
                if (parteAcrescimo.RowNum == rowNum)
                {
                    produtoParteAcrescimo = parteAcrescimo;
                    break;
                }
            }

            var listaPartesAcrescimos = new List<ParteAcrescimo>();

            var partes = _parteAcrescimoServico.GetAllPartes();
            var acrescimos = _parteAcrescimoServico.GetAllAcrescimos();

            listaPartesAcrescimos.AddRange(partes);
            listaPartesAcrescimos.AddRange(acrescimos);

            var newParteAcrescimo = listaPartesAcrescimos.FirstOrDefault(x => x.Id == produtoParteAcrescimo.ParteAcrescimo.Id);

            var option = "<option value='0'></option>";

            if (newParteAcrescimo != null)
            {
                foreach (var item in newParteAcrescimo.TipoAviamento)
                {
                    //throw new Exception();
                    option += "<option value='" + item.TipoAviamento.Id + "'>" + item.TipoAviamento.Descricao + "</option>";
                }
            }

            var retorno = new RetornoSelectViewModel { RowNum = rowNum, Select = option };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarTamanhoProdutoDoPedido(string rowNum, string pedidoAtual)
        {
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            var todosTamanhos = _tamanhoServico.GetAllTamanhos();

            var tamanhos = new List<ProdutoTamanho>();
            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                if (pedidoProduto.RowNum == rowNum)
                {
                    tamanhos = pedidoProduto.ProdutoTamanhos.ToList();
                    break;
                }
            }

            var select = string.Empty;
            foreach (var t in tamanhos)
            {
                select += 
                    @"<tr class='tamanhos'>
			            <td class='qtdeT'>" + t.Quantidade + @"</td><td class='valorT'>" + t.Tamanho.Descricao + @"</td><td class='idT' style='display:none'>" + t.Tamanho.Id + @"</td><td class='obsT'>" + t.Obs + @"</td><td><a onclick='RemoveTamanho(this)'>Remover</a></td>
		            </tr>";
            }

            var retorno = new RetornoSelectViewModel
            {
                RowNum = rowNum,
                Select = select
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarFornecedorProdutoDoPedido(string rowNum, string pedidoAtual)
        {
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            var fornecedores = new List<Fornecedor>();

            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                if (pedidoProduto.RowNum == rowNum)
                {
                    fornecedores = pedidoProduto.Fornecedores.ToList();
                    break;
                }
            }

            var todosFornecedores = _fornecedorServico.GetAll().OrderBy(x => x.Descricao);

            string select = "<tbody>";
            foreach (var fornecedor in todosFornecedores)
            {
                var selecionado = string.Empty;

                foreach (var f in fornecedores)
                {
                    if (f.Id == fornecedor.Id)
                    {
                        selecionado = "checked";
                    }
                }

                select +=
                    @"</tr>
                        <td>
                            <input type='checkbox' " + selecionado + " class='checkbox' name='fornecedores_" + fornecedor.Id + @"' value='" + fornecedor.Id + @"'>
                        </td>
                        <td>
                            " + fornecedor.Descricao + @"
                        </td>
                    </tr>";
            }

            select += "</tbody>";

            var retorno = new RetornoSelectViewModel
            {
                RowNum = rowNum,
                Select = select
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public String CarregarSelectParteCategoria(string rowNum, string pedidoProdutoAtual)
        {
            var pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();
            var produtoParteAcrescimo = new ProdutoParteAcrescimo();
            var partesAcrescimos = new List<ParteAcrescimo>();

            var partesAll = _parteAcrescimoServico.GetAllPartes();
            var acrescimosAll = _parteAcrescimoServico.GetAllAcrescimos();
            partesAcrescimos.AddRange(partesAll);
            partesAcrescimos.AddRange(acrescimosAll);

            foreach (var parteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
            {
                if (parteAcrescimo.RowNum == rowNum)
                {
                    produtoParteAcrescimo = parteAcrescimo;
                    produtoParteAcrescimo.ParteAcrescimo = 
                        partesAcrescimos.FirstOrDefault(x => x.Id == produtoParteAcrescimo.ParteAcrescimo.Id);
                    break;
                }
            }

            var acrescimos = partesAcrescimos.Where(x => x.Categoria.Id == produtoParteAcrescimo.ParteAcrescimo.Categoria.Id).OrderBy(y => y.Descricao).ToList();
            var option = string.Empty;
                
            foreach (var item in acrescimos)
            {
                if (item.Id == produtoParteAcrescimo.ParteAcrescimo.Id)
                {
                    option += "<option value='" + item.Id + "' selected>" + item.Descricao + "</option>";
                }
                else
                {
                    option += "<option value='" + item.Id + "'>" + item.Descricao + "</option>";
                }
            }

            return option;
        }

        public ActionResult RemoverParteAcrescimo(string rowNum, string pedidoProdutoAtual)
        {
            var pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();
            foreach (var parteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
            {
                if (parteAcrescimo.RowNum == rowNum)
                {
                    pedidoProduto.ProdutoParteAcrescimo.Remove(parteAcrescimo);
                    break;
                }
            }

            pedidoProduto.PedidoProdutoJson = JsonConvert.SerializeObject(pedidoProduto);
            return View("_AdicionarParteAcrescimo", pedidoProduto);
        }

        public ActionResult AdicionarParteAcrescimo(string parteAcrescimo, string parteOuAcrescimo, string pedidoProdutoAtual, string rowNumRemover)
        {
            PedidoProduto pedidoProduto;
            try
            {
                var parteAcrescimoProdutoAtualViewModel = JsonConvert.DeserializeObject<ParteAcrescimoViewModel>(parteAcrescimo);
                var produtoParteAcrescimo = new ProdutoParteAcrescimo
                {
                    Quantidade = parteAcrescimoProdutoAtualViewModel.Quantidade,
                    Obs = parteAcrescimoProdutoAtualViewModel.Obs,
                    ParteAcrescimo = new ParteAcrescimo { Id = parteAcrescimoProdutoAtualViewModel.ParteAcrescimo },
                    RowNum = new Random().Next().ToString(CultureInfo.InvariantCulture)
                };

                var aviamentos = _aviamentoServico.GetAll();
                foreach (var item in parteAcrescimoProdutoAtualViewModel.Aviamentos)
                {
                    var aviamento = aviamentos
                        .Where(tipo => tipo.TipoAviamento.Id == item.TipoAviamento)
                        .Where(codigo => codigo.Codigo.Id == item.CodigoAviamento)
                        .First(cor => cor.Cor.Id == item.CorAviamento);

                    var parteAcrescimoAviamento = new ParteAcrescimoAviamento
                    {
                        Quantidade = item.QuantidadeAviamento,
                        Aviamento = aviamento
                    };

                    produtoParteAcrescimo.ParteAcrescimoAviamento.Add(parteAcrescimoAviamento);
                }

                if (!parteAcrescimoProdutoAtualViewModel.UsaMesmoTecido)
                {
                    produtoParteAcrescimo.Tecido = CarregarTecido(parteAcrescimoProdutoAtualViewModel.Tecido.TecidoItem, parteAcrescimoProdutoAtualViewModel.Tecido.CorItem, parteAcrescimoProdutoAtualViewModel.Tecido.CodigoItem);
                }

                var partesAll = _parteAcrescimoServico.GetAllPartes();
                var acrescimosAll = _parteAcrescimoServico.GetAllAcrescimos();

                var partesAcrescimos = new List<ParteAcrescimo>();
                partesAcrescimos.AddRange(partesAll);
                partesAcrescimos.AddRange(acrescimosAll);

                produtoParteAcrescimo.ParteAcrescimo =
                    partesAcrescimos.FirstOrDefault(x => x.Id == produtoParteAcrescimo.ParteAcrescimo.Id);

                pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();
                pedidoProduto.ProdutoParteAcrescimo.Add(produtoParteAcrescimo);

                if (rowNumRemover != "0")
                {
                    var itemRemover = pedidoProduto.ProdutoParteAcrescimo.FirstOrDefault(x => x.RowNum == rowNumRemover);
                    if (itemRemover != null)
                    {
                        pedidoProduto.ProdutoParteAcrescimo.Remove(itemRemover);
                    }
                }
            }
            catch
            {
                return View("_AdicionarParteAcrescimo", new PedidoProduto());
            }

            pedidoProduto.PedidoProdutoJson = JsonConvert.SerializeObject(pedidoProduto);
             
            return View("_AdicionarParteAcrescimo", pedidoProduto);
        }

        public string CarregarTableItens(string pedidoProdutoAtual, string rowNum)
        {
            var pedidoProduto = JsonConvert.DeserializeObject<PedidoProduto>(pedidoProdutoAtual) ?? new PedidoProduto();

            var parteAcrescimoAtual = new ProdutoParteAcrescimo();

            foreach (var parteAcrescimo in pedidoProduto.ProdutoParteAcrescimo)
            {
                if (parteAcrescimo.RowNum == rowNum)
                {
                    parteAcrescimoAtual = parteAcrescimo;
                    break;
                }

            }

            var table =
              @"<table width='100%' id='tableItens' class='table'>
                    <tbody>";

            foreach (var parteAcrescimoAviamento in parteAcrescimoAtual.ParteAcrescimoAviamento)
            {
                parteAcrescimoAviamento.Aviamento =
                    _aviamentoServico.GetAviamentoByFilter(new DTOFiltroAviamento
                    {
                        Id = parteAcrescimoAviamento.Aviamento.Id
                    }).First();

                table += @"<tr>
                            <td>
                                <input name='QuantidadeAviamento' type='hidden' value='" + parteAcrescimoAviamento.Quantidade + @"' />
                                <input name='TipoAviamento' type='hidden' value='" + parteAcrescimoAviamento.Aviamento.TipoAviamento.Id + @"' />
                                <input name='CorAviamento' type='hidden' value='" + parteAcrescimoAviamento.Aviamento.Cor.Id + @"' />
                                <input name='CodigoAviamento' type='hidden' value='" + parteAcrescimoAviamento.Aviamento.Codigo.Id + @"' />
                                " + parteAcrescimoAviamento.Quantidade + @" "
                                  + parteAcrescimoAviamento.Aviamento.TipoAviamento.Descricao + @" "
                                  + parteAcrescimoAviamento.Aviamento.Cor.Descricao + @" "
                                  + parteAcrescimoAviamento.Aviamento.Codigo.Descricao + @"
                            </td>
                            <td>
                                <a onclick='RemoveAviamento(this)'>Remover</a>
                            </td>
                        </tr>";
            }
            table += @"</tbody>
                </table>";

            return table;
        }

        public ActionResult EditarParteAcrescimoProduto(string rowNum, string pedidoAtual)
        {
            Pedido pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            var pedidoProduto = pedido.PedidoProduto.First(x => x.RowNum == rowNum);

            pedidoProduto.PedidoProdutoJson = JsonConvert.SerializeObject(pedidoProduto);
            return View("_AdicionarParteAcrescimo", pedidoProduto);
        }

        public string MostrarItens(string rowNum, string pedidoAtual)
        {
            Pedido pedido = JsonConvert.DeserializeObject<Pedido>(pedidoAtual) ?? new Pedido();
            var pedidoProduto = pedido.PedidoProduto.First(x => x.RowNum == rowNum);

            string table = @"<table class='table'><tbody><tr><th>Quantidade</th><th>Item</th><th>Aviamento</th><th>Obs</th><th>Tecido</th>";
            foreach (var item in pedidoProduto.ProdutoParteAcrescimo)
            {
                var partAcr = _parteAcrescimoServico.GetParteById(item.ParteAcrescimo.Id);
                item.ParteAcrescimo = partAcr ?? _parteAcrescimoServico.GetAcrescimoById(item.ParteAcrescimo.Id);

                string aviamentos = string.Empty;

                foreach (var aviamento in item.ParteAcrescimoAviamento)
                {
                    aviamento.Aviamento = _aviamentoServico.GetAviamentoByFilter(new DTOFiltroAviamento {Id = aviamento.Aviamento.Id}).First();

                    aviamentos += string.Format("{0} {1} {2} {3} <br>", aviamento.Quantidade, aviamento.Aviamento.TipoAviamento.Descricao,
                        aviamento.Aviamento.Cor.Descricao, aviamento.Aviamento.Codigo.Descricao);
                }

                if (item.Tecido.Id > 0)
                {
                    item.Tecido = _tecidoServico.GetById(item.Tecido.Id);
                }

                table += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", item.Quantidade, item.ParteAcrescimo.Nome, aviamentos, item.Obs, item.Tecido.Nome);
            }

            table += "</table>";
            return table;
        }

        public ActionResult ManterAcrescimo(Status status = Status.Ativo)
        {
            var acrescimo = new ParteAcrescimo
            {
                ParteOuAcrescimo = ParteOuAcrescimo.Acrescimo,
                Status = status
            };
            return View("ManterParte", acrescimo);
        }

        public ActionResult ManterParte(Status status = Status.Ativo)
        {
            var parte = new ParteAcrescimo
            {
                ParteOuAcrescimo = ParteOuAcrescimo.Parte,
                Status = status
            };
            return View(parte);
        }

        [HttpPost]
        public ActionResult ManterParte(ParteAcrescimo parte, string tipoAviamentoString)
        {
            var parteAcrescimoAviamento = JsonConvert.DeserializeObject<IList<ParteAcrescimoTipoAviamentoViewModel>>(tipoAviamentoString) ?? new List<ParteAcrescimoTipoAviamentoViewModel>();
            parte.TipoAviamento = new List<ParteAcrescimoTipoAviamento>();

            foreach (var item in parteAcrescimoAviamento)
            {
                var tipo = new ParteAcrescimoTipoAviamento
                {
                    Id = item.Id,
                    ParteAcrescimo = new ParteAcrescimo { Id = parte.Id },
                    TipoAviamento = new TipoAviamento { Id = item.Aviamento },
                    ConsumoAviamento = item.Qtde
                };

                parte.TipoAviamento.Add(tipo);
            }
            

            if (!parteAcrescimoAviamento.Any())
            {
                var mensagem = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Alerta,
                    Mensagem = "Informe o(s) tipo de aviamento(s)!"
                };

                TempData["Mensagem"] = mensagem;
                return View(parte);
            }

            try
            {
                _parteAcrescimoServico.Save(parte);
                var mensagem = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Sucesso,
                    Mensagem = "Cadastro Atualizado com Sucesso!"
                };

                TempData["Mensagem"] = mensagem;
                if (parte.ParteOuAcrescimo == ParteOuAcrescimo.Parte)
                {
                    return RedirectToAction("ManterParte");
                }

                return RedirectToAction("ManterAcrescimo");
            }
            catch
            {
                return View(parte);
            }
        }

        public ActionResult PartePorCategoriaId(int idCategoria)
        {
            var listaParte = _parteAcrescimoServico.GetAllPartes();
            var partes = listaParte.Where(x => x.Categoria.Id == idCategoria).OrderBy(y => y.Descricao) .ToList();

            return View("_PartePorCategoriaId", partes);
        }

        public ActionResult AcrescimoPorCategoriaId(int idCategoria)
        {
            var listaAcrescimos = _parteAcrescimoServico.GetAllAcrescimos();
            var acrescimos = listaAcrescimos.Where(x => x.Categoria.Id == idCategoria).OrderBy(y => y.Descricao).ToList();

            return View("_AcrescimoPorCategoriaId", acrescimos);
        }

        public ActionResult AcrescimoPorCategoriaIdEditar(int idCategoria, string rowNum)
        {
            var listaAcrescimos = _parteAcrescimoServico.GetAllAcrescimos();
            var acrescimos = listaAcrescimos.Where(x => x.Categoria.Id == idCategoria).OrderBy(y => y.Descricao).ToList();

            return View("_AcrescimoPorCategoriaId", acrescimos);
        }

        public ActionResult TiposAviamentos(int idAviamento)
        {
            var listaPartesAcrescimos = new List<ParteAcrescimo>();
            var partes = _parteAcrescimoServico.GetAllPartes();
            var acrescimos = _parteAcrescimoServico.GetAllAcrescimos();

            listaPartesAcrescimos.AddRange(partes);
            listaPartesAcrescimos.AddRange(acrescimos);

            var parteAcrescimo = listaPartesAcrescimos.FirstOrDefault(x => x.Id == idAviamento);
            return View("_TiposAviamentos", parteAcrescimo);
        }

        public ActionResult Editar(int id)
        {
            IList<ParteAcrescimo> parteAcrescimos = new List<ParteAcrescimo>();

            var partes = _parteAcrescimoServico.GetAllPartes().ToList();
            var acrescimos = _parteAcrescimoServico.GetAllAcrescimos().ToList();

            parteAcrescimos.AddRange(partes);
            parteAcrescimos.AddRange(acrescimos);

            var parteAcrescimo = parteAcrescimos.First(x => x.Id == id);
            return View("ManterParte", parteAcrescimo);
        }

        private Tecido CarregarTecido(int tipoTecido, int cor, int codigo)
        {
            var filtroTecido = new DTOFiltroTecido
            {
                TipoTecido = tipoTecido,
                Cor = cor,
                Codigo = codigo
            };

            var tecido = _tecidoServico.GetTecidosByFilter(filtroTecido).FirstOrDefault();
            return tecido;
        }

        private Produto CarregarProduto(int peca, int modelo)
        {
            var filtroProduto = new DTOFiltroProduto
            {
                Peca = peca,
                Modelo = modelo
            };

            var produto = _produtoServico.GetByFilter(filtroProduto).FirstOrDefault();
            return produto;
        }
    }
}
