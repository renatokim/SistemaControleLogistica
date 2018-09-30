using Site.DTO;
using Site.IRepositorio;
using Site.IServico;
using Site.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Servico
{
    public class TransporteServico : ITransporteServico
    {
        private readonly ITransporteRepositorio _transporteRepositorio = RepositoryFactory.CreateInstance<ITransporteRepositorio>();

        public IList<DTOTransporte> GetAll()
        {
            var transportes = _transporteRepositorio.GetAll();
            return transportes;
        }

        public void Salvar(DTOTransporte transporte)
        {
            _transporteRepositorio.Salvar(transporte);
        }

        public void Excluir(int id)
        {
            _transporteRepositorio.Excluir(id);
        }

        public void IncluirStatus(DTOTransporteStatus status)
        {
            _transporteRepositorio.IncluirStatus(status);
        }

        public IList<DTOTransporteGrid> GetTransportes()
        {
            var transportes = _transporteRepositorio.GetTransportes();
            return transportes;
        }

        public IList<DTOClienteGrid> GetClientes()
        {
            var clientes = _transporteRepositorio.GetClientes();
            return clientes;
        }
    }
}
