using System;
using System.Linq;
using System.Web.Mvc;
using Site.DTO;
using Site.Enums;
using Site.IServico;
using Site.Servico;

namespace Site.View.Controllers
{
    public class EntradaEstoqueController : Controller
    {
        private readonly IEntradaEstoqueServico _entradaEstoqueServico = ServiceFactory.CreateInstance<IEntradaEstoqueServico>();
        private readonly IMateriaPrimaServico _materiaPrimaServico = ServiceFactory.CreateInstance<IMateriaPrimaServico>();
        private readonly ITecidoServico _tecidoServico = ServiceFactory.CreateInstance<ITecidoServico>();
        private readonly IFaccaoServico _faccaoServico = ServiceFactory.CreateInstance<IFaccaoServico>();

        public PartialViewResult Estoque(char entradaSaida, char tecidoAviamento)
        {
            var filtro = new DTOFiltroEntradaEstoque
            {
                EntradaSaida = entradaSaida == 'E' ? EntradaOuSaida.Entrada : EntradaOuSaida.Saida,
                TecidoAviamento = tecidoAviamento == 'T' ? TecidoOuAviamento.Tecido : TecidoOuAviamento.Aviamento
            };

            var estoque = _entradaEstoqueServico.GetByFilter(filtro).OrderByDescending(x => x.Data).Take(100);

            return PartialView("_Estoque", estoque);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult RelatorioEstoque(char tecidoAviamento)
        {
            var entradaEstoque = new DTORelatorioEstoque
            {
                TecidoAviamentoEstoque = tecidoAviamento,
                EntradaSaidaEstoque = 'T',
                EntradaSaida = EntradaOuSaida.Todos,
                DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                DataFim = DateTime.Now,
                //MateriasPrimarDropDown = _materiaPrimaServico.GetAll()
                //    .Where(x => x.Status == Status.Ativo)
                //    .Where(x => x.Tipo == 3)
                //    .OrderBy(x => x.Descricao)
                //    .ToList(),
                TecidosDropDown = _tecidoServico.GetAll()
                    .Where(x => x.Status == Status.Ativo)
                    .OrderBy(x => x.DescricaoTecido)
                    .ToList(),
                FaccoesDropDown =
                    _faccaoServico.GetAll().Where(x => x.Status == Status.Ativo).OrderBy(x => x.Descricao).ToList()
            };

            return View(entradaEstoque);
        }

        [HttpPost]
        public ActionResult RelatorioEstoque(DTORelatorioEstoque f)
        {
            var filtro = new DTOFiltroEntradaEstoque
            {
                
              MateriaPrimaId = f.MateriaPrimaId,
              TecidoId = f.TecidoId,
              CorId = f.CorId,
              CodigoId = f.CodigoId,
              DataInicio = f.DataInicio,
              DataFim = f.DataFim,
              FaccaoId = f.FaccaoId,
              Data = f.Data,
              EntradaSaidaEstoque = f.EntradaSaidaEstoque,
              TecidoAviamentoEstoque = f.TecidoAviamentoEstoque
            };

            if (f.EntradaSaidaEstoque == 'T')
                filtro.EntradaSaida = EntradaOuSaida.Todos;
            else if (f.EntradaSaidaEstoque == 'E')
                filtro.EntradaSaida = EntradaOuSaida.Entrada;
            else if (f.EntradaSaidaEstoque == 'S')
                filtro.EntradaSaida = EntradaOuSaida.Saida;

            if (f.TecidoAviamentoEstoque == 'T')
                filtro.TecidoAviamento = TecidoOuAviamento.Tecido;
            else if (f.TecidoAviamentoEstoque == 'A')
                filtro.TecidoAviamento = TecidoOuAviamento.Aviamento;
            else if (f.TecidoAviamentoEstoque == 'T')
                filtro.TecidoAviamento = TecidoOuAviamento.Ambos;

    
            //f.MateriasPrimarDropDown = _materiaPrimaServico.GetAll()
            //    .Where(x => x.Status == Status.Ativo)
            //    .Where(x => x.Tipo == 3)
            //    .OrderBy(x => x.Descricao)
            //    .ToList();

            f.TecidosDropDown = _tecidoServico.GetAll()
                .Where(x => x.Status == Status.Ativo)
                .OrderBy(x => x.DescricaoTecido)
                .ToList();

            f.FaccoesDropDown =
                _faccaoServico.GetAll().Where(x => x.Status == Status.Ativo).OrderBy(x => x.Descricao).ToList();

            if (f.TecidoAviamento == TecidoOuAviamento.Tecido)
            {
                var tecidos =
                    _tecidoServico.GetByFilter(new DTOTecido
                    {
                        IdTecido = f.IdTecido,
                        IdCodigo = f.CodigoId,
                        IdCor = f.CorId,
                        Status = Status.Todos
                    });

                if(tecidos.Count == 0)
                    return View(f);

                filtro.IdTecidos = tecidos.Select(x => x.Id).ToArray();
            }

            f.EntradaEstoque = _entradaEstoqueServico.GetByFilter(filtro);
            return View(f);
        }

        public ActionResult ManterEstoque(char entradaSaida, char tecidoAviamento, int id = 0)
        {
            DTOEntradaEstoque entradaEstoque;

            if (id > 0)
            {
                entradaEstoque = _entradaEstoqueServico.GetById(id);
            }
            else
            {
                entradaEstoque = new DTOEntradaEstoque
                {
                    EntradaSaida = entradaSaida == 'E' ? EntradaOuSaida.Entrada : EntradaOuSaida.Saida,
                    TecidoAviamento = tecidoAviamento == 'T' ? TecidoOuAviamento.Tecido : TecidoOuAviamento.Aviamento,
                    Data = DateTime.Now
                };
            }

            //entradaEstoque.MateriasPrimarDropDown =
            //    _materiaPrimaServico.GetAll()
            //        .Where(x => x.Status == Status.Ativo)
            //        .Where(x => x.Tipo == 3)
            //        .OrderBy(x => x.Descricao)
            //        .ToList();
            entradaEstoque.TecidosDropDown =
                _tecidoServico.GetAll()
                    .Where(x => x.Status == Status.Ativo)
                    .OrderBy(x => x.DescricaoTecido)
                    .ToList();
            entradaEstoque.FaccoesDropDown =
                _faccaoServico.GetAll().Where(x => x.Status == Status.Ativo).OrderBy(x => x.Descricao).ToList();

            return View(entradaEstoque);
        }

        [HttpPost]
        public ActionResult ManterEstoque(DTOEntradaEstoque entradaEstoque, string valor, char entradaSaida, char tecidoAviamento)
        {
            entradaEstoque.Valor = !string.IsNullOrEmpty(valor) ? Convert.ToDouble(valor.Replace('.', ',')) : 0;
            entradaEstoque.EntradaSaida = entradaSaida == 'E' ? EntradaOuSaida.Entrada : EntradaOuSaida.Saida;
            entradaEstoque.TecidoAviamento = tecidoAviamento == 'T' ? TecidoOuAviamento.Tecido : TecidoOuAviamento.Aviamento;

            var tecido = _tecidoServico.GetByFilter(new DTOTecido
            {
                IdTecido = entradaEstoque.Tecido.IdTecido,
                IdCodigo = entradaEstoque.Tecido.IdCodigo,
                IdCor = entradaEstoque.Tecido.IdCor,
                Status = Status.Todos
            }).FirstOrDefault();

            if (tecido == null)
            {
                //entradaEstoque.MateriasPrimarDropDown =
                //    _materiaPrimaServico.GetAll()
                //        .Where(x => x.Status == Status.Ativo)
                //        .Where(x => x.Tipo == 3)
                //        .OrderBy(x => x.Descricao)
                //        .ToList();

                entradaEstoque.TecidosDropDown =
                    _tecidoServico.GetAll()
                        .Where(x => x.Status == Status.Ativo)
                        .OrderBy(x => x.DescricaoTecido)
                        .ToList();

                entradaEstoque.FaccoesDropDown =
                    _faccaoServico.GetAll().Where(x => x.Status == Status.Ativo).OrderBy(x => x.Descricao).ToList();

                TempData["Mensagem"] = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Alerta,
                    Mensagem = "Tecido não Cadastrado!"
                };
                return View(entradaEstoque);
            }
            
            entradaEstoque.Tecido = entradaEstoque.TecidoAviamento == TecidoOuAviamento.Aviamento ? new DTOTecido() : tecido;

            try
            {
                _entradaEstoqueServico.Salvar(entradaEstoque);
                TempData["Mensagem"] = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Sucesso,
                    Mensagem = "Cadastro atualizado com sucesso!"
                };
                return RedirectToAction("ManterEstoque", "EntradaEstoque", 
                    new 
                    {   id = 0,
                        entradaSaida = entradaEstoque.EntradaSaida.ToString().Substring(0,1), 
                        tecidoAviamento = entradaEstoque.TecidoAviamento.ToString().Substring(0,1) 
                    });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var entradaEstoque = _entradaEstoqueServico.GetById(id);

            try
            {
                _entradaEstoqueServico.Delete(entradaEstoque.Id);
            }
            catch
            {
                return View("ManterEstoque", entradaEstoque);
            }

            TempData["Mensagem"] = new DTOMensagem
            {
                TipoMensagem = TipoMensagem.Sucesso,
                Mensagem = "Cadastro atualizado com sucesso!"
            };
            return RedirectToAction("ManterEstoque", "EntradaEstoque", new 
                    {   id = 0,
                        entradaSaida = entradaEstoque.EntradaSaida.ToString().Substring(0,1), 
                        tecidoAviamento = entradaEstoque.TecidoAviamento.ToString().Substring(0,1) 
                    });
        }
    }
}
