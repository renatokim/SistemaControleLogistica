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
    public class HistoricoServico : IHistoricoServico
    {
        private readonly IHistoricoRepositorio _historicoRepositorio = RepositoryFactory.CreateInstance<IHistoricoRepositorio>();

        public DTOHistorico GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<DTOHistorico> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Salvar(DTOHistorico chamado)
        {
            _historicoRepositorio.Salvar(chamado);
        }

        public IList<DTOHistorico> GetByFilter(DTOFiltroHistorico filtro)
        {
            var historicos = _historicoRepositorio.GetByFilter(filtro);
            return historicos;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
