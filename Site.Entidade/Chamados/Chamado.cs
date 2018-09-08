using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Entidade.Chamados
{
    public class Chamado
    {
        public int Id { get; set; }
        public int Sistema { get; set; }
        public DateTime Data { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public int Prioridade { get; set; }
        public int Pago { get; set; }
        public int Horas { get; set; }
        public int PendenteCom { get; set; }
        public int TipoChamado { get; set; }
        public int Status { get; set; }

        public IList<Historico> Historico { get; set; } 
    }
}
