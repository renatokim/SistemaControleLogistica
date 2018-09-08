using System.Linq;
using System.Web.Mvc;
using Site.IServico.Pedidos;
using Site.Servico;

namespace Site.View.Controllers.Pedidos
{
    public class TecidoController : Controller
    {
        private readonly ITecidoServico _tecidoServico = ServiceFactory.CreateInstance<ITecidoServico>();

        [HttpGet]
        public string CorPorTipoTecido(int idTipoTecido)
        {
            var listaCorPorTecido =
                        from tecido in _tecidoServico.GetAll().Where(x => x.TipoTecido.Id == idTipoTecido)
                        select new { tecido.Cor.Id, tecido.Cor.Descricao };

            var tecidos = listaCorPorTecido.Distinct().OrderBy(x => x.Descricao).ToList();
            string select = @"<option value='0'></option>";

            foreach (var tecido in tecidos)
            {
                select += 
                @"<option value='" + tecido.Id + "'>"
                    + tecido.Descricao + 
                @"</option>";
            }

            return select;
        }

        public string CodigoPorTipoTecidoCor(int idTipoTecido, int idCor)
        {
            var listaCodigos =
                        from tecido in _tecidoServico.GetAll().Where(x => x.TipoTecido.Id == idTipoTecido).Where(y => y.Cor.Id == idCor)
                        select new { tecido.Codigo.Id, tecido.Codigo.Descricao };

            var codigos = listaCodigos.Distinct().OrderBy(x => x.Descricao).ToList();
            string select = @"<option value='0'></option>";

            foreach (var codigo in codigos)
            {
                select += 
                @"<option value='" + codigo.Id + "'>"
                    + codigo.Descricao + 
                @"</option>";
            }

            return select;
        }

        public string GetTecidosForModal()
        {
            var listaTipoTecido = from tecido in _tecidoServico.GetAll()
                                  select new { tecido.TipoTecido.Id, tecido.TipoTecido.Descricao };
            var tecidos = listaTipoTecido.Distinct().OrderBy(x => x.Descricao).ToList();

            string select = "<option value='0'></option>";

            foreach (var tecido in tecidos)
            {
                select += string.Format("<option value='{0}'>{1}</option>", tecido.Id, tecido.Descricao);
            }

            return select;
        }


        //
        // GET: /Tecido/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Tecido/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tecido/Create

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
        // GET: /Tecido/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Tecido/Edit/5

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
        // GET: /Tecido/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Tecido/Delete/5

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
