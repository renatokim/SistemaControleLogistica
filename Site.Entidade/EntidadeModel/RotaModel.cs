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
    [Tabela("rota")]
    public class RotaModel
    {
        [Campo("id")]
        public string Id { get; set; }

        [Campo("transporte_id")]
        public string TransporteId { get; set; }

        [Campo("frota_id")]
        public string FrotaId { get; set; }

        [Campo("uf_id")]
        public string UfId { get; set; }

        [Campo("data_criacao")]
        public string DataCriacao { get; set; }

        [Campo("data_entrega")]
        public string DataEntrega { get; set; }
        
        [Campo("funcionario_id")]
        public string FuncionarioId { get; set; }        

        public static RotaModel Transform(DTORota rota)
        {
            var modeloModel = new RotaModel
            {
                Id = rota.Id.ToString(CultureInfo.InvariantCulture),
                TransporteId = rota.TransporteId.ToString(CultureInfo.InvariantCulture),
                FrotaId = rota.FrotaId.ToString(CultureInfo.InvariantCulture),
                UfId = rota.UfId.ToString(CultureInfo.InvariantCulture),
                DataCriacao = rota.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                DataEntrega = rota.DataEntrega.ToString("yyyy-MM-dd"),
                FuncionarioId = rota.FuncionarioId.ToString(CultureInfo.InvariantCulture)                
            };

            return modeloModel;
        }
    }
}
