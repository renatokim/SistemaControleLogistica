using System;
using System.Web.Mvc;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico.Pedidos;
using Site.Servico;

namespace Site.View.Controllers.Pedidos
{
    public class PecaController : BaseController
    {
        private readonly IPecaServico _pecaServico = ServiceFactory.CreateInstance<IPecaServico>();

        public ActionResult Index()
        {
            var pecas = _pecaServico.GetList();
            return View();
        }

        public ActionResult Create()
        {
            var peca = new Peca {Ativo = Status.Ativo};
            return View(peca);
        }

        [HttpPost]
        public ActionResult Create(Peca peca)
        {
            if (peca.Descricao == null || peca.Descricao.Trim() == string.Empty)
            {
                Mensagem("Preencha a descrição!", TipoMensagem.Erro);
                return View(peca); 
            }

            peca.Descricao = peca.Descricao.ToUpper();

            try
            {
                _pecaServico.Save(peca);
                Mensagem("Registro Atualizado com Sucesso!", TipoMensagem.Sucesso);
                return RedirectToAction("Create");
            }
            catch(Exception exception)
            {
                Mensagem(exception.Message, TipoMensagem.Erro);
                return View(peca);
            }
        }

        public ActionResult Edit(int id)
        {
            var peca = _pecaServico.GetById(id);
            return View("Create", peca);
        }
    }
}
