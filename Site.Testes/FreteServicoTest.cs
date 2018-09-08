using Microsoft.VisualStudio.TestTools.UnitTesting;
using Site.IServico;
using Site.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Testes
{
    [TestClass]
    public class FreteServicoTest
    {
        private readonly IFreteServico _freteServico = ServiceFactory.CreateInstance<IFreteServico>();

        [TestMethod]
        public void ListarFretesServico()
        {
            var fretes = _freteServico.GetAll();




        }
    }
}
