using System;
using System.Web.Mvc;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico.Cadastro;
using Site.Servico;

namespace Site.View.Controllers.Cadastro
{
    public class ModeloController : BaseController
    {
        private readonly IModeloServico _modeloServico = ServiceFactory.CreateInstance<IModeloServico>();

        public ActionResult Create()
        {
            var modelo = new Modelo { Ativo = Status.Ativo };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Create(Modelo modelo)
        {
            if (modelo.Descricao == null || modelo.Descricao.Trim() == string.Empty)
            {
                Mensagem("Preencha a descrição!", TipoMensagem.Erro);
                return View(modelo);
            }

            modelo.Descricao = modelo.Descricao.ToUpper();

            try
            {
                _modeloServico.Save(modelo);
                Mensagem("Registro Atualizado com Sucesso!", TipoMensagem.Sucesso);
                return RedirectToAction("Create");
            }
            catch (Exception exception)
            {
                Mensagem(exception.Message, TipoMensagem.Erro);
                return View(modelo);
            }
        }

        public ActionResult Edit(int id)
        {
            var modelo = _modeloServico.GetById(id);
            return View("Create", modelo);
        }
    }
}
