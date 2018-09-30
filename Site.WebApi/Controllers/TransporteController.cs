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
    public class TransporteController : ApiController
    {
        private readonly ITransporteServico _transporteServico = ServiceFactory.CreateInstance<ITransporteServico>();

        [HttpGet]
        [Route("transporte_clientes")]
        public IList<DTOClienteGrid> GetClientes()
        {
            var clientes = _transporteServico.GetClientes();
            return clientes;
        }

        [HttpGet]
        [Route("transportes")]
        public IList<DTOTransporte> Get()
        {
            var transportes = _transporteServico.GetAll();
            return transportes;
        }

        [HttpGet]
        [Route("transportes_grid")]
        public IList<DTOTransporteGrid> GetTransportes()
        {
            var transportes = _transporteServico.GetTransportes();
            return transportes;
        }

        [HttpGet]
        [Route("transporte/{id}")]
        public DTOTransporte Get(int id)
        {
            var transporte = _transporteServico.GetAll().Where(x => x.Id == id).FirstOrDefault();
            return transporte;
        }

        [HttpPost]
        [Route("transporte")]
        public HttpResponseMessage Post(DTOTransporte transporte)
        {
            try
            {
                _transporteServico.Salvar(transporte);
                return Request.CreateResponse(HttpStatusCode.OK, transporte);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            } 
        }

        [HttpPut]
        [Route("transporte")]
        public HttpResponseMessage Put(DTOTransporte transporte)
        {
            try
            {
                var transporteAntigo = _transporteServico.GetAll().Where(x => x.Id == transporte.Id).FirstOrDefault();
                if (transporte.Status != transporteAntigo.Status)
                {
                    var dtoStatus = new DTOTransporteStatus();
                    dtoStatus.StatusId = (int)transporteAntigo.Status;
                    dtoStatus.TransporteId = transporte.Id;
                    dtoStatus.Data = DateTime.Now;

                    _transporteServico.IncluirStatus(dtoStatus);
                }

                _transporteServico.Salvar(transporte);
                return Request.CreateResponse(HttpStatusCode.OK, transporte);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("transporte/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _transporteServico.Excluir(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
