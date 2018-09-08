﻿using System.Web.Mvc;

namespace Site.View.Controllers.Pedidos
{
    public class ProdutoController : Controller
    {
        public PartialViewResult ModeloProdutoPorPeca(int peca)
        {
            ViewBag.Peca = peca;
            return PartialView("_ModeloProdutoPorPeca");
        }

        //
        // GET: /Produto/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Produto/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Produto/Create

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
        // GET: /Produto/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Produto/Edit/5

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
        // GET: /Produto/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Produto/Delete/5

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
