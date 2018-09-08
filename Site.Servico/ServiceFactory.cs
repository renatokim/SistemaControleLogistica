using System;
using System.Collections.Generic;
using System.Reflection;

namespace Site.Servico
{
    public static class ServiceFactory
    {
        private static readonly Dictionary<string, string> Tabela =
             new Dictionary<string, string>() {
                {"Site.IServico, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Site.Servico, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"},

                {"Site.IServico.Relatorios.IRelatorioServico", "Site.Servico.Relatorios.RelatorioServico"},
                {"Site.IServico.Cadastro.IModeloServico", "Site.Servico.Cadastro.ModeloServico"},
                {"Site.IServico.Fluxo.IFluxoServico", "Site.Servico.Fluxo.FluxoServico"},
                {"Site.IServico.Chamados.IChamadoServico", "Site.Servico.Chamados.ChamadoServico"},
                {"Site.IServico.IFreteServico", "Site.Servico.FreteServico"},
                {"Site.IServico.ITransporteServico", "Site.Servico.TransporteServico"},
                {"Site.IServico.IRotaServico", "Site.Servico.RotaServico"}
            };

        public static T CreateInstance<T>() where T : class
        {
            try
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
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
