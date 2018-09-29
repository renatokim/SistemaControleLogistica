using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DTO
{
    public class DTOFrete
    {
        public int Id { get; set; }
        public int Frota { get; set; }
        public int Uf { get; set; }
        public double Valor { get; set; }
    }

    public class DTOFreteGrid
    {
        public int Id { get; set; }
        public string Frota { get; set; }
        public string Uf { get; set; }
        public string Valor { get; set; }
    }
}
