using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.View.ViewModel
{
    public class PedidoProdutoViewModel
    {
        public int Quantidade { get; set; }
        public int Peca { get; set; }
        public int Modelo { get; set; }
        public int MasculinoFeminino { get; set; }
        public int Tecido { get; set; }
        public int Cor { get; set; }
        public int Codigo { get; set; }
        public string Valor { get; set; }
        public string Obs { get; set; }
        public int[] Fornecedores { get; set; }
        public TamanhoViewModel[] Tamanhos { get; set; } 
    }

    public class TamanhoViewModel
    {
        public int Id { get; set; }
        public int Qtde { get; set; }
        public string Tamanho { get; set; }
        public string Obs { get; set; }
     }
}