using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.View.ViewModel
{
    public class ParteAcrescimoViewModel
    {
        public int Quantidade { get; set; }
        public int ParteAcrescimo { get; set; }
        public string Obs { get; set; }
        public IList<AviamentosViewModel> Aviamentos { get; set; }
        public bool UsaMesmoTecido { get; set; }
        public TecidoViewModel Tecido { get; set; }
    }

    public class AviamentosViewModel
    {
        public int QuantidadeAviamento { get; set; }
        public int TipoAviamento { get; set; }
        public int CorAviamento { get; set; }
        public int CodigoAviamento { get; set; }
    }

    public class TecidoViewModel
    {
        public int TecidoItem { get; set; }
        public int CorItem { get; set; }
        public int CodigoItem { get; set; }
    }
}