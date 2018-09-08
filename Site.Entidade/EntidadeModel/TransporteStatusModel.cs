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
    [Tabela("transporte_status")]
    public class TransporteStatusModel
    {
        [Campo("id")]
        public string Id { get; set; }

        [Campo("transporte_id")]
        public string TransporteId { get; set; }

        [Campo("status_id")]
        public string StatusId { get; set; }

        [Campo("data")]
        public string Data { get; set; }

        public static TransporteStatusModel Transform(DTOTransporteStatus transporteStatus)
        {
            var modeloModel = new TransporteStatusModel
            {
                Id = transporteStatus.Id.ToString(CultureInfo.InvariantCulture),
                TransporteId = transporteStatus.TransporteId.ToString(CultureInfo.InvariantCulture),
                StatusId = transporteStatus.StatusId.ToString(CultureInfo.InvariantCulture),
                Data = transporteStatus.Data.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return modeloModel;
        }
    }
}
