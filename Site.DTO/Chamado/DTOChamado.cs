﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.Enums;

namespace Site.DTO.Chamado
{
    public class DTOChamado
    {
        public DTOChamado()
        {
            Historico = new List<DTOHistorico>();
        }

        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public PrioridadeChamado Prioridade { get; set; }
        public SimNao Pago { get; set; }
        public int Horas { get; set; }
        public PendenteComChamado PendenteCom { get; set; }
        public TipoChamado TipoChamado { get; set; }
        public StatusChamado Status { get; set; }
        public IList<DTOHistorico> Historico { get; set; }
        public String Comentario { get; set; }
    }
}
