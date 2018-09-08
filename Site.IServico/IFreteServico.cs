using Site.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.IServico
{
    public interface IFreteServico
    {
        IList<DTOFrete> GetAll();
        void Salvar(DTOFrete frete);
        void Excluir(int id);
    }
}
