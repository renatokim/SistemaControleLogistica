using System.Collections.Generic;
using System.Web.Mvc;
using Site.DTO.Chamado;
using Site.Entidade.Chamados;
using Site.Entidade.EntidadeModel;
using Site.Enums;
using Site.Transforms;

namespace Site.View.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var chamado = new Chamado
            {
                Id = 55,
                Sistema = 1,
                Historico = new List<Historico>()
            };

            chamado.Historico.Add(new Historico { Id = 9 });

            var dtoChamado = Transform.JTransform<DTOChamado>(chamado);
            var dtoChamadoExt = chamado.JeTransform<DTOChamado>();
            var chamadoConvert = Transform.JTransform<Chamado>(dtoChamado);
            var chamadoModel = Transform.JTransform<ChamadoModel>(dtoChamado);
            var chamadoModel2 = Transform.JTransform<ChamadoModel>(chamadoConvert);
            var chamado2 = Transform.JTransform<Chamado>(chamadoModel);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
