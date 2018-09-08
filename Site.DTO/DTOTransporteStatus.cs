using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DTO
{
    public class DTOTransporteStatus
    {
        public int Id { get; set; }
        public int TransporteId { get; set; }
        public int StatusId { get; set; }
        public DateTime Data { get; set; }
    }
}
