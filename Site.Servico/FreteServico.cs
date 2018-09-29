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
    public class FreteServico : IFreteServico
    {
        private readonly IFreteRepositorio _freteRepositorio = RepositoryFactory.CreateInstance<IFreteRepositorio>();

        public IList<DTOFrete> GetAll()
        {
            var fretes = _freteRepositorio.GetAll();
            return fretes;
        }

        public void Salvar(DTOFrete frete)
        {
            _freteRepositorio.Salvar(frete);
        }

        public void Excluir(int id)
        {
            _freteRepositorio.Excluir(id);
        }

        public IList<DTOFreteGrid> GetFretes()
        {
            var fretes = _freteRepositorio.GetFretes();
            return fretes;
        }
    }
}
