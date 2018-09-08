using System.Linq;
using System.Web.Mvc;
using Site.Enums;
using Site.IServico.Pedidos;
using Site.Servico;

namespace Site.View.Controllers.Pedidos
{
    public class ClienteController : Controller
    {
        private readonly IClienteServico _clienteServico = ServiceFactory.CreateInstance<IClienteServico>();
        private readonly IVendedorServico _vendedorServico = ServiceFactory.CreateInstance<IVendedorServico>();

        public ActionResult Index()
        {
            return View();
        }

        public string ClientePorId(int id)
        {
            var cliente = _clienteServico.GetAll().FirstOrDefault(x => x.Id == id);
            var vendedores = _vendedorServico.GetAll().Where(x => x.Ativo == Status.Ativo);

            string select = "<option value='0'></option>";

            foreach (var vendedor in vendedores)
            {
                if (cliente != null && vendedor.Id == cliente.Vendedor.Id)
                {
                    select += string.Format("<option selected='selected' value='{0}'>{1}</option>", vendedor.Id, vendedor.Descricao.ToUpper());
                }
                else
                {
                    select += string.Format("<option value='{0}'>{1}</option>", vendedor.Id, vendedor.Descricao.ToUpper());
                }
            }

            return select;
        }

    }
}
