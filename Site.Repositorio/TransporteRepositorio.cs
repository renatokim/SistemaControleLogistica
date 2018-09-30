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

        public IList<DTOTransporteGrid> GetTransportes()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTOTransporteGrid>(@"
                        SELECT t.id AS id,
                               upper(c.nome) AS clienteColeta,
                               upper(c2.nome) AS clienteEntrega,
                               DATE_FORMAT(t.previsao_entrega, '%d/%m/%Y') AS previsaoEntrega,
                               CASE
                                   WHEN c.uf = 1 THEN 'MG'
                                   WHEN c.uf = 2 THEN 'RJ'
                                   WHEN c.uf = 3 THEN 'SP'
                                   ELSE 'ES'
                               END AS ufremetente,
                               CASE
                                   WHEN c.uf = 1 THEN 'MG'
                                   WHEN c.uf = 2 THEN 'RJ'
                                   WHEN c.uf = 3 THEN 'SP'
                                   ELSE 'ES'
                               END AS ufdestinatario,
                               CASE
                                   WHEN t.status = 0 THEN 'SOLICITADO'
                                   WHEN t.status = 1 THEN 'COLETADO'
                                   WHEN t.status = 2 THEN 'EM EXPEDIÇÃO'
                                   WHEN t.status = 3 THEN 'EM ENTREGA'
                                   WHEN t.status = 4 THEN 'ENTREGUE'
                                   WHEN t.status = 5 THEN 'DEVOLVIDO'
                                   ELSE 'CANCELADO'
                               END AS status
                        FROM transporte t
                        INNER JOIN clientes c ON c.id = t.cliente_id_coleta
                        INNER JOIN clientes c2 ON c2.id = t.cliente_id_entrega").ToList();

                return result;
            }
        }

        public IList<DTOClienteGrid> GetClientes()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTOClienteGrid>(@"
                        SELECT c.id AS Id,
                               upper(c.nome) AS Nome,
                               upper(c.logradouro)AS Endereco,
                               (c.numero) AS Numero,
                               upper(c.bairro) AS Bairro,
                               c.cep AS Cep,
                               CASE
                                   WHEN c.uf = 1 THEN 'MG'
                                   WHEN c.uf = 2 THEN 'RJ'
                                   WHEN c.uf = 3 THEN 'SP'
                                   ELSE 'ES'
                               END AS Uf,
                               upper(c.cidade) AS Cidade
                        FROM clientes c").ToList();

                return result;
            }
        }
    }
}
