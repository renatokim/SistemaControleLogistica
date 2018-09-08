using Site.DTO;
using Site.IServico;
using Site.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Site.WebApi.Controllers
{
    [RoutePrefix("api/v1")]
    public class RotaController : ApiController
    {
        private readonly IRotaServico _rotaServico = ServiceFactory.CreateInstance<IRotaServico>();

        [HttpGet]
        [Route("rotas")]
        public IList<DTORota> Get()
        {
            var rotas = _rotaServico.GetAll();
            return rotas;
        }

        [HttpGet]
        [Route("rota/{id}")]
        public DTORota Get(int id)
        {
            var rotas = _rotaServico.GetAll().Where(x => x.Id == id).FirstOrDefault();
            return rotas;
        }

        [HttpPost]
        [Route("rota")]
        public HttpResponseMessage Post(DTORota rota)
        {
            try
            {
                _rotaServico.Salvar(rota);
                return Request.CreateResponse(HttpStatusCode.OK, rota);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("rota")]
        public HttpResponseMessage Put(DTORota rota)
        {
            try
            {
                _rotaServico.Salvar(rota);
                return Request.CreateResponse(HttpStatusCode.OK, rota);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("rota/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _rotaServico.Excluir(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
