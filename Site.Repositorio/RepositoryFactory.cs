using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Site.Repositorio
{
    public static class RepositoryFactory
    {
        private static readonly Dictionary<string, string> Tabela =
             new Dictionary<string, string>() {
                {"Site.IRepositorio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Site.Repositorio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"},

                {"Site.IRepositorio.Pedidos.IPedidoRepositorio", "Site.Repositorio.Pedidos.PedidoRepositorio"},
                {"Site.IRepositorio.Cadastro.IModeloRepositorio", "Site.Repositorio.Cadastro.ModeloRepositorio"},
                {"Site.IRepositorio.Chamados.IChamadoRepositorio", "Site.Repositorio.Chamados.ChamadoRepositorio"},
                {"Site.IRepositorio.Chamados.IHistoricoRepositorio", "Site.Repositorio.Chamados.HistoricoRepositorio"},
                {"Site.IRepositorio.IFreteRepositorio", "Site.Repositorio.FreteRepositorio"},
                {"Site.IRepositorio.ITransporteRepositorio", "Site.Repositorio.TransporteRepositorio"},
                {"Site.IRepositorio.IRotaRepositorio", "Site.Repositorio.RotaRepositorio"}
            };

        public static T CreateInstance<T>() where T : class
        {
            var typeClass = typeof(T);
            var nameSpace = typeClass.Assembly.FullName;
            var nameClass = typeClass.FullName;

            var nameSpaceClasse = Tabela[nameSpace];
            var nomeClasse = Tabela[nameClass];

            var asm = Assembly.Load(nameSpaceClasse);
            var classeModelo = asm.GetType(nomeClasse);
            var objeto = Activator.CreateInstance(classeModelo);
            return (T)objeto;
        }
    }
}
