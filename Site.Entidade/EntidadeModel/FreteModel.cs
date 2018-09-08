using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.ActiveRecords;
using System.Globalization;
using Site.DTO;

namespace Site.Entidade.EntidadeModel
{
    [Tabela("frete")]
    public class FreteModel
    {
        [Campo("id")]
        public string Id { get; set; }

        [Campo("uf")]
        public string Uf { get; set; }

        [Campo("frota_id")]
        public string Frota { get; set; }

        [Campo("valor")]
        public string Valor { get; set; }

        public static FreteModel Transform(DTOFrete frete)
        {
            var modeloModel = new FreteModel
            {
                Id = frete.Id.ToString(CultureInfo.InvariantCulture),
                Uf = frete.Uf.ToString(CultureInfo.InvariantCulture),
                Frota =  frete.Frota.ToString(CultureInfo.InvariantCulture),
                Valor = frete.Valor.ToString(CultureInfo.InvariantCulture)
            };

            return modeloModel;
        }
    }
}
