using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.Enums;

namespace Site.DTO.Relatorio
{
    public class DTOFiltroRelatorioPorFaccao
    {
        public int Faccao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public Acao Acao { get; set; }
        public StatusFluxo StatusFluxo { get; set; }
    }

    public class DTOFiltroRelatorioFaccao
    {
        public int Faccao { get; set; }
        public IList<int> IdPedidos { get; set; }
        public Acao Acao { get; set; }
        public StatusFluxo StatusFluxo { get; set; }
    }
}
