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

    public class DTOTransporteGrid
    {
        public int Id { get; set; }
        public string ClienteColeta { get; set; }
        public string ClienteEntrega { get; set; }
        public string PrevisaoEntrega { get; set; }
        public string UfRemetente { get; set; }
        public string UfDestinatario { get; set; }
        public string Status { get; set; }
    }

    public class DTOClienteGrid
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
    }

    public class DTOTransporteRota
    {
        public int Id { get; set; }
        public string Rota { get; set; }
    }
}
