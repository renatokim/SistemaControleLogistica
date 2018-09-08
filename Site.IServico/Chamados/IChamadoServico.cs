using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.DTO.Chamado;

namespace Site.IServico.Chamados
{
    public interface IChamadoServico
    {
        DTOChamado GetById(int id);
        IList<DTOChamado> GetAll();
        void Salvar(DTOChamado chamado);
        IList<DTOChamado> GetByFilter(DTOFiltroChamado filtro);
        void Delete(int id);
    }
}
