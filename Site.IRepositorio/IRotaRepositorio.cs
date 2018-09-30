using Site.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.IRepositorio
{
    public interface IRotaRepositorio
    {
        IList<DTORota> GetAll();
        void Salvar(DTORota frete);
        void Excluir(int id);
        IList<DTORotaGrid> GetRotas();
    }
}
