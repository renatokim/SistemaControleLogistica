using Site.DTO;
using Site.IServico;
using Site.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Site.WebApi.Controllers
{
    [RoutePrefix("api/v1"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FreteController : ApiController
    {
        private readonly IFreteServico _freteServico = ServiceFactory.CreateInstance<IFreteServico>();

        [HttpGet]
        [Route("fretes")]
        public IList<DTOFrete> Get()
        {
            var fretes = _freteServico.GetAll();
            return fretes;
        }

        [HttpGet]
        [Route("fretes_grid")]
        public IList<DTOFreteGrid> GetFretes()
        {
            var fretes = _freteServico.GetFretes();
            return fretes;
        }

        [HttpGet]
        [Route("frete/{id}")]
        public DTOFrete Get(int id)
        {
            var frete = _freteServico.GetAll().Where(x => x.Id == id).FirstOrDefault();
            return frete;
        }

        [HttpPost]
        [Route("frete")]
        public HttpResponseMessage Post(DTOFrete frete)
        {
            try
            {
                _freteServico.Salvar(frete);
                return Request.CreateResponse(HttpStatusCode.OK, frete);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            } 
        }

        [HttpPut]
        [Route("frete")]
        public HttpResponseMessage Put(DTOFrete frete)
        {
            try
            {
                _freteServico.Salvar(frete);
                return Request.CreateResponse(HttpStatusCode.OK, frete);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            } 
        }

        [HttpDelete]
        [Route("frete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _freteServico.Excluir(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            } 
        }
    }
}
