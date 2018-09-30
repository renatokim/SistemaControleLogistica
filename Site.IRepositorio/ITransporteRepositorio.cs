using Site.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.IRepositorio
{
    public interface ITransporteRepositorio
    {
        IList<DTOTransporte> GetAll();
        void Salvar(DTOTransporte transporte);
        void Excluir(int id);
        void IncluirStatus(DTOTransporteStatus status);
        IList<DTOTransporteGrid> GetTransportes();
        IList<DTOClienteGrid> GetClientes();
    }
}
