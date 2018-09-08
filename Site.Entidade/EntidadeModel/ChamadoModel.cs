using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Entidade.EntidadeModel
{
    public class ChamadoModel
    {
        public string Id { get; set; }
        public string Sistema { get; set; }
        public string Data { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public string Prioridade { get; set; }
        public string Pago { get; set; }
        public string Horas { get; set; }
        public string PendenteCom { get; set; }
        public string TipoChamado { get; set; }
        public string Status { get; set; }
    }
}
