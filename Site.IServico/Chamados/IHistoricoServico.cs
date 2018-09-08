using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.DTO.Chamado;

namespace Site.IServico.Chamados
{
    public interface IHistoricoServico
    {
        DTOHistorico GetById(int id);
        IList<DTOHistorico> GetAll();
        void Salvar(DTOHistorico chamado);
        IList<DTOHistorico> GetByFilter(DTOFiltroHistorico filtro);
        void Delete(int id);
    }
}
