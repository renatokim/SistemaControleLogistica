using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Site.DTO.Chamado;
using Site.Enums;
using Site.IServico.Chamados;
using Site.Servico;

namespace Site.View.Controllers
{
    public class ChamadoController : Controller
    {
        private readonly IChamadoServico _chamadoServico = ServiceFactory.CreateInstance<IChamadoServico>();

        public ActionResult Index()
        {
            var chamados = _chamadoServico.GetAll().OrderBy(x => x.Status).ThenByDescending(y => y.Data).ToList();
            return View(chamados);
        }

        public ActionResult Details(int id)
        {
            var chamado = _chamadoServico.GetById(id);
            return View(chamado);
        }

        public ActionResult Create()
        {
            var chamado = new DTOChamado();
            return View(chamado);
        }

        [HttpPost]
        public ActionResult Create(DTOChamado chamado)
        {
            chamado.Data = DateTime.Now;
            chamado.PendenteCom = PendenteComChamado.Renato;
            chamado.Status = StatusChamado.Aberto;

            try
            {
                _chamadoServico.Salvar(chamado);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(chamado);
            }
        }

        public ActionResult Edit(int id)
        {
            var chamado = _chamadoServico.GetById(id);
            return View(chamado);
        }

        [HttpPost]
        public ActionResult Edit(DTOChamado chamado)
        {
            var chamadoDb = _chamadoServico.GetById(chamado.Id);

            chamadoDb.Assunto = chamado.Assunto;
            chamadoDb.Descricao = chamado.Descricao;
            chamadoDb.Status = chamado.Status;
            chamadoDb.TipoChamado = chamado.TipoChamado;
            chamadoDb.Sistema = chamado.Sistema;
            chamadoDb.Prioridade = chamado.Prioridade;
            chamadoDb.PendenteCom = chamado.PendenteCom;
            chamadoDb.Comentario = chamado.Comentario;

            try
            {
                _chamadoServico.Salvar(chamadoDb);
                return RedirectToAction("Details", new { id = chamado.Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GeraExcelCsv()
        {
            var sb = new StringBuilder();
            var chamados = _chamadoServico.GetAll();

            sb.Append("NÚMERO;DATA;SISTEMA;ASSUNTO;PRIORIDADE;TIPO;PENDENTE COM;STATUS;ÚLTIMO COMENTÁRIO\r\n");
            foreach (var chamado in chamados)
            {
                sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}\r\n", 
                    chamado.Id, 
                    chamado.Data.ToString("dd/MM/yyyy"),
                    chamado.Sistema,
                    chamado.Assunto.ToUpper(),
                    chamado.Prioridade,
                    chamado.TipoChamado,
                    chamado.PendenteCom,
                    chamado.Status,
                    chamado.DataUltimoComentario.ToString("dd/MM/yyyy")));
            }

            HttpContext.Response.Clear();
            HttpContext.Response.AddHeader("content-disposition", string.Format("attachment;filename=Teste_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
            HttpContext.Response.ContentType = "application/CSV";
            HttpContext.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Write(sb.ToString());
            HttpContext.Response.End();

            return null;
        }
    }
}
