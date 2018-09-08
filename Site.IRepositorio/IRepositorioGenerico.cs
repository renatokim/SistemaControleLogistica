using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.IRepositorio
{
    public interface IRepositorioGenerico
    {
        void ExecutaComandoSemRetorno(string sql);
    }
}
