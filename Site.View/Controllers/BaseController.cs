using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.DTO;
using Site.Enums;

namespace Site.View.Controllers
{
    public class BaseController : Controller
    {
        public void Mensagem(string mensagem, TipoMensagem tipoMensagem)
        {
            var novaMensagem = new DTOMensagem { TipoMensagem = tipoMensagem, Mensagem = mensagem };
            TempData["Mensagem"] = novaMensagem;
        }
    }
}
