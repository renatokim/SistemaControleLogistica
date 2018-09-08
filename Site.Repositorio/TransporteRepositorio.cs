using Site.DTO;
using Site.Entidade.EntidadeModel;
using Site.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.ActiveRecords;
using MySql.Data.MySqlClient;
using Dapper;
using System.Configuration;

namespace Site.Repositorio
{
    public class TransporteRepositorio : ITransporteRepositorio
    {
        public IList<DTOTransporte> GetAll()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTOTransporte>(@"
                    SELECT 
                        id AS id, 
                        cliente_id_coleta AS clienteColeta, 
                        cliente_id_entrega AS clienteEntrega, 
                        status AS status,
                        data_cadastro AS DataCadastro,
                        previsao_entrega AS DataPrevisaoEntrega,
                        rota_id as RotaId
                    FROM transporte").ToList();

                return result;
            }
        }

        public void Salvar(DTOTransporte transporte)
        {
            if (transporte.Id > 0)
            {
                Editar(transporte);
            }
            else
            {
                Incluir(transporte);
            }
        }

        private void Incluir(DTOTransporte transporte)
        {
            var transporteModel = TransporteModel.Transform(transporte);
            var sql = transporteModel.QueryInsert();

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                result.Read();
                transporte.Id = Convert.ToInt32(result["LASTID"]);
            }
        }

        private void Editar(DTOTransporte transporte)
        {
            var transporteModel = TransporteModel.Transform(transporte);
            var sql = ActiveRecord.Update(transporteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }

        public void Excluir(int id)
        {
            var transporteModel = new TransporteModel { Id = id.ToString() };
            var sql = ActiveRecord.Delete(transporteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }

        public void IncluirStatus(DTOTransporteStatus status)
        {
            var statusModel = TransporteStatusModel.Transform(status);
            var sql = statusModel.QueryInsert();

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                result.Read();
                status.Id = Convert.ToInt32(result["LASTID"]);
            }
        }
    }
}
