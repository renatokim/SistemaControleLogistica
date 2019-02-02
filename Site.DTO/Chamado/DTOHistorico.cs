﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DTO.Chamado
{
    public class DTOHistorico
    {
        public DTOHistorico()
        {
            Chamado = new DTOChamado();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public DTOChamado Chamado { get; set; }
    }
}
