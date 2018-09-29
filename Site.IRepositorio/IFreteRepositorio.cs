using Site.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.IRepositorio
{
    public interface IFreteRepositorio
    {
        IList<DTOFrete> GetAll();
        IList<DTOFreteGrid> GetFretes();
        void Salvar(DTOFrete frete);
        void Excluir(int id);
    }
}
