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
    public class RotaController : ApiController
    {
        private readonly IRotaServico _rotaServico = ServiceFactory.CreateInstance<IRotaServico>();

        [HttpGet]
        [Route("rotas_grid")]
        public IList<DTORotaGrid> GetRotas()
        {
            var rotas = _rotaServico.GetRotas();
            return rotas;
        }

        [HttpGet]
        [Route("rotas")]
        public IList<DTORota> Get()
        {
            var rotas = _rotaServico.GetAll();
            return rotas;
        }

        [HttpGet]
        [Route("rotas_response")]
        public IList<DTORotaResponse> GetRotasResponse()
        {
            var rotas = _rotaServico.GetAll()
                            .Select(x => new DTORotaResponse { Id = x.Id, Rota = x.Id.ToString().PadLeft(5, '0') } )
                            .ToList();
            return rotas;
        }

        [HttpGet]
        [Route("rota/{id}")]
        public DTORota Get(int id)
        {
            var rotas = _rotaServico.GetAll().Where(x => x.Id == id).FirstOrDefault();
            return rotas;
        }
        
        [HttpGet]
        [Route("rota_editar/{id}")]
        public DTORotaEditar GetRota(int id)
        {
            var rota = _rotaServico.GetAll().Where(x => x.Id == id).FirstOrDefault();
            return new DTORotaEditar
                        {
                            Id = rota.Id,
                            TransporteId = rota.TransporteId,
                            FrotaId = rota.FrotaId,
                            FuncionarioId = rota.FuncionarioId,
                            UfId = rota.UfId,
                            DataCriacao = rota.DataCriacao.ToString("yyyy-MM-dd"),
                            DataEntrega = rota.DataEntrega.ToString("yyyy-MM-dd"),
                            RotaId = rota.Id.ToString().PadLeft(5, '0')
                        };
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
