using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.IRepositorio;

namespace Site.Repositorio
{
    public class RepositorioGenerico : IRepositorioGenerico
    {
        public void ExecutaComandoSemRetorno(string sql)
        {
            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }
    }
}
