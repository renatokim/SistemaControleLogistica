using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.DTO.Chamado;
using Site.IRepositorio.Chamados;
using Site.IServico.Chamados;
using Site.Repositorio;
using Site.Repositorio.Chamados;

namespace Site.Servico.Chamados
{
    public class ChamadoServico : IChamadoServico
    {
        private readonly IChamadoRepositorio _chamadoRepositorio = RepositoryFactory.CreateInstance<IChamadoRepositorio>();
        private readonly IHistoricoServico _historicoServico = ServiceFactory.CreateInstance<IHistoricoServico>();

        public DTOChamado GetById(int id)
        {
            var chamado = _chamadoRepositorio.GetById(id);
            chamado.Historico = _historicoServico.GetByFilter(new DTOFiltroHistorico { IdChamado = id });
            return chamado;
        }

        public IList<DTOChamado> GetAll()
        {
            var chamados = _chamadoRepositorio.GetAll();

            foreach (var chamado in chamados)
            {
                chamado.Historico =
                    _historicoServico.GetByFilter(new DTOFiltroHistorico {IdChamado = chamado.Id}).ToList();

                var firstOrDefault = chamado.Historico.OrderByDescending(x => x.Data).FirstOrDefault();
            }

            return chamados;
        }

        public void Salvar(DTOChamado chamado)
        {
            _chamadoRepositorio.Salvar(chamado);

            if (!string.IsNullOrEmpty(chamado.Comentario))
            {
                var historico = new DTOHistorico
                {
                    Chamado = new DTOChamado {Id = chamado.Id},
                    Data = DateTime.Now,
                    Descricao = chamado.Comentario
                };

                _historicoServico.Salvar(historico);
            }
        }

        public IList<DTOChamado> GetByFilter(DTOFiltroChamado filtro)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            _chamadoRepositorio.Delete(id);
        }
    }
}
