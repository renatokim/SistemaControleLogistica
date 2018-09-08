using System.Linq;
using System.Web.Mvc;
using Site.IServico.Pedidos;
using Site.Servico;

namespace Site.View.Controllers.Pedidos
{
    public class AviamentoController : Controller
    {
        private readonly IAviamentoServico _aviamentoServico = ServiceFactory.CreateInstance<IAviamentoServico>();

        public ViewResult CorPorTipoAviamento(int idTipoAviamento)
        {
            var listaAviamentos = _aviamentoServico.GetAll();
            var aviamentos = listaAviamentos.Where(x => x.TipoAviamento.Id == idTipoAviamento).ToList();

            return View("_CorPorTipoAviamento", aviamentos);
        }

        public ViewResult CodigoPorTipoAviamento(int idTipoAviamento, int idCorAviamento)
        {
            var listaAviamentos = _aviamentoServico.GetAll();
            var aviamentos = listaAviamentos.Where(x => x.TipoAviamento.Id == idTipoAviamento).Where(y => y.Cor.Id == idCorAviamento) .ToList();

            return View("_CodigoPorTipoAviamento", aviamentos);
        }

        //
        // GET: /Aviamento/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Aviamento/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Aviamento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Aviamento/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Aviamento/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Aviamento/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Aviamento/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Aviamento/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
