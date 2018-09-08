using Site.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DTO
{
    public class DTOTransporte
    {
        public int Id { get; set; }
        public int ClienteColeta { get; set; }
        public int ClienteEntrega { get; set; }
        public StatusColeta Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataPrevisaoEntrega { get; set; }
        public DTORota Rota { get; set; }
        public int RotaId { get; set; }
    }
}
