using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.Enums;

namespace Site.DTO.Relatorio
{
    public class DTOFiltroRelatorioMP
    {
        public IList<int> IdPedidos { get; set; }
        public OrdemRelatorio Ordem { get; set; }
        public bool ExibirAviamentos { get; set; }
        public bool ExibirSubTotal { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}