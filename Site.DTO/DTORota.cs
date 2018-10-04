using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DTO
{
    public class DTORota
    {
        public DTORota()
        {
            Transportes = new List<DTOTransporte>();
        }

        public int Id { get; set; }
        public int TransporteId { get; set; }
        public int FrotaId { get; set; }
        public int FuncionarioId { get; set; }
        public int UfId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataEntrega { get; set; }
        public IList<DTOTransporte> Transportes { get; set; }
    }

    public class DTORotaResponse
    {
        public int Id { get; set; }
        public string Rota { get; set; }
    }

    public class DTORotaGrid
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string DataEntrega { get; set; }
        public string UfEntrega { get; set; }
        public string Rota { get; set; }
    }
    
    public class DTORotaEditar
    {
        public int Id { get; set; }
        public int TransporteId { get; set; }
        public int FrotaId { get; set; }
        public int FuncionarioId { get; set; }
        public int UfId { get; set; }
        public string DataCriacao { get; set; }
        public string DataEntrega { get; set; }
        public string RotaId { get; set; }
    }    
}
