using System;
using System.Linq;
using System.Web.Mvc;
using Site.DTO;
using Site.Entidade.Pedidos;
using Site.Enums;
using Site.IServico.Pedidos;
using Site.Servico;

namespace Site.View.Controllers.Pedidos
{
    public class TamanhoController : Controller
    {
        private readonly ITamanhoServico _tamanhoServico = ServiceFactory.CreateInstance<ITamanhoServico>();

        public ActionResult Create()
        {
            var tamanho = new Tamanho {Status = Status.Ativo, Tipo = 3};
            return View(tamanho);
        }

        [HttpPost]
        public ActionResult Create(Tamanho tamanho)
        {
            try
            {
                _tamanhoServico.Salvar(tamanho);

                TempData["Mensagem"] = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Sucesso,
                    Mensagem = "Cadastro Atualizado com Sucesso!"
                };

                return Redirect("Create");  
            }
            catch (Exception exception)
            {
                TempData["Mensagem"] = new DTOMensagem
                {
                    TipoMensagem = TipoMensagem.Erro,
                    Mensagem = exception.Message
                };

                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var tamanho = _tamanhoServico.GetAllTamanhos().FirstOrDefault(x => x.Id == id);
            return View("Create", tamanho);
        }
    }
}
