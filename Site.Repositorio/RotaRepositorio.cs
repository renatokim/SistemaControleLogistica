using Site.DTO;
using Site.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Site.Entidade.EntidadeModel;
using Site.ActiveRecords;

namespace Site.Repositorio
{
    public class RotaRepositorio : IRotaRepositorio
    {
        public IList<DTORota> GetAll()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTORota>(@"
                        SELECT id AS id,
                               transporte_id AS TransporteId,
                               frota_id AS FrotaId,
                               funcionario_id AS FuncionarioId,
                               uf_id AS UfId,
                               data_criacao AS DataCriacao,
                               data_entrega AS DataEntrega
                        FROM rota").ToList();

                return result;
            }
        }

        public void Salvar(DTORota rota)
        {
            if (rota.Id > 0)
            {
                Editar(rota);
            }
            else
            {
                Incluir(rota);
            }
        }

        private void Incluir(DTORota rota)
        {
            var transporteModel = RotaModel.Transform(rota);
            var sql = transporteModel.QueryInsert();

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                result.Read();
                rota.Id = Convert.ToInt32(result["LASTID"]);
            }
        }

        private void Editar(DTORota rota)
        {
            var transporteModel = RotaModel.Transform(rota);
            var sql = ActiveRecord.Update(transporteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }

        public void Excluir(int id)
        {
            var transporteModel = new RotaModel { Id = id.ToString() };
            var sql = ActiveRecord.Delete(transporteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }

        public IList<DTORotaGrid> GetRotas()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTORotaGrid>(@"
                        SELECT DATE_FORMAT(r.data_criacao, '%d/%m/%Y') AS DATA,
                               DATE_FORMAT(r.data_entrega, '%d/%m/%Y') AS DataEntrega,
                               r.id AS Id,
                               LPAD(r.id, '5', '0') AS Rota,
                               CASE
                                   WHEN r.uf_id = 1 THEN 'MG'
                                   WHEN r.uf_id = 2 THEN 'RJ'
                                   WHEN r.uf_id = 3 THEN 'SP'
                                   ELSE 'ES'
                               END AS UfEntrega
                        FROM rota r").ToList();

                return result;
            }
        }
    }
}
