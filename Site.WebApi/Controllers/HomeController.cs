using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Site.DTO.Chamado;
using Site.Entidade.Chamados;
using Site.IServico.Chamados;
using Site.Servico.Chamados;
using System.Web.Http.Cors;

namespace Site.WebApi.Controllers
{
    [RoutePrefix("api/v1"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        readonly IChamadoServico _chamadoServico = new ChamadoServico();

        [HttpPost]
        [Route("chamado")]
        public HttpResponseMessage Chamado(DTOChamado chamado)
        {
            if (chamado == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                _chamadoServico.Salvar(chamado);
                return Request.CreateResponse(HttpStatusCode.OK, chamado);
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPatch]
        [Route("chamado")]
        public HttpResponseMessage PatchChamado(DTOChamado chamado)
        {
            if (chamado == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                _chamadoServico.Salvar(chamado);
                return Request.CreateResponse(HttpStatusCode.OK, chamado);
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPut]
        [Route("chamado")]
        public HttpResponseMessage PutChamado(DTOChamado chamado)
        {
            if (chamado == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                _chamadoServico.Salvar(chamado);
                return Request.CreateResponse(HttpStatusCode.OK, chamado);
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet]
        [Route("chamados")]
        public HttpResponseMessage Chamados()
        {
            var result = _chamadoServico.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("chamado/{id}")]
        public HttpResponseMessage Chamado(int id)
        {
            var chamado = _chamadoServico.GetById(id);
            return Request.CreateResponse(HttpStatusCode.OK, chamado);
        }

        [HttpDelete]
        [Route("chamado/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                _chamadoServico.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
