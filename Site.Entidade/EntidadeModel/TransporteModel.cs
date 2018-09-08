using Site.ActiveRecords;
using Site.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Entidade.EntidadeModel
{
    [Tabela("transporte")]
    public class TransporteModel
    {
        [Campo("id")]
        public string Id { get; set; }

        [Campo("cliente_id_coleta")]
        public string ClienteColeta { get; set; }

        [Campo("cliente_id_entrega")]
        public string ClienteEntrega { get; set; }

        [Campo("status")]
        public string Status { get; set; }

        [Campo("data_cadastro")]
        public string DataCadastro { get; set; }

        [Campo("previsao_entrega")]
        public string DataPrevisaoEntrega { get; set; }

        [Campo("rota_id")]
        public string Rota { get; set; }

        public static TransporteModel Transform(DTOTransporte transporte)
        {
            var modeloModel = new TransporteModel
            {
                Id = transporte.Id.ToString(CultureInfo.InvariantCulture),
                ClienteColeta = transporte.ClienteColeta.ToString(CultureInfo.InvariantCulture),
                ClienteEntrega = transporte.ClienteEntrega.ToString(CultureInfo.InvariantCulture),
                DataCadastro = transporte.DataCadastro.ToString("yyyy-MM-dd"),
                DataPrevisaoEntrega = transporte.DataPrevisaoEntrega.ToString("yyyy-MM-dd"),
                Status = ((int)transporte.Status).ToString()
            };

            return modeloModel;
        }
    }
}
