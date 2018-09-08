using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.View.ViewModel
{
    public class RelatorioMateriaPrima
    {
        public string Pedido { get; set; }
        public int QtdeProduto { get; set; }
        public string Produto { get; set; }
        public string TecidoProduto { get; set; }
        public double ProporcaoMalha { get; set; }
        public double ConsumoTecido { get; set; }
        public int QtdeParteAcrescimo { get; set; }
        public string ParteAcrescimo { get; set; }
        public double ConsumoTecidoParteAcrescimo { get; set; }
        public string TecidoParteAcrescimo { get; set; }
        public string Aviamentos { get; set; }
        public bool EhProduto { get; set; }
        public string UnidadeMedida
        {
            get { return ProporcaoMalha > 0 ? "KG" : "MT"; }
        }
    }

    public class RelatorioMateriaPrimaViewModel
    {
        public string Pedido { get; set; }
        public int Qtde { get; set; }
        public string Produto { get; set; }
        public int QtdeParteAcrescimo { get; set; }
        public string ParteAcrescimo { get; set; }
        public string Tecido { get; set; }
        public double Consumo { get; set; }
        public double SubTotalConsumo { get; set; }
        public bool EhProduto { get; set; }
        public string Aviamentos { get; set; }
        public double ProporcaoMalha { get; set; }
        public string UnidadeMedida { get; set; }
    }

    public class RelatorioAviamentos
    {
        public string Pedido { get; set; }
        public int QtdeProduto { get; set; }
        public string Produto { get; set; }
        public int QtdeParteAcrescimo { get; set; }
        public string ParteAcrescimo { get; set; }
        public string Aviamento { get; set; }
        public int QtdeAviamento { get; set; }
        public double ConsumoAviamento { get; set; }
        public double SubTotalConsumoAviamento { get; set; }
        public bool EhProduto { get; set; }
    }
}